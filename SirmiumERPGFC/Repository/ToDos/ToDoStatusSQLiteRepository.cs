using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.ToDos;
using ServiceInterfaces.Messages.Common.ToDos;
using ServiceInterfaces.ViewModels.Common.ToDos;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.ToDos
{
    public class ToDoStatusSQLiteRepository
    {
        #region SQL
        public static string ToDoStatusTableCreatePart =
           "CREATE TABLE IF NOT EXISTS ToDoStatuses " +
           "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
           "ServerId INTEGER NULL, " +
           "Identifier GUID, " +
           "Code NVARCHAR(2048) NULL, " +
           "Name NVARCHAR(2048) NULL, " +
           "IsSynced BOOL NULL, " +
           "UpdatedAt DATETIME NULL, " +
           "CreatedById INTEGER NULL, " +
           "CreatedByName NVARCHAR(2048) NULL, " +
           "CompanyId INTEGER NULL, " +
           "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, Code, Name, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO ToDoStatuses " +
            "(Id, ServerId, Identifier, Code, Name," +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @Code, @Name, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods

        private ToDoStatusViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            ToDoStatusViewModel dbEntry = new ToDoStatusViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);

            return dbEntry;
        }

        public SqliteCommand AddCreateParameters(SqliteCommand insertCommand, ToDoStatusViewModel toDoStatus)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", toDoStatus.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", toDoStatus.Identifier);
            insertCommand.Parameters.AddWithValue("@Code", ((object)toDoStatus.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Name", ((object)toDoStatus.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsSynced", toDoStatus.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)toDoStatus.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read 

        public ToDoStatusListResponse GetToDoStatusesByPage(int companyId, ToDoStatusViewModel toDoStatusSearchObject, int currentPage = 1, int itemsPerPage = 50)
        {
            ToDoStatusListResponse response = new ToDoStatusListResponse();
            List<ToDoStatusViewModel> toDoStatuses = new List<ToDoStatusViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM ToDoStatuses " +
                        "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, ServerId " +
                        "LIMIT @ItemsPerPage OFFSET @Offset;", db);

                    selectCommand.Parameters.AddWithValue("@Name", ((object)toDoStatusSearchObject.Search_Name) != null ? "%" + toDoStatusSearchObject.Search_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
                    selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                        toDoStatuses.Add(Read(query));

                    selectCommand = new SqliteCommand(
                        "SELECT Count(*) " +
                        "FROM ToDoStatuses " +
                        "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND CompanyId = @CompanyId;", db);

                    selectCommand.Parameters.AddWithValue("@Name", ((object)toDoStatusSearchObject.Search_Name) != null ? "%" + toDoStatusSearchObject.Search_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    query = selectCommand.ExecuteReader();

                    if (query.Read())
                        response.TotalItems = query.GetInt32(0);
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.ToDoStatuses = new List<ToDoStatusViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.ToDoStatuses = toDoStatuses;
            return response;
        }

        public ToDoStatusListResponse GetToDoStatusesForPopup(int companyId, string filterString)
        {
            ToDoStatusListResponse response = new ToDoStatusListResponse();
            List<ToDoStatusViewModel> toDoStatuses = new List<ToDoStatusViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM ToDoStatuses " +
                        "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage;", db);

                    selectCommand.Parameters.AddWithValue("@Name", ((object)filterString) != null ? "%" + filterString + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", ((object)filterString) != null ? companyId : 0);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", 100);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                        toDoStatuses.Add(Read(query));
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.ToDoStatuses = new List<ToDoStatusViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.ToDoStatuses = toDoStatuses;
            return response;
        }

        public ToDoStatusResponse GetToDoStatus(Guid identifier)
        {
            ToDoStatusResponse response = new ToDoStatusResponse();
            ToDoStatusViewModel toDoStatus = null;

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM ToDoStatuses " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                        toDoStatus = Read(query);
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.ToDoStatus = new ToDoStatusViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.ToDoStatus = toDoStatus;
            return response;
        }

        #endregion

        #region Sync

        public void Sync(IToDoStatusService toDoStatusService, Action<int, int> callback = null)
        {
            try
            {
                SyncToDoStatusRequest request = new SyncToDoStatusRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                ToDoStatusListResponse response = toDoStatusService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.ToDoStatuses?.Count ?? 0;
                    List<ToDoStatusViewModel> toDoStatusesFromDB = response.ToDoStatuses;

                    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM ToDoStatuses WHERE Identifier = @Identifier";

                            SqliteCommand insertCommand = db.CreateCommand();
                            insertCommand.CommandText = SqlCommandInsertPart;

                            foreach (var toDoStatus in toDoStatusesFromDB)
                            {
                                deleteCommand.Parameters.AddWithValue("@Identifier", toDoStatus.Identifier);
                                deleteCommand.ExecuteNonQuery();
                                deleteCommand.Parameters.Clear();

                                if (toDoStatus.IsActive)
                                {
                                    toDoStatus.IsSynced = true;

                                    insertCommand = AddCreateParameters(insertCommand, toDoStatus);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from ToDoStatuses WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from ToDoStatuses WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
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

        public ToDoStatusResponse Create(ToDoStatusViewModel toDoStatus)
        {
            ToDoStatusResponse response = new ToDoStatusResponse();

            using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
            {
                db.Open();

                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, toDoStatus);
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

        public ToDoStatusResponse Delete(Guid identifier)
        {
            ToDoStatusResponse response = new ToDoStatusResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM ToDoStatuses WHERE Identifier = @Identifier";
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