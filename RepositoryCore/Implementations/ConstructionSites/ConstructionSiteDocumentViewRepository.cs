using Configurator;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.ConstructionSites;
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
                "ConstructionSiteId, ConstructionSiteIdentifier, ConstructionSiteCode, ConstructionSiteName, " +
                "Name, CreateDate, Path, " +
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

                        if (reader["ConstructionSiteId"] != null)
                        {
                            constructionSiteDocument.ConstructionSite = new ConstructionSite();
                            constructionSiteDocument.ConstructionSiteId = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            constructionSiteDocument.ConstructionSite.Id = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            constructionSiteDocument.ConstructionSite.Identifier = Guid.Parse(reader["ConstructionSiteDocumentTypeIdentifier"].ToString());
                            constructionSiteDocument.ConstructionSite.Code = reader["ConstructionSiteCode"].ToString();
                            constructionSiteDocument.ConstructionSite.Name = reader["ConstructionSiteName"].ToString();
                        }

                        if (reader["Name"] != null)
                            constructionSiteDocument.Name = reader["Name"].ToString();
                        if (reader["CreateDate"] != null)
                            constructionSiteDocument.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                        if (reader["Path"] != null)
                            constructionSiteDocument.Path = reader["Path"].ToString();

                        constructionSiteDocument.Active = bool.Parse(reader["Active"].ToString());
                        constructionSiteDocument.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != null)
                        {
                            constructionSiteDocument.CreatedBy = new User();
                            constructionSiteDocument.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            constructionSiteDocument.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            constructionSiteDocument.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            constructionSiteDocument.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != null)
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
                "ConstructionSiteId, ConstructionSiteIdentifier, ConstructionSiteCode, ConstructionSiteName, " +
                "Name, CreateDate, Path, " +
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

                        if (reader["ConstructionSiteId"] != null)
                        {
                            constructionSiteDocument.ConstructionSite = new ConstructionSite();
                            constructionSiteDocument.ConstructionSiteId = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            constructionSiteDocument.ConstructionSite.Id = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            constructionSiteDocument.ConstructionSite.Identifier = Guid.Parse(reader["ConstructionSiteIdentifier"].ToString());
                            constructionSiteDocument.ConstructionSite.Code = reader["ConstructionSiteCode"].ToString();
                            constructionSiteDocument.ConstructionSite.Name = reader["ConstructionSiteName"].ToString();
                        }

                        if (reader["Name"] != null)
                            constructionSiteDocument.Name = reader["Name"].ToString();
                        if (reader["CreateDate"] != null)
                            constructionSiteDocument.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                        if (reader["Path"] != null)
                            constructionSiteDocument.Path = reader["Path"].ToString();

                        constructionSiteDocument.Active = bool.Parse(reader["Active"].ToString());
                        constructionSiteDocument.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != null)
                        {
                            constructionSiteDocument.CreatedBy = new User();
                            constructionSiteDocument.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            constructionSiteDocument.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            constructionSiteDocument.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            constructionSiteDocument.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != null)
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
                "ConstructionSiteId, ConstructionSiteIdentifier, ConstructionSiteCode, ConstructionSiteName, " +
                "Name, CreateDate, Path, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
                "CompanyId, CompanyName " +
                "FROM vConstructionSiteDocuments " +
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
                    ConstructionSiteDocument constructionSiteDocument;
                    while (reader.Read())
                    {
                        constructionSiteDocument = new ConstructionSiteDocument();
                        constructionSiteDocument.Id = Int32.Parse(reader["ConstructionSiteDocumentId"].ToString());
                        constructionSiteDocument.Identifier = Guid.Parse(reader["ConstructionSiteDocumentIdentifier"].ToString());

                        if (reader["ConstructionSiteId"] != null)
                        {
                            constructionSiteDocument.ConstructionSite = new ConstructionSite();
                            constructionSiteDocument.ConstructionSiteId = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            constructionSiteDocument.ConstructionSite.Id = Int32.Parse(reader["ConstructionSiteId"].ToString());
                            constructionSiteDocument.ConstructionSite.Identifier = Guid.Parse(reader["ConstructionSiteIdentifier"].ToString());
                            constructionSiteDocument.ConstructionSite.Code = reader["ConstructionSiteCode"].ToString();
                            constructionSiteDocument.ConstructionSite.Name = reader["ConstructionSiteName"].ToString();
                        }

                        if (reader["Name"] != null)
                            constructionSiteDocument.Name = reader["Name"].ToString();
                        if (reader["CreateDate"] != null)
                            constructionSiteDocument.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                        if (reader["Path"] != null)
                            constructionSiteDocument.Path = reader["Path"].ToString();

                        constructionSiteDocument.Active = bool.Parse(reader["Active"].ToString());
                        constructionSiteDocument.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != null)
                        {
                            constructionSiteDocument.CreatedBy = new User();
                            constructionSiteDocument.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            constructionSiteDocument.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            constructionSiteDocument.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            constructionSiteDocument.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != null)
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

        public ConstructionSiteDocument Create(ConstructionSiteDocument ConstructionSiteDocument)
        {
            if (context.ConstructionSiteDocuments.Where(x => x.Identifier != null && x.Identifier == ConstructionSiteDocument.Identifier).Count() == 0)
            {
                ConstructionSiteDocument.Id = 0;

                ConstructionSiteDocument.Active = true;

                context.ConstructionSiteDocuments.Add(ConstructionSiteDocument);
                return ConstructionSiteDocument;
            }
            else
            {
                // Load item that will be updated
                ConstructionSiteDocument dbEntry = context.ConstructionSiteDocuments
                    .FirstOrDefault(x => x.Identifier == ConstructionSiteDocument.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = ConstructionSiteDocument.CompanyId ?? null;
                    dbEntry.CreatedById = ConstructionSiteDocument.CreatedById ?? null;

                    // Set properties
                    dbEntry.Name = ConstructionSiteDocument.Name;
                    dbEntry.CreateDate = ConstructionSiteDocument.CreateDate;
                    dbEntry.Path = ConstructionSiteDocument.Path;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public ConstructionSiteDocument Delete(Guid identifier)
        {
            ConstructionSiteDocument dbEntry = context.ConstructionSiteDocuments
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
