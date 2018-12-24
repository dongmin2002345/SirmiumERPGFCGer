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
    public class MunicipalityViewRepository : IMunicipalityRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        public MunicipalityViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<Municipality> GetMunicipalities(int companyId)
        {
            List<Municipality> Municipalities = new List<Municipality>();

            string queryString =
                "SELECT MunicipalityId, MunicipalityIdentifier, Code, MunicipalityCode, Name, " +
                "RegionId, RegionIdentifier, RegionCode, RegionName, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vMunicipalities " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Municipality municipality;
                    while (reader.Read())
                    {
                        municipality = new Municipality();
                        municipality.Id = Int32.Parse(reader["MunicipalityId"].ToString());
                        municipality.Identifier = Guid.Parse(reader["MunicipalityIdentifier"].ToString());
                        municipality.Code = reader["Code"].ToString();
                        municipality.MunicipalityCode = reader["MunicipalityCode"].ToString();
                        municipality.Name = reader["Name"].ToString();

                        if (reader["RegionId"] != DBNull.Value)
                        {
                            municipality.Region = new Region();
                            municipality.RegionId = Int32.Parse(reader["RegionId"].ToString());
                            municipality.Region.Id = Int32.Parse(reader["RegionId"].ToString());
                            municipality.Region.Identifier = Guid.Parse(reader["RegionIdentifier"].ToString());
                            municipality.Region.Code = reader["RegionCode"].ToString();
                            municipality.Region.Name = reader["RegionName"].ToString();
                        }

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            municipality.Country = new Country();
                            municipality.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            municipality.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            municipality.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            municipality.Country.Code = reader["CountryCode"].ToString();
                            municipality.Country.Name = reader["CountryName"].ToString();
                        }

                        municipality.Active = bool.Parse(reader["Active"].ToString());
                        municipality.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            municipality.CreatedBy = new User();
                            municipality.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            municipality.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            municipality.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            municipality.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            municipality.Company = new Company();
                            municipality.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            municipality.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            municipality.Company.Name = reader["CompanyName"].ToString();
                        }

                        Municipalities.Add(municipality);
                    }
                }
            }
            return Municipalities;

            //List<Municipality> Municipalities = context.Municipalities
            //    .Include(x => x.Country)
            //    .Include(x => x.Region)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .OrderByDescending(x => x.CreatedAt)
            //    .AsNoTracking()
            //    .ToList();

            //return Municipalities;
        }

        public List<Municipality> GetMunicipalitiesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<Municipality> Municipalities = new List<Municipality>();

            string queryString =
                "SELECT MunicipalityId, MunicipalityIdentifier, Code, MunicipalityCode, Name, " +
                "RegionId, RegionIdentifier, RegionCode, RegionName, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vMunicipalities " +
                "WHERE CompanyId = @CompanyId " +
                "AND CONVERT(DATETIME, CONVERT(VARCHAR(20), UpdatedAt, 120)) > CONVERT(DATETIME, CONVERT(VARCHAR(20), @LastUpdateTime, 120));";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));
                command.Parameters.Add(new SqlParameter("@LastUpdateTime", lastUpdateTime));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Municipality municipality;
                    while (reader.Read())
                    {
                        municipality = new Municipality();
                        municipality.Id = Int32.Parse(reader["MunicipalityId"].ToString());
                        municipality.Identifier = Guid.Parse(reader["MunicipalityIdentifier"].ToString());
                        municipality.Code = reader["Code"].ToString();
                        municipality.MunicipalityCode = reader["MunicipalityCode"].ToString();
                        municipality.Name = reader["Name"].ToString();

                        if (reader["RegionId"] != DBNull.Value)
                        {
                            municipality.Region = new Region();
                            municipality.RegionId = Int32.Parse(reader["RegionId"].ToString());
                            municipality.Region.Id = Int32.Parse(reader["RegionId"].ToString());
                            municipality.Region.Identifier = Guid.Parse(reader["RegionIdentifier"].ToString());
                            municipality.Region.Code = reader["RegionCode"].ToString();
                            municipality.Region.Name = reader["RegionName"].ToString();
                        }

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            municipality.Country = new Country();
                            municipality.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            municipality.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            municipality.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            municipality.Country.Code = reader["CountryCode"].ToString();
                            municipality.Country.Name = reader["CountryName"].ToString();
                        }

                        municipality.Active = bool.Parse(reader["Active"].ToString());
                        municipality.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            municipality.CreatedBy = new User();
                            municipality.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            municipality.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            municipality.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            municipality.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            municipality.Company = new Company();
                            municipality.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            municipality.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            municipality.Company.Name = reader["CompanyName"].ToString();
                        }

                        Municipalities.Add(municipality);
                    }
                }
            }
            return Municipalities;

            //List<Municipality> Municipalities = context.Municipalities
            //    .Include(x => x.Country)
            //    .Include(x => x.Region)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .OrderByDescending(x => x.CreatedAt)
            //    .AsNoTracking()
            //    .ToList();

            //return Municipalities;
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
                .FirstOrDefault(x => x.Identifier == identifier);

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
