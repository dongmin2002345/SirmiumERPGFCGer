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

        public City Create(City City)
        {
            if (context.Cities.Where(x => x.Identifier != null && x.Identifier == City.Identifier).Count() == 0)
            {
                City.Id = 0;

                City.Active = true;

                context.Cities.Add(City);
                return City;
            }
            else
            {
                // Load item that will be updated
                City dbEntry = context.Cities
                    .FirstOrDefault(x => x.Identifier == City.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CreatedById = City.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = City.Code;
                    dbEntry.Name = City.Name;

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
