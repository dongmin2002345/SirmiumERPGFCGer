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
    public class BusinessPartnerByConstructionSiteRepository : IBusinessPartnerByConstructionSiteRepository
    {
        private ApplicationDbContext context;

        public BusinessPartnerByConstructionSiteRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<BusinessPartnerByConstructionSite> GetBusinessPartnerByConstructionSites(int companyId)
        {
            List<BusinessPartnerByConstructionSite> BusinessPartnerByConstructionSites = context.BusinessPartnerByConstructionSites
                .Include(x => x.BusinessPartner)
                .Include(x => x.ConstructionSite)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Active == true && x.CompanyId == companyId)
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking()
                .ToList();

            return BusinessPartnerByConstructionSites;
        }

        public List<BusinessPartnerByConstructionSite> GetBusinessPartnerByConstructionSitesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<BusinessPartnerByConstructionSite> BusinessPartnerByConstructionSites = context.BusinessPartnerByConstructionSites
                .Include(x => x.BusinessPartner)
                .Include(x => x.ConstructionSite)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking()
                .ToList();

            return BusinessPartnerByConstructionSites;
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.BusinessPartnerByConstructionSites
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(BusinessPartnerByConstructionSite))
                    .Select(x => x.Entity as BusinessPartnerByConstructionSite))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "BP-BY-CS-00001";
            else
            {
                string activeCode = context.BusinessPartnerByConstructionSites
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(BusinessPartnerByConstructionSite))
                        .Select(x => x.Entity as BusinessPartnerByConstructionSite))
                    .Where(x => x.CompanyId == companyId)
                    .OrderByDescending(x => x.Id).FirstOrDefault()
                    .Code;
                if (!String.IsNullOrEmpty(activeCode))
                {
                    int intValue = Int32.Parse(activeCode.Replace("BP-BY-CS-", ""));
                    return "BP-BY-CS-" + (intValue + 1).ToString("00000");
                }
                else
                    return "";
            }
        }

        public BusinessPartnerByConstructionSite Create(BusinessPartnerByConstructionSite businessPartnerByConstructionSite)
        {
            if (context.BusinessPartnerByConstructionSites.Where(x => x.Identifier != null && x.Identifier == businessPartnerByConstructionSite.Identifier).Count() == 0)
            {
                businessPartnerByConstructionSite.Id = 0;

                businessPartnerByConstructionSite.Code = GetNewCodeValue(businessPartnerByConstructionSite.CompanyId ?? 0);
                businessPartnerByConstructionSite.Active = true;

                businessPartnerByConstructionSite.UpdatedAt = DateTime.Now;
                businessPartnerByConstructionSite.CreatedAt = DateTime.Now;

                context.BusinessPartnerByConstructionSites.Add(businessPartnerByConstructionSite);
                return businessPartnerByConstructionSite;
            }
            else
            {
                // Load businessPartnerByConstructionSite that will be updated
                BusinessPartnerByConstructionSite dbEntry = context.BusinessPartnerByConstructionSites
                .FirstOrDefault(x => x.Identifier == businessPartnerByConstructionSite.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.BusinessPartnerId = businessPartnerByConstructionSite.BusinessPartnerId ?? null;
                    dbEntry.ConstructionSiteId = businessPartnerByConstructionSite.ConstructionSiteId ?? null;
                    dbEntry.CompanyId = businessPartnerByConstructionSite.CompanyId ?? null;
                    dbEntry.CreatedById = businessPartnerByConstructionSite.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = businessPartnerByConstructionSite.Code;
                    dbEntry.StartDate = businessPartnerByConstructionSite.StartDate;
                    dbEntry.EndDate = businessPartnerByConstructionSite.EndDate;
                    dbEntry.RealEndDate = businessPartnerByConstructionSite.RealEndDate;
                    dbEntry.MaxNumOfEmployees = businessPartnerByConstructionSite.MaxNumOfEmployees;
                    dbEntry.BusinessPartnerCount = businessPartnerByConstructionSite.BusinessPartnerCount;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public BusinessPartnerByConstructionSite Delete(BusinessPartnerByConstructionSite businessPartnerByConstructionSite)
        {
            // Load BusinessPartnerByConstructionSite that will be deleted
            BusinessPartnerByConstructionSite dbEntry = context.BusinessPartnerByConstructionSites
                .FirstOrDefault(x => x.Identifier == businessPartnerByConstructionSite.Identifier && x.Active == true);

            if (dbEntry != null)
            {
                // Set activity
                dbEntry.Active = false;
                dbEntry.RealEndDate = businessPartnerByConstructionSite.RealEndDate;
                // Set timestamp
                dbEntry.UpdatedAt = DateTime.Now;
            }

            return dbEntry;
        }
    }
}
