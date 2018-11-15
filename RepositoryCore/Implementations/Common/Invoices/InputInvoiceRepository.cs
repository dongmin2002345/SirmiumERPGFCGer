using DomainCore.Common.InputInvoices;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Common.Invoices;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.Invoices
{
    public class InputInvoiceRepository : IInputInvoiceRepository
    {
		ApplicationDbContext context;

		public InputInvoiceRepository(ApplicationDbContext context)
		{
			this.context = context;
		}

		public List<InputInvoice> GetInputInvoices(int companyId)
		{
			List<InputInvoice> InputInvoices = context.InputInvoices
				.Include(x => x.BusinessPartner)
				.Include(x => x.Company)
				.Include(x => x.CreatedBy)
				.Where(x => x.Active == true && x.CompanyId == companyId)
				.OrderByDescending(x => x.CreatedAt)
				.AsNoTracking()
				.ToList();

			return InputInvoices;
		}

		public List<InputInvoice> GetInputInvoicesNewerThan(int companyId, DateTime lastUpdateTime)
		{
			List<InputInvoice> InputInvoices = context.InputInvoices
				.Include(x => x.BusinessPartner)
				.Include(x => x.Company)
				.Include(x => x.CreatedBy)
				.Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime && x.Active == true)
				.OrderByDescending(x => x.UpdatedAt)
				.AsNoTracking()
				.ToList();

			return InputInvoices;
		}

		private string GetNewCodeValue(int companyId)
		{
			int count = context.InputInvoices
				.Union(context.ChangeTracker.Entries()
					.Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(InputInvoice))
					.Select(x => x.Entity as InputInvoice))
				.Where(x => x.CompanyId == companyId).Count();
			if (count == 0)
				return "IZLAZNI-00001";
			else
			{
				string activeCode = context.InputInvoices
					.Union(context.ChangeTracker.Entries()
						.Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(InputInvoice))
						.Select(x => x.Entity as InputInvoice))
					.Where(x => x.CompanyId == companyId)
					.OrderByDescending(x => x.Id).FirstOrDefault()
					.Code;
				if (!String.IsNullOrEmpty(activeCode))
				{
					int intValue = Int32.Parse(activeCode.Replace("IZLAZNI-", ""));
					return "IZLAZNI-" + (intValue + 1).ToString("00000");
				}
				else
					return "";
			}
		}

		public InputInvoice Create(InputInvoice inputInvoice)
		{
			if (context.InputInvoices.Where(x => x.Identifier != null && x.Identifier == inputInvoice.Identifier).Count() == 0)
			{
				inputInvoice.Id = 0;

				inputInvoice.Code = GetNewCodeValue(inputInvoice.CompanyId ?? 0);
				inputInvoice.Active = true;

				inputInvoice.UpdatedAt = DateTime.Now;
				inputInvoice.CreatedAt = DateTime.Now;

				context.InputInvoices.Add(inputInvoice);
				return inputInvoice;
			}
			else
			{
				// Load favor that will be updated
				InputInvoice dbEntry = context.InputInvoices
				.FirstOrDefault(x => x.Identifier == inputInvoice.Identifier && x.Active == true);

				if (dbEntry != null)
				{
					dbEntry.BusinessPartnerId = inputInvoice.BusinessPartnerId ?? null;
					dbEntry.CompanyId = inputInvoice.CompanyId ?? null;
					dbEntry.CreatedById = inputInvoice.CreatedById ?? null;

					// Set properties
					dbEntry.Code = inputInvoice.Code;
					dbEntry.Supplier = inputInvoice.Supplier;
					dbEntry.Address = inputInvoice.Address;
					dbEntry.InvoiceNumber = inputInvoice.InvoiceNumber;
					dbEntry.InvoiceDate = inputInvoice.InvoiceDate;
					dbEntry.AmountNet = inputInvoice.AmountNet;
					dbEntry.PDVPercent = inputInvoice.PDVPercent;
					dbEntry.PDV = inputInvoice.PDV;
					dbEntry.AmountGross = inputInvoice.AmountGross;
					dbEntry.Currency = inputInvoice.Currency;
					dbEntry.DateOfPaymet = inputInvoice.DateOfPaymet;
					dbEntry.Status = inputInvoice.Status;
					dbEntry.StatusDate = inputInvoice.StatusDate;
					dbEntry.Description = inputInvoice.Description;

					// Set timestamp
					dbEntry.UpdatedAt = DateTime.Now;
				}

				return dbEntry;
			}
		}

		public InputInvoice Delete(Guid identifier)
		{
			// Load Favor that will be deleted
			InputInvoice dbEntry = context.InputInvoices
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
