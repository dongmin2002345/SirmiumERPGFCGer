using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.ConstructionSites;
using ServiceInterfaces.Messages.ConstructionSites;
using ServiceInterfaces.ViewModels.ConstructionSites;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;

namespace SirmiumERPGFC.Repository.ConstructionSites
{
    public class ConstructionSiteSQLiteRepository
    {
        #region SQL

        public static string ConstructionSiteTableCreatePart =
          "CREATE TABLE IF NOT EXISTS ConstructionSites " +
           "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
           "ServerId INTEGER NULL, " +
           "Identifier GUID, " +
           "Code NVARCHAR(48) NULL, " +
           "InternalCode NVARCHAR(48) NULL, " +
           "Name NVARCHAR(48) NULL, " +
           "Address NVARCHAR(48) NULL, " +
           "MaxWorkers NVARCHAR(48) NULL, " +
           "ProContractDate DATETIME NULL, " +
           "ContractStart DATETIME NULL, " +
           "ContractExpiration DATETIME NULL, " +
           "CityId INTEGER NULL, " +
           "CityIdentifier GUID NULL, " +
           "CityCode NVARCHAR(2048) NULL, " +
           "CityName NVARCHAR(2048) NULL, " +
           "CountryId INTEGER NULL, " +
           "CountryIdentifier GUID NULL, " +
           "CountryCode NVARCHAR(2048) NULL, " +
           "CountryName NVARCHAR(2048) NULL, " +
           "BusinessPartnerId INTEGER NULL, " +
           "BusinessPartnerIdentifier GUID NULL, " +
           "BusinessPartnerCode NVARCHAR(2048) NULL, " +
           "BusinessPartnerName NVARCHAR(2048) NULL, " +
            "BusinessPartnerInternalCode NVARCHAR(2048) NULL, " +
           "BusinessPartnerNameGer NVARCHAR(2048) NULL, " +
           "StatusId INTEGER NULL, " +
           "StatusIdentifier GUID NULL, " +
           "StatusCode NVARCHAR(2048) NULL, " +
           "StatusName NVARCHAR(2048) NULL, " +
           "IsSynced BOOL NULL, " +
           "UpdatedAt DATETIME NULL, " +
           "CreatedById INTEGER NULL, " +
           "CreatedByName NVARCHAR(2048) NULL, " +
           "CompanyId INTEGER NULL, " +
           "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, Code, InternalCode, Name, Address, MaxWorkers, ProContractDate, ContractStart, ContractExpiration, " +
            "CityId, CityIdentifier, CityCode, CityName, " +
            "CountryId, CountryIdentifier, CountryCode, CountryName, " +
            "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, BusinessPartnerInternalCode, BusinessPartnerNameGer, " +
            "StatusId, StatusIdentifier, StatusCode, StatusName, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO ConstructionSites " +
            "(Id, ServerId, Identifier, Code, InternalCode, Name, Address, MaxWorkers, ProContractDate, ContractStart, ContractExpiration, " +
            "CityId, CityIdentifier, CityCode, CityName, " +
            "CountryId, CountryIdentifier, CountryCode, CountryName, " +
            "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, BusinessPartnerInternalCode, BusinessPartnerNameGer, " +
            "StatusId, StatusIdentifier, StatusCode, StatusName, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (" +
            "NULL, @ServerId, @Identifier, @Code, @InternalCode, @Name, @Address, @MaxWorkers, @ProContractDate, @ContractStart, @ContractExpiration, " +
            "@CityId, @CityIdentifier, @CityCode, @CityName, " +
            "@CountryId, @CountryIdentifier, @CountryCode, @CountryName, " +
            "@BusinessPartnerId, @BusinessPartnerIdentifier, @BusinessPartnerCode, @BusinessPartnerName, @BusinessPartnerInternalCode, @BusinessPartnerNameGer, " +
            "@StatusId, @StatusIdentifier, @StatusCode, @StatusName, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods

        private static ConstructionSiteViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            ConstructionSiteViewModel dbEntry = new ConstructionSiteViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
            dbEntry.InternalCode = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Address = SQLiteHelper.GetString(query, ref counter);
            dbEntry.MaxWorkers = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.ProContractDate = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.ContractStart = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.ContractExpiration = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.City = SQLiteHelper.GetCity(query, ref counter);
            dbEntry.Country = SQLiteHelper.GetCountry(query, ref counter);
            dbEntry.BusinessPartner = SQLiteHelper.GetBusinessPartner(query, ref counter);
            dbEntry.Status = SQLiteHelper.GetStatus(query, ref counter);
            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
           
            return dbEntry;
        }
        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, ConstructionSiteViewModel constructionSite)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", constructionSite.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", constructionSite.Identifier);
            insertCommand.Parameters.AddWithValue("@Code", ((object)constructionSite.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@InternalCode", ((object)constructionSite.InternalCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Name", ((object)constructionSite.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Address", ((object)constructionSite.Address) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@MaxWorkers", ((object)constructionSite.MaxWorkers) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ProContractDate", ((object)constructionSite.ProContractDate) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ContractStart", ((object)constructionSite.ContractStart) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ContractExpiration", ((object)constructionSite.ContractExpiration) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CityId", ((object)constructionSite.City?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CityIdentifier", ((object)constructionSite.City?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CityCode", ((object)constructionSite.City?.ZipCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CityName", ((object)constructionSite.City?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryId", ((object)constructionSite.Country?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryIdentifier", ((object)constructionSite.Country?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryCode", ((object)constructionSite.Country?.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryName", ((object)constructionSite.Country?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerId", ((object)constructionSite.BusinessPartner?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", ((object)constructionSite.BusinessPartner?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerCode", ((object)constructionSite.BusinessPartner?.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerName", ((object)constructionSite.BusinessPartner?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerInternalCode", ((object)constructionSite.BusinessPartner?.InternalCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerNameGer", ((object)constructionSite.BusinessPartner?.NameGer) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@StatusId", ((object)constructionSite.Status?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@StatusIdentifier", ((object)constructionSite.Status?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@StatusCode", ((object)constructionSite.Status?.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@StatusName", ((object)constructionSite.Status?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsSynced", constructionSite.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)constructionSite.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read

        public ConstructionSiteListResponse GetConstructionSitesByPage(int companyId, ConstructionSiteViewModel constructionSiteSearchObject, int currentPage = 1, int itemsPerPage = 50)
        {
            ConstructionSiteListResponse response = new ConstructionSiteListResponse();
            List<ConstructionSiteViewModel> ConstructionSites = new List<ConstructionSiteViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM ConstructionSites " +
                        "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND (@City IS NULL OR @City = '' OR CityName LIKE @City) " +
                        "AND (@InternalCode IS NULL OR @InternalCode = '' OR InternalCode LIKE @InternalCode) " +
                        "AND (@BusinessPartnerName IS NULL OR @BusinessPartnerName = '' OR BusinessPartnerName LIKE @BusinessPartnerName) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage OFFSET @Offset;", db);
                    
                    selectCommand.Parameters.AddWithValue("@Name", ((object)constructionSiteSearchObject.Search_Name) != null ? "%" + constructionSiteSearchObject.Search_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@City", ((object)constructionSiteSearchObject.Search_City) != null ? "%" + constructionSiteSearchObject.Search_City + "%" : "");
                    selectCommand.Parameters.AddWithValue("@InternalCode", ((object)constructionSiteSearchObject.Search_InternalCode) != null ? "%" + constructionSiteSearchObject.Search_InternalCode + "%" : "");
                    selectCommand.Parameters.AddWithValue("@BusinessPartnerName", ((object)constructionSiteSearchObject.Search_BusinessPartnerName) != null ? "%" + constructionSiteSearchObject.Search_BusinessPartnerName + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
                    selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        ConstructionSiteViewModel dbEntry = Read(query);
                        ConstructionSites.Add(dbEntry);
                    }


                    selectCommand = new SqliteCommand(
                        "SELECT Count(*) " +
                        "FROM ConstructionSites " +
                        "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND (@City IS NULL OR @City = '' OR CityName LIKE @City) " +
                        "AND (@InternalCode IS NULL OR @InternalCode = '' OR InternalCode LIKE @InternalCode) " +
                        "AND (@BusinessPartnerName IS NULL OR @BusinessPartnerName = '' OR BusinessPartnerName LIKE @BusinessPartnerName) " +
                        "AND CompanyId = @CompanyId;", db);
                    
                    selectCommand.Parameters.AddWithValue("@Name", ((object)constructionSiteSearchObject.Search_Name) != null ? "%" + constructionSiteSearchObject.Search_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@City", ((object)constructionSiteSearchObject.Search_City) != null ? "%" + constructionSiteSearchObject.Search_City + "%" : "");
                    selectCommand.Parameters.AddWithValue("@InternalCode", ((object)constructionSiteSearchObject.Search_InternalCode) != null ? "%" + constructionSiteSearchObject.Search_InternalCode + "%" : "");
                    selectCommand.Parameters.AddWithValue("@BusinessPartnerName", ((object)constructionSiteSearchObject.Search_BusinessPartnerName) != null ? "%" + constructionSiteSearchObject.Search_BusinessPartnerName + "%" : "");
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
                    response.ConstructionSites = new List<ConstructionSiteViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.ConstructionSites = ConstructionSites;
            return response;
        }

        public ConstructionSiteListResponse GetConstructionSitesForPopup(int companyId, string filterString)
        {
            ConstructionSiteListResponse response = new ConstructionSiteListResponse();
            List<ConstructionSiteViewModel> ConstructionSites = new List<ConstructionSiteViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM ConstructionSites " +
                        "WHERE (@Code IS NULL OR @Code = '' OR InternalCode LIKE @Code) " +
                        "AND (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND (@ShortName IS NULL OR @ShortName = '' OR ShortName LIKE @ShortName) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage;", db);
                    
                    selectCommand.Parameters.AddWithValue("@Code", ((object)filterString) != null ? "%" + filterString + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Name", ((object)filterString) != null ? "%" + filterString + "%" : "");
                    selectCommand.Parameters.AddWithValue("@ShortName", ((object)filterString) != null ? "%" + filterString + "%" : "");

                    selectCommand.Parameters.AddWithValue("@CompanyId", ((object)filterString) != null ? companyId : 0);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", 100);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        ConstructionSiteViewModel dbEntry = Read(query);
                        ConstructionSites.Add(dbEntry);
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.ConstructionSites = new List<ConstructionSiteViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.ConstructionSites = ConstructionSites;
            return response;
        }

        public ConstructionSiteResponse GetConstructionSite(Guid identifier)
        {
            ConstructionSiteResponse response = new ConstructionSiteResponse();
            ConstructionSiteViewModel constructionSite = new ConstructionSiteViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM ConstructionSites " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        ConstructionSiteViewModel dbEntry = Read(query);
                        constructionSite = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.ConstructionSite = new ConstructionSiteViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.ConstructionSite = constructionSite;
            return response;
        }

        #endregion

        #region Sync

        public void Sync(IConstructionSiteService constructionSiteService, Action<int, int> callback = null)
        {
            try
            {
                SyncConstructionSiteRequest request = new SyncConstructionSiteRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                ConstructionSiteListResponse response = constructionSiteService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.ConstructionSites?.Count ?? 0;
                    var items = new List<ConstructionSiteViewModel>(response.ConstructionSites);

                    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM ConstructionSites WHERE Identifier = @Identifier";

                            SqliteCommand insertCommand = db.CreateCommand();
                            insertCommand.CommandText = SqlCommandInsertPart;

                            foreach (var item in items)
                            {
                                deleteCommand.Parameters.AddWithValue("@Identifier", item.Identifier);
                                deleteCommand.ExecuteNonQuery();
                                deleteCommand.Parameters.Clear();

                                if (item.IsActive)
                                {
                                    item.IsSynced = true;

                                    insertCommand = AddCreateParameters(insertCommand, item);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from ConstructionSites WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from ConstructionSites WHERE CompanyId = @CompanyId", db);
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

        public ConstructionSiteResponse Create(ConstructionSiteViewModel constructionSite)
        {
            ConstructionSiteResponse response = new ConstructionSiteResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, constructionSite);
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

        public ConstructionSiteResponse Delete(Guid identifier)
        {
            ConstructionSiteResponse response = new ConstructionSiteResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM ConstructionSites WHERE Identifier = @Identifier";
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

        public ConstructionSiteResponse DeleteAll()
        {
            ConstructionSiteResponse response = new ConstructionSiteResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM ConstructionSites";
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
