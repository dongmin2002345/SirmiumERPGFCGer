using DomainCore.Common.Locations;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Common.Locations;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.Locations
{
    public class RegionRepository : IRegionRepository
    {
        ApplicationDbContext context;

        public RegionRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<Region> GetRegions(int companyId)
        {
            List<Region> Regions = context.Regions
                 .Include(x => x.Country)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Active == true && x.CompanyId == companyId)
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking()
                .ToList();

            return Regions;
        }

        public List<Region> GetRegionsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<Region> Regions = context.Regions
                  .Include(x => x.Country)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime && x.Active == true)
                .OrderByDescending(x => x.UpdatedAt)
                .AsNoTracking()
                .ToList();

            return Regions;
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.Regions
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Region))
                    .Select(x => x.Entity as Region))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "REG-00001";
            else
            {
                string activeCode = context.Regions
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Region))
                        .Select(x => x.Entity as Region))
                    .Where(x => x.CompanyId == companyId)
                    .OrderByDescending(x => x.Id).FirstOrDefault()
                    .Code;
                if (!String.IsNullOrEmpty(activeCode))
                {
                    int intValue = Int32.Parse(activeCode.Replace("REG-", ""));
                    return "REG-" + (intValue + 1).ToString("00000");
                }
                else
                    return "";
            }
        }

        public Region Create(Region region)
        {
            if (context.Regions.Where(x => x.Identifier != null && x.Identifier == region.Identifier).Count() == 0)
            {
                region.Id = 0;

                region.Code = GetNewCodeValue(region.CompanyId ?? 0);
                region.Active = true;

                region.UpdatedAt = DateTime.Now;
                region.CreatedAt = DateTime.Now;

                context.Regions.Add(region);
                return region;
            }
            else
            {
                // Load region that will be updated
                Region dbEntry = context.Regions
                .FirstOrDefault(x => x.Identifier == region.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CountryId = region.CountryId ?? null;
                    dbEntry.CompanyId = region.CompanyId ?? null;
                    dbEntry.CreatedById = region.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = region.Code;
                    dbEntry.Name = region.Name;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public Region Delete(Guid identifier)
        {
            // Load Region that will be deleted
            Region dbEntry = context.Regions
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
