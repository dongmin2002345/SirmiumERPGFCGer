using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.ConstructionSites;
using ServiceInterfaces.Messages.ConstructionSites;
using ServiceInterfaces.ViewModels.ConstructionSites;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.ConstructionSites
{
    public class ConstructionSiteDocumentSQLiteRepository
    {
        public static string ConstructionSiteDocumentTableCreatePart =
                     "CREATE TABLE IF NOT EXISTS ConstructionSiteDocuments " +
                     "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                     "ServerId INTEGER NULL, " +
                     "Identifier GUID, " +
                     "ConstructionSiteId INTEGER NULL, " +
                     "ConstructionSiteIdentifier GUID NULL, " +
                     "ConstructionSiteCode NVARCHAR(48) NULL, " +
                     "ConstructionSiteName NVARCHAR(48) NULL, " +
                     "Name NVARCHAR(2048), " +
                     "CreateDate DATETIME NULL, " +
                     "Path NVARCHAR(2048) NULL, " +
                     "IsSynced BOOL NULL, " +
                     "UpdatedAt DATETIME NULL, " +
                     "CreatedById INTEGER NULL, " +
                     "CreatedByName NVARCHAR(2048) NULL, " +
                     "CompanyId INTEGER NULL, " +
                     "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, ConstructionSiteId, ConstructionSiteIdentifier, " +
            "ConstructionSiteCode, ConstructionSiteName, Name, CreateDate, Path, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO ConstructionSiteDocuments " +
            "(Id, ServerId, Identifier, ConstructionSiteId, ConstructionSiteIdentifier, " +
            "ConstructionSiteCode, ConstructionSiteName, Name, CreateDate, Path, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @ConstructionSiteId, @ConstructionSiteIdentifier, " +
            "@ConstructionSiteCode, @ConstructionSiteName, @Name, @CreateDate, @Path, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        public ConstructionSiteDocumentListResponse GetConstructionSiteDocumentsByConstructionSite(int companyId, Guid ConstructionSiteIdentifier)
        {
            ConstructionSiteDocumentListResponse response = new ConstructionSiteDocumentListResponse();
            List<ConstructionSiteDocumentViewModel> ConstructionSiteDocuments = new List<ConstructionSiteDocumentViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM ConstructionSiteDocuments " +
                        "WHERE ConstructionSiteIdentifier = @ConstructionSiteIdentifier " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC;", db);
                    selectCommand.Parameters.AddWithValue("@ConstructionSiteIdentifier", ConstructionSiteIdentifier);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        ConstructionSiteDocumentViewModel dbEntry = new ConstructionSiteDocumentViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.ConstructionSite = SQLiteHelper.GetConstructionSite(query, ref counter);
                        dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.CreateDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.Path = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        ConstructionSiteDocuments.Add(dbEntry);
                    }

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.ConstructionSiteDocuments = new List<ConstructionSiteDocumentViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.ConstructionSiteDocuments = ConstructionSiteDocuments;
            return response;
        }

        public ConstructionSiteDocumentResponse GetConstructionSiteDocument(Guid identifier)
        {
            ConstructionSiteDocumentResponse response = new ConstructionSiteDocumentResponse();
            ConstructionSiteDocumentViewModel ConstructionSiteDocument = new ConstructionSiteDocumentViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM ConstructionSiteDocuments " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        int counter = 0;
                        ConstructionSiteDocumentViewModel dbEntry = new ConstructionSiteDocumentViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.ConstructionSite = SQLiteHelper.GetConstructionSite(query, ref counter);
                        dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.CreateDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.Path = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        ConstructionSiteDocument = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.ConstructionSiteDocument = new ConstructionSiteDocumentViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.ConstructionSiteDocument = ConstructionSiteDocument;
            return response;
        }

        public ConstructionSiteDocumentListResponse GetUnSyncedDocuments(int companyId)
        {
            ConstructionSiteDocumentListResponse response = new ConstructionSiteDocumentListResponse();
            List<ConstructionSiteDocumentViewModel> viewModels = new List<ConstructionSiteDocumentViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM  ConstructionSiteDocuments " +
                        "WHERE CompanyId = @CompanyId AND IsSynced = 0 " +
                        "ORDER BY Id DESC;", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        ConstructionSiteDocumentViewModel dbEntry = new ConstructionSiteDocumentViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.ConstructionSite = SQLiteHelper.GetConstructionSite(query, ref counter);
                        dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.CreateDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.Path = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        viewModels.Add(dbEntry);
                    }

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.ConstructionSiteDocuments = new List<ConstructionSiteDocumentViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.ConstructionSiteDocuments = viewModels;
            return response;
        }

        public void Sync(IConstructionSiteDocumentService ConstructionSiteDocumentService)
        {
            var unSynced = GetUnSyncedDocuments(MainWindow.CurrentCompanyId);
            SyncConstructionSiteDocumentRequest request = new SyncConstructionSiteDocumentRequest();
            request.CompanyId = MainWindow.CurrentCompanyId;
            request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

            ConstructionSiteDocumentListResponse response = ConstructionSiteDocumentService.Sync(request);
            if (response.Success)
            {
                List<ConstructionSiteDocumentViewModel> ConstructionSiteDocumentsFromDB = response.ConstructionSiteDocuments;
                foreach (var ConstructionSiteDocument in ConstructionSiteDocumentsFromDB.OrderBy(x => x.Id))
                {
                    Delete(ConstructionSiteDocument.Identifier);
                    if (ConstructionSiteDocument.IsActive)
                    {
                        ConstructionSiteDocument.IsSynced = true;
                        Create(ConstructionSiteDocument);
                    }
                }
            }
        }

        public DateTime? GetLastUpdatedAt(int companyId)
        {
            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from ConstructionSiteDocuments WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from ConstructionSiteDocuments WHERE CompanyId = @CompanyId", db);
                        selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                        query = selectCommand.ExecuteReader();
                        if (query.Read())
                        {
                            return query.GetDateTime(0);
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

        public ConstructionSiteDocumentResponse Create(ConstructionSiteDocumentViewModel ConstructionSiteDocument)
        {
            ConstructionSiteDocumentResponse response = new ConstructionSiteDocumentResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = SqlCommandInsertPart;

                insertCommand.Parameters.AddWithValue("@ServerId", ConstructionSiteDocument.Id);
                insertCommand.Parameters.AddWithValue("@Identifier", ConstructionSiteDocument.Identifier);
                insertCommand.Parameters.AddWithValue("@ConstructionSiteId", ((object)ConstructionSiteDocument.ConstructionSite.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ConstructionSiteIdentifier", ((object)ConstructionSiteDocument.ConstructionSite.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ConstructionSiteCode", ((object)ConstructionSiteDocument.ConstructionSite.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ConstructionSiteName", ((object)ConstructionSiteDocument.ConstructionSite.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Name", ConstructionSiteDocument.Name);
                insertCommand.Parameters.AddWithValue("@CreateDate", ((object)ConstructionSiteDocument.CreateDate) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Path", ((object)ConstructionSiteDocument.Path) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IsSynced", ConstructionSiteDocument.IsSynced);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)ConstructionSiteDocument.UpdatedAt) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
                insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
                insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
                insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

                try
                {
                    insertCommand.ExecuteReader();
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

        public ConstructionSiteDocumentResponse UpdateSyncStatus(Guid identifier, string code, DateTime? updatedAt, int serverId, bool isSynced)
        {
            ConstructionSiteDocumentResponse response = new ConstructionSiteDocumentResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "UPDATE ConstructionSiteDocuments SET " +
                    "IsSynced = @IsSynced, " +
                    "Code = @Code, " +
                    "UpdatedAt = @UpdatedAt, " +
                    "ServerId = @ServerId " +
                    "WHERE Identifier = @Identifier ";

                insertCommand.Parameters.AddWithValue("@IsSynced", isSynced);
                insertCommand.Parameters.AddWithValue("@Code", code);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", updatedAt);
                insertCommand.Parameters.AddWithValue("@ServerId", serverId);
                insertCommand.Parameters.AddWithValue("@Identifier", identifier);

                try
                {
                    insertCommand.ExecuteReader();
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

        public ConstructionSiteDocumentResponse Delete(Guid identifier)
        {
            ConstructionSiteDocumentResponse response = new ConstructionSiteDocumentResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "DELETE FROM ConstructionSiteDocuments WHERE Identifier = @Identifier";
                insertCommand.Parameters.AddWithValue("@Identifier", identifier);
                try
                {
                    insertCommand.ExecuteReader();
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

        public ConstructionSiteDocumentResponse DeleteAll()
        {
            ConstructionSiteDocumentResponse response = new ConstructionSiteDocumentResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM ConstructionSiteDocuments";
                    try
                    {
                        insertCommand.ExecuteReader();
                    }
                    catch (SqliteException error)
                    {
                        response.Success = false;
                        response.Message = error.Message;

                        MainWindow.ErrorMessage = error.Message;
                        return response;
                    }
                    db.Close();
                }
            }
            catch (SqliteException error)
            {
                response.Success = false;
                response.Message = error.Message;
                return response;
            }

            response.Success = true;
            return response;
        }
    }
}
