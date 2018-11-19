using DomainCore.Common.TaxAdministrations;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Common.TaxAdministrations;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.TaxAdministrations
{
    public class TaxAdministrationRepository : ITaxAdministrationRepository
    {
        private ApplicationDbContext context;

        public TaxAdministrationRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<TaxAdministration> GetTaxAdministrations(int companyId)
        {
            List<TaxAdministration> TaxAdministrations = context.TaxAdministrations
                .Include(x => x.City)
                .Include(x => x.Bank1)
                .Include(x => x.Bank2)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Active == true && x.CompanyId == companyId)
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking()
                .ToList();

            return TaxAdministrations;
        }

        public List<TaxAdministration> GetTaxAdministrationsNewerThan(int companyId, DateTime lastUpdateTime)
        {
            List<TaxAdministration> TaxAdministrations = context.TaxAdministrations
                .Include(x => x.City)
                .Include(x => x.Bank1)
                .Include(x => x.Bank2)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime && x.Active == true)
                .OrderByDescending(x => x.UpdatedAt)
                .AsNoTracking()
                .ToList();

            return TaxAdministrations;
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.TaxAdministrations
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(TaxAdministration))
                    .Select(x => x.Entity as TaxAdministration))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "Steuer-Ver-00001";
            else
            {
                string activeCode = context.TaxAdministrations
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(TaxAdministration))
                        .Select(x => x.Entity as TaxAdministration))
                    .Where(x => x.CompanyId == companyId)
                    .OrderByDescending(x => x.Id).FirstOrDefault()
                    .Code;
                if (!String.IsNullOrEmpty(activeCode))
                {
                    int intValue = Int32.Parse(activeCode.Replace("Steuer-Ver-", ""));
                    return "Steuer-Ver-" + (intValue + 1).ToString("00000");
                }
                else
                    return "";
            }
        }

        public TaxAdministration Create(TaxAdministration taxAdministration)
        {
            if (context.OutputInvoices.Where(x => x.Identifier != null && x.Identifier == taxAdministration.Identifier).Count() == 0)
            {
                taxAdministration.Id = 0;

                taxAdministration.Code = GetNewCodeValue(taxAdministration.CompanyId ?? 0);
                taxAdministration.Active = true;

                taxAdministration.UpdatedAt = DateTime.Now;
                taxAdministration.CreatedAt = DateTime.Now;

                context.TaxAdministrations.Add(taxAdministration);
                return taxAdministration;
            }
            else
            {
                // Load taxAdministration that will be updated
                TaxAdministration dbEntry = context.TaxAdministrations
                .FirstOrDefault(x => x.Identifier == taxAdministration.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CityId = taxAdministration.CityId ?? null;
                    dbEntry.BankId1 = taxAdministration.BankId1 ?? null;
                    dbEntry.BankId2 = taxAdministration.BankId2 ?? null;
                    dbEntry.CompanyId = taxAdministration.CompanyId ?? null;
                    dbEntry.CreatedById = taxAdministration.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = taxAdministration.Code;
                    dbEntry.Name = taxAdministration.Name;
                    dbEntry.Address1 = taxAdministration.Address1;
                    dbEntry.Address2 = taxAdministration.Address2;
                    dbEntry.Address3 = taxAdministration.Address3;
                    dbEntry.IBAN1 = taxAdministration.IBAN1;
                    dbEntry.SWIFT = taxAdministration.SWIFT;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public TaxAdministration Delete(Guid identifier)
        {
            // Load TaxAdministration that will be deleted
            TaxAdministration dbEntry = context.TaxAdministrations
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
