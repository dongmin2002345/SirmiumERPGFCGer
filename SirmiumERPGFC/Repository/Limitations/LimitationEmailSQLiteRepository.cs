using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Limitations;
using ServiceInterfaces.Messages.Limitations;
using ServiceInterfaces.ViewModels.Limitations;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.Limitations
{
    public class LimitationEmailSQLiteRepository
    {
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
                        LimitationEmails.Add(dbEntry);
                    }


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
                        LimitationEmail = dbEntry;
                    }
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

        public void Sync(ILimitationEmailService LimitationEmailService)
        {
            SyncLimitationEmailRequest request = new SyncLimitationEmailRequest();
            request.CompanyId = MainWindow.CurrentCompanyId;
            request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

            LimitationEmailListResponse response = LimitationEmailService.Sync(request);
            if (response.Success)
            {
                List<LimitationEmailViewModel> LimitationEmailsFromDB = response.LimitationEmails;
                foreach (var LimitationEmail in LimitationEmailsFromDB.OrderBy(x => x.Id))
                {
                    Delete(LimitationEmail.Identifier);
                    if (LimitationEmail.IsActive)
                    {
                        LimitationEmail.IsSynced = true;
                        Create(LimitationEmail);
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

        public LimitationEmailResponse Create(LimitationEmailViewModel LimitationEmail)
        {
            LimitationEmailResponse response = new LimitationEmailResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = SqlCommandInsertPart;

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

        public LimitationEmailResponse UpdateSyncStatus(Guid identifier, DateTime? updatedAt, int serverId, bool isSynced)
        {
            LimitationEmailResponse response = new LimitationEmailResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "UPDATE LimitationEmails SET " +
                    "IsSynced = @IsSynced, " +
                    "UpdatedAt = @UpdatedAt, " +
                    "ServerId = @ServerId " +
                    "WHERE Identifier = @Identifier ";

                insertCommand.Parameters.AddWithValue("@IsSynced", isSynced);
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

        public LimitationEmailResponse Delete(Guid identifier)
        {
            LimitationEmailResponse response = new LimitationEmailResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "DELETE FROM LimitationEmails WHERE Identifier = @Identifier";
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
