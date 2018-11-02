using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.Locations;
using ServiceInterfaces.Messages.Common.Locations;
using ServiceInterfaces.ViewModels.Common.Locations;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.Locations
{
    public class MunicipalitySQLiteRepository
    {
        public static string MunicipalityTableCreatePart =
            "CREATE TABLE IF NOT EXISTS Municipalities " +
            "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
            "ServerId INTEGER NULL, " +
            "Identifier GUID, " +
            "Code NVARCHAR(48) NULL, " +
            "MunicipalityCode NVARCHAR(48) NULL, " +
            "Name NVARCHAR(48) NULL, " +
            "RegionId INTEGER NULL, " +
            "RegionIdentifier GUID NULL, " +
            "RegionCode NVARCHAR(48) NULL, " +
            "RegionName NVARCHAR(48) NULL, " +
            "CountryId INTEGER NULL, " +
            "CountryIdentifier GUID NULL, " +
            "CountryCode NVARCHAR(2048) NULL, " +
            "CountryName NVARCHAR(2048) NULL, " +
            "IsSynced BOOL NULL, " +
            "UpdatedAt DATETIME NULL, " +
            "CreatedById INTEGER NULL, " +
            "CreatedByName NVARCHAR(2048) NULL, " +
            "CompanyId INTEGER NULL, " +
            "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, Code, MunicipalityCode, Name, " +
            "RegionId, RegionIdentifier, RegionCode, RegionName, " +
            "CountryId, CountryIdentifier, CountryCode, CountryName, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO Municipalities " +
            "(Id, ServerId, Identifier, Code, MunicipalityCode, Name, " +
            "RegionId, RegionIdentifier, RegionCode, RegionName, " +
             "CountryId, CountryIdentifier, CountryCode, CountryName, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @Code, @MunicipalityCode, @Name, " +
            "@RegionId, @RegionIdentifier, @RegionCode, @RegionName, " +
             "@CountryId, @CountryIdentifier, @CountryCode, @CountryName, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        public MunicipalityListResponse GetMunicipalitiesByPage(int companyId, MunicipalityViewModel MunicipalitySearchObject, int currentPage = 1, int itemsPerPage = 50)
        {
            MunicipalityListResponse response = new MunicipalityListResponse();
            List<MunicipalityViewModel> Municipalities = new List<MunicipalityViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Municipalities " +
                        "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND (@MunicipalityCode IS NULL OR @MunicipalityCode = '' OR MunicipalityCode LIKE @MunicipalityCode) " +
                        "AND (@RegionName IS NULL OR @RegionName = '' OR RegionName LIKE @RegionName) " +
                        "AND (@CountryName IS NULL OR @CountryName = '' OR CountryName LIKE @CountryName) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage OFFSET @Offset;", db);
                    selectCommand.Parameters.AddWithValue("@Name", ((object)MunicipalitySearchObject.Search_Name) != null ? "%" + MunicipalitySearchObject.Search_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@MunicipalityCode", ((object)MunicipalitySearchObject.Search_MunicipalityCode) != null ? "%" + MunicipalitySearchObject.Search_MunicipalityCode + "%" : "");
                    selectCommand.Parameters.AddWithValue("@RegionName", ((object)MunicipalitySearchObject.Search_Region) != null ? "%" + MunicipalitySearchObject.Search_Region + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CountryName", ((object)MunicipalitySearchObject.Search_Country) != null ? "%" + MunicipalitySearchObject.Search_Country + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
                    selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        MunicipalityViewModel dbEntry = new MunicipalityViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.MunicipalityCode = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Region = SQLiteHelper.GetRegion(query, ref counter);
                        dbEntry.Country = SQLiteHelper.GetCountry(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        Municipalities.Add(dbEntry);
                    }


                    selectCommand = new SqliteCommand(
                        "SELECT Count(*) " +
                        "FROM Municipalities " +
                        "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND (@MunicipalityCode IS NULL OR @MunicipalityCode = '' OR MunicipalityCode LIKE @MunicipalityCode) " +
                        "AND (@RegionName IS NULL OR @RegionName = '' OR RegionName LIKE @RegionName) " +
                        "AND (@CountryName IS NULL OR @CountryName = '' OR CountryName LIKE @CountryName) " +
                        "AND CompanyId = @CompanyId;", db);
                    selectCommand.Parameters.AddWithValue("@Name", ((object)MunicipalitySearchObject.Search_Name) != null ? "%" + MunicipalitySearchObject.Search_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@MunicipalityCode", ((object)MunicipalitySearchObject.Search_MunicipalityCode) != null ? "%" + MunicipalitySearchObject.Search_MunicipalityCode + "%" : "");
                    selectCommand.Parameters.AddWithValue("@RegionName", ((object)MunicipalitySearchObject.Search_Region) != null ? "%" + MunicipalitySearchObject.Search_Region + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CountryName", ((object)MunicipalitySearchObject.Search_Country) != null ? "%" + MunicipalitySearchObject.Search_Country + "%" : "");
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
                    response.Municipalities = new List<MunicipalityViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Municipalities = Municipalities;
            return response;
        }

        public MunicipalityListResponse GetMunicipalitiesForPopup(int companyId, Guid regionIdentifier, string filterString)
        {
            MunicipalityListResponse response = new MunicipalityListResponse();
            List<MunicipalityViewModel> Municipalities = new List<MunicipalityViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Municipalities " +
                        "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name OR Code LIKE @Name) " +
                        "OR (@MunicipalityCode IS NULL OR @MunicipalityCode = '' OR MunicipalityCode LIKE @MunicipalityCode) " +
                        "OR (@RegionName IS NULL OR @RegionName = '' OR RegionName LIKE @RegionName) " +
                        "OR (@CountryName IS NULL OR @CountryName = '' OR CountryName LIKE @CountryName) " +
                        "AND RegionIdentifier = @RegionIdentifier " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage;", db);
                    selectCommand.Parameters.AddWithValue("@Name", ((object)filterString) != null ? "%" + filterString + "%" : "");
                    selectCommand.Parameters.AddWithValue("@MunicipalityCode", ((object)filterString) != null ? "%" + filterString + "%" : "");
                    selectCommand.Parameters.AddWithValue("@RegionName", ((object)filterString) != null ? "%" + filterString + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CountryName", ((object)filterString) != null ? "%" + filterString + "%" : "");
                    selectCommand.Parameters.AddWithValue("@RegionIdentifier", regionIdentifier);
                    selectCommand.Parameters.AddWithValue("@CompanyId", ((object)filterString) != null ? companyId : 0);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", 100);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        MunicipalityViewModel dbEntry = new MunicipalityViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.MunicipalityCode = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Region = SQLiteHelper.GetRegion(query, ref counter);
                        dbEntry.Country = SQLiteHelper.GetCountry(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        Municipalities.Add(dbEntry);
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.Municipalities = new List<MunicipalityViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Municipalities = Municipalities;
            return response;
        }

        public MunicipalityResponse GetMunicipality(Guid identifier)
        {
            MunicipalityResponse response = new MunicipalityResponse();
            MunicipalityViewModel Municipality = new MunicipalityViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Municipalities " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        int counter = 0;
                        MunicipalityViewModel dbEntry = new MunicipalityViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.MunicipalityCode = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Region = SQLiteHelper.GetRegion(query, ref counter);
                        dbEntry.Country = SQLiteHelper.GetCountry(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        Municipality = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.Municipality = new MunicipalityViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Municipality = Municipality;
            return response;
        }

        public void Sync(IMunicipalityService municipalityService)
        {
            SyncMunicipalityRequest request = new SyncMunicipalityRequest();
            request.CompanyId = MainWindow.CurrentCompanyId;
            request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

            MunicipalityListResponse response = municipalityService.Sync(request);
            if (response.Success)
            {
                List<MunicipalityViewModel> municipalitiesFromDB = response.Municipalities;
                foreach (var municipality in municipalitiesFromDB.OrderBy(x => x.Id))
                {
                    Delete(municipality.Identifier);
                    municipality.IsSynced = true;
                    Create(municipality);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from Municipalities WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from Municipalities WHERE CompanyId = @CompanyId", db);
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

        public MunicipalityResponse Create(MunicipalityViewModel Municipality)
        {
            MunicipalityResponse response = new MunicipalityResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = SqlCommandInsertPart;

                insertCommand.Parameters.AddWithValue("@ServerId", Municipality.Id);
                insertCommand.Parameters.AddWithValue("@Identifier", Municipality.Identifier);
                insertCommand.Parameters.AddWithValue("@Code", ((object)Municipality.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@MunicipalityCode", ((object)Municipality.MunicipalityCode) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Name", ((object)Municipality.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@RegionId", ((object)Municipality.Region?.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@RegionIdentifier", ((object)Municipality.Region?.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@RegionCode", ((object)Municipality.Region?.RegionCode) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@RegionName", ((object)Municipality.Region?.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CountryId", ((object)Municipality.Country?.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CountryIdentifier", ((object)Municipality.Country?.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CountryCode", ((object)Municipality.Country?.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CountryName", ((object)Municipality.Country?.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IsSynced", Municipality.IsSynced);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", Municipality.UpdatedAt);
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

        public MunicipalityResponse UpdateSyncStatus(Guid identifier, int serverId, bool isSynced)
        {
            MunicipalityResponse response = new MunicipalityResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "UPDATE Municipalities SET " +
                    "IsSynced = @IsSynced, " +
                    "ServerId = @ServerId " +
                    "WHERE Identifier = @Identifier ";

                insertCommand.Parameters.AddWithValue("@IsSynced", isSynced);
                insertCommand.Parameters.AddWithValue("@Identifier", identifier);
                insertCommand.Parameters.AddWithValue("@ServerId", serverId);

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

        public MunicipalityResponse Delete(Guid identifier)
        {
            MunicipalityResponse response = new MunicipalityResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "DELETE FROM Municipalities WHERE Identifier = @Identifier";
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

        public MunicipalityResponse DeleteAll()
        {
            MunicipalityResponse response = new MunicipalityResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM Municipalities";
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
