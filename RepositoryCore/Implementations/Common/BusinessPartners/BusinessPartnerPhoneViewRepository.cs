using Configurator;
using DomainCore.Common.BusinessPartners;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Common.BusinessPartners;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.BusinessPartners
{
    public class BusinessPartnerPhoneViewRepository : IBusinessPartnerPhoneRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        public BusinessPartnerPhoneViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public string selectString =
            "SELECT BusinessPartnerPhoneId, BusinessPartnerPhoneIdentifier, " +
                            "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
                            "Phone, Mobile, Fax, Email, ContactPersonFirstName, ContactPersonLastName, Birthday, Description, Path, ItemStatus, " +
                            "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
            "FROM vBusinessPartnerPhones ";

        private static BusinessPartnerPhone Read(SqlDataReader reader)
        {
            BusinessPartnerPhone businessPartnerPhone = new BusinessPartnerPhone();
            businessPartnerPhone.Id = Int32.Parse(reader["BusinessPartnerPhoneId"].ToString());
            businessPartnerPhone.Identifier = Guid.Parse(reader["BusinessPartnerPhoneIdentifier"].ToString());

            if (reader["BusinessPartnerId"] != DBNull.Value)
            {
                businessPartnerPhone.BusinessPartner = new BusinessPartner();
                businessPartnerPhone.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                businessPartnerPhone.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                businessPartnerPhone.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                businessPartnerPhone.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                businessPartnerPhone.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
            }

            if (reader["Phone"] != DBNull.Value)
                businessPartnerPhone.Phone = reader["Phone"].ToString();
            if (reader["Mobile"] != DBNull.Value)
                businessPartnerPhone.Mobile = reader["Mobile"].ToString();
            if (reader["Fax"] != DBNull.Value)
                businessPartnerPhone.Fax = reader["Fax"].ToString();
            if (reader["Email"] != DBNull.Value)
                businessPartnerPhone.Email = reader["Email"].ToString();

            if (reader["ContactPersonFirstName"] != DBNull.Value)
                businessPartnerPhone.ContactPersonFirstName = reader["ContactPersonFirstName"].ToString();
            if (reader["ContactPersonLastName"] != DBNull.Value)
                businessPartnerPhone.ContactPersonLastName = reader["ContactPersonLastName"].ToString();

            if (reader["Birthday"] != DBNull.Value)
                businessPartnerPhone.Birthday = DateTime.Parse(reader["Birthday"].ToString());

            if (reader["Description"] != DBNull.Value)
                businessPartnerPhone.Description = reader["Description"].ToString();
            if (reader["Path"] != DBNull.Value)
                businessPartnerPhone.Path = reader["Path"].ToString();
            if (reader["ItemStatus"] != DBNull.Value)
                businessPartnerPhone.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());
            businessPartnerPhone.Active = bool.Parse(reader["Active"].ToString());
            businessPartnerPhone.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

            if (reader["CreatedById"] != DBNull.Value)
            {
                businessPartnerPhone.CreatedBy = new User();
                businessPartnerPhone.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                businessPartnerPhone.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                businessPartnerPhone.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                businessPartnerPhone.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
            }

            if (reader["CompanyId"] != DBNull.Value)
            {
                businessPartnerPhone.Company = new Company();
                businessPartnerPhone.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                businessPartnerPhone.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                businessPartnerPhone.Company.Name = reader["CompanyName"].ToString();
            }

            return businessPartnerPhone;
        }

        public List<BusinessPartnerPhone> GetBusinessPartnerPhones(int companyId)
        {
            List<BusinessPartnerPhone> BusinessPartnerPhones = new List<BusinessPartnerPhone>();

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
                    BusinessPartnerPhone BusinessPartnerPhone;
                    while (reader.Read())
                    {
                        BusinessPartnerPhone = Read(reader);
                        BusinessPartnerPhones.Add(BusinessPartnerPhone);
                    }
                }
            }

            return BusinessPartnerPhones;
        }

        public List<BusinessPartnerPhone> GetBusinessPartnerPhonesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<BusinessPartnerPhone> BusinessPartnerPhones = new List<BusinessPartnerPhone>();

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
                    BusinessPartnerPhone BusinessPartnerPhone;
                    while (reader.Read())
                    {
                        BusinessPartnerPhone = Read(reader);
                        BusinessPartnerPhones.Add(BusinessPartnerPhone);
                    }
                }
            }

            return BusinessPartnerPhones;
        }


        public List<BusinessPartnerPhone> GetBusinessPartnerPhonesByBusinessPartner(int businessPartnerId)
        {
            List<BusinessPartnerPhone> BusinessPartnerPhones = new List<BusinessPartnerPhone>();

            string queryString =
                selectString +
                "WHERE BusinessPartnerId = @BusinessPartnerId;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@BusinessPartnerId", businessPartnerId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    BusinessPartnerPhone BusinessPartnerPhone;
                    while (reader.Read())
                    {
                        BusinessPartnerPhone = Read(reader);
                        BusinessPartnerPhones.Add(BusinessPartnerPhone);
                    }
                }
            }

            return BusinessPartnerPhones;
        }

        public BusinessPartnerPhone GetBusinessPartnerPhone(int id)
        {
            BusinessPartnerPhone BusinessPartnerPhone = null;

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
                        BusinessPartnerPhone = Read(reader);
                    }
                }
            }
            return BusinessPartnerPhone;
        }

        public BusinessPartnerPhone Create(BusinessPartnerPhone businessPartnerPhone)
        {
            if (context.BusinessPartnerPhones.Where(x => x.Identifier != null && x.Identifier == businessPartnerPhone.Identifier).Count() == 0)
            {
                businessPartnerPhone.Id = 0;

                businessPartnerPhone.Active = true;
                businessPartnerPhone.UpdatedAt = DateTime.Now;
                businessPartnerPhone.CreatedAt = DateTime.Now;

                context.BusinessPartnerPhones.Add(businessPartnerPhone);
                return businessPartnerPhone;
            }
            else
            {
                // Load businessPartnerPhone that will be updated
                BusinessPartnerPhone dbEntry = context.BusinessPartnerPhones
                    .FirstOrDefault(x => x.Identifier == businessPartnerPhone.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.BusinessPartnerId = businessPartnerPhone.BusinessPartnerId ?? null;
                    dbEntry.CompanyId = businessPartnerPhone.CompanyId ?? null;
                    dbEntry.CreatedById = businessPartnerPhone.CreatedById ?? null;

                    // Set properties
                    dbEntry.Phone = businessPartnerPhone.Phone;
                    dbEntry.Mobile = businessPartnerPhone.Mobile;
                    dbEntry.Fax = businessPartnerPhone.Fax;
                    dbEntry.Email = businessPartnerPhone.Email;
                    dbEntry.ContactPersonFirstName = businessPartnerPhone.ContactPersonFirstName;
                    dbEntry.ContactPersonLastName = businessPartnerPhone.ContactPersonLastName;
                    dbEntry.Birthday = businessPartnerPhone.Birthday;

                    dbEntry.Description = businessPartnerPhone.Description;
                    dbEntry.Path = businessPartnerPhone.Path;
                    dbEntry.ItemStatus = businessPartnerPhone.ItemStatus;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public BusinessPartnerPhone Delete(Guid identifier)
        {
            BusinessPartnerPhone dbEntry = context.BusinessPartnerPhones
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(BusinessPartnerPhone))
                    .Select(x => x.Entity as BusinessPartnerPhone))
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

