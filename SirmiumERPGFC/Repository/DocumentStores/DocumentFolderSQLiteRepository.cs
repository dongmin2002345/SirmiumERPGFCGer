using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.DocumentStores;
using ServiceInterfaces.Messages.Common.Invoices;
using ServiceInterfaces.ViewModels.Common.DocumentStores;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.DocumentStores
{
    public class DocumentFolderSQLiteRepository
    {
        #region SQL
        public static string DocumentFolderTableCreatePart =
            "CREATE TABLE IF NOT EXISTS DocumentFolders " +
            "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
            "ServerId INTEGER NULL, " +
            "Identifier GUID, " +
            "Name NVARCHAR(128) NULL, " +
            "Path NVARCHAR(512) NULL, " +

            "ParentFolderId INTEGER NULL, " +
            "ParentFolderIdentifier GUID NULL, " +
            "ParentFolderName NVARCHAR(128) NULL, " +
            "ParentFolderPath NVARCHAR(512) NULL, " +

            "UpdatedAt DATETIME NULL, " +
            "CreatedById INTEGER NULL, " +
            "CreatedByName NVARCHAR(2048) NULL, " +
            "CompanyId INTEGER NULL, " +
            "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            @"SELECT ServerId, Identifier, Name, Path, 
            ParentFolderId, ParentFolderIdentifier, ParentFolderName, ParentFolderPath, 
            
            UpdatedAt, CreatedById, CreatedByName, 
            CompanyId, CompanyName ";

        public string SqlCommandInsertPart = @"INSERT INTO DocumentFolders  
            (Id, ServerId, Identifier, Name, Path, 
            ParentFolderId, ParentFolderIdentifier, ParentFolderName, ParentFolderPath, 
            
            UpdatedAt, CreatedById, CreatedByName, 
            CompanyId, CompanyName) 

            VALUES (NULL, @ServerId, @Identifier, @Name, @Path,
            @ParentFolderId, @ParentFolderIdentifier, @ParentFolderName, @ParentFolderPath,

            @UpdatedAt, @CreatedById, @CreatedByName,
            @CompanyId, @CompanyName)";
        #endregion

        #region Helper methods
        private static DocumentFolderViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            DocumentFolderViewModel dbEntry = new DocumentFolderViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Path = SQLiteHelper.GetString(query, ref counter);

            dbEntry.ParentFolder = SQLiteHelper.GetDocumentFolder(query, ref counter);

            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
            return dbEntry;
        }
        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, DocumentFolderViewModel folder)
        {


            insertCommand.Parameters.AddWithValue("@ServerId", folder.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", folder.Identifier);
            insertCommand.Parameters.AddWithValue("@Name", ((object)folder.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Path", ((object)folder.Path) ?? DBNull.Value);

            insertCommand.Parameters.AddWithValue("@ParentFolderId", ((object)folder.ParentFolder?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ParentFolderIdentifier", ((object)folder.ParentFolder?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ParentFolderName", ((object)folder.ParentFolder?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ParentFolderPath", ((object)folder.ParentFolder?.Path) ?? DBNull.Value);

            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)folder.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }
        #endregion

        #region Read
        public DocumentFolderListResponse GetDocumentFolders(int companyId, DocumentFolderViewModel searchObject, bool initialDisplay = false)
        {
            DocumentFolderListResponse response = new DocumentFolderListResponse();
            List<DocumentFolderViewModel> folders = new List<DocumentFolderViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand;
                    if (searchObject.Search_ShouldLoadSubDirectories)
                    {
                        selectCommand = new SqliteCommand(
                             SqlCommandSelectPart +
                             "FROM DocumentFolders " +
                             "WHERE (@FolderName IS NULL OR @FolderName = '' OR Name LIKE @FolderName) " +
                             "AND (@ParentFolderId IS NULL OR @ParentFolderId = '' OR ParentFolderId = @ParentFolderId) " +
                             "AND CompanyId = @CompanyId " +
                             "ORDER BY Name ASC ", db);
                        selectCommand.Parameters.AddWithValue("@ParentFolderId", ((object)searchObject?.Search_ParentId ?? DBNull.Value));
                    } else
                    {
                        selectCommand = new SqliteCommand(
                             SqlCommandSelectPart +
                             "FROM DocumentFolders " +
                             "WHERE (@FolderName IS NULL OR @FolderName = '' OR Name LIKE @FolderName) " +
                             "AND CompanyId = @CompanyId " +
                             "ORDER BY Name ASC ", db);
                    }

                    selectCommand.Parameters.AddWithValue("@FolderName", ((object)searchObject.Search_Name) != null ? "%" + searchObject.Search_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        DocumentFolderViewModel dbEntry = Read(query);
                        folders.Add(dbEntry);
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.DocumentFolders = new List<DocumentFolderViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.DocumentFolders = folders;
            return response;
        }

        public DocumentFolderListResponse GetRootFolder(int companyId)
        {
            DocumentFolderListResponse response = new DocumentFolderListResponse();
            List<DocumentFolderViewModel> folders = new List<DocumentFolderViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                             SqlCommandSelectPart +
                             "FROM DocumentFolders " +
                             "WHERE ParentFolderId IS NULL " +
                             "AND CompanyId = @CompanyId " +
                             "ORDER BY Name ASC ", db);

                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        DocumentFolderViewModel dbEntry = Read(query);
                        folders.Add(dbEntry);
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.DocumentFolders = new List<DocumentFolderViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.DocumentFolders = folders;
            return response;
        }


        public DocumentFolderResponse GetDirectoryByPath(int companyId, string path)
        {
            DocumentFolderResponse response = new DocumentFolderResponse();
            DocumentFolderViewModel folder = null;

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                             SqlCommandSelectPart +
                             "FROM DocumentFolders " +
                             "WHERE Path = @Path " +
                             "AND CompanyId = @CompanyId " +
                             "ORDER BY Name ASC ", db);

                    selectCommand.Parameters.AddWithValue("@Path", path);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        folder = Read(query);
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.DocumentFolder = null;
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.DocumentFolder = folder;
            return response;
        }

        #endregion

        #region Sync
        public void Sync(IDocumentFolderService service, Action<int, int> callback = null)
        {
            try
            {
                SyncDocumentFolderRequest request = new SyncDocumentFolderRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);
                request.CurrentPage = 1;
                request.ItemsPerPage = 500;

                int toSync = 0;
                int syncedItems = 0;

                DocumentFolderListResponse response = service.Sync(request);
                if (!response.Success)
                {
                    throw new Exception(response.Message);
                }
                var items = new List<DocumentFolderViewModel>(response?.DocumentFolders ?? new List<DocumentFolderViewModel>());
                while (items.Count() > 0)
                {
                    toSync += response?.DocumentFolders?.Count ?? 0;

                    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM DocumentFolders WHERE Identifier = @Identifier";

                            SqliteCommand insertCommand = db.CreateCommand();
                            insertCommand.CommandText = SqlCommandInsertPart;

                            foreach (var item in items)
                            {
                                deleteCommand.Parameters.AddWithValue("@Identifier", item.Identifier);
                                deleteCommand.ExecuteNonQuery();
                                deleteCommand.Parameters.Clear();

                                if (item.IsActive)
                                {
                                    insertCommand = AddCreateParameters(insertCommand, item);
                                    insertCommand.ExecuteNonQuery();
                                    insertCommand.Parameters.Clear();

                                    syncedItems++;
                                    callback?.Invoke(syncedItems, toSync);
                                }
                            }

                            transaction.Commit();
                        }
                        db.Close();
                    }
                    request.CurrentPage++;
                    response = service.Sync(request);
                    if (!response.Success)
                    {
                        throw new Exception(response.Message);
                    }
                    items = new List<DocumentFolderViewModel>(response?.DocumentFolders ?? new List<DocumentFolderViewModel>());
                }
            }
            catch (Exception ex)
            {
                MainWindow.ErrorMessage = ex.Message;
            }
        }

        public DateTime? GetLastUpdatedAt(int companyId)
        {
            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from DocumentFolders WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from DocumentFolders WHERE CompanyId = @CompanyId", db);
                        selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                        query = selectCommand.ExecuteReader();
                        if (query.Read())
                        {
                            int counter = 0;
                            return SQLiteHelper.GetDateTimeNullable(query, ref counter);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MainWindow.ErrorMessage = ex.Message;
                }
                db.Close();
            }
            return null;
        }
        #endregion


        #region Create
        public DocumentFolderResponse Create(DocumentFolderViewModel DocumentFolder)
        {
            DocumentFolderResponse response = new DocumentFolderResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, DocumentFolder);
                    insertCommand.ExecuteNonQuery();
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    return response;
                }
                db.Close();

                response.Success = true;
                return response;
            }
        }
        #endregion

        #region Delete

        public DocumentFolderResponse Delete(Guid identifier)
        {
            DocumentFolderResponse response = new DocumentFolderResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM DocumentFolders WHERE Identifier = @Identifier";
                insertCommand.Parameters.AddWithValue("@Identifier", identifier);

                try
                {
                    insertCommand.ExecuteNonQuery();
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    return response;
                }
                db.Close();

                response.Success = true;
                return response;
            }
        }
        #endregion
    }
}
