using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.Identity;
using ServiceInterfaces.Messages.Common.Identity;
using ServiceInterfaces.ViewModels.Common.Identity;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.Users
{
    public class UserSQLiteRepository
    {
        public static string UserTableCreatePart =
             "CREATE TABLE IF NOT EXISTS Users " +
             "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
             "ServerId INTEGER NULL, " +
             "Identifier GUID, " +
             "Code NVARCHAR(48) NULL, " +
             "Username NVARCHAR(48) NULL, " +
             "FirstName NVARCHAR(48) NULL, " +
             "LastName NVARCHAR(48) NULL, " +
             "PasswordHash NVARCHAR(48) NULL, " +
             "Email NVARCHAR(48) NULL, " +
             "IsSynced BOOL NULL, " +
             "UpdatedAt DATETIME NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, Code, Username, FirstName, LastName, " +
            "PasswordHash, Email, " +
            "IsSynced, UpdatedAt ";

        public string SqlCommandInsertPart = "INSERT INTO Users " +
            "(Id, ServerId, Identifier, Code, Username, FirstName, LastName, " +
            "PasswordHash, Email, " +
            "IsSynced, UpdatedAt) " +

            "VALUES (NULL, @ServerId, @Identifier, @Code, @Username, @FirstName, @LastName, " +
            "@PasswordHash, @Email, " +
            "@IsSynced, @UpdatedAt)";

        public UserListResponse GetUsers()
        {
            UserListResponse response = new UserListResponse();
            List<UserViewModel> users = new List<UserViewModel>();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();

                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "from Users", db);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        UserViewModel dbEntry = new UserViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Username = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.FirstName = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.LastName = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Password = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Email = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        users.Add(dbEntry);
                    }

                    db.Close();
                }
            }
            catch (SqliteException error)
            {
                response.Success = false;
                response.Message = error.Message;
                response.Users = new List<UserViewModel>();
                //MainWindow.ErrorMessage = error.Message;
                return response;
            }
            response.Success = true;
            response.Users = users;
            return response;
        }

        public UserResponse Authenticate(string username, string password, int companyId)
        {
            UserResponse response = new UserResponse();
            UserViewModel user = null;

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();

                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "from Users " +
                        "where Username = @Username AND PasswordHash = @PasswordHash ", db);
                    selectCommand.Parameters.AddWithValue("@Username", username);
                    selectCommand.Parameters.AddWithValue("@PasswordHash", password);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        int counter = 0;
                        UserViewModel dbEntry = new UserViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Username = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.FirstName = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.LastName = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Password = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Email = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        user = dbEntry;
                    }

                    db.Close();
                }
            }
            catch (SqliteException error)
            {
                response.Success = false;
                response.Message = error.Message;
                //MainWindow.ErrorMessage = error.Message;
                return response;
            }
            response.Success = true;
            response.User = user;
            return response;
        }

        public UserListResponse GetUsersByPage(int companyId, UserViewModel userSearchObject, int currentPage = 1, int itemsPerPage = 50)
        {
            UserListResponse response = new UserListResponse();
            List<UserViewModel> Users = new List<UserViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Users " +
                        "WHERE (@Username IS NULL OR @Username = '' OR Username LIKE @Username) " +
                        "AND (@Email IS NULL OR @Email = '' OR Email LIKE @Email) " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage OFFSET @Offset;", db);
                    selectCommand.Parameters.AddWithValue("@Username", ((object)userSearchObject.Search_UserName) != null ? "%" + userSearchObject.Search_UserName + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Email", ((object)userSearchObject.Search_Email) != null ? "%" + userSearchObject.Search_Email + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
                    selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        UserViewModel dbEntry = new UserViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Username = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.FirstName = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.LastName = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Password = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Email = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        Users.Add(dbEntry);
                    }


                    selectCommand = new SqliteCommand(
                        "SELECT Count(*) " +
                        "FROM Users " +
                        "WHERE (@Username IS NULL OR @Username = '' OR Username LIKE @Username) " +
                        "AND (@Email IS NULL OR @Email = '' OR Email LIKE @Email);", db);
                    selectCommand.Parameters.AddWithValue("@Username", ((object)userSearchObject.Search_UserName) != null ? "%" + userSearchObject.Search_UserName + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Email", ((object)userSearchObject.Search_Email) != null ? "%" + userSearchObject.Search_Email + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    query = selectCommand.ExecuteReader();

                    if (query.Read())
                        response.TotalItems = query.GetInt32(0);
                }
                catch (SqliteException error)
                {
                    //MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.Users = new List<UserViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Users = Users;
            return response;
        }

        public UserListResponse GetUsersForPopup(int companyId, string filterString)
        {
            UserListResponse response = new UserListResponse();
            List<UserViewModel> Users = new List<UserViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Users " +
                        "WHERE (@Username IS NULL OR @Username = '' OR Username LIKE @Username) " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage;", db);
                    selectCommand.Parameters.AddWithValue("@Username", ((object)filterString) != null ? "%" + filterString + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", ((object)filterString) != null ? companyId : 0);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", 100);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        UserViewModel dbEntry = new UserViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Username = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.FirstName = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.LastName = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Password = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Email = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        Users.Add(dbEntry);
                    }
                }
                catch (SqliteException error)
                {
                    //MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.Users = new List<UserViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Users = Users;
            return response;
        }

        public UserResponse GetUser(Guid identifier)
        {
            UserResponse response = new UserResponse();
            UserViewModel User = new UserViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Users " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        int counter = 0;
                        UserViewModel dbEntry = new UserViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Username = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.FirstName = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.LastName = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Password = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Email = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        User = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    //MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.User = new UserViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.User = User;
            return response;
        }

        public void Sync(IUserService userService, Action<int, int> callback = null)
        {
            try
            {
                SyncUserRequest request = new SyncUserRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                UserListResponse response = userService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.Users?.Count ?? 0;
                    List<UserViewModel> usersFromDB = response.Users;
                    foreach (var user in usersFromDB.OrderBy(x => x.Id))
                    {
                            Delete(user.Identifier);
                            user.IsSynced = true;
                            if (user.IsActive)
                            {
                                user.IsSynced = true;
                                Create(user);
                                syncedItems++;
                                callback?.Invoke(syncedItems, toSync);
                            }
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from Users WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from Users WHERE CompanyId = @CompanyId", db);
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
                    //MainWindow.ErrorMessage = ex.Message;
                }
                db.Close();
            }
            return null;
        }

        public UserResponse Create(UserViewModel user)
        {
            UserResponse response = new UserResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = SqlCommandInsertPart;

                insertCommand.Parameters.AddWithValue("@ServerId", user.Id);
                insertCommand.Parameters.AddWithValue("@Identifier", user.Identifier);
                insertCommand.Parameters.AddWithValue("@Code", ((object)user.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Username", ((object)user.Username) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@FirstName", ((object)user.FirstName) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@LastName", ((object)user.LastName) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@PasswordHash", ((object)user.Password) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Email", ((object)user.Email) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IsSynced", user.IsSynced);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", user.UpdatedAt);

                try
                {
                    insertCommand.ExecuteReader();
                }
                catch (SqliteException error)
                {
                    //MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    return response;
                }
                db.Close();

                response.Success = true;
                return response;
            }
        }


        public UserResponse UpdateSyncStatus(Guid identifier, int serverId, bool isSynced, DateTime? lastUpdate)
        {
            UserResponse response = new UserResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "UPDATE Users SET " +
                    "IsSynced = @IsSynced, " +
                    "ServerId = @ServerId, " +
                    "UpdatedAt = @UpdatedAt " +
                    "WHERE Identifier = @Identifier ";

                insertCommand.Parameters.AddWithValue("@IsSynced", isSynced);
                insertCommand.Parameters.AddWithValue("@ServerId", serverId);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)lastUpdate) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Identifier", identifier);

                try
                {
                    insertCommand.ExecuteReader();
                }
                catch (SqliteException error)
                {
                    //MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    return response;
                }
                db.Close();

                response.Success = true;
                return response;
            }
        }

        public UserResponse Delete(Guid identifier)
        {
            UserResponse response = new UserResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "DELETE FROM Users WHERE Identifier = @Identifier";
                insertCommand.Parameters.AddWithValue("@Identifier", identifier);
                try
                {
                    insertCommand.ExecuteReader();
                }
                catch (SqliteException error)
                {
                    //MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    return response;
                }
                db.Close();

                response.Success = true;
                return response;
            }
        }

        public UserResponse DeleteAll()
        {
            UserResponse response = new UserResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM Users";
                    try
                    {
                        insertCommand.ExecuteReader();
                    }
                    catch (SqliteException error)
                    {
                        response.Success = false;
                        response.Message = error.Message;

                        //MainWindow.ErrorMessage = error.Message;
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

