﻿using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.ToDos;
using ServiceInterfaces.Messages.Common.ToDos;
using ServiceInterfaces.ViewModels.Common.ToDos;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;

namespace SirmiumERPGFC.Repository.ToDos
{
    public class ToDoSQLiteRepository
    {
        #region SQL

        public static string ToDoTableCreatePart =
            "CREATE TABLE IF NOT EXISTS ToDos " +
            "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
            "ServerId INTEGER NULL, " +
            "Identifier GUID, " +
            "Name NVARCHAR(48) NULL, " +
            "Description NVARCHAR(2048) NULL, " +
            "Path NVARCHAR(2048) NULL, " +
            "ToDoDate DATETIME NULL, " +
            "IsPrivate BOOL NULL, " +

            "ToDoStatusId INTEGER NULL, " +
            "ToDoStatusIdentifier GUID NULL, " +
            "ToDoStatusCode NVARCHAR(2048) NULL, " +
            "ToDoStatusName NVARCHAR(2048) NULL, " +

            "UserId INTEGER NULL, " +
            "UserIdentifier GUID NULL, " +
            "UserCode NVARCHAR(2048) NULL, " +
            "UserFirstName NVARCHAR(2048) NULL, " +
            "UserLastName NVARCHAR(2048) NULL, " +

            "IsSynced BOOL NULL, " +
            "UpdatedAt DATETIME NULL, " +
            "CreatedById INTEGER NULL, " +
            "CreatedByName NVARCHAR(2048) NULL, " +
            "CompanyId INTEGER NULL, " +
            "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, Name, Description, Path, ToDoDate, IsPrivate, " +
            "ToDoStatusId, ToDoStatusIdentifier, ToDoStatusCode, ToDoStatusName, " +
            "UserId, UserIdentifier, UserCode, UserFirstName, UserLastName, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO ToDos " +
            "(Id, ServerId, Identifier, Name, Description, Path, ToDoDate, IsPrivate, " +
            "ToDoStatusId, ToDoStatusIdentifier, ToDoStatusCode, ToDoStatusName, " +
            "UserId, UserIdentifier, UserCode, UserFirstName, UserLastName, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @Name, @Description, @Path, @ToDoDate, @IsPrivate, " +
            "@ToDoStatusId, @ToDoStatusIdentifier, @ToDoStatusCode, @ToDoStatusName, " +
            "@UserId, @UserIdentifier, @UserCode, @UserFirstName, @UserLastName, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods

        private ToDoViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            ToDoViewModel dbEntry = new ToDoViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Description = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Path = SQLiteHelper.GetString(query, ref counter);
            dbEntry.ToDoDate = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.IsPrivate = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.ToDoStatus = SQLiteHelper.GetToDoStatus(query, ref counter);
            dbEntry.User = SQLiteHelper.GetUser(query, ref counter);

            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);

            return dbEntry;
        }

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, ToDoViewModel toDo)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", toDo.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", toDo.Identifier);
            insertCommand.Parameters.AddWithValue("@Name", ((object)toDo.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Description", ((object)toDo.Description) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Path", ((object)toDo.Path) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ToDoDate", ((object)toDo.ToDoDate) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsPrivate", ((object)toDo.IsPrivate) ?? DBNull.Value);

            insertCommand.Parameters.AddWithValue("@ToDoStatusId", ((object)toDo.ToDoStatus?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ToDoStatusIdentifier", ((object)toDo.ToDoStatus?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ToDoStatusCode", ((object)toDo.ToDoStatus?.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ToDoStatusName", ((object)toDo.ToDoStatus?.Name) ?? DBNull.Value);

            insertCommand.Parameters.AddWithValue("@UserId", ((object)toDo.User?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@UserIdentifier", ((object)toDo.User?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@UserCode", ((object)toDo.User?.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@UserFirstName", ((object)toDo.User?.FirstName) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@UserLastName", ((object)toDo.User?.LastName) ?? DBNull.Value);

            insertCommand.Parameters.AddWithValue("@IsSynced", toDo.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)toDo.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read

        public ToDoListResponse GetToDos(int companyId, string filterString)
        {
            ToDoListResponse response = new ToDoListResponse();
            List<ToDoViewModel> ToDos = new List<ToDoViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM ToDos " +
                        "WHERE CompanyId = @CompanyId " +
                        "AND IsPrivate = 0 " + 
                        "ORDER BY IsSynced, Id DESC;", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                        ToDos.Add(Read(query));
                    
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.ToDos = new List<ToDoViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.ToDos = ToDos;
            return response;
        }

        public ToDoListResponse GetPrivateToDos(int companyId, string filterString)
        {
            ToDoListResponse response = new ToDoListResponse();
            List<ToDoViewModel> ToDos = new List<ToDoViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM ToDos " +
                        "WHERE CompanyId = @CompanyId " +
                        "AND IsPrivate = 1 " +
                        "AND CreatedById = @CreatedById " + 
                        "ORDER BY IsSynced, Id DESC;", db);
                    
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    selectCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUserId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                        ToDos.Add(Read(query));
                    
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.ToDos = new List<ToDoViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.ToDos = ToDos;
            return response;
        }

        public ToDoResponse GetToDo(Guid identifier)
        {
            ToDoResponse response = new ToDoResponse();
            ToDoViewModel toDo = new ToDoViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM ToDos " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                        toDo = Read(query);
                    
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.ToDo = new ToDoViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.ToDo = toDo;
            return response;
        }

        #endregion

        #region Sync

        public void Sync(IToDoService toDoService, Action<int, int> callback = null)
        {
            try
            {
                SyncToDoRequest request = new SyncToDoRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                ToDoListResponse response = toDoService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.ToDos?.Count ?? 0;
                    List<ToDoViewModel> toDosFromDB = response.ToDos;

                    using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM ToDos WHERE Identifier = @Identifier";

                            SqliteCommand insertCommand = db.CreateCommand();
                            insertCommand.CommandText = SqlCommandInsertPart;

                            foreach (var toDo in toDosFromDB)
                            {
                                deleteCommand.Parameters.AddWithValue("@Identifier", toDo.Identifier);
                                deleteCommand.ExecuteNonQuery();
                                deleteCommand.Parameters.Clear();

                                if (toDo.IsActive)
                                {
                                    toDo.IsSynced = true;

                                    insertCommand = AddCreateParameters(insertCommand, toDo);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from ToDos WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from ToDos WHERE CompanyId = @CompanyId", db);
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

        public ToDoResponse Create(ToDoViewModel toDo)
        {
            ToDoResponse response = new ToDoResponse();

            using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
            {
                db.Open();

                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, toDo);
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

        public ToDoResponse Delete(Guid identifier)
        {
            ToDoResponse response = new ToDoResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM ToDos WHERE Identifier = @Identifier";
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

        public ToDoResponse DeleteAll()
        {
            ToDoResponse response = new ToDoResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM ToDos";
                    try
                    {
                        insertCommand.ExecuteNonQuery();
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

        #endregion
    }
}
