using Configurator;
using DomainCore.Common.Companies;
using DomainCore.Common.DocumentStores;
using DomainCore.Common.Identity;
using RepositoryCore.Abstractions.Common.DocumentStores;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.Implementations.Common.DocumentStores
{
    public class DocumentFileRepository : IDocumentFileRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        string selectQuery = @"SELECT doc.Id AS DocumentId, doc.Identifier AS DocumentIdentifier, doc.Name AS DocumentName, doc.Path AS DocumentPath,
                                     DocumentFolder.Id AS DocumentFolderId, 
                                     DocumentFolder.Identifier AS DocumentFolderIdentifier, 
                                     DocumentFolder.Name AS DocumentFolderName, 
                                     DocumentFolder.Path AS DocumentFolderPath, 
	                                 createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, 
	                                 company.Id AS CompanyId, company.Name AS CompanyName, 
	                                 doc.Active, doc.CreatedAt, doc.UpdatedAt, doc.Size
                               FROM DocumentFiles doc
                               LEFT JOIN DocumentFolders DocumentFolder ON doc.DocumentFolderId = DocumentFolder.Id 
                               LEft JOIN Companies company ON doc.CompanyId = company.Id
                               LEFT JOIN Users createdBy ON doc.CreatedById = createdBy.Id  ";

        public DocumentFileRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public DocumentFile Create(DocumentFile document)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                CreateOrUpdate(connection, document);

                return document;
            }
        }


        void CreateOrUpdate(SqlConnection connection, DocumentFile document)
        {
            using (SqlCommand command = connection.CreateCommand())
            {
                if (document.Id > 0)
                {
                    command.CommandText = "UPDATE DocumentFiles SET " +
                        "Name = @Name, " +
                        "Path = @Path, " +
                        "Size = @Size, " +
                        "DocumentFolderId = @DocumentFolderId, " +
                        "UpdatedAt = @UpdatedAt " +
                        "WHERE Id = @Id";

                    command.Parameters.AddWithValue("@Id", document.Id);
                    command.Parameters.AddWithValue("@Name", document.Name);
                    command.Parameters.AddWithValue("@Path", document.Path);
                    command.Parameters.AddWithValue("@Size", document.Size);
                    command.Parameters.AddWithValue("@DocumentFolderId", (object)document?.DocumentFolderId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);

                    command.ExecuteScalar();
                }
                else
                {
                    command.CommandText = "INSERT INTO DocumentFiles (Identifier, Name, Path, Size, DocumentFolderId, CompanyId, CreatedById, Active, CreatedAt, UpdatedAt) " +
                        "VALUES (@Identifier, @Name, @Path, @Size, @DocumentFolderId, @CompanyId, @CreatedById, @Active, @CreatedAt, @UpdatedAt) ";

                    command.Parameters.AddWithValue("@Identifier", document.Identifier);
                    command.Parameters.AddWithValue("@Name", document.Name);
                    command.Parameters.AddWithValue("@Path", document.Path);
                    command.Parameters.AddWithValue("@Size", document.Size);
                    command.Parameters.AddWithValue("@DocumentFolderId", (object)document?.DocumentFolderId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@CompanyId", (object)document?.CompanyId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@CreatedById", (object)document?.CreatedById ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Active", true);
                    command.Parameters.AddWithValue("@CreatedAt", (object)document.CreatedAt);
                    command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);


                    command.ExecuteNonQuery();

                    command.Parameters.Clear();
                    command.CommandText = "SELECT MAX(Id) AS Id FROM DocumentFiles WHERE Identifier = @Identifier ";

                    command.Parameters.AddWithValue("@Identifier", document.Identifier);
                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        document.Id = Int32.Parse(reader["Id"].ToString());
                    }
                }
            }
        }

        public List<DocumentFile> SubmitList(List<DocumentFile> documents)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                foreach (var document in documents)
                {
                    CreateOrUpdate(connection, document);
                }
                return documents;
            }
        }

        public DocumentFile Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE DocumentFiles SET " +
                        "Active = @Active, " +
                        "UpdatedAt = @UpdatedAt " +
                        "WHERE Id = @Id";

                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Active", (object)false);
                    command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);


                    command.ExecuteNonQuery();
                }
            }
            var DocumentFile = GetDocumentFile(id);
            return DocumentFile;
        }

        public DocumentFile GetDocumentFile(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = selectQuery +
                        "WHERE doc.Id = @Id ";

                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();


                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            DocumentFile folder = Read(reader);

                            return folder;
                        }
                    }
                }
            }
            return null;
        }

        public DocumentFile Read(SqlDataReader reader)
        {
            DocumentFile folder = new DocumentFile();
            if (reader["DocumentId"] != DBNull.Value)
            {
                folder.Id = Int32.Parse(reader["DocumentId"].ToString());
                folder.Identifier = Guid.Parse(reader["DocumentIdentifier"].ToString());
                folder.Name = reader["DocumentName"]?.ToString();
                folder.Path = reader["DocumentPath"]?.ToString();
                folder.Size = double.Parse(reader["Size"].ToString());
                folder.Active = bool.Parse(reader["Active"].ToString());
                folder.CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString());
                folder.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());
            }

            if (reader["DocumentId"] != DBNull.Value)
            {
                folder.DocumentFolder = new DocumentFolder();

                folder.DocumentFolderId = Int32.Parse(reader["DocumentFolderId"].ToString());
                folder.DocumentFolder.Id = Int32.Parse(reader["DocumentFolderId"].ToString());
                folder.DocumentFolder.Identifier = Guid.Parse(reader["DocumentFolderIdentifier"].ToString());
                folder.DocumentFolder.Name = reader["DocumentFolderName"]?.ToString();
                folder.DocumentFolder.Path = reader["DocumentFolderPath"]?.ToString();
            }

            if (reader["CompanyId"] != DBNull.Value)
            {
                folder.Company = new Company();

                folder.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                folder.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                folder.Company.Name = reader["CompanyName"]?.ToString();
            }
            if (reader["CreatedById"] != DBNull.Value)
            {
                folder.CreatedBy = new User();
                folder.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                folder.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                folder.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                folder.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
            }

            return folder;
        }

        public List<DocumentFile> GetDocumentFiles(int companyId, DateTime? lastUpdate, int numOfPages = 1, int itemsPerPage = 500)
        {
            List<DocumentFile> folders = new List<DocumentFile>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    int toSkip = ((numOfPages - 1) * itemsPerPage);

                    command.CommandText = selectQuery +
                        @"WHERE (doc.CompanyId = @CompanyId) 
                        AND (@UpdatedAt IS NULL OR @UpdatedAt = '' OR CONVERT(DATETIME, CONVERT(VARCHAR(20), doc.UpdatedAt, 120)) > CONVERT(DATETIME, CONVERT(VARCHAR(20), @UpdatedAt, 120))) 
                        ORDER BY doc.UpdatedAt ASC 
                        OFFSET @Offset ROWS FETCH FIRST @Count ROWS ONLY ";

                    command.Parameters.AddWithValue("@CompanyId", ((object)companyId ?? DBNull.Value));
                    command.Parameters.AddWithValue("@UpdatedAt", ((object)lastUpdate ?? DBNull.Value));

                    command.Parameters.AddWithValue("@Offset", ((object)toSkip));
                    command.Parameters.AddWithValue("@Count", ((object)itemsPerPage));

                    connection.Open();

                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            DocumentFile folder = Read(reader);

                            folders.Add(folder);
                        }
                    }
                }
            }
            return folders;
        }

        public void Clear(int companyId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE DocumentFiles SET " +
                        "Active = 0, " +
                        "UpdatedAt = CURRENT_TIMESTAMP " +
                        "WHERE CompanyId = @CompanyId ";

                    command.Parameters.AddWithValue("@CompanyId", companyId);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
