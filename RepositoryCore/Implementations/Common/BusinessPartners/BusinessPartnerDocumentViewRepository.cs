using Configurator;
using DomainCore.Common.BusinessPartners;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using RepositoryCore.Abstractions.Common.BusinessPartners;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.BusinessPartners
{
    public class BusinessPartnerDocumentViewRepository : IBusinessPartnerDocumentRepository
    {
        ApplicationDbContext context;
        private string connectionString;

        public BusinessPartnerDocumentViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<BusinessPartnerDocument> GetBusinessPartnerDocuments(int companyId)
        {
            List<BusinessPartnerDocument> BusinessPartnerDocuments = new List<BusinessPartnerDocument>();

            string queryString =
                "SELECT BusinessPartnerDocumentId, BusinessPartnerDocumentIdentifier, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
                "Name, CreateDate, Path, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
                "CompanyId, CompanyName " +
                "FROM vBusinessPartnerDocuments " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    BusinessPartnerDocument businessPartnerDocument;
                    while (reader.Read())
                    {
                        businessPartnerDocument = new BusinessPartnerDocument();
                        businessPartnerDocument.Id = Int32.Parse(reader["BusinessPartnerDocumentId"].ToString());
                        businessPartnerDocument.Identifier = Guid.Parse(reader["BusinessPartnerDocumentIdentifier"].ToString());

                        if (reader["BusinessPartnerId"] != null)
                        {
                            businessPartnerDocument.BusinessPartner = new BusinessPartner();
                            businessPartnerDocument.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerDocument.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerDocument.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerIdentifier"].ToString());
                            businessPartnerDocument.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            businessPartnerDocument.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                        }

                        if (reader["Name"] != null)
                            businessPartnerDocument.Name = reader["Name"].ToString();
                        if (reader["CreateDate"] != null)
                            businessPartnerDocument.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                        if (reader["Path"] != null)
                            businessPartnerDocument.Path = reader["Path"].ToString();

                        businessPartnerDocument.Active = bool.Parse(reader["Active"].ToString());
                        businessPartnerDocument.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != null)
                        {
                            businessPartnerDocument.CreatedBy = new User();
                            businessPartnerDocument.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerDocument.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerDocument.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            businessPartnerDocument.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != null)
                        {
                            businessPartnerDocument.Company = new Company();
                            businessPartnerDocument.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerDocument.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerDocument.Company.Name = reader["CompanyName"].ToString();
                        }

                        BusinessPartnerDocuments.Add(businessPartnerDocument);
                    }
                }
            }
            return BusinessPartnerDocuments;

            //List<BusinessPartnerDocument> BusinessPartnerDocuments = context.BusinessPartnerDocuments
            //    .Include(x => x.BusinessPartner)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .AsNoTracking()
            //    .ToList();

            //return BusinessPartnerDocuments;
        }

        public List<BusinessPartnerDocument> GetBusinessPartnerDocumentsByBusinessPartner(int BusinessPartnerId)
        {
            List<BusinessPartnerDocument> BusinessPartnerDocuments = new List<BusinessPartnerDocument>();

            string queryString =
                "SELECT BusinessPartnerDocumentId, BusinessPartnerDocumentIdentifier, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
                "Name, CreateDate, Path, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
                "CompanyId, CompanyName " +
                "FROM vBusinessPartnerDocuments " +
                "WHERE BusinessPartnerId = @BusinessPartnerId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@BusinessPartnerId", BusinessPartnerId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    BusinessPartnerDocument businessPartnerDocument;
                    while (reader.Read())
                    {
                        businessPartnerDocument = new BusinessPartnerDocument();
                        businessPartnerDocument.Id = Int32.Parse(reader["BusinessPartnerDocumentId"].ToString());
                        businessPartnerDocument.Identifier = Guid.Parse(reader["BusinessPartnerDocumentIdentifier"].ToString());

                        if (reader["BusinessPartnerId"] != null)
                        {
                            businessPartnerDocument.BusinessPartner = new BusinessPartner();
                            businessPartnerDocument.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerDocument.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerDocument.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerDocumentTypeIdentifier"].ToString());
                            businessPartnerDocument.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            businessPartnerDocument.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                        }

                        if (reader["Name"] != null)
                            businessPartnerDocument.Name = reader["Name"].ToString();
                        if (reader["CreateDate"] != null)
                            businessPartnerDocument.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                        if (reader["Path"] != null)
                            businessPartnerDocument.Path = reader["Path"].ToString();

                        businessPartnerDocument.Active = bool.Parse(reader["Active"].ToString());
                        businessPartnerDocument.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != null)
                        {
                            businessPartnerDocument.CreatedBy = new User();
                            businessPartnerDocument.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerDocument.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerDocument.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            businessPartnerDocument.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != null)
                        {
                            businessPartnerDocument.Company = new Company();
                            businessPartnerDocument.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerDocument.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerDocument.Company.Name = reader["CompanyName"].ToString();
                        }

                        BusinessPartnerDocuments.Add(businessPartnerDocument);
                    }
                }
            }

            return BusinessPartnerDocuments;

            //List<BusinessPartnerDocument> BusinessPartners = context.BusinessPartnerDocuments
            //    .Include(x => x.BusinessPartner)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.BusinessPartnerId == BusinessPartnerId && x.Active == true)
            //    .AsNoTracking()
            //    .ToList();

            //return BusinessPartners;
        }

        public List<BusinessPartnerDocument> GetBusinessPartnerDocumentsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<BusinessPartnerDocument> BusinessPartnerDocuments = new List<BusinessPartnerDocument>();

            string queryString =
                "SELECT BusinessPartnerDocumentId, BusinessPartnerDocumentIdentifier, " +
                "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
                "Name, CreateDate, Path, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
                "CompanyId, CompanyName " +
                "FROM vBusinessPartnerDocuments " +
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
                    BusinessPartnerDocument businessPartnerDocument;
                    while (reader.Read())
                    {
                        businessPartnerDocument = new BusinessPartnerDocument();
                        businessPartnerDocument.Id = Int32.Parse(reader["BusinessPartnerDocumentId"].ToString());
                        businessPartnerDocument.Identifier = Guid.Parse(reader["BusinessPartnerDocumentIdentifier"].ToString());

                        if (reader["BusinessPartnerId"] != null)
                        {
                            businessPartnerDocument.BusinessPartner = new BusinessPartner();
                            businessPartnerDocument.BusinessPartnerId = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerDocument.BusinessPartner.Id = Int32.Parse(reader["BusinessPartnerId"].ToString());
                            businessPartnerDocument.BusinessPartner.Identifier = Guid.Parse(reader["BusinessPartnerDocumentTypeIdentifier"].ToString());
                            businessPartnerDocument.BusinessPartner.Code = reader["BusinessPartnerCode"].ToString();
                            businessPartnerDocument.BusinessPartner.Name = reader["BusinessPartnerName"].ToString();
                        }

                        if (reader["Name"] != null)
                            businessPartnerDocument.Name = reader["Name"].ToString();
                        if (reader["CreateDate"] != null)
                            businessPartnerDocument.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                        if (reader["Path"] != null)
                            businessPartnerDocument.Path = reader["Path"].ToString();

                        businessPartnerDocument.Active = bool.Parse(reader["Active"].ToString());
                        businessPartnerDocument.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != null)
                        {
                            businessPartnerDocument.CreatedBy = new User();
                            businessPartnerDocument.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerDocument.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            businessPartnerDocument.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            businessPartnerDocument.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != null)
                        {
                            businessPartnerDocument.Company = new Company();
                            businessPartnerDocument.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerDocument.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            businessPartnerDocument.Company.Name = reader["CompanyName"].ToString();
                        }

                        BusinessPartnerDocuments.Add(businessPartnerDocument);
                    }
                }
            }
            return BusinessPartnerDocuments;

            //List<BusinessPartnerDocument> BusinessPartners = context.BusinessPartnerDocuments
            //    .Include(x => x.BusinessPartner)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .AsNoTracking()
            //    .ToList();

            //return BusinessPartners;
        }

        public BusinessPartnerDocument Create(BusinessPartnerDocument BusinessPartnerDocument)
        {
            if (context.BusinessPartnerDocuments.Where(x => x.Identifier != null && x.Identifier == BusinessPartnerDocument.Identifier).Count() == 0)
            {
                BusinessPartnerDocument.Id = 0;

                BusinessPartnerDocument.Active = true;

                context.BusinessPartnerDocuments.Add(BusinessPartnerDocument);
                return BusinessPartnerDocument;
            }
            else
            {
                // Load item that will be updated
                BusinessPartnerDocument dbEntry = context.BusinessPartnerDocuments
                    .FirstOrDefault(x => x.Identifier == BusinessPartnerDocument.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = BusinessPartnerDocument.CompanyId ?? null;
                    dbEntry.CreatedById = BusinessPartnerDocument.CreatedById ?? null;

                    // Set properties
                    dbEntry.Name = BusinessPartnerDocument.Name;
                    dbEntry.CreateDate = BusinessPartnerDocument.CreateDate;
                    dbEntry.Path = BusinessPartnerDocument.Path;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public BusinessPartnerDocument Delete(Guid identifier)
        {
            BusinessPartnerDocument dbEntry = context.BusinessPartnerDocuments
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
