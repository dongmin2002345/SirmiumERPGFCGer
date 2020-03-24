using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.ConstructionSites;
using ServiceInterfaces.Gloabals;
using ServiceInterfaces.Messages.ConstructionSites;
using ServiceInterfaces.ViewModels.ConstructionSites;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;

namespace SirmiumERPGFC.Repository.ConstructionSites
{
    public class ConstructionSiteDocumentSQLiteRepository
    {
        #region SQL

        public static string ConstructionSiteDocumentTableCreatePart =
                     "CREATE TABLE IF NOT EXISTS ConstructionSiteDocuments " +
                     "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                     "ServerId INTEGER NULL, " +
                     "Identifier GUID, " +
                     "ConstructionSiteId INTEGER NULL, " +
                     "ConstructionSiteIdentifier GUID NULL, " +
                     "ConstructionSiteCode NVARCHAR(48) NULL, " +
                     "ConstructionSiteName NVARCHAR(48) NULL, " +
                     "ConstructionSiteInternalCode NVARCHAR(48) NULL, " +
                     "Name NVARCHAR(2048), " +
                     "CreateDate DATETIME NULL, " +
                     "Path NVARCHAR(2048) NULL, " +
                     "ItemStatus INTEGER NOT NULL, " +
                     "IsSynced BOOL NULL, " +
                     "UpdatedAt DATETIME NULL, " +
                     "CreatedById INTEGER NULL, " +
                     "CreatedByName NVARCHAR(2048) NULL, " +
                     "CompanyId INTEGER NULL, " +
                     "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, ConstructionSiteId, ConstructionSiteIdentifier, " +
            "ConstructionSiteCode, ConstructionSiteName, ConstructionSiteInternalCode, Name, CreateDate, Path, ItemStatus, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO ConstructionSiteDocuments " +
            "(Id, ServerId, Identifier, ConstructionSiteId, ConstructionSiteIdentifier, " +
            "ConstructionSiteCode, ConstructionSiteName, ConstructionSiteInternalCode, Name, CreateDate, Path, ItemStatus, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @ConstructionSiteId, @ConstructionSiteIdentifier, " +
            "@ConstructionSiteCode, @ConstructionSiteName, @ConstructionSiteInternalCode, @Name, @CreateDate, @Path, @ItemStatus, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods
        private static ConstructionSiteDocumentViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            ConstructionSiteDocumentViewModel dbEntry = new ConstructionSiteDocumentViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.ConstructionSite = SQLiteHelper.GetConstructionSiteWithInternalCode(query, ref counter);
            dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
            dbEntry.CreateDate = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.Path = SQLiteHelper.GetString(query, ref counter);
            dbEntry.ItemStatus = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
            return dbEntry;
        }

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, ConstructionSiteDocumentViewModel constructionSiteDocument)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", constructionSiteDocument.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", constructionSiteDocument.Identifier);
            insertCommand.Parameters.AddWithValue("@ConstructionSiteId", ((object)constructionSiteDocument.ConstructionSite.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ConstructionSiteIdentifier", ((object)constructionSiteDocument.ConstructionSite.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ConstructionSiteCode", ((object)constructionSiteDocument.ConstructionSite.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ConstructionSiteName", ((object)constructionSiteDocument.ConstructionSite.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ConstructionSiteInternalCode", ((object)constructionSiteDocument.ConstructionSite.InternalCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Name", ((object)constructionSiteDocument.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreateDate", ((object)constructionSiteDocument.CreateDate) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Path", ((object)constructionSiteDocument.Path) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ItemStatus", ((object)constructionSiteDocument.ItemStatus) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsSynced", constructionSiteDocument.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)constructionSiteDocument.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read

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
                        ConstructionSiteDocumentViewModel dbEntry = Read(query);
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



        public ConstructionSiteDocumentListResponse GetFilteredConstructionSiteDocuments(int companyId, ConstructionSiteDocumentViewModel filterObject)
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
                        "WHERE (" +
                        "   (@ConstructionSiteName IS NULL OR @ConstructionSiteName = '' OR ConstructionSiteName LIKE @ConstructionSiteName) OR " +
                        "   (@ConstructionSiteName IS NULL OR @ConstructionSiteName = '' OR Name LIKE @ConstructionSiteName) " +
                        ") " +
                        "AND (@ConstructionSiteInternalCode IS NULL OR @ConstructionSiteInternalCode = '' OR ConstructionSiteInternalCode LIKE @ConstructionSiteInternalCode) " +
                        "AND (@DateFrom IS NULL OR @DateFrom = '' OR DATE(CreateDate) >= DATE(@DateFrom)) " +
                        "AND (@DateTo IS NULL OR @DateTo = '' OR DATE(CreateDate) <= DATE(@DateTo)) " +
                        "ORDER BY IsSynced, Id DESC;", db);

                    selectCommand.Parameters.AddWithValue("@ConstructionSiteName", (String.IsNullOrEmpty(filterObject.Search_Name) ? "" : "%" + filterObject.Search_Name + "%"));
                    selectCommand.Parameters.AddWithValue("@ConstructionSiteInternalCode", (String.IsNullOrEmpty(filterObject.Search_Code) ? "" : "%" + filterObject.Search_Code + "%"));
                    selectCommand.Parameters.AddWithValue("@DateFrom", ((object)filterObject.Search_DateFrom) ?? DBNull.Value);
                    selectCommand.Parameters.AddWithValue("@DateTo", ((object)filterObject.Search_DateTo) ?? DBNull.Value);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {

                        ConstructionSiteDocumentViewModel dbEntry = Read(query);
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
                        ConstructionSiteDocumentViewModel dbEntry = Read(query);
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

        //public ConstructionSiteDocumentListResponse GetUnSyncedDocuments(int companyId)
        //{
        //    ConstructionSiteDocumentListResponse response = new ConstructionSiteDocumentListResponse();
        //    List<ConstructionSiteDocumentViewModel> viewModels = new List<ConstructionSiteDocumentViewModel>();

        //    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
        //    {
        //        db.Open();
        //        try
        //        {
        //            SqliteCommand selectCommand = new SqliteCommand(
        //                SqlCommandSelectPart +
        //                "FROM  ConstructionSiteDocuments " +
        //                "WHERE CompanyId = @CompanyId AND IsSynced = 0 " +
        //                "ORDER BY Id DESC;", db);
        //            selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

        //            SqliteDataReader query = selectCommand.ExecuteReader();

        //            while (query.Read())
        //            {
        //                int counter = 0;
        //                ConstructionSiteDocumentViewModel dbEntry = new ConstructionSiteDocumentViewModel();
        //                dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
        //                dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
        //                dbEntry.ConstructionSite = SQLiteHelper.GetConstructionSite(query, ref counter);
        //                dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
        //                dbEntry.CreateDate = SQLiteHelper.GetDateTime(query, ref counter);
        //                dbEntry.Path = SQLiteHelper.GetString(query, ref counter);
        //                dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
        //                dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
        //                dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
        //                dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
        //                viewModels.Add(dbEntry);
        //            }

        //        }
        //        catch (SqliteException error)
        //        {
        //            MainWindow.ErrorMessage = error.Message;
        //            response.Success = false;
        //            response.Message = error.Message;
        //            response.ConstructionSiteDocuments = new List<ConstructionSiteDocumentViewModel>();
        //            return response;
        //        }
        //        db.Close();
        //    }
        //    response.Success = true;
        //    response.ConstructionSiteDocuments = viewModels;
        //    return response;
        //}

        #endregion

        #region Sync

        public void Sync(IConstructionSiteDocumentService ConstructionSiteDocumentService, Action<int, int> callback = null)
        {
            try
            {
                SyncConstructionSiteDocumentRequest request = new SyncConstructionSiteDocumentRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                ConstructionSiteDocumentListResponse response = ConstructionSiteDocumentService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.ConstructionSiteDocuments?.Count ?? 0;
                    List<ConstructionSiteDocumentViewModel> items = response.ConstructionSiteDocuments;

                    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM ConstructionSiteDocuments WHERE Identifier = @Identifier";

                            SqliteCommand insertCommand = db.CreateCommand();
                            insertCommand.CommandText = SqlCommandInsertPart;

                            foreach (var item in items)
                            {
                                deleteCommand.Parameters.AddWithValue("@Identifier", item.Identifier);
                                deleteCommand.ExecuteNonQuery();
                                deleteCommand.Parameters.Clear();

                                if (item.IsActive)
                                {
                                    item.IsSynced = true;

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
                }
                else
                    throw new Exception(response.Message);
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

        public ConstructionSiteDocumentResponse Create(ConstructionSiteDocumentViewModel constructionSiteDocument)
        {
            ConstructionSiteDocumentResponse response = new ConstructionSiteDocumentResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, constructionSiteDocument);
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

        public ConstructionSiteDocumentResponse Delete(Guid identifier)
        {
            ConstructionSiteDocumentResponse response = new ConstructionSiteDocumentResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM ConstructionSiteDocuments WHERE Identifier = @Identifier";
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

        //public ConstructionSiteDocumentResponse DeleteAll()
        //{
        //    ConstructionSiteDocumentResponse response = new ConstructionSiteDocumentResponse();

        //    try
        //    {
        //        using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
        //        {
        //            db.Open();
        //            db.EnableExtensions(true);

        //            SqliteCommand insertCommand = new SqliteCommand();
        //            insertCommand.Connection = db;

        //            //Use parameterized query to prevent SQL injection attacks
        //            insertCommand.CommandText = "DELETE FROM ConstructionSiteDocuments";
        //            try
        //            {
        //                insertCommand.ExecuteReader();
        //            }
        //            catch (SqliteException error)
        //            {
        //                response.Success = false;
        //                response.Message = error.Message;

        //                MainWindow.ErrorMessage = error.Message;
        //                return response;
        //            }
        //            db.Close();
        //        }
        //    }
        //    catch (SqliteException error)
        //    {
        //        response.Success = false;
        //        response.Message = error.Message;
        //        return response;
        //    }

        //    response.Success = true;
        //    return response;
        //}

        public ConstructionSiteDocumentResponse SetStatusDeleted(Guid identifier)
        {
            ConstructionSiteDocumentResponse response = new ConstructionSiteDocumentResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "UPDATE ConstructionSiteDocuments SET ItemStatus = @ItemStatus WHERE Identifier = @Identifier";
                insertCommand.Parameters.AddWithValue("@ItemStatus", ItemStatus.Deleted);
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

        #endregion
    }
}
