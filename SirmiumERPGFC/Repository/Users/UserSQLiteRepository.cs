using Microsoft.Data.Sqlite;
using ServiceInterfaces.Messages.Common.Identity;
using ServiceInterfaces.ViewModels.Common.Identity;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.Users
{
    public class UserSQLiteRepository
    {
        public static string UserTableCreatePart =
            "CREATE TABLE IF NOT EXISTS Users (" +
            "Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
            "Identifier GUID, " +
            "ServerId INTEGER, " +
            "Username NVARCHAR(2048), " +
            "FirstName NVARCHAR(2048) NULL, " +
            "LastName NVARCHAR(2048) NULL, " +
            "Password NVARCHAR(2048) NULL, " +
            "Roles NVARCHAR(2048) NULL, " +
            "Email NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, Username, FirstName, LastName, Password, Roles, Email ";

        public string SqlCommandInsertPart = "INSERT INTO Users " +
            "(Id, ServerId, Identifier, Username, FirstName, LastName, Password, Email) " +
            "VALUES (NULL, @ServerId, @Identifier, @Username, @FirstName, @LastName, @Password, @Email);";

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
                        dbEntry.Username = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.FirstName = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.LastName = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Password = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Roles = SQLiteHelper.GetString(query, ref counter).Split(',').ToList();
                        dbEntry.Email = SQLiteHelper.GetString(query, ref counter);
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
                MainWindow.ErrorMessage = error.Message;
                return response;
            }
            response.Success = true;
            response.Users = users;
            return response;
        }

        public UserResponse Authenticate(string username, string password)
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
                        "where Username = @Username AND Password = @Password", db);
                    selectCommand.Parameters.AddWithValue("@Username", username);
                    selectCommand.Parameters.AddWithValue("@Password", password);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        int counter = 0;
                        UserViewModel dbEntry = new UserViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Username = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.FirstName = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.LastName = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Password = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Roles = SQLiteHelper.GetString(query, ref counter).Split(',').ToList();
                        dbEntry.Email = SQLiteHelper.GetString(query, ref counter);
                        user = dbEntry;
                    }

                    db.Close();
                }
            }
            catch (SqliteException error)
            {
                response.Success = false;
                response.Message = error.Message;
                MainWindow.ErrorMessage = error.Message;
                return response;
            }
            response.Success = true;
            response.User = user;
            return response;
        }

        public UserResponse Create(UserViewModel user)
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
                    insertCommand.CommandText = SqlCommandInsertPart;

                    insertCommand.Parameters.AddWithValue("@ServerId", user.Id);
                    insertCommand.Parameters.AddWithValue("@Identifier", user.Identifier);
                    insertCommand.Parameters.AddWithValue("@Username", ((object)user.Username) ?? DBNull.Value);
                    insertCommand.Parameters.AddWithValue("@FirstName", ((object)user.FirstName) ?? DBNull.Value);
                    insertCommand.Parameters.AddWithValue("@LastName", ((object)user.LastName) ?? DBNull.Value);
                    insertCommand.Parameters.AddWithValue("@Password", ((object)user.Password) ?? DBNull.Value);
                    insertCommand.Parameters.AddWithValue("@Roles", ((object)user.RolesCSV) ?? DBNull.Value);
                    insertCommand.Parameters.AddWithValue("@Email", ((object)user.Email) ?? DBNull.Value);

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

