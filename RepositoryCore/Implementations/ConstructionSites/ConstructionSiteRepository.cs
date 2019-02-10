using DomainCore.ConstructionSites;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.ConstructionSites;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.ConstructionSites
{
    public class ConstructionSiteRepository : IConstructionSiteRepository
    {
        private ApplicationDbContext context;

        public ConstructionSiteRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<ConstructionSite> GetConstructionSites(int companyId)
        {
            List<ConstructionSite> constructionSites = context.ConstructionSites
                .Include(x => x.City)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Active == true && x.CompanyId == companyId)
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking()
                .ToList();

            return constructionSites;
        }

        public ConstructionSite GetConstructionSite(int constructionSiteId)
        {
            ConstructionSite constructionSite = context.ConstructionSites
                .Include(x => x.City)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .FirstOrDefault(x => x.Id == constructionSiteId);

            return constructionSite;
        }

        public List<ConstructionSite> GetConstructionSitesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<ConstructionSite> constructionSites = context.ConstructionSites
                .Include(x => x.City)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
                .OrderByDescending(x => x.UpdatedAt)
                .AsNoTracking()
                .ToList();

            return constructionSites;
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.ConstructionSites
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(ConstructionSite))
                    .Select(x => x.Entity as ConstructionSite))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "GRA-00001";
            else
            {
                string activeCode = context.ConstructionSites
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(ConstructionSite))
                        .Select(x => x.Entity as ConstructionSite))
                    .Where(x => x.CompanyId == companyId)
                    .OrderByDescending(x => x.Id).FirstOrDefault()
                    .Code;
                if (!String.IsNullOrEmpty(activeCode))
                {
                    int intValue = Int32.Parse(activeCode.Replace("GRA-", ""));
                    return "GRA-" + (intValue + 1).ToString("00000");
                }
                else
                    return "";
            }
        }

        public ConstructionSite Create(ConstructionSite constructionSite)
        {
            if (context.ConstructionSites.Where(x => x.Identifier != null && x.Identifier == constructionSite.Identifier).Count() == 0)
            {
                if (context.ConstructionSites.Where(x => x.InternalCode == constructionSite.InternalCode).Count() > 0)
                    throw new Exception("Gradilište sa datom šifrom već postoji u bazi!");

                constructionSite.Id = 0;

                constructionSite.Code = GetNewCodeValue(constructionSite.CompanyId ?? 0);
                constructionSite.Active = true;

                constructionSite.UpdatedAt = DateTime.Now;
                constructionSite.CreatedAt = DateTime.Now;

                context.ConstructionSites.Add(constructionSite);
                return constructionSite;
            }
            else
            {
                // Load constructionSite that will be updated
                ConstructionSite dbEntry = context.ConstructionSites
                .FirstOrDefault(x => x.Identifier == constructionSite.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CityId = constructionSite.CityId ?? null;
                    dbEntry.CompanyId = constructionSite.CompanyId ?? null;
                    dbEntry.CreatedById = constructionSite.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = constructionSite.Code;
                    dbEntry.InternalCode = constructionSite.InternalCode;
                    dbEntry.Name = constructionSite.Name;
                    dbEntry.Address = constructionSite.Address;
                    dbEntry.MaxWorkers = constructionSite.MaxWorkers;
                    dbEntry.Status = constructionSite.Status;
                    dbEntry.ProContractDate = constructionSite.ProContractDate;
                    dbEntry.ContractStart = constructionSite.ContractStart;
                    dbEntry.ContractExpiration = constructionSite.ContractExpiration;


                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public ConstructionSite Delete(Guid identifier)
        {
            // Load ConstructionSite that will be deleted
            ConstructionSite dbEntry = context.ConstructionSites
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
