using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.Companies;
using ServiceInterfaces.Helpers;
using ServiceInterfaces.Messages.Common.Companies;
using ServiceInterfaces.Messages.Common.Identity;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.Companies
{
    public class CompanyUserSQLiteRepository
    {
        public static string CompanyUserTableCreatePart =
                  "CREATE TABLE IF NOT EXISTS CompanyUsers " +
                  "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                  "ServerId INTEGER NULL, " +
                  "Identifier GUID, " +
                  "UserId INTEGER NULL, " +
                  "UserIdentifier GUID NULL, " +
                  "UserName NVARCHAR(2048) NULL, " +
                  "UserRoles NVARCHAR(2048) NULL, " +
                  "IsSynced BOOL NULL, " +
                  "UpdatedAt DATETIME NULL, " +
                  "CompanyId INTEGER NULL, " +
                  "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, UserId, UserIdentifier, UserName, " +
            "UserRoles, " +
            "IsSynced, UpdatedAt, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO CompanyUsers " +
            "(Id, ServerId, Identifier, UserId, UserIdentifier, UserName, " +
            "UserRoles, " +
            "IsSynced, UpdatedAt, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @UserId, @UserIdentifier, @UserName, " +
            "@UserRoles, " +
            "@IsSynced, @UpdatedAt, @CompanyId, @CompanyName) ";


        public CompanyUserResponse GetCompanyUser(int companyId, int userId)
        {
            CompanyUserResponse response = new CompanyUserResponse();
            CompanyUserViewModel user = null;
            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();

                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "from CompanyUsers " +
                        "WHERE @CompanyId = CompanyId AND @UserId = UserId ", db);

                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    selectCommand.Parameters.AddWithValue("@UserId", userId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        CompanyUserViewModel dbEntry = new CompanyUserViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.User = SQLiteHelper.GetCompanyUser(query, ref counter);

                        var userRoles = SQLiteHelper.GetString(query, ref counter)?.RolesFromCSV() ?? new List<UserRoleViewModel>();
                        dbEntry.UserRoles = userRoles;

                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);

                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);

                        user = dbEntry;
                    }

                    db.Close();
                }
            }
            catch (SqliteException error)
            {
                response.Success = false;
                response.Message = error.Message;
                response.CompanyUser = new CompanyUserViewModel();
                //MainWindow.ErrorMessage = error.Message;
                return response;
            }
            response.Success = true;
            response.CompanyUser = user;
            return response;
        }

        public CompanyUserResponse GetCompanyUser(int companyId, Guid identifier)
        {
            CompanyUserResponse response = new CompanyUserResponse();
            CompanyUserViewModel user = null;
            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();

                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "from CompanyUsers " +
                        "WHERE @CompanyId = CompanyId AND @UserIdentifier = UserIdentifier ", db);

                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    selectCommand.Parameters.AddWithValue("@UserIdentifier", ((object)identifier) ?? DBNull.Value);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        CompanyUserViewModel dbEntry = new CompanyUserViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.User = SQLiteHelper.GetCompanyUser(query, ref counter);

                        var userRoles = SQLiteHelper.GetString(query, ref counter)?.RolesFromCSV() ?? new List<UserRoleViewModel>();
                        dbEntry.UserRoles = userRoles;

                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);

                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);

                        user = dbEntry;
                    }

                    db.Close();
                }
            }
            catch (SqliteException error)
            {
                response.Success = false;
                response.Message = error.Message;
                response.CompanyUser = new CompanyUserViewModel();
                //MainWindow.ErrorMessage = error.Message;
                return response;
            }
            response.Success = true;
            response.CompanyUser = user;
            return response;
        }

        public CompanyUserListResponse GetCompanyUsers(Guid identifier)
        {
            CompanyUserListResponse response = new CompanyUserListResponse();
            List<CompanyUserViewModel> user = new List<CompanyUserViewModel>();
            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();

                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "from CompanyUsers " +
                        "WHERE @UserIdentifier = UserIdentifier ", db);

                    selectCommand.Parameters.AddWithValue("@UserIdentifier", ((object)identifier) ?? DBNull.Value);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        CompanyUserViewModel dbEntry = new CompanyUserViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.User = SQLiteHelper.GetCompanyUser(query, ref counter);

                        var userRoles = SQLiteHelper.GetString(query, ref counter)?.RolesFromCSV() ?? new List<UserRoleViewModel>();
                        dbEntry.UserRoles = userRoles;

                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);

                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);

                        user.Add(dbEntry);
                    }

                    db.Close();
                }
            }
            catch (SqliteException error)
            {
                response.Success = false;
                response.Message = error.Message;
                response.CompanyUsers = new List<CompanyUserViewModel>();
                //MainWindow.ErrorMessage = error.Message;
                return response;
            }
            response.Success = true;
            response.CompanyUsers = user;
            return response;
        }

        public CompanyUserResponse Create(CompanyUserViewModel viewModel)
        {
            CompanyUserResponse response = new CompanyUserResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = SqlCommandInsertPart;

                var userRoleStr = viewModel?.UserRoles?.RolesToCSV();

                insertCommand.Parameters.AddWithValue("@ServerId", viewModel.Id);
                insertCommand.Parameters.AddWithValue("@Identifier", viewModel.Identifier);
                insertCommand.Parameters.AddWithValue("@UserId", ((object)viewModel.User?.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@UserIdentifier", ((object)viewModel.User?.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@UserName", ((object)(viewModel.User?.FirstName + " " + viewModel.User?.LastName)) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@UserRoles", ((object)userRoleStr) ?? DBNull.Value);

                insertCommand.Parameters.AddWithValue("@IsSynced", viewModel.IsSynced);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)viewModel.UpdatedAt) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CompanyId", ((object)viewModel.Company?.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CompanyName", ((object)viewModel.Company?.CompanyName) ?? DBNull.Value);

                try
                {
                    insertCommand.ExecuteNonQuery();
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

                insertCommand.CommandText = "UPDATE CompanyUsers SET " +
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
                    insertCommand.ExecuteNonQuery();
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
                    "DELETE FROM CompanyUsers WHERE Identifier = @Identifier";
                insertCommand.Parameters.AddWithValue("@Identifier", identifier);
                try
                {
                    insertCommand.ExecuteNonQuery();
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
                    insertCommand.CommandText = "DELETE FROM CompanyUsers ";
                    try
                    {
                        insertCommand.ExecuteNonQuery();
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

        public DateTime? GetLastUpdatedAt()
        {
            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from CompanyUsers", db);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from CompanyUsers", db);
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

        public void Sync(ICompanyUserService userService)
        {
            SyncCompanyUserRequest request = new SyncCompanyUserRequest();
            request.LastUpdatedAt = GetLastUpdatedAt();

            CompanyUserListResponse response = userService.Sync(request);
            if (response.Success)
            {
                List<CompanyUserViewModel> usersFromDB = response.CompanyUsers;
                foreach (var user in usersFromDB.OrderBy(x => x.Id))
                {
                    Delete(user.Identifier);
                    user.IsSynced = true;
                    if (user.IsActive)
                        Create(user);
                }
            }
        }
    }
}
