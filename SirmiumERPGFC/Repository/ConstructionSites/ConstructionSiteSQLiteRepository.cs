using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.ConstructionSites;
using ServiceInterfaces.Messages.ConstructionSites;
using ServiceInterfaces.ViewModels.ConstructionSites;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.ConstructionSites
{
    public class ConstructionSiteSQLiteRepository
    {
        public static string ConstructionSiteTableCreatePart =
          "CREATE TABLE IF NOT EXISTS ConstructionSites " +
           "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
           "ServerId INTEGER NULL, " +
           "Identifier GUID, " +
           "Code NVARCHAR(48) NULL, " +
           "Name NVARCHAR(48) NULL, " +
           "Address NVARCHAR(48) NULL, " +
           "MaxWorkers NVARCHAR(48) NULL, " +
           "ContractExpiration DATETIME NULL, " +
           "CityId INTEGER NULL, " +
           "CityIdentifier GUID NULL, " +
           "CityCode NVARCHAR(2048) NULL, " +
           "CityName NVARCHAR(2048) NULL, " +
           "IsSynced BOOL NULL, " +
           "UpdatedAt DATETIME NULL, " +
           "CreatedById INTEGER NULL, " +
           "CreatedByName NVARCHAR(2048) NULL, " +
           "CompanyId INTEGER NULL, " +
           "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, Code, Name, Address, MaxWorkers, ContractExpiration, " +
            "CityId, CityIdentifier, CityCode, CityName, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO ConstructionSites " +
            "(Id, ServerId, Identifier, Code, Name, Address, MaxWorkers, ContractExpiration, " +
            "CityId, CityIdentifier, CityCode, CityName, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @Code, @Name, @Address, @MaxWorkers, @ContractExpiration, " +
            "@CityId, @CityIdentifier, @CityCode, @CityName, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

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
                        "WHERE (@Code IS NULL OR @Code = '' OR Code LIKE @Code) " +
                        "AND (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        //"AND (@City IS NULL OR @City = '' OR CityCode LIKE @City) " +
                        "AND (@City IS NULL OR @City = '' OR CityName LIKE @City) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage OFFSET @Offset;", db);
                    selectCommand.Parameters.AddWithValue("@Code", ((object)constructionSiteSearchObject.Search_Code) != null ? "%" + constructionSiteSearchObject.Search_Code + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Name", ((object)constructionSiteSearchObject.Search_Name) != null ? "%" + constructionSiteSearchObject.Search_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@City", ((object)constructionSiteSearchObject.Search_City) != null ? "%" + constructionSiteSearchObject.Search_City + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
                    selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        ConstructionSiteViewModel dbEntry = new ConstructionSiteViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Address = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.MaxWorkers = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.ContractExpiration = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.City = SQLiteHelper.GetCity(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        ConstructionSites.Add(dbEntry);
                    }


                    selectCommand = new SqliteCommand(
                        "SELECT Count(*) " +
                        "FROM ConstructionSites " +
                        "WHERE (@Code IS NULL OR @Code = '' OR Name LIKE @Code) " +
                        "AND (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                       // "AND (@City IS NULL OR @City = '' OR CityCode LIKE @City) " +
                        "AND (@City IS NULL OR @City = '' OR CityName LIKE @City) " +
                        "AND CompanyId = @CompanyId;", db);
                    selectCommand.Parameters.AddWithValue("@Code", ((object)constructionSiteSearchObject.Search_Code) != null ? "%" + constructionSiteSearchObject.Search_Code + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Name", ((object)constructionSiteSearchObject.Search_Name) != null ? "%" + constructionSiteSearchObject.Search_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@City", ((object)constructionSiteSearchObject.Search_City) != null ? "%" + constructionSiteSearchObject.Search_City + "%" : "");
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
                        "WHERE (@Code IS NULL OR @Code = '' OR Code LIKE @Code) " +
                        "AND (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        //"AND (@City IS NULL OR @City = '' OR CityCode LIKE @City) " +
                        "AND (@City IS NULL OR @City = '' OR CityName LIKE @City) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage;", db);
                    selectCommand.Parameters.AddWithValue("@Code", ((object)filterString) != null ? "%" + filterString + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Name", ((object)filterString) != null ? "%" + filterString + "%" : "");
                    selectCommand.Parameters.AddWithValue("@City", ((object)filterString) != null ? "%" + filterString + "%" : "");

                    selectCommand.Parameters.AddWithValue("@CompanyId", ((object)filterString) != null ? companyId : 0);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", 100);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        ConstructionSiteViewModel dbEntry = new ConstructionSiteViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Address = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.MaxWorkers = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.ContractExpiration = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.City = SQLiteHelper.GetCity(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
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
                        int counter = 0;
                        ConstructionSiteViewModel dbEntry = new ConstructionSiteViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Address = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.MaxWorkers = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.ContractExpiration = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.City = SQLiteHelper.GetCity(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
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

        public void Sync(IConstructionSiteService constructionSiteService)
        {
            SyncConstructionSiteRequest request = new SyncConstructionSiteRequest();
            request.CompanyId = MainWindow.CurrentCompanyId;
            request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

            ConstructionSiteListResponse response = constructionSiteService.Sync(request);
            if (response.Success)
            {
                List<ConstructionSiteViewModel> constructionSitesFromDB = response.ConstructionSites;
                foreach (var constructionSite in constructionSitesFromDB.OrderBy(x => x.Id))
                {
                    Delete(constructionSite.Identifier);
                    if (constructionSite.IsActive)
                    {
                        constructionSite.IsSynced = true;
                        Create(constructionSite);
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

        public ConstructionSiteResponse Create(ConstructionSiteViewModel constructionSite)
        {
            ConstructionSiteResponse response = new ConstructionSiteResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = SqlCommandInsertPart;

                insertCommand.Parameters.AddWithValue("@ServerId", constructionSite.Id);
                insertCommand.Parameters.AddWithValue("@Identifier", constructionSite.Identifier);
                insertCommand.Parameters.AddWithValue("@Code", ((object)constructionSite.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Name", ((object)constructionSite.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Address", ((object)constructionSite.Address) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@MaxWorkers", ((object)constructionSite.MaxWorkers) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ContractExpiration", ((object)constructionSite.ContractExpiration) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CityId", ((object)constructionSite.City?.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CityIdentifier", ((object)constructionSite.City?.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CityCode", ((object)constructionSite.City?.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CityName", ((object)constructionSite.City?.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IsSynced", constructionSite.IsSynced);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", constructionSite.UpdatedAt);
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

        public ConstructionSiteResponse UpdateSyncStatus(Guid identifier, int serverId, bool isSynced)
        {
            ConstructionSiteResponse response = new ConstructionSiteResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "UPDATE ConstructionSites SET " +
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

        public ConstructionSiteResponse Delete(Guid identifier)
        {
            ConstructionSiteResponse response = new ConstructionSiteResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "DELETE FROM ConstructionSites WHERE Identifier = @Identifier";
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
