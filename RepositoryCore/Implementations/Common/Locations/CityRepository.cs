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
    public class CityRepository : ICityRepository
    {
        private ApplicationDbContext context;

        public CityRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<City> GetCities(int companyId)
        {
            List<City> cities = context.Cities
                .Include(x => x.Country)
                .Include(x => x.Region)
                .Include(x => x.Municipality)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Active == true && x.CompanyId == companyId)
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking()
                .ToList();

            return cities;
        }

        public List<City> GetCitiesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<City> cities = context.Cities
                .Include(x => x.Country)
                .Include(x => x.Region)
                .Include(x => x.Municipality)
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime && x.Active == true)
                .OrderByDescending(x => x.UpdatedAt)
                .AsNoTracking()
                .ToList();

            return cities;
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.Cities
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(City))
                    .Select(x => x.Entity as City))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "GRAD-00001";
            else
            {
                string activeCode = context.Cities
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(City))
                        .Select(x => x.Entity as City))
                    .Where(x => x.CompanyId == companyId)
                    .OrderByDescending(x => x.Id).FirstOrDefault()
                    .Code;
                if (!String.IsNullOrEmpty(activeCode))
                {
                    int intValue = Int32.Parse(activeCode.Replace("GRAD-", ""));
                    return "GRAD-" + (intValue + 1).ToString("00000");
                }
                else
                    return "";
            }
        }

        public City Create(City city)
        {
            if (context.Cities.Where(x => x.Identifier != null && x.Identifier == city.Identifier).Count() == 0)
            {
                city.Id = 0;

                city.Code = GetNewCodeValue(city.CompanyId ?? 0);
                city.Active = true;

                city.UpdatedAt = DateTime.Now;
                city.CreatedAt = DateTime.Now;

                context.Cities.Add(city);
                return city;
            }
            else
            {
                // Load remedy that will be updated
                City dbEntry = context.Cities
                .FirstOrDefault(x => x.Identifier == city.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CountryId = city.Country?.Id ?? null;
                    dbEntry.RegionId = city.Region?.Id ?? null;
                    dbEntry.MunicipalityId = city.Municipality?.Id ?? null;
                    dbEntry.CompanyId = city.CompanyId ?? null;
                    dbEntry.CreatedById = city.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = city.Code;
                    dbEntry.ZipCode = city.ZipCode;
                    dbEntry.Name = city.Name;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public City Delete(Guid identifier)
        {
            // Load Remedy that will be deleted
            City dbEntry = context.Cities
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
