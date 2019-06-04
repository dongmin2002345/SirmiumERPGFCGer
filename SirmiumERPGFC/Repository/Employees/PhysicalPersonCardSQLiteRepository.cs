using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.Employees
{
    public class PhysicalPersonCardSQLiteRepository
    {
        public static string PhysicalPersonCardTableCreatePart =
               "CREATE TABLE IF NOT EXISTS PhysicalPersonCards " +
               "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
               "ServerId INTEGER NULL, " +
               "Identifier GUID, " +
               "PhysicalPersonId INTEGER NULL, " +
               "PhysicalPersonIdentifier GUID NULL, " +
               "PhysicalPersonCode NVARCHAR(48) NULL, " +
               "PhysicalPersonName NVARCHAR(48) NULL, " +
               "CardDate DATETIME NULL, " +
               "Description NVARCHAR(2048) NULL, " +
               "PlusMinus NVARCHAR(48) NULL, " +
               "IsSynced BOOL NULL, " +
               "UpdatedAt DATETIME NULL, " +
               "CreatedById INTEGER NULL, " +
               "CreatedByName NVARCHAR(2048) NULL, " +
               "CompanyId INTEGER NULL, " +
               "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, PhysicalPersonId, PhysicalPersonIdentifier, " +
            "PhysicalPersonCode, PhysicalPersonName, CardDate, Description, PlusMinus, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO PhysicalPersonCards " +
            "(Id, ServerId, Identifier, PhysicalPersonId, PhysicalPersonIdentifier, " +
            "PhysicalPersonCode, PhysicalPersonName, CardDate, Description, PlusMinus, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @PhysicalPersonId, @PhysicalPersonIdentifier, " +
            "@PhysicalPersonCode, @PhysicalPersonName, @CardDate, @Description, @PlusMinus, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        public PhysicalPersonCardListResponse GetPhysicalPersonCardsByPhysicalPerson(int companyId, Guid PhysicalPersonIdentifier)
        {
            PhysicalPersonCardListResponse response = new PhysicalPersonCardListResponse();
            List<PhysicalPersonCardViewModel> PhysicalPersonCards = new List<PhysicalPersonCardViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM PhysicalPersonCards " +
                        "WHERE PhysicalPersonIdentifier = @PhysicalPersonIdentifier " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC;", db);
                    selectCommand.Parameters.AddWithValue("@PhysicalPersonIdentifier", PhysicalPersonIdentifier);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        PhysicalPersonCardViewModel dbEntry = new PhysicalPersonCardViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.PhysicalPerson = SQLiteHelper.GetPhysicalPerson(query, ref counter);
                        dbEntry.CardDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.Description = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.PlusMinus = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        PhysicalPersonCards.Add(dbEntry);
                    }

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.PhysicalPersonCards = new List<PhysicalPersonCardViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.PhysicalPersonCards = PhysicalPersonCards;
            return response;
        }

        public PhysicalPersonCardResponse GetPhysicalPersonCard(Guid identifier)
        {
            PhysicalPersonCardResponse response = new PhysicalPersonCardResponse();
            PhysicalPersonCardViewModel PhysicalPersonCard = new PhysicalPersonCardViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM PhysicalPersonCards " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        int counter = 0;
                        PhysicalPersonCardViewModel dbEntry = new PhysicalPersonCardViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.PhysicalPerson = SQLiteHelper.GetPhysicalPerson(query, ref counter);
                        dbEntry.CardDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.Description = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.PlusMinus = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        PhysicalPersonCard = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.PhysicalPersonCard = new PhysicalPersonCardViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.PhysicalPersonCard = PhysicalPersonCard;
            return response;
        }

        public void Sync(IPhysicalPersonCardService PhysicalPersonCardService, Action<int, int> callback = null)
        {
            try
            {
                SyncPhysicalPersonCardRequest request = new SyncPhysicalPersonCardRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                PhysicalPersonCardListResponse response = PhysicalPersonCardService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.PhysicalPersonCards?.Count ?? 0;
                    List<PhysicalPersonCardViewModel> PhysicalPersonCardsFromDB = response.PhysicalPersonCards;
                    foreach (var PhysicalPersonCard in PhysicalPersonCardsFromDB.OrderBy(x => x.Id))
                    {
                        ThreadPool.QueueUserWorkItem((k) =>
                        {
                            Delete(PhysicalPersonCard.Identifier);
                            if (PhysicalPersonCard.IsActive)
                            {
                                PhysicalPersonCard.IsSynced = true;
                                Create(PhysicalPersonCard);
                                syncedItems++;
                                callback?.Invoke(syncedItems, toSync);
                            }
                        });
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from PhysicalPersonCards WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from PhysicalPersonCards WHERE CompanyId = @CompanyId", db);
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

        public PhysicalPersonCardResponse Create(PhysicalPersonCardViewModel PhysicalPersonCard)
        {
            PhysicalPersonCardResponse response = new PhysicalPersonCardResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = SqlCommandInsertPart;

                insertCommand.Parameters.AddWithValue("@ServerId", PhysicalPersonCard.Id);
                insertCommand.Parameters.AddWithValue("@Identifier", PhysicalPersonCard.Identifier);
                insertCommand.Parameters.AddWithValue("@PhysicalPersonId", ((object)PhysicalPersonCard.PhysicalPerson.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@PhysicalPersonIdentifier", ((object)PhysicalPersonCard.PhysicalPerson.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@PhysicalPersonCode", ((object)PhysicalPersonCard.PhysicalPerson.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@PhysicalPersonName", ((object)PhysicalPersonCard.PhysicalPerson.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CardDate", ((object)PhysicalPersonCard.CardDate) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Description", ((object)PhysicalPersonCard.Description) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@PlusMinus", ((object)PhysicalPersonCard.PlusMinus) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IsSynced", PhysicalPersonCard.IsSynced);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)PhysicalPersonCard.UpdatedAt) ?? DBNull.Value);
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

        public PhysicalPersonCardResponse UpdateSyncStatus(Guid identifier, string code, DateTime? updatedAt, int serverId, bool isSynced)
        {
            PhysicalPersonCardResponse response = new PhysicalPersonCardResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "UPDATE PhysicalPersonCards SET " +
                    "IsSynced = @IsSynced, " +
                    "Code = @Code, " +
                    "UpdatedAt = @UpdatedAt, " +
                    "ServerId = @ServerId " +
                    "WHERE Identifier = @Identifier ";

                insertCommand.Parameters.AddWithValue("@IsSynced", isSynced);
                insertCommand.Parameters.AddWithValue("@Code", code);
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

        public PhysicalPersonCardResponse Delete(Guid identifier)
        {
            PhysicalPersonCardResponse response = new PhysicalPersonCardResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "DELETE FROM PhysicalPersonCards WHERE Identifier = @Identifier";
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

        public PhysicalPersonCardResponse DeleteAll()
        {
            PhysicalPersonCardResponse response = new PhysicalPersonCardResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM PhysicalPersonCards";
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
