using DomainCore.Employees;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Employees;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Employees
{
    public class LicenceTypeRepository : ILicenceTypeRepository
	{
		private ApplicationDbContext context;

		public LicenceTypeRepository(ApplicationDbContext context)
		{
			this.context = context;
		}

		public List<LicenceType> GetLicenceTypes(int companyId)
		{
			List<LicenceType> licenceTypes = context.LicenceTypes
				.Include(x => x.Country)
				.Include(x => x.Company)
				.Include(x => x.CreatedBy)
				.Where(x => x.Active == true && x.CompanyId == companyId)
				.OrderByDescending(x => x.CreatedAt)
				.AsNoTracking()
				.ToList();

			return licenceTypes;
		}

		public List<LicenceType> GetLicenceTypesNewerThen(int companyId, DateTime lastUpdateTime)
		{
			List<LicenceType> licenceTypes = context.LicenceTypes
				.Include(x => x.Country)
				.Include(x => x.Company)
				.Include(x => x.CreatedBy)
				.Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime && x.Active == true)
				.OrderByDescending(x => x.UpdatedAt)
				.AsNoTracking()
				.ToList();

			return licenceTypes;
		}

		private string GetNewCodeValue(int companyId)
		{
			int count = context.LicenceTypes
				.Union(context.ChangeTracker.Entries()
					.Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(LicenceType))
					.Select(x => x.Entity as LicenceType))
				.Where(x => x.CompanyId == companyId).Count();
			if (count == 0)
				return "LIC-00001";
			else
			{
				string activeCode = context.LicenceTypes
					.Union(context.ChangeTracker.Entries()
						.Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(LicenceType))
						.Select(x => x.Entity as LicenceType))
					.Where(x => x.CompanyId == companyId)
					.OrderByDescending(x => x.Id).FirstOrDefault()
					.Code;
				if (!String.IsNullOrEmpty(activeCode))
				{
					int intValue = Int32.Parse(activeCode.Replace("LIC-", ""));
					return "LIC-" + (intValue + 1).ToString("00000");
				}
				else
					return "";
			}
		}

		public LicenceType Create(LicenceType licenceType)
		{
			if (context.Banks.Where(x => x.Identifier != null && x.Identifier == licenceType.Identifier).Count() == 0)
			{
				licenceType.Id = 0;

				licenceType.Code = GetNewCodeValue(licenceType.CompanyId ?? 0);
				licenceType.Active = true;

				licenceType.UpdatedAt = DateTime.Now;
				licenceType.CreatedAt = DateTime.Now;

				context.LicenceTypes.Add(licenceType);
				return licenceType;
			}
			else
			{
				// Load Sector that will be updated
				LicenceType dbEntry = context.LicenceTypes
				.FirstOrDefault(x => x.Identifier == licenceType.Identifier && x.Active == true);

				if (dbEntry != null)
				{
					dbEntry.CountryId = licenceType.CountryId ?? null;
					dbEntry.CompanyId = licenceType.CompanyId ?? null;
					dbEntry.CreatedById = licenceType.CreatedById ?? null;

					// Set properties
					dbEntry.Code = licenceType.Code;
					dbEntry.Category = licenceType.Category;

					dbEntry.Description = licenceType.Description;

					// Set timestamp
					dbEntry.UpdatedAt = DateTime.Now;
				}

				return dbEntry;
			}
		}

		public LicenceType Delete(Guid identifier)
		{
			// Load Remedy that will be deleted
			LicenceType dbEntry = context.LicenceTypes
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
