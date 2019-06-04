using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.BusinessPartners
{
    public class BusinessPartnerBankSQLiteRepository
    {
        public static string BusinessPartnerBankTableCreatePart =
               "CREATE TABLE IF NOT EXISTS BusinessPartnerBanks " +
               "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
               "ServerId INTEGER NULL, " +
               "Identifier GUID, " +
               "BusinessPartnerId INTEGER NULL, " +
               "BusinessPartnerIdentifier GUID NULL, " +
               "BusinessPartnerCode NVARCHAR(2048) NULL, " +
               "BusinessPartnerName NVARCHAR(2048) NULL, " +
               "BusinessPartnerInternalCode NVARCHAR(2048) NULL, " +
               "BusinessPartnerNameGer NVARCHAR(2048) NULL, " +
               "BankId INTEGER NULL, " +
               "BankIdentifier GUID NULL, " +
               "BankCode NVARCHAR(2048) NULL, " +
               "BankName NVARCHAR(2048) NULL, " +
               "CountryId INTEGER NULL, " +
               "CountryIdentifier GUID NULL, " +
               "CountryCode NVARCHAR(2048) NULL, " +
               "CountryName NVARCHAR(2048) NULL, " +
               "AccountNumber NVARCHAR(2048) NULL, " +
               "IsSynced BOOL NULL, " +
               "UpdatedAt DATETIME NULL, " +
               "CreatedById INTEGER NULL, " +
               "CreatedByName NVARCHAR(2048) NULL, " +
               "CompanyId INTEGER NULL, " +
               "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, " + 
            "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, BusinessPartnerInternalCode, BusinessPartnerNameGer, " +
            "BankId, BankIdentifier, BankCode, BankName, " +
            "CountryId, CountryIdentifier, CountryCode, CountryName, " +
            "AccountNumber, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO BusinessPartnerBanks " +
            "(Id, ServerId, Identifier, " +
            "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, BusinessPartnerInternalCode, BusinessPartnerNameGer, " +
            "BankId, BankIdentifier, BankCode, BankName, " +
            "CountryId, CountryIdentifier, CountryCode, CountryName, " +
            "AccountNumber, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, " +
            "@BusinessPartnerId, @BusinessPartnerIdentifier, @BusinessPartnerCode, @BusinessPartnerName, @BusinessPartnerInternalCode, @BusinessPartnerNameGer, " +
            "@BankId, @BankIdentifier, @BankCode, @BankName, " +
            "@CountryId, @CountryIdentifier, @CountryCode, @CountryName, " +
            "@AccountNumber, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        public BusinessPartnerBankListResponse GetBusinessPartnerBanksByBusinessPartner(int companyId, Guid businessPartnerIdentifier)
        {
            BusinessPartnerBankListResponse response = new BusinessPartnerBankListResponse();
            List<BusinessPartnerBankViewModel> businessPartnerBanks = new List<BusinessPartnerBankViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM BusinessPartnerBanks " +
                        "WHERE BusinessPartnerIdentifier = @BusinessPartnerIdentifier " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC;", db);
                    selectCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", businessPartnerIdentifier);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        BusinessPartnerBankViewModel dbEntry = new BusinessPartnerBankViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.BusinessPartner = SQLiteHelper.GetBusinessPartner(query, ref counter);
                        dbEntry.Bank = SQLiteHelper.GetBank(query, ref counter);
                        dbEntry.Country = SQLiteHelper.GetCountry(query, ref counter);
                        dbEntry.AccountNumber = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        businessPartnerBanks.Add(dbEntry);
                    }

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.BusinessPartnerBanks = new List<BusinessPartnerBankViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.BusinessPartnerBanks = businessPartnerBanks;
            return response;
        }

        public BusinessPartnerBankResponse GetBusinessPartnerBank(Guid identifier)
        {
            BusinessPartnerBankResponse response = new BusinessPartnerBankResponse();
            BusinessPartnerBankViewModel businessPartnerBank = new BusinessPartnerBankViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM BusinessPartnerBanks " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        int counter = 0;
                        BusinessPartnerBankViewModel dbEntry = new BusinessPartnerBankViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.BusinessPartner = SQLiteHelper.GetBusinessPartner(query, ref counter);
                        dbEntry.Bank = SQLiteHelper.GetBank(query, ref counter);
                        dbEntry.Country = SQLiteHelper.GetCountry(query, ref counter);
                        dbEntry.AccountNumber = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        businessPartnerBank = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.BusinessPartnerBank = new BusinessPartnerBankViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.BusinessPartnerBank = businessPartnerBank;
            return response;
        }

        public void Sync(IBusinessPartnerBankService BusinessPartnerBankService, Action<int, int> callback = null)
        {
            try
            {
                SyncBusinessPartnerBankRequest request = new SyncBusinessPartnerBankRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                BusinessPartnerBankListResponse response = BusinessPartnerBankService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.BusinessPartnerBanks?.Count ?? 0;
                    List<BusinessPartnerBankViewModel> BusinessPartnerBanksFromDB = response.BusinessPartnerBanks;
                    foreach (var BusinessPartnerBank in BusinessPartnerBanksFromDB.OrderBy(x => x.Id))
                    {
                        ThreadPool.QueueUserWorkItem((k) =>
                        {
                            Delete(BusinessPartnerBank.Identifier);
                            if (BusinessPartnerBank.IsActive)
                            {
                                BusinessPartnerBank.IsSynced = true;
                                Create(BusinessPartnerBank);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from BusinessPartnerBanks WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from BusinessPartnerBanks WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
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

        public BusinessPartnerBankResponse Create(BusinessPartnerBankViewModel businessPartnerBank)
        {
            BusinessPartnerBankResponse response = new BusinessPartnerBankResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = SqlCommandInsertPart;

                insertCommand.Parameters.AddWithValue("@ServerId", businessPartnerBank.Id);
                insertCommand.Parameters.AddWithValue("@Identifier", businessPartnerBank.Identifier);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerId", ((object)businessPartnerBank.BusinessPartner.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", ((object)businessPartnerBank.BusinessPartner.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerCode", ((object)businessPartnerBank.BusinessPartner.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerName", ((object)businessPartnerBank.BusinessPartner.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerInternalCode", ((object)businessPartnerBank.BusinessPartner.InternalCode) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerNameGer", ((object)businessPartnerBank.BusinessPartner.NameGer) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BankId", ((object)businessPartnerBank.Bank.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BankIdentifier", ((object)businessPartnerBank.Bank.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BankCode", ((object)businessPartnerBank.Bank.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BankName", ((object)(businessPartnerBank.Bank.Name + ", Swift:" + businessPartnerBank.Bank.Swift)) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CountryId", ((object)businessPartnerBank.Country.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CountryIdentifier", ((object)businessPartnerBank.Country.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CountryCode", ((object)businessPartnerBank.Country.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CountryName", ((object)businessPartnerBank.Country.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@AccountNumber", ((object)businessPartnerBank.AccountNumber) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IsSynced", businessPartnerBank.IsSynced);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)businessPartnerBank.UpdatedAt) ?? DBNull.Value);
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

        public BusinessPartnerBankResponse UpdateSyncStatus(Guid identifier, int serverId, string code, DateTime? updatedAt, bool isSynced)
        {
            BusinessPartnerBankResponse response = new BusinessPartnerBankResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "UPDATE BusinessPartnerBanks SET " +
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

        public BusinessPartnerBankResponse Delete(Guid identifier)
        {
            BusinessPartnerBankResponse response = new BusinessPartnerBankResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "DELETE FROM BusinessPartnerBanks WHERE Identifier = @Identifier";
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

        public BusinessPartnerBankResponse DeleteAll()
        {
            BusinessPartnerBankResponse response = new BusinessPartnerBankResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM BusinessPartnerBanks";
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
