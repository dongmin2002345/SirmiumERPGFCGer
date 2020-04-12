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
    public class DocumentFolderRepository : IDocumentFolderRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        string selectQuery = @"SELECT folder.Id AS FolderId, folder.Identifier AS FolderIdentifier, folder.Name AS FolderName, folder.Path AS FolderPath, 
                         ParentFolder.Id AS ParentId, 
                         ParentFolder.Identifier AS ParentIdentifier, 
                         ParentFolder.Name AS ParentName, 
                         ParentFolder.Path AS ParentPath, 
						 createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, 
                         company.Id AS CompanyId, company.Name AS CompanyName, 
                         folder.Active, folder.UpdatedAt 
                         FROM DocumentFolders folder
                         LEFT JOIN DocumentFolders ParentFolder ON folder.ParentFolderId = ParentFolder.Id 
                         LEft JOIN Companies company ON folder.CompanyId = company.Id
						 LEFT JOIN Users createdBy ON folder.CreatedById = createdBy.Id  ";

        public DocumentFolderRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public DocumentFolder Create(DocumentFolder docFolder)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    if (docFolder.Id > 0)
                    {
                        command.CommandText = "UPDATE DocumentFolders SET " +
                            "Name = @Name, " +
                            "Path = @Path, " +
                            "ParentFolderId = @ParentFolderId, " +
                            "UpdatedAt = @UpdatedAt " +
                            "WHERE Id = @Id";

                        command.Parameters.AddWithValue("@Id", docFolder.Id);
                        command.Parameters.AddWithValue("@Name", docFolder.Name);
                        command.Parameters.AddWithValue("@Path", docFolder.Path);
                        command.Parameters.AddWithValue("@ParentFolderId", (object)docFolder?.ParentFolderId ?? DBNull.Value);
                        command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);

                        connection.Open();

                        command.ExecuteScalar();


                        return docFolder;
                    }
                    else
                    {
                        command.CommandText = "INSERT INTO DocumentFolders (Identifier, Name, Path, ParentFolderId, CompanyId, CreatedById, Active, CreatedAt, UpdatedAt) " +
                            "VALUES (@Identifier, @Name, @Path, @ParentFolderId, @CompanyId, @CreatedById, @Active, @CreatedAt, @UpdatedAt) ";

                        command.Parameters.AddWithValue("@Identifier", docFolder.Identifier);
                        command.Parameters.AddWithValue("@Name", docFolder.Name);
                        command.Parameters.AddWithValue("@Path", docFolder.Path);
                        command.Parameters.AddWithValue("@ParentFolderId", (object)docFolder?.ParentFolderId ?? DBNull.Value);
                        command.Parameters.AddWithValue("@CompanyId", (object)docFolder?.CompanyId ?? DBNull.Value);
                        command.Parameters.AddWithValue("@CreatedById", (object)docFolder?.CreatedById ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Active", true);
                        command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                        command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);


                        connection.Open();
                        command.ExecuteNonQuery();
                        
                        command.Parameters.Clear();
                        command.CommandText = "SELECT MAX(Id) AS Id FROM DocumentFolders WHERE Identifier = @Identifier ";

                        command.Parameters.AddWithValue("@Identifier", docFolder.Identifier);
                        var reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            docFolder.Id = Int32.Parse(reader["Id"].ToString());
                        }

                        return docFolder;
                    }
                }
            }
        }


        public List<DocumentFolder> SubmitList(List<DocumentFolder> docFolders)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                foreach (var docFolder in docFolders)
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        if (docFolder.Id > 0)
                        {
                            command.CommandText = "UPDATE DocumentFolders SET " +
                                "Name = @Name, " +
                                "Path = @Path, " +
                                "ParentFolderId = @ParentFolderId, " +
                                "UpdatedAt = @UpdatedAt " +
                                "WHERE Id = @Id";

                            command.Parameters.AddWithValue("@Id", docFolder.Id);
                            command.Parameters.AddWithValue("@Name", docFolder.Name);
                            command.Parameters.AddWithValue("@Path", docFolder.Path);
                            command.Parameters.AddWithValue("@ParentFolderId", (object)docFolder?.ParentFolderId ?? DBNull.Value);
                            command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);

                            connection.Open();

                            command.ExecuteScalar();
                        }
                        else
                        {
                            command.CommandText = "INSERT INTO DocumentFolders (Identifier, Name, Path, ParentFolderId, CompanyId, CreatedById, Active, CreatedAt, UpdatedAt) " +
                                "VALUES (@Identifier, @Name, @Path, @ParentFolderId, @CompanyId, @CreatedById, @Active, @CreatedAt, @UpdatedAt) ";

                            command.Parameters.AddWithValue("@Identifier", docFolder.Identifier);
                            command.Parameters.AddWithValue("@Name", docFolder.Name);
                            command.Parameters.AddWithValue("@Path", docFolder.Path);
                            command.Parameters.AddWithValue("@ParentFolderId", (object)docFolder?.ParentFolderId ?? DBNull.Value);
                            command.Parameters.AddWithValue("@CompanyId", (object)docFolder?.CompanyId ?? DBNull.Value);
                            command.Parameters.AddWithValue("@CreatedById", (object)docFolder?.CreatedById ?? DBNull.Value);
                            command.Parameters.AddWithValue("@Active", true);
                            command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                            command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);


                            command.ExecuteNonQuery();

                            command.Parameters.Clear();
                            command.CommandText = "SELECT MAX(Id) AS Id FROM DocumentFolders WHERE Identifier = @Identifier ";

                            command.Parameters.AddWithValue("@Identifier", docFolder.Identifier);
                            var reader = command.ExecuteReader();

                            if (reader.Read())
                            {
                                docFolder.Id = Int32.Parse(reader["Id"].ToString());
                            }
                        }
                    }
                }
                return docFolders;
            }
        }

        public DocumentFolder Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE DocumentFolders SET " +
                        "Active = @Active, " +
                        "UpdatedAt = @UpdatedAt " +
                        "WHERE Id = @Id";

                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Active", (object)false);
                    command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                }
            }
            var documentFolder = GetDocumentFolder(id);
            return documentFolder;
        }

        public DocumentFolder GetDocumentFolder(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = selectQuery + 
                        "WHERE Id = @Id ";

                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();


                    var reader = command.ExecuteReader();
                    if(reader.HasRows)
                    {
                        if(reader.Read())
                        {
                            DocumentFolder folder = Read(reader);

                            return folder;
                        }
                    }
                }
            }
            return null;
        }

        public DocumentFolder Read(SqlDataReader reader)
        {
            DocumentFolder folder = new DocumentFolder();
            if (reader["FolderId"] != DBNull.Value)
            {
                folder.Id = Int32.Parse(reader["FolderId"].ToString());
                folder.Identifier = Guid.Parse(reader["FolderIdentifier"].ToString());
                folder.Name = reader["FolderName"]?.ToString();
                folder.Path = reader["FolderPath"]?.ToString();
                folder.Active = bool.Parse(reader["Active"].ToString());
                folder.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());
            }

            if (reader["ParentId"] != DBNull.Value)
            {
                folder.ParentFolder = new DocumentFolder();

                folder.ParentFolderId = Int32.Parse(reader["ParentId"].ToString());
                folder.ParentFolder.Id = Int32.Parse(reader["ParentId"].ToString());
                folder.ParentFolder.Identifier = Guid.Parse(reader["ParentIdentifier"].ToString());
                folder.ParentFolder.Name = reader["ParentName"]?.ToString();
                folder.ParentFolder.Path = reader["ParentPath"]?.ToString();
            }

            if(reader["CompanyId"] != DBNull.Value)
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

        public List<DocumentFolder> GetDocumentFolders(int companyId, DateTime? lastUpdate, int numOfPages = 1, int itemsPerPage = 500)
        {
            List<DocumentFolder> folders = new List<DocumentFolder>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {

                    int toSkip = ((numOfPages - 1) * itemsPerPage);

                    command.CommandText = selectQuery +
                        @"WHERE (folder.CompanyId = @CompanyId) 
                        AND (@UpdatedAt IS NULL OR @UpdatedAt = '' OR CONVERT(DATETIME, CONVERT(VARCHAR(20), folder.UpdatedAt, 120)) > CONVERT(DATETIME, CONVERT(VARCHAR(20), @UpdatedAt, 120))) 
                        ORDER BY folder.UpdatedAt ASC 
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
                            DocumentFolder folder = Read(reader);

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
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE DocumentFolders SET " +
                        "Active = @Active, " +
                        "UpdatedAt = CURRENT_TIMESTAMP " +
                        "WHERE CompanyId = @CompanyId ";

                    command.Parameters.AddWithValue("@CompanyId", companyId);
                    command.Parameters.AddWithValue("@Active", 0);
                }
            }
        }
    }
}
