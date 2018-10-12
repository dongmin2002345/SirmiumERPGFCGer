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
    public class MunicipalityRepository : IMunicipalityRepository
    {
        private ApplicationDbContext context;

        public MunicipalityRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<Municipality> GetMunicipalities(int companyId)
        {
            List<Municipality> Municipalities = context.Municipalities
                .Include(x => x.Country)
                .Include(x => x.Region)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Active == true && x.CompanyId == companyId)
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking()
                .ToList();

            return Municipalities;
        }

        public List<Municipality> GetMunicipalitiesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<Municipality> Municipalities = context.Municipalities
                .Include(x => x.Country)
                .Include(x => x.Region)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime && x.Active == true)
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking()
                .ToList();

            return Municipalities;
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.Municipalities
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Municipality))
                    .Select(x => x.Entity as Municipality))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "OPST-00001";
            else
            {
                string activeCode = context.Municipalities
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Municipality))
                        .Select(x => x.Entity as Municipality))
                    .Where(x => x.CompanyId == companyId)
                    .OrderByDescending(x => x.Id).FirstOrDefault()
                    .Code;
                if (!String.IsNullOrEmpty(activeCode))
                {
                    int intValue = Int32.Parse(activeCode.Replace("OPST-", ""));
                    return "OPST-" + (intValue + 1).ToString("00000");
                }
                else
                    return "";
            }
        }

        public Municipality Create(Municipality municipality)
        {
            if (context.Municipalities.Where(x => x.Identifier != null && x.Identifier == municipality.Identifier).Count() == 0)
            {
                municipality.Id = 0;

                municipality.Code = GetNewCodeValue(municipality.CompanyId ?? 0);
                municipality.Active = true;

                municipality.UpdatedAt = DateTime.Now;
                municipality.CreatedAt = DateTime.Now;

                context.Municipalities.Add(municipality);
                return municipality;
            }
            else
            {
                // Load Municipality that will be updated
                Municipality dbEntry = context.Municipalities
                .FirstOrDefault(x => x.Identifier == municipality.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CountryId = municipality.CountryId ?? null;
                    dbEntry.RegionId = municipality.RegionId ?? null;
                    dbEntry.CompanyId = municipality.CompanyId ?? null;
                    dbEntry.CreatedById = municipality.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = municipality.Code;
                    dbEntry.MunicipalityCode = municipality.MunicipalityCode;
                    dbEntry.Name = municipality.Name;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public Municipality Delete(Guid identifier)
        {
            // Load Municipality that will be deleted
            Municipality dbEntry = context.Municipalities
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
