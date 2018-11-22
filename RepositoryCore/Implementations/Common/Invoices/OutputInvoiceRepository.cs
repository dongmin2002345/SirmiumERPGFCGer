using DomainCore.Common.OutputInvoices;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RepositoryCore.Abstractions.Common.Invoices;
using RepositoryCore.Context;
using ServiceInterfaces.ViewModels.Common.OutputInvoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.Invoices
{
    public class OutputInvoiceRepository : IOutputInvoiceRepository
    {
        private ApplicationDbContext context;

        public OutputInvoiceRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<OutputInvoice> GetOutputInvoices(int companyId)
        {
            List<OutputInvoice> OutputInvoices = context.OutputInvoices
                .Include(x => x.BusinessPartner)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Active == true && x.CompanyId == companyId)
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking()
                .ToList();

            return OutputInvoices;
        }

        public List<OutputInvoice> GetOutputInvoicesNewerThan(int companyId, DateTime lastUpdateTime)
        {
            List<OutputInvoice> OutputInvoices = context.OutputInvoices
                .Include(x => x.BusinessPartner)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime && x.Active == true)
                .OrderByDescending(x => x.UpdatedAt)
                .AsNoTracking()
                .ToList();

            return OutputInvoices;
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.OutputInvoices
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(OutputInvoice))
                    .Select(x => x.Entity as OutputInvoice))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "1";
            else
            {
                string activeCode = context.OutputInvoices
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(OutputInvoice))
                        .Select(x => x.Entity as OutputInvoice))
                    .Where(x => x.CompanyId == companyId)
                    .OrderByDescending(x => x.Id).FirstOrDefault()
                    .Code;
                if (!String.IsNullOrEmpty(activeCode))
                {
                    int intValue = Int32.Parse(activeCode);
                    return  (intValue + 1).ToString();
                }
                else
                    return "";
            }
        }

        public OutputInvoice Create(OutputInvoice outputInvoice)
        {
            if (context.OutputInvoices.Where(x => x.Identifier != null && x.Identifier == outputInvoice.Identifier).Count() == 0)
            {
                outputInvoice.Id = 0;

                outputInvoice.Code = GetNewCodeValue(outputInvoice.CompanyId ?? 0);
                outputInvoice.Active = true;

                outputInvoice.UpdatedAt = DateTime.Now;
                outputInvoice.CreatedAt = DateTime.Now;

                context.OutputInvoices.Add(outputInvoice);
                return outputInvoice;
            }
            else
            {
                // Load outputInvoice that will be updated
                OutputInvoice dbEntry = context.OutputInvoices
                .FirstOrDefault(x => x.Identifier == outputInvoice.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.BusinessPartnerId = outputInvoice.BusinessPartnerId ?? null;
                    dbEntry.CompanyId = outputInvoice.CompanyId ?? null;
                    dbEntry.CreatedById = outputInvoice.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = outputInvoice.Code;
                    dbEntry.Supplier = outputInvoice.Supplier;
                    dbEntry.Address = outputInvoice.Address;
                    dbEntry.InvoiceNumber = outputInvoice.InvoiceNumber;
                    dbEntry.InvoiceDate = outputInvoice.InvoiceDate;
                    dbEntry.AmountNet = outputInvoice.AmountNet;
                    dbEntry.PdvPercent = outputInvoice.PdvPercent;
                    dbEntry.Pdv = outputInvoice.Pdv;
                    dbEntry.AmountGross = outputInvoice.AmountGross;
                    dbEntry.Currency = outputInvoice.Currency;
                    dbEntry.DateOfPayment = outputInvoice.DateOfPayment;
                    dbEntry.Status = outputInvoice.Status;
                    dbEntry.StatusDate = outputInvoice.StatusDate;
                    dbEntry.Description = outputInvoice.Description;


                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public OutputInvoice Delete(Guid identifier)
        {
            // Load OutputInvoice that will be deleted
            OutputInvoice dbEntry = context.OutputInvoices
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

