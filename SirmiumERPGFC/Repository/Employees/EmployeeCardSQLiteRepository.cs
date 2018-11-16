using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.Employees
{
    public class EmployeeCardSQLiteRepository
    {
        public static string EmployeeCardTableCreatePart =
                     "CREATE TABLE IF NOT EXISTS EmployeeCards " +
                     "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                     "ServerId INTEGER NULL, " +
                     "Identifier GUID, " +
                     "EmployeeId INTEGER NULL, " +
                     "EmployeeIdentifier GUID NULL, " +
                     "EmployeeCode NVARCHAR(48) NULL, " +
                     "EmployeeName NVARCHAR(48) NULL, " +
                     "CardDate DATETIME NULL, " +
                     "Description NVARCHAR(2048) NULL, " +
                     "IsSynced BOOL NULL, " +
                     "UpdatedAt DATETIME NULL, " +
                     "CreatedById INTEGER NULL, " +
                     "CreatedByName NVARCHAR(2048) NULL, " +
                     "CompanyId INTEGER NULL, " +
                     "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, EmployeeId, EmployeeIdentifier, " +
            "EmployeeCode, EmployeeName, CardDate, Description, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO EmployeeCards " +
            "(Id, ServerId, Identifier, EmployeeId, EmployeeIdentifier, " +
            "EmployeeCode, EmployeeName, CardDate, Description, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @EmployeeId, @EmployeeIdentifier, " +
            "@EmployeeCode, @EmployeeName, @CardDate, @Description, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        public EmployeeCardListResponse GetEmployeeCardsByEmployee(int companyId, Guid EmployeeIdentifier)
        {
            EmployeeCardListResponse response = new EmployeeCardListResponse();
            List<EmployeeCardViewModel> EmployeeCards = new List<EmployeeCardViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM EmployeeCards " +
                        "WHERE EmployeeIdentifier = @EmployeeIdentifier " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC;", db);
                    selectCommand.Parameters.AddWithValue("@EmployeeIdentifier", EmployeeIdentifier);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        EmployeeCardViewModel dbEntry = new EmployeeCardViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Employee = SQLiteHelper.GetEmployee(query, ref counter);
                        dbEntry.CardDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.Description = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        EmployeeCards.Add(dbEntry);
                    }

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.EmployeeCards = new List<EmployeeCardViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.EmployeeCards = EmployeeCards;
            return response;
        }

        public EmployeeCardResponse GetEmployeeCard(Guid identifier)
        {
            EmployeeCardResponse response = new EmployeeCardResponse();
            EmployeeCardViewModel EmployeeCard = new EmployeeCardViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM EmployeeCards " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        int counter = 0;
                        EmployeeCardViewModel dbEntry = new EmployeeCardViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Employee = SQLiteHelper.GetEmployee(query, ref counter);
                        dbEntry.CardDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.Description = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        EmployeeCard = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.EmployeeCard = new EmployeeCardViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.EmployeeCard = EmployeeCard;
            return response;
        }

        public EmployeeCardListResponse GetUnSyncedCards(int companyId)
        {
            EmployeeCardListResponse response = new EmployeeCardListResponse();
            List<EmployeeCardViewModel> viewModels = new List<EmployeeCardViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM  EmployeeCards " +
                        "WHERE CompanyId = @CompanyId AND IsSynced = 0 " +
                        "ORDER BY Id DESC;", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        EmployeeCardViewModel dbEntry = new EmployeeCardViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Employee = SQLiteHelper.GetEmployee(query, ref counter);
                        dbEntry.CardDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.Description = SQLiteHelper.GetString(query, ref counter);
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
                    response.EmployeeCards = new List<EmployeeCardViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.EmployeeCards = viewModels;
            return response;
        }

        public void Sync(IEmployeeCardService EmployeeCardService)
        {
            var unSynced = GetUnSyncedCards(MainWindow.CurrentCompanyId);
            SyncEmployeeCardRequest request = new SyncEmployeeCardRequest();
            request.CompanyId = MainWindow.CurrentCompanyId;
            request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

            EmployeeCardListResponse response = EmployeeCardService.Sync(request);
            if (response.Success)
            {
                List<EmployeeCardViewModel> EmployeeCardsFromDB = response.EmployeeCards;
                foreach (var EmployeeCard in EmployeeCardsFromDB.OrderBy(x => x.Id))
                {
                    Delete(EmployeeCard.Identifier);
                    if (EmployeeCard.IsActive)
                    {
                        EmployeeCard.IsSynced = true;
                        Create(EmployeeCard);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from EmployeeCards WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from EmployeeCards WHERE CompanyId = @CompanyId", db);
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

        public EmployeeCardResponse Create(EmployeeCardViewModel EmployeeCard)
        {
            EmployeeCardResponse response = new EmployeeCardResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = SqlCommandInsertPart;

                insertCommand.Parameters.AddWithValue("@ServerId", EmployeeCard.Id);
                insertCommand.Parameters.AddWithValue("@Identifier", EmployeeCard.Identifier);
                insertCommand.Parameters.AddWithValue("@EmployeeId", ((object)EmployeeCard.Employee.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@EmployeeIdentifier", ((object)EmployeeCard.Employee.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@EmployeeCode", ((object)EmployeeCard.Employee.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@EmployeeName", ((object)EmployeeCard.Employee.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CardDate", ((object)EmployeeCard.CardDate) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Description", ((object)EmployeeCard.Description) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IsSynced", EmployeeCard.IsSynced);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", EmployeeCard.UpdatedAt);
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

        public EmployeeCardResponse UpdateSyncStatus(Guid identifier, int serverId, bool isSynced)
        {
            EmployeeCardResponse response = new EmployeeCardResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "UPDATE EmployeeCards SET " +
                    "IsSynced = @IsSynced, " +
                    "ServerId = @ServerId " +
                    "WHERE Identifier = @Identifier ";

                insertCommand.Parameters.AddWithValue("@IsSynced", isSynced);
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

        public EmployeeCardResponse Delete(Guid identifier)
        {
            EmployeeCardResponse response = new EmployeeCardResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "DELETE FROM EmployeeCards WHERE Identifier = @Identifier";
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

        public EmployeeCardResponse DeleteAll()
        {
            EmployeeCardResponse response = new EmployeeCardResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM EmployeeCards";
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
