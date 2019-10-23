using Configurator;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
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
    public class PhonebookPhoneViewRepository: IPhonebookPhoneRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        public PhonebookPhoneViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public string selectString =
            "SELECT PhonebookPhoneId, PhonebookPhoneIdentifier, " +
                            "PhonebookId, PhonebookIdentifier, PhonebookCode, PhonebookName, " +
                            "Name, SurName, PhoneNumber, Email, ItemStatus, " +
                            "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
            "FROM vPhonebookPhones ";

        private static PhonebookPhone Read(SqlDataReader reader)
        {
            PhonebookPhone PhonebookPhone = new PhonebookPhone();
            PhonebookPhone.Id = Int32.Parse(reader["PhonebookPhoneId"].ToString());
            PhonebookPhone.Identifier = Guid.Parse(reader["PhonebookPhoneIdentifier"].ToString());

            if (reader["PhonebookId"] != DBNull.Value)
            {
                PhonebookPhone.Phonebook = new Phonebook();
                PhonebookPhone.PhonebookId = Int32.Parse(reader["PhonebookId"].ToString());
                PhonebookPhone.Phonebook.Id = Int32.Parse(reader["PhonebookId"].ToString());
                PhonebookPhone.Phonebook.Identifier = Guid.Parse(reader["PhonebookIdentifier"].ToString());
                PhonebookPhone.Phonebook.Code = reader["PhonebookCode"].ToString();
                PhonebookPhone.Phonebook.Name = reader["PhonebookName"].ToString();
            }


            if (reader["Name"] != DBNull.Value)
                PhonebookPhone.Name = reader["Name"].ToString();
            if (reader["SurName"] != DBNull.Value)
                PhonebookPhone.SurName = reader["SurName"].ToString();

            if (reader["PhoneNumber"] != DBNull.Value)
                PhonebookPhone.PhoneNumber = reader["PhoneNumber"].ToString();

            if (reader["Email"] != DBNull.Value)
                PhonebookPhone.Email = reader["Email"].ToString();
            
            if (reader["ItemStatus"] != DBNull.Value)
                PhonebookPhone.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());
            PhonebookPhone.Active = bool.Parse(reader["Active"].ToString());
            PhonebookPhone.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

            if (reader["CreatedById"] != DBNull.Value)
            {
                PhonebookPhone.CreatedBy = new User();
                PhonebookPhone.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                PhonebookPhone.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                PhonebookPhone.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                PhonebookPhone.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
            }

            if (reader["CompanyId"] != DBNull.Value)
            {
                PhonebookPhone.Company = new Company();
                PhonebookPhone.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                PhonebookPhone.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                PhonebookPhone.Company.Name = reader["CompanyName"].ToString();
            }

            return PhonebookPhone;
        }

        public List<PhonebookPhone> GetPhonebookPhones(int companyId)
        {
            List<PhonebookPhone> PhonebookPhones = new List<PhonebookPhone>();

            string queryString =
                selectString +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    PhonebookPhone PhonebookPhone;
                    while (reader.Read())
                    {
                        PhonebookPhone = Read(reader);
                        PhonebookPhones.Add(PhonebookPhone);
                    }
                }
            }

            return PhonebookPhones;
        }

        public List<PhonebookPhone> GetPhonebookPhonesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<PhonebookPhone> PhonebookPhones = new List<PhonebookPhone>();

            string queryString =
                selectString +
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
                    PhonebookPhone PhonebookPhone;
                    while (reader.Read())
                    {
                        PhonebookPhone = Read(reader);
                        PhonebookPhones.Add(PhonebookPhone);
                    }
                }
            }

            return PhonebookPhones;
        }


        public List<PhonebookPhone> GetPhonebookPhonesByPhonebook(int PhonebookId)
        {
            List<PhonebookPhone> PhonebookPhones = new List<PhonebookPhone>();

            string queryString =
                selectString +
                "WHERE PhonebookId = @PhonebookId;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@PhonebookId", PhonebookId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    PhonebookPhone PhonebookPhone;
                    while (reader.Read())
                    {
                        PhonebookPhone = Read(reader);
                        PhonebookPhones.Add(PhonebookPhone);
                    }
                }
            }

            return PhonebookPhones;
        }

        public PhonebookPhone GetPhonebookPhone(int id)
        {
            PhonebookPhone PhonebookPhone = null;

            string queryString =
                selectString +
                "WHERE Id = @Id;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@Id", id));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        PhonebookPhone = Read(reader);
                    }
                }
            }
            return PhonebookPhone;
        }

        public PhonebookPhone Create(PhonebookPhone PhonebookPhone)
        {
            if (context.PhonebookPhones.Where(x => x.Identifier != null && x.Identifier == PhonebookPhone.Identifier).Count() == 0)
            {
                PhonebookPhone.Id = 0;

                PhonebookPhone.Active = true;
                PhonebookPhone.UpdatedAt = DateTime.Now;
                PhonebookPhone.CreatedAt = DateTime.Now;

                context.PhonebookPhones.Add(PhonebookPhone);
                return PhonebookPhone;
            }
            else
            {
                // Load PhonebookPhone that will be updated
                PhonebookPhone dbEntry = context.PhonebookPhones
                    .FirstOrDefault(x => x.Identifier == PhonebookPhone.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.PhonebookId = PhonebookPhone.PhonebookId ?? null;
                    dbEntry.CompanyId = PhonebookPhone.CompanyId ?? null;
                    dbEntry.CreatedById = PhonebookPhone.CreatedById ?? null;

                    // Set properties
                   
                    dbEntry.Email = PhonebookPhone.Email;
                    dbEntry.Name = PhonebookPhone.Name;
                    dbEntry.SurName = PhonebookPhone.SurName;
                    dbEntry.PhoneNumber = PhonebookPhone.PhoneNumber;

                    dbEntry.ItemStatus = PhonebookPhone.ItemStatus;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public PhonebookPhone Delete(Guid identifier)
        {
            PhonebookPhone dbEntry = context.PhonebookPhones
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(PhonebookPhone))
                    .Select(x => x.Entity as PhonebookPhone))
                .FirstOrDefault(x => x.Identifier == identifier && x.Active == true);

            if (dbEntry != null)
            {
                dbEntry.Active = false;
                dbEntry.UpdatedAt = DateTime.Now;
            }
            return dbEntry;
        }
    }
}
