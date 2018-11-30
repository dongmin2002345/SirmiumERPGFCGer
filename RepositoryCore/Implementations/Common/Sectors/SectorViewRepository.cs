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
    public class SectorViewRepository : ISectorRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        public SectorViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<Sector> GetSectors(int companyId)
        {
            List<Sector> Sectors = new List<Sector>();

            string queryString =
                "SELECT SectorId, SectorIdentifier, SectorCode, SectorSecondCode, SectorName, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vSectors " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Sector sector;
                    while (reader.Read())
                    {
                        sector = new Sector();
                        sector.Id = Int32.Parse(reader["SectorId"].ToString());
                        sector.Identifier = Guid.Parse(reader["SectorIdentifier"].ToString());
                        sector.Code = reader["SectorCode"].ToString();
                        sector.SecondCode = reader["SectorSecondCode"]?.ToString();
                        sector.Name = reader["SectorName"].ToString();

                        if (reader["CountryId"] != null)
                        {
                            sector.Country = new Country();
                            sector.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            sector.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            sector.Country.Identifier = Guid.Parse(reader["FoodTypeIdentifier"].ToString());
                            sector.Country.Code = reader["FoodTypeCode"].ToString();
                            sector.Country.Name = reader["FoodTypeName"].ToString();
                        }

                        sector.Active = bool.Parse(reader["Active"].ToString());
                        sector.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != null)
                        {
                            sector.CreatedBy = new User();
                            sector.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            sector.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            sector.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            sector.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != null)
                        {
                            sector.Company = new Company();
                            sector.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            sector.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            sector.Company.Name = reader["CompanyName"].ToString();
                        }

                        Sectors.Add(sector);
                    }
                }
            }
            return Sectors;

            //List<Sector> sectors = context.Sectors
            //    .Include(x => x.Country)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .OrderByDescending(x => x.CreatedAt)
            //    .AsNoTracking()
            //    .ToList();

            //return sectors;
        }

        public List<Sector> GetSectorsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<Sector> Sectors = new List<Sector>();

            string queryString =
                "SELECT SectorId, SectorIdentifier, SectorCode, SectorSecondCode, SectorName, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vSectors " +
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
                    Sector sector;
                    while (reader.Read())
                    {
                        sector = new Sector();
                        sector.Id = Int32.Parse(reader["SectorId"].ToString());
                        sector.Identifier = Guid.Parse(reader["SectorIdentifier"].ToString());
                        sector.Code = reader["SectorCode"].ToString();
                        sector.SecondCode = reader["SectorSecondCode"]?.ToString();
                        sector.Name = reader["SectorName"].ToString();

                        if (reader["CountryId"] != null)
                        {
                            sector.Country = new Country();
                            sector.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            sector.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            sector.Country.Identifier = Guid.Parse(reader["FoodTypeIdentifier"].ToString());
                            sector.Country.Code = reader["FoodTypeCode"].ToString();
                            sector.Country.Name = reader["FoodTypeName"].ToString();
                        }

                        sector.Active = bool.Parse(reader["Active"].ToString());
                        sector.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != null)
                        {
                            sector.CreatedBy = new User();
                            sector.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            sector.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            sector.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            sector.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != null)
                        {
                            sector.Company = new Company();
                            sector.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            sector.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            sector.Company.Name = reader["CompanyName"].ToString();
                        }

                        Sectors.Add(sector);
                    }
                }
            }
            return Sectors;

            //List<Sector> sectors = context.Sectors
            //    .Include(x => x.Country)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .OrderByDescending(x => x.UpdatedAt)
            //    .AsNoTracking()
            //    .ToList();

            //return sectors;
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.Sectors
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Sector))
                    .Select(x => x.Entity as Sector))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "SEK-00001";
            else
            {
                string activeCode = context.Sectors
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Sector))
                        .Select(x => x.Entity as Sector))
                    .Where(x => x.CompanyId == companyId)
                    .OrderByDescending(x => x.Id).FirstOrDefault()
                    .Code;
                if (!String.IsNullOrEmpty(activeCode))
                {
                    int intValue = Int32.Parse(activeCode.Replace("SEK-", ""));
                    return "SEK-" + (intValue + 1).ToString("00000");
                }
                else
                    return "";
            }
        }

        public Sector Create(Sector sector)
        {
            if (context.Sectors.Where(x => x.Identifier != null && x.Identifier == sector.Identifier).Count() == 0)
            {
                sector.Id = 0;

                sector.Code = GetNewCodeValue(sector.CompanyId ?? 0);
                sector.Active = true;

                sector.UpdatedAt = DateTime.Now;
                sector.CreatedAt = DateTime.Now;

                context.Sectors.Add(sector);
                return sector;
            }
            else
            {
                // Load Sector that will be updated
                Sector dbEntry = context.Sectors
                .FirstOrDefault(x => x.Identifier == sector.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CountryId = sector.CountryId ?? null;
                    dbEntry.CompanyId = sector.CompanyId ?? null;
                    dbEntry.CreatedById = sector.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = sector.Code;
                    dbEntry.SecondCode = sector.SecondCode;
                    dbEntry.Name = sector.Name;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public Sector Delete(Guid identifier)
        {
            // Load Remedy that will be deleted
            Sector dbEntry = context.Sectors
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
