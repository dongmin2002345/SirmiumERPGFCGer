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
    public class RegionViewRepository : IRegionRepository
    {
        ApplicationDbContext context;
        private string connectionString;

        public RegionViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<Region> GetRegions(int companyId)
        {
            List<Region> Regions = new List<Region>();

            string queryString =
                "SELECT RegionId, RegionIdentifier, Code, RegionCode, RegionName, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vRegions " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Region region;
                    while (reader.Read())
                    {
                        region = new Region();
                        region.Id = Int32.Parse(reader["RegionId"].ToString());
                        region.Identifier = Guid.Parse(reader["RegionIdentifier"].ToString());
                        region.Code = reader["Code"].ToString();
                        region.RegionCode = reader["RegionCode"].ToString();
                        region.Name = reader["RegionName"].ToString();

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            region.Country = new Country();
                            region.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            region.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            region.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            region.Country.Mark = reader["CountryCode"].ToString();
                            region.Country.Name = reader["CountryName"].ToString();
                        }

                        region.Active = bool.Parse(reader["Active"].ToString());
                        region.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            region.CreatedBy = new User();
                            region.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            region.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            region.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            region.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            region.Company = new Company();
                            region.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            region.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            region.Company.Name = reader["CompanyName"].ToString();
                        }

                        Regions.Add(region);
                    }
                }
            }
            return Regions;

            //List<Region> Regions = context.Regions
            //    .Include(x => x.Country)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .OrderByDescending(x => x.CreatedAt)
            //    .AsNoTracking()
            //    .ToList();

            //return Regions;
        }

        public List<Region> GetRegionsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<Region> Regions = new List<Region>();

            string queryString =
                "SELECT RegionId, RegionIdentifier, Code, RegionCode, RegionName, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vRegions " +
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
                    Region region;
                    while (reader.Read())
                    {
                        region = new Region();
                        region.Id = Int32.Parse(reader["RegionId"].ToString());
                        region.Identifier = Guid.Parse(reader["RegionIdentifier"].ToString());
                        region.Code = reader["Code"].ToString();
                        region.RegionCode = reader["RegionCode"].ToString();
                        region.Name = reader["RegionName"].ToString();

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            region.Country = new Country();
                            region.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            region.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            region.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            region.Country.Mark = reader["CountryCode"].ToString();
                            region.Country.Name = reader["CountryName"].ToString();
                        }

                        region.Active = bool.Parse(reader["Active"].ToString());
                        region.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            region.CreatedBy = new User();
                            region.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            region.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            region.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            region.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            region.Company = new Company();
                            region.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            region.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            region.Company.Name = reader["CompanyName"].ToString();
                        }

                        Regions.Add(region);
                    }
                }
            }
            return Regions;

            //List<Region> Regions = context.Regions
            //      .Include(x => x.Country)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .OrderByDescending(x => x.UpdatedAt)
            //    .AsNoTracking()
            //    .ToList();

            //return Regions;
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
                    dbEntry.RegionCode = region.RegionCode;
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
