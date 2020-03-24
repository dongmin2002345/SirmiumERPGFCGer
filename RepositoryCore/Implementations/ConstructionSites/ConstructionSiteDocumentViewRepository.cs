using Configurator;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.ConstructionSites;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.ConstructionSites;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.ConstructionSites
{
    public class ConstructionSiteDocumentViewRepository : IConstructionSiteDocumentRepository
    {
        ApplicationDbContext context;
        private string connectionString;

        public ConstructionSiteDocumentViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<ConstructionSiteDocument> GetConstructionSiteDocuments(int companyId)
        {
            List<ConstructionSiteDocument> ConstructionSiteDocuments = new List<ConstructionSiteDocument>();

            string queryString =
                "SELECT ConstructionSiteDocumentId, ConstructionSiteDocumentIdentifier, " +
                "ConstructionSiteId, ConstructionSiteIdentifier, ConstructionSiteCode, ConstructionSiteName, ConstructionSiteInternalCode, " +
                "Name, CreateDate, Path, ItemStatus, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
                "CompanyId, CompanyName " +
                "FROM vConstructionSiteDocuments " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    ConstructionSiteDocument constructionSiteDocument;
                    while (reader.Read())
                    {
                        constructionSiteDocument = new ConstructionSiteDocument();
                        constructionSiteDocument.Id = Int32.Parse(reader["ConstructionSiteDocumentId"].ToString());
                        constructionSiteDocument.Identifier = Guid.Parse(reader["ConstructionSiteDocumentIdentifier"].ToString());

                        if (reader["ConstructionSiteId"] != DBNull.Value)
                        {
                            constructionSiteDocument.ConstructionSite = new ConstructionSite();
                            constructionSiteDocument.ConstructionSiteId = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            constructionSiteDocument.ConstructionSite.Id = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            constructionSiteDocument.ConstructionSite.Identifier = Guid.Parse(reader["ConstructionSiteIdentifier"].ToString());
                            constructionSiteDocument.ConstructionSite.Code = reader["ConstructionSiteCode"].ToString();
                            constructionSiteDocument.ConstructionSite.Name = reader["ConstructionSiteName"].ToString();
                            constructionSiteDocument.ConstructionSite.InternalCode = reader["ConstructionSiteInternalCode"].ToString();
                        }

                        if (reader["Name"] != DBNull.Value)
                            constructionSiteDocument.Name = reader["Name"].ToString();
                        if (reader["CreateDate"] != DBNull.Value)
                            constructionSiteDocument.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                        if (reader["Path"] != DBNull.Value)
                            constructionSiteDocument.Path = reader["Path"].ToString();
                        if (reader["ItemStatus"] != DBNull.Value)
                            constructionSiteDocument.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());

                        constructionSiteDocument.Active = bool.Parse(reader["Active"].ToString());
                        constructionSiteDocument.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            constructionSiteDocument.CreatedBy = new User();
                            constructionSiteDocument.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            constructionSiteDocument.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            constructionSiteDocument.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            constructionSiteDocument.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            constructionSiteDocument.Company = new Company();
                            constructionSiteDocument.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            constructionSiteDocument.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            constructionSiteDocument.Company.Name = reader["CompanyName"].ToString();
                        }

                        ConstructionSiteDocuments.Add(constructionSiteDocument);
                    }
                }
            }
            return ConstructionSiteDocuments;

            //List<ConstructionSiteDocument> ConstructionSiteDocuments = context.ConstructionSiteDocuments
            //    .Include(x => x.ConstructionSite)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Active == true && x.CompanyId == companyId)
            //    .AsNoTracking()
            //    .ToList();

            //return ConstructionSiteDocuments;
        }

        public List<ConstructionSiteDocument> GetConstructionSiteDocumentsByConstructionSite(int ConstructionSiteId)
        {
            List<ConstructionSiteDocument> ConstructionSiteDocuments = new List<ConstructionSiteDocument>();

            string queryString =
                "SELECT ConstructionSiteDocumentId, ConstructionSiteDocumentIdentifier, " +
                "ConstructionSiteId, ConstructionSiteIdentifier, ConstructionSiteCode, ConstructionSiteName, ConstructionSiteInternalCode, " +
                "Name, CreateDate, Path, ItemStatus, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
                "CompanyId, CompanyName " +
                "FROM vConstructionSiteDocuments " +
                "WHERE ConstructionSiteId = @ConstructionSiteId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@ConstructionSiteId", ConstructionSiteId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    ConstructionSiteDocument constructionSiteDocument;
                    while (reader.Read())
                    {
                        constructionSiteDocument = new ConstructionSiteDocument();
                        constructionSiteDocument.Id = Int32.Parse(reader["ConstructionSiteDocumentId"].ToString());
                        constructionSiteDocument.Identifier = Guid.Parse(reader["ConstructionSiteDocumentIdentifier"].ToString());

                        if (reader["ConstructionSiteId"] != DBNull.Value)
                        {
                            constructionSiteDocument.ConstructionSite = new ConstructionSite();
                            constructionSiteDocument.ConstructionSiteId = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            constructionSiteDocument.ConstructionSite.Id = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            constructionSiteDocument.ConstructionSite.Identifier = Guid.Parse(reader["ConstructionSiteIdentifier"].ToString());
                            constructionSiteDocument.ConstructionSite.Code = reader["ConstructionSiteCode"].ToString();
                            constructionSiteDocument.ConstructionSite.Name = reader["ConstructionSiteName"].ToString();
                            constructionSiteDocument.ConstructionSite.InternalCode = reader["ConstructionSiteInternalCode"].ToString();
                        }

                        if (reader["Name"] != DBNull.Value)
                            constructionSiteDocument.Name = reader["Name"].ToString();
                        if (reader["CreateDate"] != DBNull.Value)
                            constructionSiteDocument.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                        if (reader["Path"] != DBNull.Value)
                            constructionSiteDocument.Path = reader["Path"].ToString();
                        if (reader["ItemStatus"] != DBNull.Value)
                            constructionSiteDocument.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());

                        constructionSiteDocument.Active = bool.Parse(reader["Active"].ToString());
                        constructionSiteDocument.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            constructionSiteDocument.CreatedBy = new User();
                            constructionSiteDocument.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            constructionSiteDocument.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            constructionSiteDocument.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            constructionSiteDocument.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            constructionSiteDocument.Company = new Company();
                            constructionSiteDocument.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            constructionSiteDocument.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            constructionSiteDocument.Company.Name = reader["CompanyName"].ToString();
                        }

                        ConstructionSiteDocuments.Add(constructionSiteDocument);
                    }
                }
            }

            return ConstructionSiteDocuments;

            //List<ConstructionSiteDocument> ConstructionSites = context.ConstructionSiteDocuments
            //    .Include(x => x.ConstructionSite)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.ConstructionSiteId == ConstructionSiteId && x.Active == true)
            //    .AsNoTracking()
            //    .ToList();

            //return ConstructionSites;
        }

        public List<ConstructionSiteDocument> GetConstructionSiteDocumentsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<ConstructionSiteDocument> ConstructionSiteDocuments = new List<ConstructionSiteDocument>();

            string queryString =
                "SELECT ConstructionSiteDocumentId, ConstructionSiteDocumentIdentifier, " +
                "ConstructionSiteId, ConstructionSiteIdentifier, ConstructionSiteCode, ConstructionSiteName, ConstructionSiteInternalCode, " +
                "Name, CreateDate, Path, ItemStatus, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
                "CompanyId, CompanyName " +
                "FROM vConstructionSiteDocuments " +
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
                    ConstructionSiteDocument constructionSiteDocument;
                    while (reader.Read())
                    {
                        constructionSiteDocument = new ConstructionSiteDocument();
                        constructionSiteDocument.Id = Int32.Parse(reader["ConstructionSiteDocumentId"].ToString());
                        constructionSiteDocument.Identifier = Guid.Parse(reader["ConstructionSiteDocumentIdentifier"].ToString());

                        if (reader["ConstructionSiteId"] != DBNull.Value)
                        {
                            constructionSiteDocument.ConstructionSite = new ConstructionSite();
                            constructionSiteDocument.ConstructionSiteId = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            constructionSiteDocument.ConstructionSite.Id = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            constructionSiteDocument.ConstructionSite.Identifier = Guid.Parse(reader["ConstructionSiteIdentifier"].ToString());
                            constructionSiteDocument.ConstructionSite.Code = reader["ConstructionSiteCode"].ToString();
                            constructionSiteDocument.ConstructionSite.Name = reader["ConstructionSiteName"].ToString();
                            constructionSiteDocument.ConstructionSite.InternalCode = reader["ConstructionSiteInternalCode"].ToString();
                        }

                        if (reader["Name"] != DBNull.Value)
                            constructionSiteDocument.Name = reader["Name"].ToString();
                        if (reader["CreateDate"] != DBNull.Value)
                            constructionSiteDocument.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                        if (reader["Path"] != DBNull.Value)
                            constructionSiteDocument.Path = reader["Path"].ToString();
                        if (reader["ItemStatus"] != DBNull.Value)
                            constructionSiteDocument.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());

                        constructionSiteDocument.Active = bool.Parse(reader["Active"].ToString());
                        constructionSiteDocument.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            constructionSiteDocument.CreatedBy = new User();
                            constructionSiteDocument.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            constructionSiteDocument.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            constructionSiteDocument.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            constructionSiteDocument.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            constructionSiteDocument.Company = new Company();
                            constructionSiteDocument.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            constructionSiteDocument.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            constructionSiteDocument.Company.Name = reader["CompanyName"].ToString();
                        }

                        ConstructionSiteDocuments.Add(constructionSiteDocument);
                    }
                }
            }
            return ConstructionSiteDocuments;

            //List<ConstructionSiteDocument> ConstructionSites = context.ConstructionSiteDocuments
            //    .Include(x => x.ConstructionSite)
            //    .Include(x => x.Company)
            //    .Include(x => x.CreatedBy)
            //    .Where(x => x.Company.Id == companyId && x.UpdatedAt > lastUpdateTime)
            //    .AsNoTracking()
            //    .ToList();

            //return ConstructionSites;
        }

        public ConstructionSiteDocument Create(ConstructionSiteDocument constructionSiteDocument)
        {
            if (context.ConstructionSiteDocuments.Where(x => x.Identifier != null && x.Identifier == constructionSiteDocument.Identifier).Count() == 0)
            {
                constructionSiteDocument.Id = 0;

                constructionSiteDocument.Active = true;
                constructionSiteDocument.UpdatedAt = DateTime.Now;
                constructionSiteDocument.CreatedAt = DateTime.Now;

                context.ConstructionSiteDocuments.Add(constructionSiteDocument);
                return constructionSiteDocument;
            }
            else
            {
                // Load item that will be updated
                ConstructionSiteDocument dbEntry = context.ConstructionSiteDocuments
                    .FirstOrDefault(x => x.Identifier == constructionSiteDocument.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = constructionSiteDocument.CompanyId ?? null;
                    dbEntry.CreatedById = constructionSiteDocument.CreatedById ?? null;

                    // Set properties
                    dbEntry.Name = constructionSiteDocument.Name;
                    dbEntry.CreateDate = constructionSiteDocument.CreateDate;
                    dbEntry.Path = constructionSiteDocument.Path;
                    dbEntry.ItemStatus = constructionSiteDocument.ItemStatus;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public ConstructionSiteDocument Delete(Guid identifier)
        {
            ConstructionSiteDocument dbEntry = context.ConstructionSiteDocuments
                 .Union(context.ChangeTracker.Entries()
                     .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(ConstructionSiteDocument))
                     .Select(x => x.Entity as ConstructionSiteDocument))
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
