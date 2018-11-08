using DomainCore.Common.Sectors;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Common.Sectors;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.Sectors
{
    public class AgencyRepository : IAgencyRepository
    {
        private ApplicationDbContext context;

        public AgencyRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<Agency> GetAgencies(int companyId)
        {
            List<Agency> Agencies = context.Agencies
                .Include(x => x.Country)
                .Include(x => x.Sector)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Active == true && x.CompanyId == companyId)
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking()
                .ToList();

            return Agencies;
        }

        public List<Agency> GetAgenciesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<Agency> Agencies = context.Agencies
                .Include(x => x.Country)
                .Include(x => x.Sector)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
                .OrderByDescending(x => x.UpdatedAt)
                .AsNoTracking()
                .ToList();

            return Agencies;
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.Agencies
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Agency))
                    .Select(x => x.Entity as Agency))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "DEL-00001";
            else
            {
                string activeCode = context.Agencies
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Agency))
                        .Select(x => x.Entity as Agency))
                    .Where(x => x.CompanyId == companyId)
                    .OrderByDescending(x => x.Id).FirstOrDefault()
                    .Code;
                if (!String.IsNullOrEmpty(activeCode))
                {
                    int intValue = Int32.Parse(activeCode.Replace("DEL-", ""));
                    return "DEL-" + (intValue + 1).ToString("00000");
                }
                else
                    return "";
            }
        }

        public Agency Create(Agency Agency)
        {
            if (context.Agencies.Where(x => x.Identifier != null && x.Identifier == Agency.Identifier).Count() == 0)
            {
                Agency.Id = 0;

                Agency.Code = GetNewCodeValue(Agency.CompanyId ?? 0);
                Agency.Active = true;

                Agency.UpdatedAt = DateTime.Now;
                Agency.CreatedAt = DateTime.Now;

                context.Agencies.Add(Agency);
                return Agency;
            }
            else
            {
                // Load remedy that will be updated
                Agency dbEntry = context.Agencies
                .FirstOrDefault(x => x.Identifier == Agency.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CountryId = Agency.CountryId ?? null;
                    dbEntry.SectorId = Agency.SectorId ?? null;
                    dbEntry.CompanyId = Agency.CompanyId ?? null;
                    dbEntry.CreatedById = Agency.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = Agency.Code;
                    dbEntry.Name = Agency.Name;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public Agency Delete(Guid identifier)
        {
            // Load Remedy that will be deleted
            Agency dbEntry = context.Agencies
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
