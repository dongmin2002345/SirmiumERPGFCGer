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
    public class BusinessPartnerByConstructionSiteHistoryRepository : IBusinessPartnerByConstructionSiteHistoryRepository
    {
        private ApplicationDbContext context;

        public BusinessPartnerByConstructionSiteHistoryRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<BusinessPartnerByConstructionSiteHistory> GetBusinessPartnerByConstructionSiteHistories(int companyId)
        {
            List<BusinessPartnerByConstructionSiteHistory> BusinessPartnerByConstructionSiteHistories = context.BusinessPartnerByConstructionSiteHistories
                .Include(x => x.BusinessPartner)
                .Include(x => x.ConstructionSite)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Active == true && x.CompanyId == companyId)
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking()
                .ToList();

            return BusinessPartnerByConstructionSiteHistories;
        }

        public List<BusinessPartnerByConstructionSiteHistory> GetBusinessPartnerByConstructionSiteHistoriesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<BusinessPartnerByConstructionSiteHistory> BusinessPartnerByConstructionSiteHistories = context.BusinessPartnerByConstructionSiteHistories
                .Include(x => x.BusinessPartner)
                .Include(x => x.ConstructionSite)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime && x.Active == true)
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking()
                .ToList();

            return BusinessPartnerByConstructionSiteHistories;
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.BusinessPartnerByConstructionSiteHistories
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(BusinessPartnerByConstructionSiteHistory))
                    .Select(x => x.Entity as BusinessPartnerByConstructionSiteHistory))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "EMP-BY-CS-HIS-00001";
            else
            {
                string activeCode = context.BusinessPartnerByConstructionSiteHistories
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(BusinessPartnerByConstructionSiteHistory))
                        .Select(x => x.Entity as BusinessPartnerByConstructionSiteHistory))
                    .Where(x => x.CompanyId == companyId)
                    .OrderByDescending(x => x.Id).FirstOrDefault()
                    .Code;
                if (!String.IsNullOrEmpty(activeCode))
                {
                    int intValue = Int32.Parse(activeCode.Replace("EMP-BY-CS-HIS-", ""));
                    return "EMP-BY-CS-HIS-" + (intValue + 1).ToString("00000");
                }
                else
                    return "";
            }
        }

        public BusinessPartnerByConstructionSiteHistory Create(BusinessPartnerByConstructionSiteHistory businessPartnerByConstructionSiteHistory)
        {
            if (context.BusinessPartnerByConstructionSiteHistories.Where(x => x.Identifier != null && x.Identifier == businessPartnerByConstructionSiteHistory.Identifier).Count() == 0)
            {
                businessPartnerByConstructionSiteHistory.Id = 0;

                businessPartnerByConstructionSiteHistory.Code = GetNewCodeValue(businessPartnerByConstructionSiteHistory.CompanyId ?? 0);
                businessPartnerByConstructionSiteHistory.Active = true;

                businessPartnerByConstructionSiteHistory.UpdatedAt = DateTime.Now;
                businessPartnerByConstructionSiteHistory.CreatedAt = DateTime.Now;

                context.BusinessPartnerByConstructionSiteHistories.Add(businessPartnerByConstructionSiteHistory);
                return businessPartnerByConstructionSiteHistory;
            }
            else
            {
                // Load businessPartnerByConstructionSiteHistory that will be updated
                BusinessPartnerByConstructionSiteHistory dbEntry = context.BusinessPartnerByConstructionSiteHistories
                .FirstOrDefault(x => x.Identifier == businessPartnerByConstructionSiteHistory.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.BusinessPartnerId = businessPartnerByConstructionSiteHistory.BusinessPartnerId ?? null;
                    dbEntry.ConstructionSiteId = businessPartnerByConstructionSiteHistory.ConstructionSiteId ?? null;
                    dbEntry.CompanyId = businessPartnerByConstructionSiteHistory.CompanyId ?? null;
                    dbEntry.CreatedById = businessPartnerByConstructionSiteHistory.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = businessPartnerByConstructionSiteHistory.Code;
                    dbEntry.StartDate = businessPartnerByConstructionSiteHistory.StartDate;
                    dbEntry.EndDate = businessPartnerByConstructionSiteHistory.EndDate;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public BusinessPartnerByConstructionSiteHistory Delete(Guid identifier)
        {
            // Load BusinessPartnerByConstructionSiteHistory that will be deleted
            BusinessPartnerByConstructionSiteHistory dbEntry = context.BusinessPartnerByConstructionSiteHistories
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
