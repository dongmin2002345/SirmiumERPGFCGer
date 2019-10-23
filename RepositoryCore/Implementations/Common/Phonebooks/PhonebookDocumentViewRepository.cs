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
    public class PhonebookDocumentViewRepository: IPhonebookDocumentRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        public PhonebookDocumentViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public string selectString =
            "SELECT PhonebookDocumentId, PhonebookDocumentIdentifier, " +
                            "PhonebookId, PhonebookIdentifier, PhonebookCode, PhonebookName, " +
                            "Name, CreateDate, Path, ItemStatus, " +
                            "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
            "FROM vPhonebookDocuments ";

        private static PhonebookDocument Read(SqlDataReader reader)
        {
            PhonebookDocument PhonebookDocument = new PhonebookDocument();
            PhonebookDocument.Id = Int32.Parse(reader["PhonebookDocumentId"].ToString());
            PhonebookDocument.Identifier = Guid.Parse(reader["PhonebookDocumentIdentifier"].ToString());

            if (reader["PhonebookId"] != DBNull.Value)
            {
                PhonebookDocument.Phonebook = new Phonebook();
                PhonebookDocument.PhonebookId = Int32.Parse(reader["PhonebookId"].ToString());
                PhonebookDocument.Phonebook.Id = Int32.Parse(reader["PhonebookId"].ToString());
                PhonebookDocument.Phonebook.Identifier = Guid.Parse(reader["PhonebookIdentifier"].ToString());
                PhonebookDocument.Phonebook.Code = reader["PhonebookCode"].ToString();
                PhonebookDocument.Phonebook.Name = reader["PhonebookName"].ToString();
            }


            if (reader["Name"] != DBNull.Value)
                PhonebookDocument.Name = reader["Name"].ToString();
            if (reader["CreateDate"] != DBNull.Value)
                PhonebookDocument.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
            if (reader["Path"] != DBNull.Value)
                PhonebookDocument.Path = reader["Path"].ToString();
            if (reader["ItemStatus"] != DBNull.Value)
                PhonebookDocument.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());
            PhonebookDocument.Active = bool.Parse(reader["Active"].ToString());
            PhonebookDocument.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

            if (reader["CreatedById"] != DBNull.Value)
            {
                PhonebookDocument.CreatedBy = new User();
                PhonebookDocument.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                PhonebookDocument.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                PhonebookDocument.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                PhonebookDocument.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
            }

            if (reader["CompanyId"] != DBNull.Value)
            {
                PhonebookDocument.Company = new Company();
                PhonebookDocument.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                PhonebookDocument.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                PhonebookDocument.Company.Name = reader["CompanyName"].ToString();
            }

            return PhonebookDocument;
        }

        public List<PhonebookDocument> GetPhonebookDocuments(int companyId)
        {
            List<PhonebookDocument> PhonebookDocuments = new List<PhonebookDocument>();

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
                    PhonebookDocument PhonebookDocument;
                    while (reader.Read())
                    {
                        PhonebookDocument = Read(reader);
                        PhonebookDocuments.Add(PhonebookDocument);
                    }
                }
            }

            return PhonebookDocuments;
        }

        public List<PhonebookDocument> GetPhonebookDocumentsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<PhonebookDocument> PhonebookDocuments = new List<PhonebookDocument>();

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
                    PhonebookDocument PhonebookDocument;
                    while (reader.Read())
                    {
                        PhonebookDocument = Read(reader);
                        PhonebookDocuments.Add(PhonebookDocument);
                    }
                }
            }

            return PhonebookDocuments;
        }


        public List<PhonebookDocument> GetPhonebookDocumentsByPhonebook(int PhonebookId)
        {
            List<PhonebookDocument> PhonebookDocuments = new List<PhonebookDocument>();

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
                    PhonebookDocument PhonebookDocument;
                    while (reader.Read())
                    {
                        PhonebookDocument = Read(reader);
                        PhonebookDocuments.Add(PhonebookDocument);
                    }
                }
            }

            return PhonebookDocuments;
        }

        public PhonebookDocument GetPhonebookDocument(int id)
        {
            PhonebookDocument PhonebookDocument = null;

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
                        PhonebookDocument = Read(reader);
                    }
                }
            }
            return PhonebookDocument;
        }

        public PhonebookDocument Create(PhonebookDocument PhonebookDocument)
        {
            if (context.PhonebookDocuments.Where(x => x.Identifier != null && x.Identifier == PhonebookDocument.Identifier).Count() == 0)
            {
                PhonebookDocument.Id = 0;

                PhonebookDocument.Active = true;
                PhonebookDocument.UpdatedAt = DateTime.Now;
                PhonebookDocument.CreatedAt = DateTime.Now;

                context.PhonebookDocuments.Add(PhonebookDocument);
                return PhonebookDocument;
            }
            else
            {
                // Load PhonebookDocument that will be updated
                PhonebookDocument dbEntry = context.PhonebookDocuments
                    .FirstOrDefault(x => x.Identifier == PhonebookDocument.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.PhonebookId = PhonebookDocument.PhonebookId ?? null;
                    dbEntry.CompanyId = PhonebookDocument.CompanyId ?? null;
                    dbEntry.CreatedById = PhonebookDocument.CreatedById ?? null;

                    // Set properties

                    dbEntry.Name = PhonebookDocument.Name;
                    dbEntry.CreateDate = PhonebookDocument.CreateDate;
                    dbEntry.Path = PhonebookDocument.Path;
                    dbEntry.ItemStatus = PhonebookDocument.ItemStatus;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public PhonebookDocument Delete(Guid identifier)
        {
            PhonebookDocument dbEntry = context.PhonebookDocuments
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(PhonebookDocument))
                    .Select(x => x.Entity as PhonebookDocument))
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
