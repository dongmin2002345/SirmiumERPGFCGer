using DomainCore.Banks;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Banks;
using RepositoryCore.Abstractions.Common.Sectors;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Banks
{
    public class BankRepository : IBankRepository
	{
		private ApplicationDbContext context;

		public BankRepository(ApplicationDbContext context)
		{
			this.context = context;
		}

		public List<Bank> GetBanks(int companyId)
		{
			List<Bank> banks = context.Banks
				.Include(x => x.Country)
				.Include(x => x.Company)
				.Include(x => x.CreatedBy)
				.Where(x => x.Active == true && x.CompanyId == companyId)
				.OrderByDescending(x => x.CreatedAt)
				.AsNoTracking()
				.ToList();

			return banks;
		}

		public List<Bank> GetBanksNewerThen(int companyId, DateTime lastUpdateTime)
		{
			List<Bank> banks = context.Banks
				.Include(x => x.Country)
				.Include(x => x.Company)
				.Include(x => x.CreatedBy)
				.Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime && x.Active == true)
				.OrderByDescending(x => x.UpdatedAt)
				.AsNoTracking()
				.ToList();

			return banks;
		}

		private string GetNewCodeValue(int companyId)
		{
			int count = context.Banks
				.Union(context.ChangeTracker.Entries()
					.Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Bank))
					.Select(x => x.Entity as Bank))
				.Where(x => x.CompanyId == companyId).Count();
			if (count == 0)
				return "BANK-00001";
			else
			{
				string activeCode = context.Banks
					.Union(context.ChangeTracker.Entries()
						.Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Bank))
						.Select(x => x.Entity as Bank))
					.Where(x => x.CompanyId == companyId)
					.OrderByDescending(x => x.Id).FirstOrDefault()
					.Code;
				if (!String.IsNullOrEmpty(activeCode))
				{
					int intValue = Int32.Parse(activeCode.Replace("BANK-", ""));
					return "BANK-" + (intValue + 1).ToString("00000");
				}
				else
					return "";
			}
		}

		public Bank Create(Bank bank)
		{
			if (context.Banks.Where(x => x.Identifier != null && x.Identifier == bank.Identifier).Count() == 0)
			{
				bank.Id = 0;

				bank.Code = GetNewCodeValue(bank.CompanyId ?? 0);
				bank.Active = true;

				bank.UpdatedAt = DateTime.Now;
				bank.CreatedAt = DateTime.Now;

				context.Banks.Add(bank);
				return bank;
			}
			else
			{
				// Load Sector that will be updated
				Bank dbEntry = context.Banks
				.FirstOrDefault(x => x.Identifier == bank.Identifier && x.Active == true);

				if (dbEntry != null)
				{
					dbEntry.CountryId = bank.CountryId ?? null;
					dbEntry.CompanyId = bank.CompanyId ?? null;
					dbEntry.CreatedById = bank.CreatedById ?? null;

					// Set properties
					dbEntry.Code = bank.Code;
					dbEntry.Name = bank.Name;

					// Set timestamp
					dbEntry.UpdatedAt = DateTime.Now;
				}

				return dbEntry;
			}
		}

		public Bank Delete(Guid identifier)
		{
			// Load Remedy that will be deleted
			Bank dbEntry = context.Banks
				.FirstOrDefault(x => x.Identifier == identifier && x.Active == true);

			if (dbEntry != null)
			{
				// Set activity
				dbEntry.Active = false;
				// Set timestamp
				dbEntry.UpdatedAt = DateTime.Now;
			}

			return dbEntry;
		}
	}
}
