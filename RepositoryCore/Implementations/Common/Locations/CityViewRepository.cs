using Configurator;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Common.Locations;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Common.Locations;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.Locations
{
    public class CityViewRepository : ICityRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        public CityViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<City> GetCities(int companyId)
        {
            List<City> Cities = new List<City>();

            string queryString =
                "SELECT CityId, CityIdentifier, CityCode, CityName, ZipCode, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "RegionId, RegionIdentifier, RegionCode, RegionName, " +
                "MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vCities " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    City city;
                    while (reader.Read())
                    {
                        city = new City();
                        city.Id = Int32.Parse(reader["CityId"].ToString());
                        city.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                        city.Code = reader["CityCode"].ToString();
                        city.Name = reader["CityName"].ToString();
                        city.ZipCode = reader["ZipCode"]?.ToString();
                        
                        if (reader["CountryId"] != DBNull.Value)
                        {
                            city.Country = new Country();
                            city.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            city.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            city.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            city.Country.Code = reader["CountryCode"].ToString();
                            city.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["RegionId"] != DBNull.Value)
                        {
                            city.Region = new Region();
                            city.RegionId = Int32.Parse(reader["RegionId"].ToString());
                            city.Region.Id = Int32.Parse(reader["RegionId"].ToString());
                            city.Region.Identifier = Guid.Parse(reader["RegionIdentifier"].ToString());
                            city.Region.Code = reader["RegionCode"].ToString();
                            city.Region.Name = reader["RegionName"].ToString();
                        }

                        if (reader["MunicipalityId"] != DBNull.Value)
                        {
                            city.Municipality = new Municipality();
                            city.MunicipalityId = Int32.Parse(reader["MunicipalityId"].ToString());
                            city.Municipality.Id = Int32.Parse(reader["MunicipalityId"].ToString());
                            city.Municipality.Identifier = Guid.Parse(reader["MunicipalityIdentifier"].ToString());
                            city.Municipality.Code = reader["MunicipalityCode"].ToString();
                            city.Municipality.Name = reader["MunicipalityName"].ToString();
                        }

                        city.Active = bool.Parse(reader["Active"].ToString());
                        city.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            city.CreatedBy = new User();
                            city.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            city.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            city.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            city.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            city.Company = new Company();
                            city.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            city.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            city.Company.Name = reader["CompanyName"].ToString();
                        }

                        Cities.Add(city);
                    }
                }
            }
            return Cities;

            //List<City> cities = context.Cities
            //    .Include(x => x.Country)
            //    .Include(x => x.Region)
            //    .Include(x => x.Municipality)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .OrderByDescending(x => x.CreatedAt)
            //    .AsNoTracking()
            //    .ToList();

            //return cities;
        }

        public List<City> GetCitiesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<City> Cities = new List<City>();

            string queryString =
                "SELECT CityId, CityIdentifier, CityCode, CityName, ZipCode, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "RegionId, RegionIdentifier, RegionCode, RegionName, " +
                "MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vCities " +
                "WHERE CompanyId = @CompanyId AND UpdatedAt > @LastUpdateTime;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));
                command.Parameters.Add(new SqlParameter("@LastUpdateTime", lastUpdateTime));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    City city;
                    while (reader.Read())
                    {
                        city = new City();
                        city.Id = Int32.Parse(reader["CityId"].ToString());
                        city.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                        city.Code = reader["CityCode"].ToString();
                        city.Name = reader["CityName"].ToString();
                        city.ZipCode = reader["ZipCode"]?.ToString();

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            city.Country = new Country();
                            city.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            city.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            city.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            city.Country.Code = reader["CountryCode"].ToString();
                            city.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["RegionId"] != DBNull.Value)
                        {
                            city.Region = new Region();
                            city.RegionId = Int32.Parse(reader["RegionId"].ToString());
                            city.Region.Id = Int32.Parse(reader["RegionId"].ToString());
                            city.Region.Identifier = Guid.Parse(reader["RegionIdentifier"].ToString());
                            city.Region.Code = reader["RegionCode"].ToString();
                            city.Region.Name = reader["RegionName"].ToString();
                        }

                        if (reader["MunicipalityId"] != DBNull.Value)
                        {
                            city.Municipality = new Municipality();
                            city.MunicipalityId = Int32.Parse(reader["MunicipalityId"].ToString());
                            city.Municipality.Id = Int32.Parse(reader["MunicipalityId"].ToString());
                            city.Municipality.Identifier = Guid.Parse(reader["MunicipalityIdentifier"].ToString());
                            city.Municipality.Code = reader["MunicipalityCode"].ToString();
                            city.Municipality.Name = reader["MunicipalityName"].ToString();
                        }

                        city.Active = bool.Parse(reader["Active"].ToString());
                        city.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            city.CreatedBy = new User();
                            city.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            city.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            city.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            city.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            city.Company = new Company();
                            city.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            city.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            city.Company.Name = reader["CompanyName"].ToString();
                        }

                        Cities.Add(city);
                    }
                }
            }
            return Cities;

            //List<City> cities = context.Cities
            //    .Include(x => x.Country)
            //    .Include(x => x.Region)
            //    .Include(x => x.Municipality)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .OrderByDescending(x => x.UpdatedAt)
            //    .AsNoTracking()
            //    .ToList();

            //return cities;
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
                    dbEntry.CountryId = city.CountryId ?? null;
                    dbEntry.RegionId = city.RegionId ?? null;
                    dbEntry.MunicipalityId = city.MunicipalityId ?? null;
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
