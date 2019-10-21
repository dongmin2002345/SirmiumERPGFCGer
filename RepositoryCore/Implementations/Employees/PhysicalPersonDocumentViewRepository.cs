using Configurator;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Employees;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Employees;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.PhysicalPersons
{
    public class PhysicalPersonDocumentViewRepository : IPhysicalPersonDocumentRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        public PhysicalPersonDocumentViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public string selectString =
            "SELECT PhysicalPersonDocumentId, PhysicalPersonDocumentIdentifier, " +
                "PhysicalPersonId, PhysicalPersonIdentifier, PhysicalPersonCode, PhysicalPersonName, " +
                "Name, CreateDate, Path, ItemStatus, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
                "CompanyId, CompanyName " +
                "FROM vPhysicalPersonDocuments ";

        private static PhysicalPersonDocument Read(SqlDataReader reader)
        {
            PhysicalPersonDocument physicalPersonDocument = new PhysicalPersonDocument();
            physicalPersonDocument.Id = Int32.Parse(reader["PhysicalPersonDocumentId"].ToString());
            physicalPersonDocument.Identifier = Guid.Parse(reader["PhysicalPersonDocumentIdentifier"].ToString());

            if (reader["PhysicalPersonId"] != DBNull.Value)
            {
                physicalPersonDocument.PhysicalPerson = new PhysicalPerson();
                physicalPersonDocument.PhysicalPersonId = Int32.Parse(reader["PhysicalPersonId"].ToString());
                physicalPersonDocument.PhysicalPerson.Id = Int32.Parse(reader["PhysicalPersonId"].ToString());
                physicalPersonDocument.PhysicalPerson.Identifier = Guid.Parse(reader["PhysicalPersonIdentifier"].ToString());
                physicalPersonDocument.PhysicalPerson.Code = reader["PhysicalPersonCode"].ToString();
                physicalPersonDocument.PhysicalPerson.Name = reader["PhysicalPersonName"].ToString();
            }

            if (reader["Name"] != DBNull.Value)
                physicalPersonDocument.Name = reader["Name"].ToString();
            if (reader["CreateDate"] != DBNull.Value)
                physicalPersonDocument.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
            if (reader["Path"] != DBNull.Value)
                physicalPersonDocument.Path = reader["Path"].ToString();
            if (reader["ItemStatus"] != DBNull.Value)
                physicalPersonDocument.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());

            physicalPersonDocument.Active = bool.Parse(reader["Active"].ToString());
            physicalPersonDocument.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

            if (reader["CreatedById"] != DBNull.Value)
            {
                physicalPersonDocument.CreatedBy = new User();
                physicalPersonDocument.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                physicalPersonDocument.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                physicalPersonDocument.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                physicalPersonDocument.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
            }

            if (reader["CompanyId"] != DBNull.Value)
            {
                physicalPersonDocument.Company = new Company();
                physicalPersonDocument.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                physicalPersonDocument.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                physicalPersonDocument.Company.Name = reader["CompanyName"].ToString();
            }

            return physicalPersonDocument;
        }

        public List<PhysicalPersonDocument> GetPhysicalPersonDocuments(int companyId)
        {
            List<PhysicalPersonDocument> PhysicalPersonDocuments = new List<PhysicalPersonDocument>();

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
                    PhysicalPersonDocument PhysicalPersonDocument;
                    while (reader.Read())
                    {
                        PhysicalPersonDocument = Read(reader);
                        PhysicalPersonDocuments.Add(PhysicalPersonDocument);
                    }
                }
            }

            return PhysicalPersonDocuments;
        }

        public List<PhysicalPersonDocument> GetPhysicalPersonDocumentsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<PhysicalPersonDocument> PhysicalPersonDocuments = new List<PhysicalPersonDocument>();

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
                    PhysicalPersonDocument PhysicalPersonDocument;
                    while (reader.Read())
                    {
                        PhysicalPersonDocument = Read(reader);
                        PhysicalPersonDocuments.Add(PhysicalPersonDocument);
                    }
                }
            }

            return PhysicalPersonDocuments;
        }


        public List<PhysicalPersonDocument> GetPhysicalPersonDocumentsByPhysicalPerson(int PhysicalPersonId)
        {
            List<PhysicalPersonDocument> PhysicalPersonDocuments = new List<PhysicalPersonDocument>();

            string queryString =
                selectString +
                "WHERE PhysicalPersonId = @PhysicalPersonId;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@PhysicalPersonId", PhysicalPersonId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    PhysicalPersonDocument PhysicalPersonDocument;
                    while (reader.Read())
                    {
                        PhysicalPersonDocument = Read(reader);
                        PhysicalPersonDocuments.Add(PhysicalPersonDocument);
                    }
                }
            }

            return PhysicalPersonDocuments;
        }

        public PhysicalPersonDocument GetPhysicalPersonDocument(int id)
        {
            PhysicalPersonDocument PhysicalPersonDocument = null;

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
                        PhysicalPersonDocument = Read(reader);
                    }
                }
            }
            return PhysicalPersonDocument;
        }

        public PhysicalPersonDocument Create(PhysicalPersonDocument physicalPersonDocument)
        {
            if (context.PhysicalPersonDocuments.Where(x => x.Identifier != null && x.Identifier == physicalPersonDocument.Identifier).Count() == 0)
            {
                physicalPersonDocument.Id = 0;

                physicalPersonDocument.Active = true;
                physicalPersonDocument.UpdatedAt = DateTime.Now;
                physicalPersonDocument.CreatedAt = DateTime.Now;

                context.PhysicalPersonDocuments.Add(physicalPersonDocument);
                return physicalPersonDocument;
            }
            else
            {
                // Load PhysicalPersonDocument that will be updated
                PhysicalPersonDocument dbEntry = context.PhysicalPersonDocuments
                    .FirstOrDefault(x => x.Identifier == physicalPersonDocument.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = physicalPersonDocument.CompanyId ?? null;
                    dbEntry.CreatedById = physicalPersonDocument.CreatedById ?? null;

                    // Set properties
                    dbEntry.Name = physicalPersonDocument.Name;
                    dbEntry.CreateDate = physicalPersonDocument.CreateDate;
                    dbEntry.Path = physicalPersonDocument.Path;
                    dbEntry.ItemStatus = physicalPersonDocument.ItemStatus;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public PhysicalPersonDocument Delete(Guid identifier)
        {
            PhysicalPersonDocument dbEntry = context.PhysicalPersonDocuments
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(PhysicalPersonDocument))
                    .Select(x => x.Entity as PhysicalPersonDocument))
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


//        ApplicationDbContext context;
//        private string connectionString;

//        public PhysicalPersonDocumentViewRepository(ApplicationDbContext context)
//        {
//            this.context = context;
//            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
//        }

//        public List<PhysicalPersonDocument> GetPhysicalPersonDocuments(int companyId)
//        {
//            List<PhysicalPersonDocument> PhysicalPersonDocuments = new List<PhysicalPersonDocument>();

//            string queryString =
//                "SELECT PhysicalPersonDocumentId, PhysicalPersonDocumentIdentifier, " +
//                "PhysicalPersonId, PhysicalPersonIdentifier, PhysicalPersonCode, PhysicalPersonName, " +
//                "Name, CreateDate, Path, ItemStatus, " +
//                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
//                "CompanyId, CompanyName " +
//                "FROM vPhysicalPersonDocuments " +
//                "WHERE CompanyId = @CompanyId AND Active = 1;";

//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                SqlCommand command = connection.CreateCommand();
//                command.CommandText = queryString;
//                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

//                connection.Open();
//                using (SqlDataReader reader = command.ExecuteReader())
//                {
//                    PhysicalPersonDocument physicalPersonDocument;
//                    while (reader.Read())
//                    {
//                        physicalPersonDocument = new PhysicalPersonDocument();
//                        physicalPersonDocument.Id = Int32.Parse(reader["PhysicalPersonDocumentId"].ToString());
//                        physicalPersonDocument.Identifier = Guid.Parse(reader["PhysicalPersonDocumentIdentifier"].ToString());

//                        if (reader["PhysicalPersonId"] != DBNull.Value)
//                        {
//                            physicalPersonDocument.PhysicalPerson = new PhysicalPerson();
//                            physicalPersonDocument.PhysicalPersonId = Int32.Parse(reader["PhysicalPersonId"].ToString());
//                            physicalPersonDocument.PhysicalPerson.Id = Int32.Parse(reader["PhysicalPersonId"].ToString());
//                            physicalPersonDocument.PhysicalPerson.Identifier = Guid.Parse(reader["PhysicalPersonIdentifier"].ToString());
//                            physicalPersonDocument.PhysicalPerson.Code = reader["PhysicalPersonCode"].ToString();
//                            physicalPersonDocument.PhysicalPerson.Name = reader["PhysicalPersonName"].ToString();
//                        }

//                        if (reader["Name"] != DBNull.Value)
//                            physicalPersonDocument.Name = reader["Name"].ToString();
//                        if (reader["CreateDate"] != DBNull.Value)
//                            physicalPersonDocument.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
//                        if (reader["Path"] != DBNull.Value)
//                            physicalPersonDocument.Path = reader["Path"].ToString();
//                        if (reader["ItemStatus"] != DBNull.Value)
//                            physicalPersonDocument.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());

//                        physicalPersonDocument.Active = bool.Parse(reader["Active"].ToString());
//                        physicalPersonDocument.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

//                        if (reader["CreatedById"] != DBNull.Value)
//                        {
//                            physicalPersonDocument.CreatedBy = new User();
//                            physicalPersonDocument.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
//                            physicalPersonDocument.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
//                            physicalPersonDocument.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
//                            physicalPersonDocument.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
//                        }

//                        if (reader["CompanyId"] != DBNull.Value)
//                        {
//                            physicalPersonDocument.Company = new Company();
//                            physicalPersonDocument.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
//                            physicalPersonDocument.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
//                            physicalPersonDocument.Company.Name = reader["CompanyName"].ToString();
//                        }

//                        PhysicalPersonDocuments.Add(physicalPersonDocument);
//                    }
//                }
//            }
//            return PhysicalPersonDocuments;

//            //List<PhysicalPersonDocument> PhysicalPersonDocuments = context.PhysicalPersonDocuments
//            //    .Include(x => x.PhysicalPerson)
//            //    .Include(x => x.Company)
//            //    .Include(x => x.CreatedBy)
//            //    .Where(x => x.Active == true && x.CompanyId == companyId)
//            //    .AsNoTracking()
//            //    .ToList();

//            //return PhysicalPersonDocuments;
//        }

//        public List<PhysicalPersonDocument> GetPhysicalPersonDocumentsByPhysicalPerson(int PhysicalPersonId)
//        {
//            List<PhysicalPersonDocument> PhysicalPersonDocuments = new List<PhysicalPersonDocument>();

//            string queryString =
//                "SELECT PhysicalPersonDocumentId, PhysicalPersonDocumentIdentifier, " +
//                "PhysicalPersonId, PhysicalPersonIdentifier, PhysicalPersonCode, PhysicalPersonName, " +
//                "Name, CreateDate, Path, ItemStatus, " +
//                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
//                "CompanyId, CompanyName " +
//                "FROM vPhysicalPersonDocuments " +
//                "WHERE PhysicalPersonId = @PhysicalPersonId AND Active = 1;";

//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                SqlCommand command = connection.CreateCommand();
//                command.CommandText = queryString;
//                command.Parameters.Add(new SqlParameter("@PhysicalPersonId", PhysicalPersonId));

//                connection.Open();
//                using (SqlDataReader reader = command.ExecuteReader())
//                {
//                    PhysicalPersonDocument physicalPersonDocument;
//                    while (reader.Read())
//                    {
//                        physicalPersonDocument = new PhysicalPersonDocument();
//                        physicalPersonDocument.Id = Int32.Parse(reader["PhysicalPersonDocumentId"].ToString());
//                        physicalPersonDocument.Identifier = Guid.Parse(reader["PhysicalPersonDocumentIdentifier"].ToString());

//                        if (reader["PhysicalPersonId"] != DBNull.Value)
//                        {
//                            physicalPersonDocument.PhysicalPerson = new PhysicalPerson();
//                            physicalPersonDocument.PhysicalPersonId = Int32.Parse(reader["PhysicalPersonId"].ToString());
//                            physicalPersonDocument.PhysicalPerson.Id = Int32.Parse(reader["PhysicalPersonId"].ToString());
//                            physicalPersonDocument.PhysicalPerson.Identifier = Guid.Parse(reader["PhysicalPersonIdentifier"].ToString());
//                            physicalPersonDocument.PhysicalPerson.Code = reader["PhysicalPersonCode"].ToString();
//                            physicalPersonDocument.PhysicalPerson.Name = reader["PhysicalPersonName"].ToString();
//                        }

//                        if (reader["Name"] != DBNull.Value)
//                            physicalPersonDocument.Name = reader["Name"].ToString();
//                        if (reader["CreateDate"] != DBNull.Value)
//                            physicalPersonDocument.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
//                        if (reader["Path"] != DBNull.Value)
//                            physicalPersonDocument.Path = reader["Path"].ToString();
//                        if (reader["ItemStatus"] != DBNull.Value)
//                            physicalPersonDocument.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());

//                        physicalPersonDocument.Active = bool.Parse(reader["Active"].ToString());
//                        physicalPersonDocument.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

//                        if (reader["CreatedById"] != DBNull.Value)
//                        {
//                            physicalPersonDocument.CreatedBy = new User();
//                            physicalPersonDocument.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
//                            physicalPersonDocument.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
//                            physicalPersonDocument.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
//                            physicalPersonDocument.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
//                        }

//                        if (reader["CompanyId"] != DBNull.Value)
//                        {
//                            physicalPersonDocument.Company = new Company();
//                            physicalPersonDocument.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
//                            physicalPersonDocument.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
//                            physicalPersonDocument.Company.Name = reader["CompanyName"].ToString();
//                        }

//                        PhysicalPersonDocuments.Add(physicalPersonDocument);
//                    }
//                }
//            }

//            return PhysicalPersonDocuments;

//            //List<PhysicalPersonDocument> PhysicalPersons = context.PhysicalPersonDocuments
//            //    .Include(x => x.PhysicalPerson)
//            //    .Include(x => x.Company)
//            //    .Include(x => x.CreatedBy)
//            //    .Where(x => x.PhysicalPersonId == PhysicalPersonId && x.Active == true)
//            //    .AsNoTracking()
//            //    .ToList();

//            //return PhysicalPersons;
//        }

//        public List<PhysicalPersonDocument> GetPhysicalPersonDocumentsNewerThen(int companyId, DateTime lastUpdateTime)
//        {
//            List<PhysicalPersonDocument> PhysicalPersonDocuments = new List<PhysicalPersonDocument>();

//            string queryString =
//                "SELECT PhysicalPersonDocumentId, PhysicalPersonDocumentIdentifier, " +
//                "PhysicalPersonId, PhysicalPersonIdentifier, PhysicalPersonCode, PhysicalPersonName, " +
//                "Name, CreateDate, Path, ItemStatus, " +
//                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
//                "CompanyId, CompanyName " +
//                "FROM vPhysicalPersonDocuments " +
//                "WHERE CompanyId = @CompanyId AND UpdatedAt > @LastUpdateTime;";

//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                SqlCommand command = connection.CreateCommand();
//                command.CommandText = queryString;
//                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));
//                command.Parameters.Add(new SqlParameter("@LastUpdateTime", lastUpdateTime));

//                connection.Open();
//                using (SqlDataReader reader = command.ExecuteReader())
//                {
//                    PhysicalPersonDocument physicalPersonDocument;
//                    while (reader.Read())
//                    {
//                        physicalPersonDocument = new PhysicalPersonDocument();
//                        physicalPersonDocument.Id = Int32.Parse(reader["PhysicalPersonDocumentId"].ToString());
//                        physicalPersonDocument.Identifier = Guid.Parse(reader["PhysicalPersonDocumentIdentifier"].ToString());

//                        if (reader["PhysicalPersonId"] != DBNull.Value)
//                        {
//                            physicalPersonDocument.PhysicalPerson = new PhysicalPerson();
//                            physicalPersonDocument.PhysicalPersonId = Int32.Parse(reader["PhysicalPersonId"].ToString());
//                            physicalPersonDocument.PhysicalPerson.Id = Int32.Parse(reader["PhysicalPersonId"].ToString());
//                            physicalPersonDocument.PhysicalPerson.Identifier = Guid.Parse(reader["PhysicalPersonIdentifier"].ToString());
//                            physicalPersonDocument.PhysicalPerson.Code = reader["PhysicalPersonCode"].ToString();
//                            physicalPersonDocument.PhysicalPerson.Name = reader["PhysicalPersonName"].ToString();
//                        }

//                        if (reader["Name"] != DBNull.Value)
//                            physicalPersonDocument.Name = reader["Name"].ToString();
//                        if (reader["CreateDate"] != DBNull.Value)
//                            physicalPersonDocument.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
//                        if (reader["Path"] != DBNull.Value)
//                            physicalPersonDocument.Path = reader["Path"].ToString();
//                        if (reader["ItemStatus"] != DBNull.Value)
//                            physicalPersonDocument.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());

//                        physicalPersonDocument.Active = bool.Parse(reader["Active"].ToString());
//                        physicalPersonDocument.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

//                        if (reader["CreatedById"] != DBNull.Value)
//                        {
//                            physicalPersonDocument.CreatedBy = new User();
//                            physicalPersonDocument.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
//                            physicalPersonDocument.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
//                            physicalPersonDocument.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
//                            physicalPersonDocument.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
//                        }

//                        if (reader["CompanyId"] != DBNull.Value)
//                        {
//                            physicalPersonDocument.Company = new Company();
//                            physicalPersonDocument.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
//                            physicalPersonDocument.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
//                            physicalPersonDocument.Company.Name = reader["CompanyName"].ToString();
//                        }

//                        PhysicalPersonDocuments.Add(physicalPersonDocument);
//                    }
//                }
//            }
//            return PhysicalPersonDocuments;

//            //List<PhysicalPersonDocument> PhysicalPersons = context.PhysicalPersonDocuments
//            //    .Include(x => x.PhysicalPerson)
//            //    .Include(x => x.Company)
//            //    .Include(x => x.CreatedBy)
//            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
//            //    .AsNoTracking()
//            //    .ToList();

//            //return PhysicalPersons;
//        }

//        public PhysicalPersonDocument Create(PhysicalPersonDocument PhysicalPersonDocument)
//        {
//            if (context.PhysicalPersonDocuments.Where(x => x.Identifier != null && x.Identifier == PhysicalPersonDocument.Identifier).Count() == 0)
//            {
//                PhysicalPersonDocument.Id = 0;

//                PhysicalPersonDocument.Active = true;
//                PhysicalPersonDocument.UpdatedAt = DateTime.Now;
//                PhysicalPersonDocument.CreatedAt = DateTime.Now;

//                context.PhysicalPersonDocuments.Add(PhysicalPersonDocument);
//                return PhysicalPersonDocument;
//            }
//            else
//            {
//                // Load item that will be updated
//                PhysicalPersonDocument dbEntry = context.PhysicalPersonDocuments
//                    .FirstOrDefault(x => x.Identifier == PhysicalPersonDocument.Identifier && x.Active == true);

//                if (dbEntry != null)
//                {
//                    dbEntry.CompanyId = PhysicalPersonDocument.CompanyId ?? null;
//                    dbEntry.CreatedById = PhysicalPersonDocument.CreatedById ?? null;

//                    // Set properties
//                    dbEntry.Name = PhysicalPersonDocument.Name;
//                    dbEntry.CreateDate = PhysicalPersonDocument.CreateDate;
//                    dbEntry.Path = PhysicalPersonDocument.Path;
//                    dbEntry.ItemStatus = PhysicalPersonDocument.ItemStatus;


//                    // Set timestamp
//                    dbEntry.UpdatedAt = DateTime.Now;
//                }

//                return dbEntry;
//            }
//        }

//        public PhysicalPersonDocument Delete(Guid identifier)
//        {
//            PhysicalPersonDocument dbEntry = context.PhysicalPersonDocuments
//                .Union(context.ChangeTracker.Entries()
//                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(PhysicalPersonDocument))
//                    .Select(x => x.Entity as PhysicalPersonDocument))
//                .FirstOrDefault(x => x.Identifier == identifier && x.Active == true);

//            if (dbEntry != null)
//            {
//                dbEntry.Active = false;
//                dbEntry.UpdatedAt = DateTime.Now;
//            }
//            return dbEntry;
//        }
//    }
//}
