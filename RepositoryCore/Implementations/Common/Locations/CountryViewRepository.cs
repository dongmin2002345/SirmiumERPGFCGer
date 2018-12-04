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
    public class CountryViewRepository : ICountryRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        public CountryViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<Country> GetCountries(int companyId)
        {
            List<Country> Countries = new List<Country>();

            string queryString =
                "SELECT CountryId, CountryIdentifier, CountryCode, AlfaCode, NumericCode, Mark, CountryName, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vCountries " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Country country;
                    while (reader.Read())
                    {
                        country = new Country();
                        country.Id = Int32.Parse(reader["CountryId"].ToString());
                        country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                        country.Code = reader["CountryCode"].ToString();
                        country.AlfaCode = reader["AlfaCode"].ToString();
                        country.NumericCode = reader["NumericCode"].ToString();
                        country.Mark = reader["Mark"].ToString();
                        country.Name = reader["CountryName"].ToString();

                        country.Active = bool.Parse(reader["Active"].ToString());
                        country.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            country.CreatedBy = new User();
                            country.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            country.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            country.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            country.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            country.Company = new Company();
                            country.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            country.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            country.Company.Name = reader["CompanyName"].ToString();
                        }

                        Countries.Add(country);
                    }
                }
            }
            return Countries;

            //List<Country> Countries = context.Countries
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .OrderByDescending(x => x.CreatedAt)
            //    .AsNoTracking()
            //    .ToList();

            //return Countries;
        }

        public List<Country> GetCountriesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<Country> Countries = new List<Country>();

            string queryString =
                "SELECT CountryId, CountryIdentifier, CountryCode, AlfaCode, NumericCode, Mark, CountryName, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vCountries " +
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
                    Country country;
                    while (reader.Read())
                    {
                        country = new Country();
                        country.Id = Int32.Parse(reader["CountryId"].ToString());
                        country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                        country.Code = reader["CountryCode"].ToString();
                        country.AlfaCode = reader["AlfaCode"].ToString();
                        country.NumericCode = reader["NumericCode"].ToString();
                        country.Mark = reader["Mark"].ToString();
                        country.Name = reader["CountryName"].ToString();

                        country.Active = bool.Parse(reader["Active"].ToString());
                        country.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            country.CreatedBy = new User();
                            country.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            country.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            country.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            country.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            country.Company = new Company();
                            country.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            country.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            country.Company.Name = reader["CompanyName"].ToString();
                        }

                        Countries.Add(country);
                    }
                }
            }
            return Countries;

            //List<Country> Countries = context.Countries
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .OrderByDescending(x => x.CreatedAt)
            //    .AsNoTracking()
            //    .ToList();

            //return Countries;
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
