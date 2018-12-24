using Configurator;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Common.Locations;
using DomainCore.Employees;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Employees;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Employees
{
    public class LicenceTypeViewRepository : ILicenceTypeRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        public LicenceTypeViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<LicenceType> GetLicenceTypes(int companyId)
        {
            List<LicenceType> LicenceTypes = new List<LicenceType>();

            string queryString =
                "SELECT LicenceTypeId, LicenceTypeIdentifier, LicenceTypeCode, Category, Description, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vLicenceTypes " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    LicenceType licenceType;
                    while (reader.Read())
                    {
                        licenceType = new LicenceType();
                        licenceType.Id = Int32.Parse(reader["LicenceTypeId"].ToString());
                        licenceType.Identifier = Guid.Parse(reader["LicenceTypeIdentifier"].ToString());
                        licenceType.Code = reader["LicenceTypeCode"].ToString();
                        licenceType.Category = reader["Category"]?.ToString();
                        licenceType.Description = reader["Description"]?.ToString();

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            licenceType.Country = new Country();
                            licenceType.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            licenceType.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            licenceType.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            licenceType.Country.Code = reader["CountryCode"].ToString();
                            licenceType.Country.Name = reader["CountryName"].ToString();
                        }

                        licenceType.Active = bool.Parse(reader["Active"].ToString());
                        licenceType.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            licenceType.CreatedBy = new User();
                            licenceType.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            licenceType.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            licenceType.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            licenceType.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            licenceType.Company = new Company();
                            licenceType.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            licenceType.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            licenceType.Company.Name = reader["CompanyName"].ToString();
                        }

                        LicenceTypes.Add(licenceType);
                    }
                }
            }
            return LicenceTypes;

            //List<LicenceType> licenceTypes = context.LicenceTypes
            //    .Include(x => x.Country)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .OrderByDescending(x => x.CreatedAt)
            //    .AsNoTracking()
            //    .ToList();

            //return licenceTypes;
        }

        public List<LicenceType> GetLicenceTypesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<LicenceType> LicenceTypes = new List<LicenceType>();

            string queryString =
                "SELECT LicenceTypeId, LicenceTypeIdentifier, LicenceTypeCode, Category, Description, " +
                "CountryId, CountryIdentifier, CountryCode, CountryName, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vLicenceTypes " +
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
                    LicenceType licenceType;
                    while (reader.Read())
                    {
                        licenceType = new LicenceType();
                        licenceType.Id = Int32.Parse(reader["LicenceTypeId"].ToString());
                        licenceType.Identifier = Guid.Parse(reader["LicenceTypeIdentifier"].ToString());
                        licenceType.Code = reader["LicenceTypeCode"].ToString();
                        licenceType.Category = reader["Category"]?.ToString();
                        licenceType.Description = reader["Description"]?.ToString();

                        if (reader["CountryId"] != DBNull.Value)
                        {
                            licenceType.Country = new Country();
                            licenceType.CountryId = Int32.Parse(reader["CountryId"].ToString());
                            licenceType.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                            licenceType.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                            licenceType.Country.Code = reader["CountryCode"].ToString();
                            licenceType.Country.Name = reader["CountryName"].ToString();
                        }

                        licenceType.Active = bool.Parse(reader["Active"].ToString());
                        licenceType.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            licenceType.CreatedBy = new User();
                            licenceType.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            licenceType.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            licenceType.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            licenceType.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            licenceType.Company = new Company();
                            licenceType.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            licenceType.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            licenceType.Company.Name = reader["CompanyName"].ToString();
                        }

                        LicenceTypes.Add(licenceType);
                    }
                }
            }
            return LicenceTypes;

            //List<LicenceType> licenceTypes = context.LicenceTypes
            //    .Include(x => x.Country)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .OrderByDescending(x => x.UpdatedAt)
            //    .AsNoTracking()
            //    .ToList();

            //return licenceTypes;
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.LicenceTypes
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(LicenceType))
                    .Select(x => x.Entity as LicenceType))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "LIC-00001";
            else
            {
                string activeCode = context.LicenceTypes
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(LicenceType))
                        .Select(x => x.Entity as LicenceType))
                    .Where(x => x.CompanyId == companyId)
                    .OrderByDescending(x => x.Id).FirstOrDefault()
                    .Code;
                if (!String.IsNullOrEmpty(activeCode))
                {
                    int intValue = Int32.Parse(activeCode.Replace("LIC-", ""));
                    return "LIC-" + (intValue + 1).ToString("00000");
                }
                else
                    return "";
            }
        }

        public LicenceType Create(LicenceType licenceType)
        {
            if (context.LicenceTypes.Where(x => x.Identifier != null && x.Identifier == licenceType.Identifier).Count() == 0)
            {
                licenceType.Id = 0;

                licenceType.Code = GetNewCodeValue(licenceType.CompanyId ?? 0);
                licenceType.Active = true;

                licenceType.UpdatedAt = DateTime.Now;
                licenceType.CreatedAt = DateTime.Now;

                context.LicenceTypes.Add(licenceType);
                return licenceType;
            }
            else
            {
                // Load Sector that will be updated
                LicenceType dbEntry = context.LicenceTypes
                .FirstOrDefault(x => x.Identifier == licenceType.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CountryId = licenceType.CountryId ?? null;
                    dbEntry.CompanyId = licenceType.CompanyId ?? null;
                    dbEntry.CreatedById = licenceType.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = licenceType.Code;
                    dbEntry.Category = licenceType.Category;

                    dbEntry.Description = licenceType.Description;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public LicenceType Delete(Guid identifier)
        {
            // Load Remedy that will be deleted
            LicenceType dbEntry = context.LicenceTypes
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
