using DomainCore.Common.BusinessPartners;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Common.BusinessPartners;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.BusinessPartners
{
    public class BusinessPartnerTypeRepository : IBusinessPartnerTypeRepository
    {
        private ApplicationDbContext context;

        public BusinessPartnerTypeRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<BusinessPartnerType> GetBusinessPartnerTypes(int companyId)
        {
            List<BusinessPartnerType> BusinessPartnerTypes = context.BusinessPartnerTypes
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Active == true && x.CompanyId == companyId)
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking()
                .ToList();

            return BusinessPartnerTypes;
        }

        public List<BusinessPartnerType> GetBusinessPartnerTypesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<BusinessPartnerType> BusinessPartnerTypes = context.BusinessPartnerTypes
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime && x.Active == true)
                .OrderByDescending(x => x.UpdatedAt)
                .AsNoTracking()
                .ToList();

            return BusinessPartnerTypes;
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.BusinessPartnerTypes
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(BusinessPartnerType))
                    .Select(x => x.Entity as BusinessPartnerType))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "KOR-TIP-00001";
            else
            {
                string activeCode = context.BusinessPartnerTypes
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(BusinessPartnerType))
                        .Select(x => x.Entity as BusinessPartnerType))
                    .Where(x => x.CompanyId == companyId)
                    .OrderByDescending(x => x.Id).FirstOrDefault()
                    .Code;
                if (!String.IsNullOrEmpty(activeCode))
                {
                    int intValue = Int32.Parse(activeCode.Replace("KOR-TIP-", ""));
                    return "KOR-TIP-" + (intValue + 1).ToString("00000");
                }
                else
                    return "";
            }
        }

        public BusinessPartnerType Create(BusinessPartnerType businessPartnerType)
        {
            if (context.BusinessPartnerTypes.Where(x => x.Identifier != null && x.Identifier == businessPartnerType.Identifier).Count() == 0)
            {
                businessPartnerType.Id = 0;

                businessPartnerType.Code = GetNewCodeValue(businessPartnerType.CompanyId ?? 0);
                businessPartnerType.Active = true;

                businessPartnerType.UpdatedAt = DateTime.Now;
                businessPartnerType.CreatedAt = DateTime.Now;

                context.BusinessPartnerTypes.Add(businessPartnerType);
                return businessPartnerType;
            }
            else
            {
                // Load businessPartnerType that will be updated
                BusinessPartnerType dbEntry = context.BusinessPartnerTypes
                .FirstOrDefault(x => x.Identifier == businessPartnerType.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = businessPartnerType.CompanyId ?? null;
                    dbEntry.CreatedById = businessPartnerType.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = businessPartnerType.Code;
                    dbEntry.Name = businessPartnerType.Name;
                    dbEntry.IsBuyer = businessPartnerType.IsBuyer;
                    dbEntry.IsSupplier = businessPartnerType.IsSupplier;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public BusinessPartnerType Delete(Guid identifier)
        {
            // Load BusinessPartnerType that will be deleted
            BusinessPartnerType dbEntry = context.BusinessPartnerTypes
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
