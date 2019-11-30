using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.CallCentars;
using ServiceInterfaces.Messages.Common.CallCentars;
using ServiceInterfaces.ViewModels.Common.CallCentars;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.CallCentars
{
    public class CallCentarSQLiteRepository
    {
        #region SQL

        public static string CallCentarTableCreatePart =
           "CREATE TABLE IF NOT EXISTS CallCentars " +
           "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
           "ServerId INTEGER NULL, " +
           "Identifier GUID, " +
           "Code NVARCHAR(2048) NULL, " +
           "ReceivingDate DATETIME NULL, " +

           "UserId INTEGER NULL, " +
           "UserIdentifier GUID NULL, " +
           "UserCode NVARCHAR(2048) NULL, " +
           "UserFirstName NVARCHAR(2048) NULL, " +
           "UserLastName NVARCHAR(2048) NULL, " +

           "Comment NVARCHAR(2048) NULL, " +
           "EndingDate DATETIME NULL, " +
           "CheckedDone BOOL NOT NULL, " +
           "IsSynced BOOL NULL, " +
           "UpdatedAt DATETIME NULL, " +
           "CreatedById INTEGER NULL, " +
           "CreatedByName NVARCHAR(2048) NULL, " +
           "CompanyId INTEGER NULL, " +
           "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, Code, ReceivingDate, " +
            "UserId, UserIdentifier, UserCode, UserFirstName, UserLastName, " +
            "Comment, EndingDate, CheckedDone, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO CallCentars " +
            "(Id, ServerId, Identifier, Code, ReceivingDate, " +
            "UserId, UserIdentifier, UserCode, UserFirstName, UserLastName, " +
            "Comment, EndingDate, CheckedDone, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @Code, @ReceivingDate, " +
            "@UserId, @UserIdentifier, @UserCode, @UserFirstName, @UserLastName, " +
            "@Comment, @EndingDate, @CheckedDone, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods

        private CallCentarViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            CallCentarViewModel dbEntry = new CallCentarViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
            dbEntry.ReceivingDate = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.User = SQLiteHelper.GetUser(query, ref counter);
            dbEntry.Comment = SQLiteHelper.GetString(query, ref counter);
            dbEntry.EndingDate = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CheckedDone = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);

            return dbEntry;
        }

        public SqliteCommand AddCreateParameters(SqliteCommand insertCommand, CallCentarViewModel CallCentar)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", CallCentar.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", CallCentar.Identifier);
            insertCommand.Parameters.AddWithValue("@Code", ((object)CallCentar.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ReceivingDate", ((object)CallCentar.ReceivingDate) ?? DBNull.Value);

            insertCommand.Parameters.AddWithValue("@UserId", ((object)CallCentar.User?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@UserIdentifier", ((object)CallCentar.User?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@UserCode", ((object)CallCentar.User?.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@UserFirstName", ((object)CallCentar.User?.FirstName) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@UserLastName", ((object)CallCentar.User?.LastName) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Comment", ((object)CallCentar.Comment) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EndingDate", ((object)CallCentar.EndingDate) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CheckedDone", ((object)CallCentar.CheckedDone) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsSynced", CallCentar.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)CallCentar.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read 

        public CallCentarListResponse GetCallCentarsByPage(int companyId, CallCentarViewModel CallCentarSearchObject, int currentPage = 1, int itemsPerPage = 50, int? userId = null)
        {
            CallCentarListResponse response = new CallCentarListResponse();
            List<CallCentarViewModel> CallCentars = new List<CallCentarViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM CallCentars " +
                        "WHERE CompanyId = @CompanyId " +
                        "AND (@UserId IS NULL OR @UserId = '' OR UserId = @UserId) " +
                        "ORDER BY IsSynced, ServerId " +
                        "LIMIT @ItemsPerPage OFFSET @Offset;", db);

                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
                    selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);
                    selectCommand.Parameters.AddWithValue("@UserId", ((object)userId ?? DBNull.Value));

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        CallCentarViewModel dbEntry = Read(query);
                        CallCentars.Add(dbEntry);
                    }
                    selectCommand = new SqliteCommand(
                        "SELECT Count(*) " +
                        "FROM CallCentars " +
                        "WHERE CompanyId = @CompanyId " +
                        "AND (@UserId IS NULL OR @UserId = '' OR UserId = @UserId) ", db);

                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    selectCommand.Parameters.AddWithValue("@UserId", ((object)userId ?? DBNull.Value));

                    query = selectCommand.ExecuteReader();

                    if (query.Read())
                        response.TotalItems = query.GetInt32(0);
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.CallCentars = new List<CallCentarViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.CallCentars = CallCentars;
            return response;
        }

        public CallCentarListResponse GetCallCentarsForPopup(int companyId, string filterString)
        {
            CallCentarListResponse response = new CallCentarListResponse();
            List<CallCentarViewModel> CallCentars = new List<CallCentarViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM CallCentars " +
                        "WHERE CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage;", db);

                    selectCommand.Parameters.AddWithValue("@CompanyId", ((object)filterString) != null ? companyId : 0);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", 100);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        CallCentarViewModel dbEntry = Read(query);
                        CallCentars.Add(dbEntry);
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.CallCentars = new List<CallCentarViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.CallCentars = CallCentars;
            return response;
        }

        public CallCentarResponse GetCallCentar(Guid identifier)
        {
            CallCentarResponse response = new CallCentarResponse();
            CallCentarViewModel CallCentar = null;

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM CallCentars " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        CallCentarViewModel dbEntry = Read(query);
                        CallCentar = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.CallCentar = new CallCentarViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.CallCentar = CallCentar;
            return response;
        }

        #endregion

        #region Sync

        public void Sync(ICallCentarService CallCentarService, Action<int, int> callback = null)
        {
            try
            {
                SyncCallCentarRequest request = new SyncCallCentarRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                CallCentarListResponse response = CallCentarService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.CallCentars?.Count ?? 0;
                    List<CallCentarViewModel> CallCentarsFromDB = response.CallCentars;

                    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM CallCentars WHERE Identifier = @Identifier";

                            SqliteCommand insertCommand = db.CreateCommand();
                            insertCommand.CommandText = SqlCommandInsertPart;

                            foreach (var CallCentar in CallCentarsFromDB)
                            {
                                deleteCommand.Parameters.AddWithValue("@Identifier", CallCentar.Identifier);
                                deleteCommand.ExecuteNonQuery();
                                deleteCommand.Parameters.Clear();

                                if (CallCentar.IsActive)
                                {
                                    CallCentar.IsSynced = true;

                                    insertCommand = AddCreateParameters(insertCommand, CallCentar);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from CallCentars WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from CallCentars WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
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

        public CallCentarResponse Create(CallCentarViewModel CallCentar)
        {
            CallCentarResponse response = new CallCentarResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, CallCentar);
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

        public CallCentarResponse Delete(Guid identifier)
        {
            CallCentarResponse response = new CallCentarResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM CallCentars WHERE Identifier = @Identifier";
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
