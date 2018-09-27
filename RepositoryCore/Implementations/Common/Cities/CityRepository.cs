using DomainCore.Common.Cities;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Common.Cities;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.Cities
{
    public class CityRepository : ICityRepository
    {
        private ApplicationDbContext context;

        public CityRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<City> GetCities()
        {
            List<City> Cities = context.Cities
                .Include(x => x.CreatedBy)
                .Where(x => x.Active == true)
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking()
                .ToList();

            return Cities;
        }

        public List<City> GetCitiesNewerThen(DateTime lastUpdateTime)
        {
            List<City> Cities = context.Cities
                .Include(x => x.CreatedBy)
                .Where(x => x.UpdatedAt > lastUpdateTime && x.Active == true)
                .OrderByDescending(x => x.UpdatedAt)
                .AsNoTracking()
                .ToList();

            return Cities;
        }

        public City GetCity(int id)
        {
            return context.Cities
                .Include(x => x.CreatedBy)
            .FirstOrDefault(x => x.Id == id && x.Active == true);
        }

        public City Create(City city)
        {
            if (context.Cities.Where(x => x.Identifier != null && x.Identifier == city.Identifier).Count() == 0)
            {
                city.Id = 0;

                city.Active = true;

                context.Cities.Add(city);
                return city;
            }
            else
            {
                // Load item that will be updated
                City dbEntry = context.Cities
                    .FirstOrDefault(x => x.Identifier == city.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CreatedById = city.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = city.Code;
                    dbEntry.Name = city.Name;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public City Delete(Guid identifier)
        {
            // Load City that will be deleted
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
