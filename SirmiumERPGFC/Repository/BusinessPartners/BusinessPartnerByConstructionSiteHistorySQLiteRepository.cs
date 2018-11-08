using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.BusinessPartners
{
    public class BusinessPartnerByConstructionSiteHistorySQLiteRepository
    {
        public static string BusinessPartnerByConstructionSiteHistoryTableCreatePart =
               "CREATE TABLE IF NOT EXISTS BusinessPartnerByConstructionSiteHistories " +
               "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
               "ServerId INTEGER NULL, " +
               "Identifier GUID, " +
               "Code NVARCHAR(48) NULL, " +
               "StartDate DATETIME NULL, " +
               "BusinessPartnerId INTEGER NULL, " +
               "BusinessPartnerIdentifier GUID NULL, " +
               "BusinessPartnerCode INTEGER NULL, " +
               "BusinessPartnerName NVARCHAR(2048) NULL, " +
               "ConstructionSiteHistoryId INTEGER NULL, " +
               "ConstructionSiteHistoryIdentifier GUID NULL, " +
               "ConstructionSiteHistoryCode INTEGER NULL, " +
               "ConstructionSiteHistoryName NVARCHAR(2048) NULL, " +
               "IsSynced BOOL NULL, " +
               "UpdatedAt DATETIME NULL, " +
               "CreatedById INTEGER NULL, " +
               "CreatedByName NVARCHAR(2048) NULL, " +
               "CompanyId INTEGER NULL, " +
               "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
           "SELECT ServerId, Identifier, Code, StartDate, " +
           "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName,  " +
           "ConstructionSiteHistoryId, ConstructionSiteHistoryIdentifier, ConstructionSiteHistoryCode, ConstructionSiteHistoryName,  " +
           "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO BusinessPartnerByConstructionSiteHistories " +
           "(Id, ServerId, Identifier, Code, StartDate, " +
           "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName,  " +
           "ConstructionSiteHistoryId, ConstructionSiteHistoryIdentifier, ConstructionSiteHistoryCode, ConstructionSiteHistoryName,  " +
           "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

           "VALUES (NULL, @ServerId, @Identifier, @Code, @StartDate, " +
           "@BusinessPartnerId, @BusinessPartnerIdentifier, @BusinessPartnerCode, @BusinessPartnerName,  " +
           "@ConstructionSiteHistoryId, @ConstructionSiteHistoryIdentifier, @ConstructionSiteHistoryCode, @ConstructionSiteHistoryName,  " +
           "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";


        public BusinessPartnerByConstructionSiteHistoryListResponse GetByConstructionSiteHistory(Guid constructionSiteHistoryIdentifier)
        {
            BusinessPartnerByConstructionSiteHistoryListResponse response = new BusinessPartnerByConstructionSiteHistoryListResponse();
            List<BusinessPartnerByConstructionSiteHistoryViewModel> businessPartnerByConstructionSiteHistories = new List<BusinessPartnerByConstructionSiteHistoryViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM BusinessPartnerByConstructionSiteHistories " +
                        "WHERE ConstructionSiteHistoryIdentifier = @ConstructionSiteHistoryIdentifier;", db);
                    selectCommand.Parameters.AddWithValue("@ConstructionSiteHistoryIdentifier", constructionSiteHistoryIdentifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        int counter = 0;
                        BusinessPartnerByConstructionSiteHistoryViewModel dbEntry = new BusinessPartnerByConstructionSiteHistoryViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.StartDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.BusinessPartner = SQLiteHelper.GetBusinessPartner(query, ref counter);
                        dbEntry.ConstructionSite = SQLiteHelper.GetConstructionSite(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        businessPartnerByConstructionSiteHistories.Add(dbEntry);
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.BusinessPartnerByConstructionSiteHistories = new List<BusinessPartnerByConstructionSiteHistoryViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.BusinessPartnerByConstructionSiteHistories = businessPartnerByConstructionSiteHistories;
            return response;
        }

        public void Sync(IBusinessPartnerByConstructionSiteHistoryService businessPartnerByConstructionSiteHistoryService)
        {
            SyncBusinessPartnerByConstructionSiteHistoryRequest request = new SyncBusinessPartnerByConstructionSiteHistoryRequest();
            request.CompanyId = MainWindow.CurrentCompanyId;
            request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

            BusinessPartnerByConstructionSiteHistoryListResponse response = businessPartnerByConstructionSiteHistoryService.Sync(request);
            if (response.Success)
            {
                List<BusinessPartnerByConstructionSiteHistoryViewModel> businessPartnerByConstructionSiteHistoryFromDB = response.BusinessPartnerByConstructionSiteHistories;
                foreach (var businessPartnerByConstructionSiteHistory in businessPartnerByConstructionSiteHistoryFromDB.OrderBy(x => x.Id))
                {
                    Delete(businessPartnerByConstructionSiteHistory.BusinessPartner.Identifier, businessPartnerByConstructionSiteHistory.ConstructionSite.Identifier);

                    if (businessPartnerByConstructionSiteHistory.IsActive)
                    {
                        businessPartnerByConstructionSiteHistory.IsSynced = true;
                        Create(businessPartnerByConstructionSiteHistory);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from BusinessPartnerByConstructionSitehistories WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from BusinessPartnerByConstructionSiteHistories WHERE CompanyId = @CompanyId", db);
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

        public BusinessPartnerByConstructionSiteHistoryResponse Create(BusinessPartnerByConstructionSiteHistoryViewModel businessPartnerByConstructionSiteHistory)
        {
            BusinessPartnerByConstructionSiteHistoryResponse response = new BusinessPartnerByConstructionSiteHistoryResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = SqlCommandInsertPart;

                insertCommand.Parameters.AddWithValue("@ServerId", businessPartnerByConstructionSiteHistory.Id);
                insertCommand.Parameters.AddWithValue("@Identifier", businessPartnerByConstructionSiteHistory.Identifier);
                insertCommand.Parameters.AddWithValue("@Code", ((object)businessPartnerByConstructionSiteHistory.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@StartDate", ((object)businessPartnerByConstructionSiteHistory.StartDate) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerId", ((object)businessPartnerByConstructionSiteHistory.BusinessPartner?.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", ((object)businessPartnerByConstructionSiteHistory.BusinessPartner?.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerCode", ((object)businessPartnerByConstructionSiteHistory.BusinessPartner?.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerName", ((object)businessPartnerByConstructionSiteHistory.BusinessPartner?.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ConstructionSiteHistoryId", ((object)businessPartnerByConstructionSiteHistory.ConstructionSite?.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ConstructionSiteHistoryIdentifier", ((object)businessPartnerByConstructionSiteHistory.ConstructionSite?.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ConstructionSiteHistoryCode", ((object)businessPartnerByConstructionSiteHistory.ConstructionSite?.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ConstructionSiteHistoryName", ((object)businessPartnerByConstructionSiteHistory.ConstructionSite?.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IsSynced", businessPartnerByConstructionSiteHistory.IsSynced);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", businessPartnerByConstructionSiteHistory.UpdatedAt);
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

        public BusinessPartnerByConstructionSiteHistoryResponse UpdateSyncStatus(Guid identifier, int serverId, bool isSynced)
        {
            BusinessPartnerByConstructionSiteHistoryResponse response = new BusinessPartnerByConstructionSiteHistoryResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "UPDATE BusinessPartnerByConstructionSiteHistories SET " +
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

        public BusinessPartnerByConstructionSiteHistoryResponse Delete(Guid businessPartnerIdentifier, Guid constructionSiteHistoryIdentifier)
        {
            BusinessPartnerByConstructionSiteHistoryResponse response = new BusinessPartnerByConstructionSiteHistoryResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "DELETE FROM BusinessPartnerByConstructionSiteHistories WHERE BusinessPartnerIdentifier = @BusinessPartnerIdentifier AND ConstructionSiteHistoryIdentifier = @ConstructionSiteHistoryIdentifier";
                insertCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", businessPartnerIdentifier);
                insertCommand.Parameters.AddWithValue("@ConstructionSiteHistoryIdentifier", constructionSiteHistoryIdentifier);
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

        public BusinessPartnerByConstructionSiteHistoryResponse DeleteAll()
        {
            BusinessPartnerByConstructionSiteHistoryResponse response = new BusinessPartnerByConstructionSiteHistoryResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM BusinessPartnerByConstructionSiteHistories";
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
