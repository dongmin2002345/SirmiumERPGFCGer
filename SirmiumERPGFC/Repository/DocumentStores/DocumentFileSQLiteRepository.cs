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
    public class DocumentFileSQLiteRepository
    {
        #region SQL
        public static string DocumentFileTableCreatePart =
            "CREATE TABLE IF NOT EXISTS DocumentFiles " +
            "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
            "ServerId INTEGER NULL, " +
            "Identifier GUID, " +
            "Name NVARCHAR(128) NULL, " +
            "Path NVARCHAR(512) NULL, " +
            "Size DECIMAL(18, 2) NULL, " +

            "DocumentFolderId INTEGER NULL, " +
            "DocumentFolderIdentifier GUID NULL, " +
            "DocumentFolderName NVARCHAR(128) NULL, " +
            "DocumentFolderPath NVARCHAR(512) NULL, " +

            "CreatedAt DATETIME NULL, " +
            "UpdatedAt DATETIME NULL, " +
            "CreatedById INTEGER NULL, " +
            "CreatedByName NVARCHAR(2048) NULL, " +
            "CompanyId INTEGER NULL, " +
            "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            @"SELECT ServerId, Identifier, Name, Path, Size, 
            DocumentFolderId, DocumentFolderIdentifier, DocumentFolderName, DocumentFolderPath, 
            
            CreatedAt, UpdatedAt, CreatedById, CreatedByName, 
            CompanyId, CompanyName ";

        public string SqlCommandInsertPart = @"INSERT INTO DocumentFiles  
            (Id, ServerId, Identifier, Name, Path, Size, 
            DocumentFolderId, DocumentFolderIdentifier, DocumentFolderName, DocumentFolderPath, 
            
            CreatedAt, UpdatedAt, CreatedById, CreatedByName, 
            CompanyId, CompanyName) 

            VALUES (NULL, @ServerId, @Identifier, @Name, @Path, @Size, 
            @DocumentFolderId, @DocumentFolderIdentifier, @DocumentFolderName, @DocumentFolderPath,

            @CreatedAt, @UpdatedAt, @CreatedById, @CreatedByName,
            @CompanyId, @CompanyName)";
        #endregion

        #region Helper methods
        private static DocumentFileViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            DocumentFileViewModel dbEntry = new DocumentFileViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Path = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Size = SQLiteHelper.GetDouble(query, ref counter);

            dbEntry.DocumentFolder = SQLiteHelper.GetDocumentFolder(query, ref counter);

            dbEntry.CreatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
            return dbEntry;
        }
        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, DocumentFileViewModel File)
        {


            insertCommand.Parameters.AddWithValue("@ServerId", File.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", File.Identifier);
            insertCommand.Parameters.AddWithValue("@Name", ((object)File.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Path", ((object)File.Path) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Size", ((object)File.Size) ?? DBNull.Value);

            insertCommand.Parameters.AddWithValue("@DocumentFolderId", ((object)File.DocumentFolder?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@DocumentFolderIdentifier", ((object)File.DocumentFolder?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@DocumentFolderName", ((object)File.DocumentFolder?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@DocumentFolderPath", ((object)File.DocumentFolder?.Path) ?? DBNull.Value);

            insertCommand.Parameters.AddWithValue("@CreatedAt", ((object)File.CreatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)File.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }
        #endregion

        #region Read
        public DocumentFileListResponse GetDocumentFiles(int companyId, DocumentFileViewModel searchObject)
        {
            DocumentFileListResponse response = new DocumentFileListResponse();
            List<DocumentFileViewModel> Files = new List<DocumentFileViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand;
                    if (!String.IsNullOrEmpty(searchObject?.Search_Name))
                    {
                        selectCommand = new SqliteCommand(
                             SqlCommandSelectPart +
                             "FROM DocumentFiles " +
                             "WHERE (@FileName IS NULL OR @FileName = '' OR Name LIKE @FileName) " +
                             "AND (@FilterDateFrom IS NULL OR @FilterDateFrom = '' OR DATE(CreatedAt) >= DATE(@FilterDateFrom))" +
                             "AND (@FilterDateTo IS NULL OR @FilterDateTo = '' OR DATE(CreatedAt) <= DATE(@FilterDateTo)) " +
                             "AND (DocumentFolderPath LIKE @ParentPath) " +
                             "AND CompanyId = @CompanyId " +
                             "ORDER BY Name ASC ", db);

                        selectCommand.Parameters.AddWithValue("@ParentPath", ((object)searchObject.Search_ParentPath) != null ? searchObject.Search_ParentPath + "%" : "");
                        selectCommand.Parameters.AddWithValue("@FileName", ((object)searchObject.Search_Name) != null ? "%" + searchObject.Search_Name + "%" : "");
                        selectCommand.Parameters.AddWithValue("@FilterDateFrom", ((object)searchObject?.Search_DateFrom ?? DBNull.Value));
                        selectCommand.Parameters.AddWithValue("@FilterDateTo", ((object)searchObject?.Search_DateTo ?? DBNull.Value));
                        selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    } else
                    {
                        selectCommand = new SqliteCommand(
                             SqlCommandSelectPart +
                             "FROM DocumentFiles " +
                             "WHERE (DocumentFolderPath = @ParentPath) " +
                             "AND (@FilterDateFrom IS NULL OR @FilterDateFrom = '' OR DATE(CreatedAt) >= DATE(@FilterDateFrom))" +
                             "AND (@FilterDateTo IS NULL OR @FilterDateTo = '' OR DATE(CreatedAt) <= DATE(@FilterDateTo)) " +
                             "AND CompanyId = @CompanyId " +
                             "ORDER BY Name ASC ", db);

                        selectCommand.Parameters.AddWithValue("@ParentPath", ((object)searchObject.Search_ParentPath) != null ? searchObject.Search_ParentPath : "");
                        selectCommand.Parameters.AddWithValue("@FilterDateFrom", ((object)searchObject?.Search_DateFrom ?? DBNull.Value));
                        selectCommand.Parameters.AddWithValue("@FilterDateTo", ((object)searchObject?.Search_DateTo ?? DBNull.Value));
                        selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    }

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        DocumentFileViewModel dbEntry = Read(query);
                        Files.Add(dbEntry);
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.DocumentFiles = new List<DocumentFileViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.DocumentFiles = Files;
            return response;
        }
        #endregion

        #region Sync
        public void Sync(IDocumentFileService service, Action<int, int> callback = null)
        {
            try
            {
                SyncDocumentFileRequest request = new SyncDocumentFileRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);
                request.CurrentPage = 1;
                request.ItemsPerPage = 500;

                int toSync = 0;
                int syncedItems = 0;

                DocumentFileListResponse response = service.Sync(request);
                if (!response.Success)
                {
                    throw new Exception(response.Message);
                }
                var items = new List<DocumentFileViewModel>(response?.DocumentFiles ?? new List<DocumentFileViewModel>());
                while (items.Count() > 0)
                {
                    toSync += response?.DocumentFiles?.Count ?? 0;

                    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM DocumentFiles WHERE Identifier = @Identifier";

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
                    items = new List<DocumentFileViewModel>(response?.DocumentFiles ?? new List<DocumentFileViewModel>());
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from DocumentFiles WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from DocumentFiles WHERE CompanyId = @CompanyId", db);
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
        public DocumentFileResponse Create(DocumentFileViewModel DocumentFile)
        {
            DocumentFileResponse response = new DocumentFileResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, DocumentFile);
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

        public DocumentFileResponse Delete(Guid identifier)
        {
            DocumentFileResponse response = new DocumentFileResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM DocumentFiles WHERE Identifier = @Identifier";
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
