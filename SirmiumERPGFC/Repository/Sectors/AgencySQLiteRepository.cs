using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.Sectors;
using ServiceInterfaces.Messages.Common.Sectors;
using ServiceInterfaces.ViewModels.Common.Sectors;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.Sectors
{
    public class AgencySQLiteRepository
    {
        public static string AgencyTableCreatePart =
           "CREATE TABLE IF NOT EXISTS Agencies " +
            "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
            "ServerId INTEGER NULL, " +
            "Identifier GUID, " +
            "Code NVARCHAR(48) NULL, " +
            "Name NVARCHAR(48) NULL, " +
            "CountryId INTEGER NULL, " +
            "CountryIdentifier GUID NULL, " +
            "CountryCode NVARCHAR(2048) NULL, " +
            "CountryName NVARCHAR(2048) NULL, " +
            "SectorId INTEGER NULL, " +
            "SectorIdentifier GUID NULL, " +
            "SectorCode NVARCHAR(2048) NULL, " +
            "SectorName NVARCHAR(2048) NULL, " +
            "IsSynced BOOL NULL, " +
            "UpdatedAt DATETIME NULL, " +
            "CreatedById INTEGER NULL, " +
            "CreatedByName NVARCHAR(2048) NULL, " +
            "CompanyId INTEGER NULL, " +
            "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, Code, Name, " +
            "CountryId, CountryIdentifier, CountryCode, CountryName, " +
            "SectorId, SectorIdentifier, SectorCode, SectorName, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO Agencies " +
            "(Id, ServerId, Identifier, Code, Name, " +
            "CountryId, CountryIdentifier, CountryCode, CountryName, " +
            "SectorId, SectorIdentifier, SectorCode, SectorName, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @Code, @Name, " +
            "@CountryId, @CountryIdentifier, @CountryCode, @CountryName, " +
            "@SectorId, @SectorIdentifier, @SectorCode, @SectorName, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        public AgencyListResponse GetAgenciesByPage(int companyId, AgencyViewModel AgencySearchObject, int currentPage = 1, int itemsPerPage = 50)
        {
            AgencyListResponse response = new AgencyListResponse();
            List<AgencyViewModel> Remedies = new List<AgencyViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Agencies " +
                        "WHERE (@Code IS NULL OR @Code = '' OR Code LIKE @Code) " +
                        "AND (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND (@CountryName IS NULL OR @CountryName = '' OR CountryName LIKE @CountryName) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage OFFSET @Offset;", db);
                    selectCommand.Parameters.AddWithValue("@Code", ((object)AgencySearchObject.Search_Code) != null ? "%" + AgencySearchObject.Search_Code + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Name", ((object)AgencySearchObject.Search_Name) != null ? "%" + AgencySearchObject.Search_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CountryName", ((object)AgencySearchObject.Search_Country) != null ? "%" + AgencySearchObject.Search_Country + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
                    selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        AgencyViewModel dbEntry = new AgencyViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Country = SQLiteHelper.GetCountry(query, ref counter);
                        dbEntry.Sector = SQLiteHelper.GetSector(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        Remedies.Add(dbEntry);
                    }


                    selectCommand = new SqliteCommand(
                        "SELECT Count(*) " +
                        "FROM Agencies " +
                        "WHERE (@Code IS NULL OR @Code = '' OR Code LIKE @Code) " +
                        "AND (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND (@CountryName IS NULL OR @CountryName = '' OR CountryName LIKE @CountryName) " +
                        "AND CompanyId = @CompanyId;", db);
                    selectCommand.Parameters.AddWithValue("@Code", ((object)AgencySearchObject.Search_Code) != null ? "%" + AgencySearchObject.Search_Code + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Name", ((object)AgencySearchObject.Search_Name) != null ? "%" + AgencySearchObject.Search_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CountryName", ((object)AgencySearchObject.Search_Country) != null ? "%" + AgencySearchObject.Search_Country + "%" : "");
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
                    response.Agencies = new List<AgencyViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Agencies = Remedies;
            return response;
        }

        public AgencyListResponse GetAgenciesForPopup(int companyId, Guid sectorIdentifier, string filterString)
        {
            AgencyListResponse response = new AgencyListResponse();
            List<AgencyViewModel> Agencies = new List<AgencyViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Agencies " +
                        "WHERE (@Filter IS NULL OR @Filter = '' OR Code LIKE @Filter OR Name LIKE @Filter) " +
                        "AND SectorIdentifier = @SectorIdentifier " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage;", db);
                    selectCommand.Parameters.AddWithValue("@Filter", ((object)filterString) != null ? "%" + filterString + "%" : "");
                    selectCommand.Parameters.AddWithValue("@SectorIdentifier", sectorIdentifier);
                    selectCommand.Parameters.AddWithValue("@CompanyId", ((object)companyId) != null ? companyId : 0);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", 100);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        AgencyViewModel dbEntry = new AgencyViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Country = SQLiteHelper.GetCountry(query, ref counter);
                        dbEntry.Sector = SQLiteHelper.GetSector(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        Agencies.Add(dbEntry);
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.Agencies = new List<AgencyViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Agencies = Agencies;
            return response;
        }

        public AgencyResponse GetAgency(Guid identifier)
        {
            AgencyResponse response = new AgencyResponse();
            AgencyViewModel Agency = new AgencyViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Agencies " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        int counter = 0;
                        AgencyViewModel dbEntry = new AgencyViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Country = SQLiteHelper.GetCountry(query, ref counter);
                        dbEntry.Sector = SQLiteHelper.GetSector(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        Agency = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.Agency = new AgencyViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Agency = Agency;
            return response;
        }

        public void Sync(IAgencyService AgencyService, Action<int, int> callback = null)
        {
            try
            {
                SyncAgencyRequest request = new SyncAgencyRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                AgencyListResponse response = AgencyService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.Agencies?.Count ?? 0;
                    List<AgencyViewModel> AgenciesFromDB = response.Agencies;
                    foreach (var Agency in AgenciesFromDB.OrderBy(x => x.Id))
                    {
                        ThreadPool.QueueUserWorkItem((k) =>
                        {
                            Delete(Agency.Identifier);
                            if (Agency.IsActive)
                            {
                                Agency.IsSynced = true;
                                Create(Agency);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from Agencies WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from Agencies WHERE CompanyId = @CompanyId", db);
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

        public AgencyResponse Create(AgencyViewModel Agency)
        {
            AgencyResponse response = new AgencyResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = SqlCommandInsertPart;

                insertCommand.Parameters.AddWithValue("@ServerId", Agency.Id);
                insertCommand.Parameters.AddWithValue("@Identifier", Agency.Identifier);
                insertCommand.Parameters.AddWithValue("@Code", ((object)Agency.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Name", ((object)Agency.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CountryId", ((object)Agency.Country?.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CountryIdentifier", ((object)Agency.Country?.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CountryCode", ((object)Agency.Country?.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CountryName", ((object)Agency.Country?.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@SectorId", ((object)Agency.Sector?.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@SectorIdentifier", ((object)Agency.Sector?.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@SectorCode", ((object)Agency.Sector?.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@SectorName", ((object)Agency.Sector?.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IsSynced", Agency.IsSynced);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)Agency.UpdatedAt) ?? DBNull.Value);
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

        public AgencyResponse UpdateSyncStatus(Guid identifier, string code, DateTime? updatedAt, int serverId, bool isSynced)
        {
            AgencyResponse response = new AgencyResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "UPDATE Agencies SET " +
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

        public AgencyResponse Delete(Guid identifier)
        {
            AgencyResponse response = new AgencyResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "DELETE FROM Agencies WHERE Identifier = @Identifier";
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

        public AgencyResponse DeleteAll()
        {
            AgencyResponse response = new AgencyResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM Agencies";
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
