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
    public class CountryRepository : ICountryRepository
    {
        private ApplicationDbContext context;

        public CountryRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<Country> GetCountries(int companyId)
        {
            List<Country> Countries = context.Countries
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Active == true && x.CompanyId == companyId)
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking()
                .ToList();

            return Countries;
        }

        public List<Country> GetCountriesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<Country> Countries = context.Countries
                .Include(x => x.Company)
                .Include(x => x.CreatedBy)
                .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime && x.Active == true)
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking()
                .ToList();

            return Countries;
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.Countries
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Country))
                    .Select(x => x.Entity as Country))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "DRZ-00001";
            else
            {
                string activeCode = context.Countries
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Country))
                        .Select(x => x.Entity as Country))
                    .Where(x => x.CompanyId == companyId)
                    .OrderByDescending(x => x.Id).FirstOrDefault()
                    .Code;
                if (!String.IsNullOrEmpty(activeCode))
                {
                    int intValue = Int32.Parse(activeCode.Replace("DRZ-", ""));
                    return "DRZ-" + (intValue + 1).ToString("00000");
                }
                else
                    return "";
            }
        }

        public Country Create(Country country)
        {
            if (context.Countries.Where(x => x.Identifier != null && x.Identifier == country.Identifier).Count() == 0)
            {
                country.Id = 0;

                country.Code = GetNewCodeValue(country.CompanyId ?? 0);
                country.Active = true;

                country.UpdatedAt = DateTime.Now;
                country.CreatedAt = DateTime.Now;

                context.Countries.Add(country);
                return country;
            }
            else
            {
                // Load Country that will be updated
                Country dbEntry = context.Countries
                .FirstOrDefault(x => x.Identifier == country.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = country.CompanyId ?? null;
                    dbEntry.CreatedById = country.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = country.Code;
                    dbEntry.AlfaCode = country.AlfaCode;
                    dbEntry.NumericCode = country.NumericCode;
                    dbEntry.Mark = country.Mark;
                    dbEntry.Name = country.Name;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public Country Delete(Guid identifier)
        {
            // Load Remedy that will be deleted
            Country dbEntry = context.Countries
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
