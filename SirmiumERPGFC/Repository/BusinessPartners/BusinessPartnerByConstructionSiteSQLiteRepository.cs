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
    public class BusinessPartnerByConstructionSiteSQLiteRepository
    {
        public static string BusinessPartnerByConstructionSiteTableCreatePart =
            "CREATE TABLE IF NOT EXISTS BusinessPartnerByConstructionSites " +
            "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
            "ServerId INTEGER NULL, " +
            "Identifier GUID, " +
            "Code NVARCHAR(48) NULL, " +
            "StartDate DATETIME NULL, " +
            "EndDate DATETIME NULL, " +
            "MaxNumOfEmployees INTEGER NULL, " + 
            "BusinessPartnerId INTEGER NULL, " +
            "BusinessPartnerIdentifier GUID NULL, " +
            "BusinessPartnerCode INTEGER NULL, " +
            "BusinessPartnerName NVARCHAR(2048) NULL, " +
            "ConstructionSiteId INTEGER NULL, " +
            "ConstructionSiteIdentifier GUID NULL, " +
            "ConstructionSiteCode INTEGER NULL, " +
            "ConstructionSiteName NVARCHAR(2048) NULL, " +
            "IsSynced BOOL NULL, " +
            "UpdatedAt DATETIME NULL, " +
            "CreatedById INTEGER NULL, " +
            "CreatedByName NVARCHAR(2048) NULL, " +
            "CompanyId INTEGER NULL, " +
            "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
           "SELECT ServerId, Identifier, Code, StartDate, EndDate, MaxNumOfEmployees, " +
           "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName,  " +
           "ConstructionSiteId, ConstructionSiteIdentifier, ConstructionSiteCode, ConstructionSiteName,  " +
           "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO BusinessPartnerByConstructionSites " +
           "(Id, ServerId, Identifier, Code, StartDate, EndDate, MaxNumOfEmployees, " +
           "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName,  " +
           "ConstructionSiteId, ConstructionSiteIdentifier, ConstructionSiteCode, ConstructionSiteName,  " +
           "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

           "VALUES (NULL, @ServerId, @Identifier, @Code, @StartDate, @EndDate, @MaxNumOfEmployees, " +
           "@BusinessPartnerId, @BusinessPartnerIdentifier, @BusinessPartnerCode, @BusinessPartnerName,  " +
           "@ConstructionSiteId, @ConstructionSiteIdentifier, @ConstructionSiteCode, @ConstructionSiteName,  " +
           "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";


        public BusinessPartnerByConstructionSiteListResponse GetByConstructionSite(Guid constructionSiteIdentifier)
        {
            BusinessPartnerByConstructionSiteListResponse response = new BusinessPartnerByConstructionSiteListResponse();
            List<BusinessPartnerByConstructionSiteViewModel> businessPartnerByConstructionSites = new List<BusinessPartnerByConstructionSiteViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM BusinessPartnerByConstructionSites " +
                        "WHERE ConstructionSiteIdentifier = @ConstructionSiteIdentifier;", db);
                    selectCommand.Parameters.AddWithValue("@ConstructionSiteIdentifier", constructionSiteIdentifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        BusinessPartnerByConstructionSiteViewModel dbEntry = new BusinessPartnerByConstructionSiteViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.StartDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.EndDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.MaxNumOfEmployees = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.BusinessPartner = SQLiteHelper.GetBusinessPartner(query, ref counter);
                        dbEntry.ConstructionSite = SQLiteHelper.GetConstructionSite(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        businessPartnerByConstructionSites.Add(dbEntry);
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.BusinessPartnerByConstructionSites = new List<BusinessPartnerByConstructionSiteViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.BusinessPartnerByConstructionSites = businessPartnerByConstructionSites;
            return response;
        }

        public void Sync(IBusinessPartnerByConstructionSiteService businessPartnerByConstructionSiteService)
        {
            SyncBusinessPartnerByConstructionSiteRequest request = new SyncBusinessPartnerByConstructionSiteRequest();
            request.CompanyId = MainWindow.CurrentCompanyId;
            request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

            BusinessPartnerByConstructionSiteListResponse response = businessPartnerByConstructionSiteService.Sync(request);
            if (response.Success)
            {
                List<BusinessPartnerByConstructionSiteViewModel> businessPartnerByConstructionSiteFromDB = response.BusinessPartnerByConstructionSites;
                foreach (var businessPartnerByConstructionSite in businessPartnerByConstructionSiteFromDB.OrderBy(x => x.Id))
                {
                    Delete(businessPartnerByConstructionSite.BusinessPartner.Identifier, businessPartnerByConstructionSite.ConstructionSite.Identifier);
                    businessPartnerByConstructionSite.IsSynced = true;
                    Create(businessPartnerByConstructionSite);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from BusinessPartnerByConstructionSites WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from BusinessPartnerByConstructionSites WHERE CompanyId = @CompanyId", db);
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

        public BusinessPartnerByConstructionSiteResponse Create(BusinessPartnerByConstructionSiteViewModel businessPartnerByConstructionSite)
        {
            BusinessPartnerByConstructionSiteResponse response = new BusinessPartnerByConstructionSiteResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = SqlCommandInsertPart;

                insertCommand.Parameters.AddWithValue("@ServerId", businessPartnerByConstructionSite.Id);
                insertCommand.Parameters.AddWithValue("@Identifier", businessPartnerByConstructionSite.Identifier);
                insertCommand.Parameters.AddWithValue("@Code", ((object)businessPartnerByConstructionSite.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@StartDate", ((object)businessPartnerByConstructionSite.StartDate) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@EndDate", ((object)businessPartnerByConstructionSite.EndDate) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@MaxNumOfEmployees", ((object)businessPartnerByConstructionSite.MaxNumOfEmployees) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerId", ((object)businessPartnerByConstructionSite.BusinessPartner?.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", ((object)businessPartnerByConstructionSite.BusinessPartner?.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerCode", ((object)businessPartnerByConstructionSite.BusinessPartner?.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerName", ((object)businessPartnerByConstructionSite.BusinessPartner?.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ConstructionSiteId", ((object)businessPartnerByConstructionSite.ConstructionSite?.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ConstructionSiteIdentifier", ((object)businessPartnerByConstructionSite.ConstructionSite?.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ConstructionSiteCode", ((object)businessPartnerByConstructionSite.ConstructionSite?.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ConstructionSiteName", ((object)businessPartnerByConstructionSite.ConstructionSite?.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IsSynced", businessPartnerByConstructionSite.IsSynced);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", businessPartnerByConstructionSite.UpdatedAt);
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

        public BusinessPartnerByConstructionSiteResponse UpdateSyncStatus(Guid identifier, int serverId, bool isSynced)
        {
            BusinessPartnerByConstructionSiteResponse response = new BusinessPartnerByConstructionSiteResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "UPDATE BusinessPartnerByConstructionSites SET " +
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

        public BusinessPartnerByConstructionSiteResponse Delete(Guid businessPartnerIdentifier, Guid constructionSiteIdentifier)
        {
            BusinessPartnerByConstructionSiteResponse response = new BusinessPartnerByConstructionSiteResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "DELETE FROM BusinessPartnerByConstructionSites WHERE BusinessPartnerIdentifier = @BusinessPartnerIdentifier AND ConstructionSiteIdentifier = @ConstructionSiteIdentifier";
                insertCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", businessPartnerIdentifier);
                insertCommand.Parameters.AddWithValue("@ConstructionSiteIdentifier", constructionSiteIdentifier);
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

        public BusinessPartnerByConstructionSiteResponse DeleteAll()
        {
            BusinessPartnerByConstructionSiteResponse response = new BusinessPartnerByConstructionSiteResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM BusinessPartnerByConstructionSites";
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
