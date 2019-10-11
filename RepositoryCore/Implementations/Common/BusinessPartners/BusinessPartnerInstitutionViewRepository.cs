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
    public class BusinessPartnerInstitutionViewRepository : IBusinessPartnerInstitutionRepository
    {
        ApplicationDbContext context;
        private string connectionString;

        public BusinessPartnerInstitutionViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<BusinessPartnerInstitution> GetBusinessPartnerInstitutions(int companyId)
        {
            List<BusinessPartnerInstitution> BusinessPartnerInstitutions = new List<BusinessPartnerInstitution>();

            string queryString =
                "SELECT BusinessPartnerInstitutionId, BusinessPartnerInstitutionIdentifier, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
                "Institution, Username, Password, ContactPerson, Phone, Fax, Email, ItemStatus, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vBusinessPartnerInstitutions " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    BusinessPartnerInstitution businessPartnerInstitution;
                    while (reader.Read())
                    {
                        businessPartnerInstitution = new BusinessPartnerInstitution();
                        businessPartnerInstitution.Id = Int32.Parse(reader["BusinessPartnerInstitutionId"].ToString());
                        businessPartnerInstitution.Identifier = Guid.Parse(reader["BusinessPartnerInstitutionIdentifier"].ToString());

                        if (reader["BusinessPartnerId"] != DBNull.Value)
                        {
                            businessPartnerInstitution.BusinessPartner = new BusinessPartner();
                            businessPartnerInstitution.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerInstitution.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerInstitution.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                            businessPartnerInstitution.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            businessPartnerInstitution.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                        }

                        if (reader["Institution"] != DBNull.Value)
                            businessPartnerInstitution.Institution = reader["Institution"].ToString();
                        if (reader["Username"] != DBNull.Value)
                            businessPartnerInstitution.Username = reader["Username"].ToString();
                        if (reader["Password"] != DBNull.Value)
                            businessPartnerInstitution.Password = reader["Password"].ToString();
                        if (reader["ContactPerson"] != DBNull.Value)
                            businessPartnerInstitution.ContactPerson = reader["ContactPerson"].ToString();
                        if (reader["Phone"] != DBNull.Value)
                            businessPartnerInstitution.Phone = reader["Phone"].ToString();
                        if (reader["Fax"] != DBNull.Value)
                            businessPartnerInstitution.Fax = reader["Fax"].ToString();
                        if (reader["Email"] != DBNull.Value)
                            businessPartnerInstitution.Email = reader["Email"].ToString();
                        if (reader["ItemStatus"] != DBNull.Value)
                            businessPartnerInstitution.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());
                        businessPartnerInstitution.Active = bool.Parse(reader["Active"].ToString());
                        businessPartnerInstitution.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            businessPartnerInstitution.CreatedBy = new User();
                            businessPartnerInstitution.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerInstitution.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerInstitution.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            businessPartnerInstitution.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            businessPartnerInstitution.Company = new Company();
                            businessPartnerInstitution.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerInstitution.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerInstitution.Company.Name = reader["CompanyName"].ToString();
                        }

                        BusinessPartnerInstitutions.Add(businessPartnerInstitution);
                    }
                }
            }
            return BusinessPartnerInstitutions;

            //List<BusinessPartnerInstitution> businessPartnerInstitutions = context.BusinessPartnerInstitutions
            //    .Include(x => x.BusinessPartner)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .AsNoTracking()
            //    .ToList();

            //return businessPartnerInstitutions;
        }

        public List<BusinessPartnerInstitution> GetBusinessPartnerInstitutionsByBusinessPartner(int businessPartnerId)
        {
            List<BusinessPartnerInstitution> BusinessPartnerInstitutions = new List<BusinessPartnerInstitution>();

            string queryString =
                "SELECT BusinessPartnerInstitutionId, BusinessPartnerInstitutionIdentifier, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
                "Institution, Username, Password, ContactPerson, Phone, Fax, Email, ItemStatus, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vBusinessPartnerInstitutions " +
                "WHERE BusinessPartnerId = @BusinessPartnerId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@BusinessPartnerId", businessPartnerId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    BusinessPartnerInstitution businessPartnerInstitution;
                    while (reader.Read())
                    {
                        businessPartnerInstitution = new BusinessPartnerInstitution();
                        businessPartnerInstitution.Id = Int32.Parse(reader["BusinessPartnerInstitutionId"].ToString());
                        businessPartnerInstitution.Identifier = Guid.Parse(reader["BusinessPartnerInstitutionIdentifier"].ToString());

                        if (reader["BusinessPartnerId"] != DBNull.Value)
                        {
                            businessPartnerInstitution.BusinessPartner = new BusinessPartner();
                            businessPartnerInstitution.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerInstitution.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerInstitution.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                            businessPartnerInstitution.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            businessPartnerInstitution.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                        }

                        if (reader["Institution"] != DBNull.Value)
                            businessPartnerInstitution.Institution = reader["Institution"].ToString();
                        if (reader["Username"] != DBNull.Value)
                            businessPartnerInstitution.Username = reader["Username"].ToString();
                        if (reader["Password"] != DBNull.Value)
                            businessPartnerInstitution.Password = reader["Password"].ToString();
                        if (reader["ContactPerson"] != DBNull.Value)
                            businessPartnerInstitution.ContactPerson = reader["ContactPerson"].ToString();
                        if (reader["Phone"] != DBNull.Value)
                            businessPartnerInstitution.Phone = reader["Phone"].ToString();
                        if (reader["Fax"] != DBNull.Value)
                            businessPartnerInstitution.Fax = reader["Fax"].ToString();
                        if (reader["Email"] != DBNull.Value)
                            businessPartnerInstitution.Email = reader["Email"].ToString();
                        if (reader["ItemStatus"] != DBNull.Value)
                            businessPartnerInstitution.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());
                        businessPartnerInstitution.Active = bool.Parse(reader["Active"].ToString());
                        businessPartnerInstitution.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            businessPartnerInstitution.CreatedBy = new User();
                            businessPartnerInstitution.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerInstitution.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerInstitution.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            businessPartnerInstitution.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            businessPartnerInstitution.Company = new Company();
                            businessPartnerInstitution.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerInstitution.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerInstitution.Company.Name = reader["CompanyName"].ToString();
                        }

                        BusinessPartnerInstitutions.Add(businessPartnerInstitution);
                    }
                }
            }
            return BusinessPartnerInstitutions;

            //List<BusinessPartnerInstitution> businessPartnerInstitutions = context.BusinessPartnerInstitutions
            //    .Include(x => x.BusinessPartner)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.BusinessPartnerId == businessPartnerId && x.Active == true)
            //    .AsNoTracking()
            //    .ToList();

            //return businessPartnerInstitutions;
        }

        public List<BusinessPartnerInstitution> GetBusinessPartnerInstitutionsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<BusinessPartnerInstitution> BusinessPartnerInstitutions = new List<BusinessPartnerInstitution>();

            string queryString =
                "SELECT BusinessPartnerInstitutionId, BusinessPartnerInstitutionIdentifier, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
                "Institution, Username, Password, ContactPerson, Phone, Fax, Email, ItemStatus, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vBusinessPartnerInstitutions " +
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
                    BusinessPartnerInstitution businessPartnerInstitution;
                    while (reader.Read())
                    {
                        businessPartnerInstitution = new BusinessPartnerInstitution();
                        businessPartnerInstitution.Id = Int32.Parse(reader["BusinessPartnerInstitutionId"].ToString());
                        businessPartnerInstitution.Identifier = Guid.Parse(reader["BusinessPartnerInstitutionIdentifier"].ToString());

                        if (reader["BusinessPartnerId"] != DBNull.Value)
                        {
                            businessPartnerInstitution.BusinessPartner = new BusinessPartner();
                            businessPartnerInstitution.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerInstitution.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerInstitution.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                            businessPartnerInstitution.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            businessPartnerInstitution.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                        }

                        if (reader["Institution"] != DBNull.Value)
                            businessPartnerInstitution.Institution = reader["Institution"].ToString();
                        if (reader["Username"] != DBNull.Value)
                            businessPartnerInstitution.Username = reader["Username"].ToString();
                        if (reader["Password"] != DBNull.Value)
                            businessPartnerInstitution.Password = reader["Password"].ToString();
                        if (reader["ContactPerson"] != DBNull.Value)
                            businessPartnerInstitution.ContactPerson = reader["ContactPerson"].ToString();
                        if (reader["Phone"] != DBNull.Value)
                            businessPartnerInstitution.Phone = reader["Phone"].ToString();
                        if (reader["Fax"] != DBNull.Value)
                            businessPartnerInstitution.Fax = reader["Fax"].ToString();
                        if (reader["Email"] != DBNull.Value)
                            businessPartnerInstitution.Email = reader["Email"].ToString();
                        if (reader["ItemStatus"] != DBNull.Value)
                            businessPartnerInstitution.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());
                        businessPartnerInstitution.Active = bool.Parse(reader["Active"].ToString());
                        businessPartnerInstitution.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            businessPartnerInstitution.CreatedBy = new User();
                            businessPartnerInstitution.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerInstitution.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerInstitution.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            businessPartnerInstitution.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            businessPartnerInstitution.Company = new Company();
                            businessPartnerInstitution.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerInstitution.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerInstitution.Company.Name = reader["CompanyName"].ToString();
                        }

                        BusinessPartnerInstitutions.Add(businessPartnerInstitution);
                    }
                }
            }
            return BusinessPartnerInstitutions;

            //List<BusinessPartnerInstitution> businessPartnerInstitutions = context.BusinessPartnerInstitutions
            //    .Include(x => x.BusinessPartner)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .AsNoTracking()
            //    .ToList();

            //return businessPartnerInstitutions;
        }

        public BusinessPartnerInstitution GetBusinessPartnerInstitution(int id)
        {
            BusinessPartnerInstitution businessPartnerInstitution = new BusinessPartnerInstitution();

            string queryString =
                "SELECT BusinessPartnerInstitutionId, BusinessPartnerInstitutionIdentifier, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
                "Institution, Username, Password, ContactPerson, Phone, Fax, Email, ItemStatus, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
                "FROM vBusinessPartnerInstitutions " +
                "WHERE BusinessPartnerInstitutionId = @BusinessPartnerInstitutionId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@BusinessPartnerInstitutionId", id));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        businessPartnerInstitution = new BusinessPartnerInstitution();
                        businessPartnerInstitution.Id = Int32.Parse(reader["BusinessPartnerInstitutionId"].ToString());
                        businessPartnerInstitution.Identifier = Guid.Parse(reader["BusinessPartnerInstitutionIdentifier"].ToString());

                        if (reader["BusinessPartnerId"] != DBNull.Value)
                        {
                            businessPartnerInstitution.BusinessPartner = new BusinessPartner();
                            businessPartnerInstitution.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerInstitution.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerInstitution.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                            businessPartnerInstitution.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            businessPartnerInstitution.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                        }

                        if (reader["Institution"] != DBNull.Value)
                            businessPartnerInstitution.Institution = reader["Institution"].ToString();
                        if (reader["Username"] != DBNull.Value)
                            businessPartnerInstitution.Username = reader["Username"].ToString();
                        if (reader["Password"] != DBNull.Value)
                            businessPartnerInstitution.Password = reader["Password"].ToString();
                        if (reader["ContactPerson"] != DBNull.Value)
                            businessPartnerInstitution.ContactPerson = reader["ContactPerson"].ToString();
                        if (reader["Phone"] != DBNull.Value)
                            businessPartnerInstitution.Phone = reader["Phone"].ToString();
                        if (reader["Fax"] != DBNull.Value)
                            businessPartnerInstitution.Fax = reader["Fax"].ToString();
                        if (reader["Email"] != DBNull.Value)
                            businessPartnerInstitution.Email = reader["Email"].ToString();
                        if (reader["ItemStatus"] != DBNull.Value)
                            businessPartnerInstitution.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());
                        businessPartnerInstitution.Active = bool.Parse(reader["Active"].ToString());
                        businessPartnerInstitution.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            businessPartnerInstitution.CreatedBy = new User();
                            businessPartnerInstitution.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerInstitution.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerInstitution.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            businessPartnerInstitution.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            businessPartnerInstitution.Company = new Company();
                            businessPartnerInstitution.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerInstitution.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerInstitution.Company.Name = reader["CompanyName"].ToString();
                        }

                    }

                }
            }
            return businessPartnerInstitution;

            //return context.BusinessPartnerInstitutions
            //    .Include(x => x.BusinessPartner)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .FirstOrDefault(x => x.Id == id && x.Active == true);
        }

        public BusinessPartnerInstitution Create(BusinessPartnerInstitution businessPartnerInstitution)
        {
            if (context.BusinessPartnerInstitutions.Where(x => x.Identifier != null && x.Identifier == businessPartnerInstitution.Identifier).Count() == 0)
            {
                businessPartnerInstitution.Id = 0;

                businessPartnerInstitution.Active = true;

                businessPartnerInstitution.CreatedAt = DateTime.Now;
                businessPartnerInstitution.UpdatedAt = DateTime.Now;

                context.BusinessPartnerInstitutions.Add(businessPartnerInstitution);
                return businessPartnerInstitution;
            }
            else
            {
                // Load businessPartnerInstitution that will be updated
                BusinessPartnerInstitution dbEntry = context.BusinessPartnerInstitutions
                    .FirstOrDefault(x => x.Identifier == businessPartnerInstitution.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.BusinessPartnerId = businessPartnerInstitution.BusinessPartnerId ?? null;
                    dbEntry.CompanyId = businessPartnerInstitution.CompanyId ?? null;
                    dbEntry.CreatedById = businessPartnerInstitution.CreatedById ?? null;

                    // Set properties
                    dbEntry.Institution = businessPartnerInstitution.Institution;
                    dbEntry.Username = businessPartnerInstitution.Username;
                    dbEntry.Password = businessPartnerInstitution.Password;
                    dbEntry.ContactPerson = businessPartnerInstitution.ContactPerson;
                    dbEntry.Phone = businessPartnerInstitution.Phone;
                    dbEntry.Fax = businessPartnerInstitution.Fax;
                    dbEntry.Email = businessPartnerInstitution.Email;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public BusinessPartnerInstitution Delete(Guid identifier)
        {
            BusinessPartnerInstitution dbEntry = context.BusinessPartnerInstitutions
               .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(BusinessPartnerInstitution))
                    .Select(x => x.Entity as BusinessPartnerInstitution))
                .FirstOrDefault(x => x.Identifier == identifier);

            if (dbEntry != null)
            {
                dbEntry.Active = false;
                dbEntry.UpdatedAt = DateTime.Now;
            }
            return dbEntry;
        }
    }
}
