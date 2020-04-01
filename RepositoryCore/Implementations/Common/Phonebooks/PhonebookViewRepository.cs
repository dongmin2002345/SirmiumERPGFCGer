using Configurator;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Common.Locations;
using DomainCore.Common.Phonebooks;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Common.Phonebooks;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.Phonebooks
{
    public class PhonebookViewRepository : IPhonebookRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        private string SelectString =
            "SELECT PhonebookId, PhonebookIdentifier, PhonebookCode, PhonebookName, " +
            "CountryId, CountryIdentifier, CountryCode, CountryName, " +
            "RegionId, RegionIdentifier, RegionCode, RegionName, " +
            "MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +
            "CityId, CityIdentifier, CityZipCode, CityName, " +
            "Address, " +
            "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
            "FROM vPhonebooks ";

        private Phonebook Read(SqlDataReader reader)
        {
            Phonebook Phonebook = new Phonebook();
            Phonebook.Id = Int32.Parse(reader["PhonebookId"].ToString());
            Phonebook.Identifier = Guid.Parse(reader["PhonebookIdentifier"].ToString());

            if (reader["PhonebookCode"] != DBNull.Value)
                Phonebook.Code = reader["PhonebookCode"].ToString();
            if (reader["PhonebookName"] != DBNull.Value)
                Phonebook.Name = reader["PhonebookName"].ToString();

            if (reader["CountryId"] != DBNull.Value)
            {
                Phonebook.Country = new Country();
                Phonebook.CountryId = Int32.Parse(reader["CountryId"].ToString());
                Phonebook.Country.Id = Int32.Parse(reader["CountryId"].ToString());
                Phonebook.Country.Identifier = Guid.Parse(reader["CountryIdentifier"].ToString());
                Phonebook.Country.Mark = reader["CountryCode"].ToString();
                Phonebook.Country.Name = reader["CountryName"].ToString();
            }

            if (reader["RegionId"] != DBNull.Value) //"RegionId, RegionIdentifier, RegionCode, RegionName, " +
            {
                Phonebook.Region = new Region();
                Phonebook.RegionId = Int32.Parse(reader["RegionId"].ToString());
                Phonebook.Region.Id = Int32.Parse(reader["RegionId"].ToString());
                Phonebook.Region.Identifier = Guid.Parse(reader["RegionIdentifier"].ToString());
                Phonebook.Region.RegionCode = reader["RegionCode"].ToString();
                Phonebook.Region.Name = reader["RegionName"].ToString();
            }

            if (reader["MunicipalityId"] != DBNull.Value) /*"MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +*/
            {
                Phonebook.Municipality = new Municipality();
                Phonebook.MunicipalityId = Int32.Parse(reader["MunicipalityId"].ToString());
                Phonebook.Municipality.Id = Int32.Parse(reader["MunicipalityId"].ToString());
                Phonebook.Municipality.Identifier = Guid.Parse(reader["MunicipalityIdentifier"].ToString());
                Phonebook.Municipality.MunicipalityCode = reader["MunicipalityCode"].ToString();
                Phonebook.Municipality.Name = reader["MunicipalityName"].ToString();
            }

            if (reader["CityId"] != DBNull.Value) // "CityId, CityIdentifier, CityZipCode, CityName, " +
            {
                Phonebook.City = new City();
                Phonebook.CityId = Int32.Parse(reader["CityId"].ToString());
                Phonebook.City.Id = Int32.Parse(reader["CityId"].ToString());
                Phonebook.City.Identifier = Guid.Parse(reader["CityIdentifier"].ToString());
                Phonebook.City.ZipCode = reader["CityZipCode"].ToString();
                Phonebook.City.Name = reader["CityName"].ToString();
            }

            if (reader["Address"] != DBNull.Value)
                Phonebook.Address = reader["Address"].ToString();

            Phonebook.Active = bool.Parse(reader["Active"].ToString());
            Phonebook.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

            if (reader["CreatedById"] != DBNull.Value)
            {
                Phonebook.CreatedBy = new User();
                Phonebook.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                Phonebook.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                Phonebook.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                Phonebook.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
            }

            if (reader["CompanyId"] != DBNull.Value)
            {
                Phonebook.Company = new Company();
                Phonebook.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                Phonebook.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                Phonebook.Company.Name = reader["CompanyName"].ToString();
            }

            return Phonebook;
        }

        public PhonebookViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<Phonebook> GetPhonebooks(int companyId)
        {
            List<Phonebook> Phonebooks = new List<Phonebook>();

            string queryString =
                SelectString +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Phonebook Phonebook;
                    while (reader.Read())
                    {
                        Phonebook = Read(reader);
                        Phonebooks.Add(Phonebook);
                    }
                }
            }

            return Phonebooks;
        }

        public List<Phonebook> GetPhonebooksNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<Phonebook> Phonebooks = new List<Phonebook>();

            string queryString =
                SelectString +
                "WHERE CompanyId = @CompanyId AND " +
                "CONVERT(DATETIME, CONVERT(VARCHAR(20), UpdatedAt, 120)) > CONVERT(DATETIME, CONVERT(VARCHAR(20), @LastUpdateTime, 120));";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));
                command.Parameters.Add(new SqlParameter("@LastUpdateTime", lastUpdateTime));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Phonebook Phonebook;
                    while (reader.Read())
                    {
                        Phonebook = Read(reader);
                        Phonebooks.Add(Phonebook);
                    }
                }
            }

            return Phonebooks;
        }

        public Phonebook GetPhonebook(int PhonebookId)
        {
            Phonebook Phonebook = null;

            string queryString =
                SelectString +
                "WHERE PhonebookId = @PhonebookId;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@PhonebookId", PhonebookId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Phonebook = Read(reader);
                    }
                }
            }

            return Phonebook;
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.Phonebooks
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Phonebook))
                    .Select(x => x.Entity as Phonebook))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "1";
            else
            {
                string activeCode = context.Phonebooks
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Phonebook))
                        .Select(x => x.Entity as Phonebook))
                    .Where(x => x.CompanyId == companyId)
                    .OrderByDescending(x => x.Id).FirstOrDefault()
                    .Code;
                if (!String.IsNullOrEmpty(activeCode))
                {
                    int intValue = Int32.Parse(activeCode);
                    return (intValue + 1).ToString();
                }
                else
                    return count.ToString();
            }
        }

        public Phonebook Create(Phonebook Phonebook)
        {
            if (context.Phonebooks.Where(x => x.Identifier != null && x.Identifier == Phonebook.Identifier).Count() == 0)
            {
                Phonebook.Id = 0;

                Phonebook.Code = GetNewCodeValue(Phonebook.CompanyId ?? 0);
                Phonebook.Active = true;

                Phonebook.UpdatedAt = DateTime.Now;
                Phonebook.CreatedAt = DateTime.Now;

                context.Phonebooks.Add(Phonebook);
                return Phonebook;
            }
            else
            {
                // Load Phonebook that will be updated
                Phonebook dbEntry = context.Phonebooks
                    .FirstOrDefault(x => x.Identifier == Phonebook.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = Phonebook.CompanyId ?? null;
                    dbEntry.CreatedById = Phonebook.CreatedById ?? null;
                    dbEntry.CountryId = Phonebook.CountryId;
                    dbEntry.RegionId = Phonebook.RegionId;
                    dbEntry.MunicipalityId = Phonebook.MunicipalityId ?? null;
                    dbEntry.CityId = Phonebook.CityId ?? null;
                    // Set properties
                    dbEntry.Code = Phonebook.Code;
                    dbEntry.Name = Phonebook.Name;
                    dbEntry.Address = Phonebook.Address;
                    
                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public Phonebook Delete(Guid identifier)
        {
            // Load item that will be deleted
            Phonebook dbEntry = context.Phonebooks
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
