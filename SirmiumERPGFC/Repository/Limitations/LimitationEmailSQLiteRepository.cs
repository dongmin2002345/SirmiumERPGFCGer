using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Limitations;
using ServiceInterfaces.Messages.Limitations;
using ServiceInterfaces.ViewModels.Limitations;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;

namespace SirmiumERPGFC.Repository.Limitations
{
    public class LimitationEmailSQLiteRepository
    {
        #region SQL

        public static string LimitationEmailTableCreatePart =
                   "CREATE TABLE IF NOT EXISTS LimitationEmails " +
                   "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                   "ServerId INTEGER NULL, " +
                   "Identifier GUID, " +
                   "Name NVARCHAR(48) NULL, " +
                   "LastName NVARCHAR(48) NULL, " +
                   "Email NVARCHAR(48) NULL, " +
                   "IsSynced BOOL NULL, " +
                   "UpdatedAt DATETIME NULL, " +
                   "CreatedById INTEGER NULL, " +
                   "CreatedByName NVARCHAR(2048) NULL, " +
                   "CompanyId INTEGER NULL, " +
                   "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, Name, LastName, Email, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO LimitationEmails " +
            "(Id, ServerId, Identifier, Name, LastName, Email, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @Name, @LastName, @Email, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods

        private LimitationEmailViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            LimitationEmailViewModel dbEntry = new LimitationEmailViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
            dbEntry.LastName = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Email = SQLiteHelper.GetString(query, ref counter);
            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);

            return dbEntry;
        }

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, LimitationEmailViewModel LimitationEmail)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", LimitationEmail.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", LimitationEmail.Identifier);
            insertCommand.Parameters.AddWithValue("@Name", ((object)LimitationEmail.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@LastName", ((object)LimitationEmail.LastName) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Email", ((object)LimitationEmail.Email) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsSynced", LimitationEmail.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)LimitationEmail.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }
        #endregion

        #region Read

        public LimitationEmailListResponse GetLimitationEmailsByPage(int companyId, LimitationEmailViewModel LimitationEmailSearchObject, int currentPage = 1, int itemsPerPage = 50)
        {
            LimitationEmailListResponse response = new LimitationEmailListResponse();
            List<LimitationEmailViewModel> LimitationEmails = new List<LimitationEmailViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM LimitationEmails " +
                        "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name OR LastName LIKE @Name) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage OFFSET @Offset;", db);
                    
                    selectCommand.Parameters.AddWithValue("@Name", ((object)LimitationEmailSearchObject.Search_Name) != null ? "%" + LimitationEmailSearchObject.Search_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
                    selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                        LimitationEmails.Add(Read(query));
                    


                    selectCommand = new SqliteCommand(
                        "SELECT Count(*) " +
                        "FROM LimitationEmails " +
                        "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name OR LastName LIKE @Name) " +
                        "AND CompanyId = @CompanyId;", db);
                    
                    selectCommand.Parameters.AddWithValue("@Name", ((object)LimitationEmailSearchObject.Search_Name) != null ? "%" + LimitationEmailSearchObject.Search_Name + "%" : "");
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
                    response.LimitationEmails = new List<LimitationEmailViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.LimitationEmails = LimitationEmails;
            return response;
        }

        public LimitationEmailResponse GetLimitationEmail(Guid identifier)
        {
            LimitationEmailResponse response = new LimitationEmailResponse();
            LimitationEmailViewModel LimitationEmail = new LimitationEmailViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM LimitationEmails " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                        LimitationEmail = Read(query);
                    
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.LimitationEmail = new LimitationEmailViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.LimitationEmail = LimitationEmail;
            return response;
        }

        #endregion

        #region Sync

        public void Sync(ILimitationEmailService LimitationEmailService, Action<int, int> callback = null)
        {
            try
            {
                SyncLimitationEmailRequest request = new SyncLimitationEmailRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                LimitationEmailListResponse response = LimitationEmailService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.LimitationEmails?.Count ?? 0;
                    List<LimitationEmailViewModel> limitationEmailsFromDB = response.LimitationEmails;

                    using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM LimitationEmails WHERE Identifier = @Identifier";

                            SqliteCommand insertCommand = db.CreateCommand();
                            insertCommand.CommandText = SqlCommandInsertPart;

                            foreach (var limitationEmail in limitationEmailsFromDB)
                            {
                                deleteCommand.Parameters.AddWithValue("@Identifier", limitationEmail.Identifier);
                                deleteCommand.ExecuteNonQuery();
                                deleteCommand.Parameters.Clear();

                                if (limitationEmail.IsActive)
                                {
                                    limitationEmail.IsSynced = true;

                                    insertCommand = AddCreateParameters(insertCommand, limitationEmail);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from LimitationEmails WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from LimitationEmails WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
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

        #endregion

        #region Create

        public LimitationEmailResponse Create(LimitationEmailViewModel LimitationEmail)
        {
            LimitationEmailResponse response = new LimitationEmailResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, LimitationEmail);
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

        public LimitationEmailResponse Delete(Guid identifier)
        {
            LimitationEmailResponse response = new LimitationEmailResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM LimitationEmails WHERE Identifier = @Identifier";
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

        public LimitationEmailResponse DeleteAll()
        {
            LimitationEmailResponse response = new LimitationEmailResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM LimitationEmails";
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
