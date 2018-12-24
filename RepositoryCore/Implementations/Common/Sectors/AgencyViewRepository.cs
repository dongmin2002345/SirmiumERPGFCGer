using Configurator;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Common.Locations;
using DomainCore.Common.Sectors;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Common.Sectors;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.Sectors
{
    public class AgencyViewRepository : IAgencyRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        public AgencyViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<Agency> GetAgencies(int companyId)
        {
            List<Agency> Agencies = new List<Agency>();

            string queryString =
                "SELECT AgencyId, AgencyIdentifier, AgencyCode, AgencyName, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "SectorId, SectorIdentifier, SectorCode, SectorName, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vAgencies " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Agency agency;
                    while (reader.Read())
                    {
                        agency = new Agency();
                        agency.Id = Int32.Parse(reader["AgencyId"].ToString());
                        agency.Identifier = Guid.Parse(reader["AgencyIdentifier"].ToString());
                        agency.Code = reader["AgencyCode"]?.ToString();
                        agency.Name = reader["AgencyName"].ToString();

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            agency.Country = new Country();
                            agency.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            agency.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            agency.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            agency.Country.Code = reader["CountryCode"].ToString();
                            agency.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["SectorId"] != DBNull.Value)
                        {
                            agency.Sector = new Sector();
                            agency.SectorId = Int32.Parse(reader["SectorId"].ToString());
                            agency.Sector.Id = Int32.Parse(reader["SectorId"].ToString());
                            agency.Sector.Identifier = Guid.Parse(reader["SectorIdentifier"].ToString());
                            agency.Sector.Code = reader["SectorCode"].ToString();
                            agency.Sector.Name = reader["SectorName"].ToString();
                        }

                        agency.Active = bool.Parse(reader["Active"].ToString());
                        agency.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            agency.CreatedBy = new User();
                            agency.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            agency.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            agency.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            agency.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            agency.Company = new Company();
                            agency.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            agency.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            agency.Company.Name = reader["CompanyName"].ToString();
                        }

                        Agencies.Add(agency);
                    }
                }
            }
            return Agencies;

            //List<Agency> Agencies = context.Agencies
            //    .Include(x => x.Country)
            //    .Include(x => x.Sector)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .OrderByDescending(x => x.CreatedAt)
            //    .AsNoTracking()
            //    .ToList();

            //return Agencies;
        }

        public List<Agency> GetAgenciesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<Agency> Agencies = new List<Agency>();

            string queryString =
                "SELECT AgencyId, AgencyIdentifier, AgencyCode, AgencyName, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "SectorId, SectorIdentifier, SectorCode, SectorName, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vAgencies " +
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
                    Agency agency;
                    while (reader.Read())
                    {
                        agency = new Agency();
                        agency.Id = Int32.Parse(reader["AgencyId"].ToString());
                        agency.Identifier = Guid.Parse(reader["AgencyIdentifier"].ToString());
                        agency.Code = reader["AgencyCode"]?.ToString();
                        agency.Name = reader["AgencyName"].ToString();

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            agency.Country = new Country();
                            agency.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            agency.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            agency.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            agency.Country.Code = reader["CountryCode"].ToString();
                            agency.Country.Name = reader["CountryName"].ToString();
                        }

                        if (reader["SectorId"] != DBNull.Value)
                        {
                            agency.Sector = new Sector();
                            agency.SectorId = Int32.Parse(reader["SectorId"].ToString());
                            agency.Sector.Id = Int32.Parse(reader["SectorId"].ToString());
                            agency.Sector.Identifier = Guid.Parse(reader["SectorIdentifier"].ToString());
                            agency.Sector.Code = reader["SectorCode"].ToString();
                            agency.Sector.Name = reader["SectorName"].ToString();
                        }

                        agency.Active = bool.Parse(reader["Active"].ToString());
                        agency.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            agency.CreatedBy = new User();
                            agency.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            agency.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            agency.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            agency.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            agency.Company = new Company();
                            agency.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            agency.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            agency.Company.Name = reader["CompanyName"].ToString();
                        }

                        Agencies.Add(agency);
                    }
                }
            }
            return Agencies;

            //List<Agency> Agencies = context.Agencies
            //    .Include(x => x.Country)
            //    .Include(x => x.Sector)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .OrderByDescending(x => x.UpdatedAt)
            //    .AsNoTracking()
            //    .ToList();

            //return Agencies;
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
