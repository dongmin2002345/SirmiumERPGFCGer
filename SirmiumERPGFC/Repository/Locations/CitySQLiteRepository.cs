using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.Locations;
using ServiceInterfaces.Messages.Common.Locations;
using ServiceInterfaces.ViewModels.Common.Locations;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;

namespace SirmiumERPGFC.Repository.Locations
{
    public class CitySQLiteRepository
    {
        #region SQL

        public static string CityTableCreatePart =
           "CREATE TABLE IF NOT EXISTS Cities " +
            "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
            "ServerId INTEGER NULL, " +
            "Identifier GUID, " +
            "ZipCode NVARCHAR(48) NULL, " +
            "Name NVARCHAR(48) NULL, " +
            "CountryId INTEGER NULL, " +
            "CountryIdentifier GUID NULL, " +
            "CountryCode NVARCHAR(2048) NULL, " +
            "CountryName NVARCHAR(2048) NULL, " +
            "RegionId INTEGER NULL, " +
            "RegionIdentifier GUID NULL, " +
            "RegionCode NVARCHAR(2048) NULL, " +
            "RegionName NVARCHAR(2048) NULL, " +
            "MunicipalityId INTEGER NULL, " +
            "MunicipalityIdentifier GUID NULL, " +
            "MunicipalityCode NVARCHAR(2048) NULL, " +
            "MunicipalityName NVARCHAR(2048) NULL, " +
            "IsSynced BOOL NULL, " +
            "UpdatedAt DATETIME NULL, " +
            "CreatedById INTEGER NULL, " +
            "CreatedByName NVARCHAR(2048) NULL, " +
            "CompanyId INTEGER NULL, " +
            "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, ZipCode, Name, " +
            "CountryId, CountryIdentifier, CountryCode, CountryName, " +
            "RegionId, RegionIdentifier, RegionCode, RegionName, " +
            "MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO Cities " +
            "(Id, ServerId, Identifier, ZipCode, Name, " +
            "CountryId, CountryIdentifier, CountryCode, CountryName, " +
            "RegionId, RegionIdentifier, RegionCode, RegionName, " +
            "MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @ZipCode, @Name, " +
            "@CountryId, @CountryIdentifier, @CountryCode, @CountryName, " +
            "@RegionId, @RegionIdentifier, @RegionCode, @RegionName, " +
            "@MunicipalityId, @MunicipalityIdentifier, @MunicipalityCode, @MunicipalityName, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods

        private CityViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            CityViewModel dbEntry = new CityViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.ZipCode = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Country = SQLiteHelper.GetCountry(query, ref counter);
            dbEntry.Region = SQLiteHelper.GetRegion(query, ref counter);
            dbEntry.Municipality = SQLiteHelper.GetMunicipality(query, ref counter);
            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);

            return dbEntry;
        }

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, CityViewModel city)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", city.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", city.Identifier);
            insertCommand.Parameters.AddWithValue("@ZipCode", ((object)city.ZipCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Name", ((object)city.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryId", ((object)city.Country?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryIdentifier", ((object)city.Country?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryCode", ((object)city.Country?.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryName", ((object)city.Country?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@RegionId", ((object)city.Region?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@RegionIdentifier", ((object)city.Region?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@RegionCode", ((object)city.Region?.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@RegionName", ((object)city.Region?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@MunicipalityId", ((object)city.Municipality?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@MunicipalityIdentifier", ((object)city.Municipality?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@MunicipalityCode", ((object)city.Municipality?.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@MunicipalityName", ((object)city.Municipality?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsSynced", city.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)city.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read

        public CityListResponse GetCitiesByPage(int companyId, CityViewModel citySearchObject, int currentPage = 1, int itemsPerPage = 50)
        {
            CityListResponse response = new CityListResponse();
            List<CityViewModel> Cities = new List<CityViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Cities " +
                        "WHERE (@ZipCode IS NULL OR @ZipCode = '' OR ZipCode LIKE @ZipCode) " +
                        "AND (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND (@RegionName IS NULL OR @RegionName = '' OR RegionName LIKE @RegionName) " +
                        "AND (@MunicipalityName IS NULL OR @MunicipalityName = '' OR MunicipalityName LIKE @MunicipalityName) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage OFFSET @Offset;", db);

                    selectCommand.Parameters.AddWithValue("@ZipCode", ((object)citySearchObject.Search_ZipCode) != null ? "%" + citySearchObject.Search_ZipCode + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Name", ((object)citySearchObject.Search_Name) != null ? "%" + citySearchObject.Search_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@RegionName", ((object)citySearchObject.Search_Region) != null ? "%" + citySearchObject.Search_Region + "%" : "");
                    selectCommand.Parameters.AddWithValue("@MunicipalityName", ((object)citySearchObject.Search_Municipality) != null ? "%" + citySearchObject.Search_Municipality + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
                    selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        CityViewModel dbEntry = Read(query);
                        Cities.Add(dbEntry);
                    }


                    selectCommand = new SqliteCommand(
                        "SELECT Count(*) " +
                        "FROM Cities " +
                        "WHERE (@ZipCode IS NULL OR @ZipCode = '' OR Name LIKE @ZipCode) " +
                        "AND (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND (@RegionName IS NULL OR @RegionName = '' OR RegionName LIKE @RegionName) " +
                        "AND (@MunicipalityName IS NULL OR @MunicipalityName = '' OR MunicipalityName LIKE @MunicipalityName) " +
                        "AND CompanyId = @CompanyId;", db);
                    
                    selectCommand.Parameters.AddWithValue("@ZipCode", ((object)citySearchObject.Search_ZipCode) != null ? "%" + citySearchObject.Search_ZipCode + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Name", ((object)citySearchObject.Search_Name) != null ? "%" + citySearchObject.Search_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@RegionName", ((object)citySearchObject.Search_Region) != null ? "%" + citySearchObject.Search_Region + "%" : "");
                    selectCommand.Parameters.AddWithValue("@MunicipalityName", ((object)citySearchObject.Search_Municipality) != null ? "%" + citySearchObject.Search_Municipality + "%" : "");
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
                    response.Cities = new List<CityViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Cities = Cities;
            return response;
        }

        public CityListResponse GetCitiesForPopup(int companyId, Guid countryIdentifier, string filterString)
        {
            CityListResponse response = new CityListResponse();
            List<CityViewModel> Cities = new List<CityViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Cities " +
                        "WHERE (@ZipCode IS NULL OR @ZipCode = '' OR ZipCode LIKE @ZipCode) " +
                        "AND (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND CountryIdentifier = @CountryIdentifier " +
                        //"AND MunicipalityIdentifier = @MunicipalityIdentifier " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage;", db);
                    
                    selectCommand.Parameters.AddWithValue("@ZipCode", ((object)filterString) != null ? "%" + filterString + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Name", ((object)filterString) != null ? "%" + filterString + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CountryIdentifier", countryIdentifier);
                    //selectCommand.Parameters.AddWithValue("@MunicipalityIdentifier", municipalityIdentifier);
                    selectCommand.Parameters.AddWithValue("@CompanyId", ((object)filterString) != null ? companyId : 0);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", 100);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        CityViewModel dbEntry = Read(query);
                        Cities.Add(dbEntry);
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.Cities = new List<CityViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Cities = Cities;
            return response;
        }

        public CityListResponse GetCitiesForPopupCountry(int companyId, Guid? countryIdentifier, string filterString)
        {
            CityListResponse response = new CityListResponse();
            List<CityViewModel> Cities = new List<CityViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Cities " +
                        "WHERE ((@ZipCode IS NULL OR @ZipCode = '' OR ZipCode LIKE @ZipCode) " +
                        "OR (@Name IS NULL OR @Name = '' OR Name LIKE @Name)) " +
                        "AND (@CountryIdentifier IS NULL OR @CountryIdentifier = '' OR CountryIdentifier = @CountryIdentifier) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage;", db);
                    
                    selectCommand.Parameters.AddWithValue("@ZipCode", ((object)filterString) != null ? "%" + filterString + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Name", ((object)filterString) != null ? "%" + filterString + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CountryIdentifier", ((object)countryIdentifier) ?? DBNull.Value);
                    selectCommand.Parameters.AddWithValue("@CompanyId", ((object)filterString) != null ? companyId : 0);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", 100);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        CityViewModel dbEntry = Read(query);
                        Cities.Add(dbEntry);
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.Cities = new List<CityViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Cities = Cities;
            return response;
        }


        public CityResponse GetCity(Guid identifier)
        {
            CityResponse response = new CityResponse();
            CityViewModel city = new CityViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Cities " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                        city = Read(query);
                    
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.City = new CityViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.City = city;
            return response;
        }

        #endregion

        #region Sync

        public void Sync(ICityService cityService, Action<int, int> callback = null)
        {
            try
            {
                SyncCityRequest request = new SyncCityRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                CityListResponse response = cityService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.Cities?.Count ?? 0;
                    List<CityViewModel> citiesFromDB = response.Cities;

                    using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM Cities WHERE Identifier = @Identifier";

                            SqliteCommand insertCommand = db.CreateCommand();
                            insertCommand.CommandText = SqlCommandInsertPart;

                            foreach (var city in citiesFromDB)
                            {
                                deleteCommand.Parameters.AddWithValue("@Identifier", city.Identifier);
                                deleteCommand.ExecuteNonQuery();
                                deleteCommand.Parameters.Clear();

                                if (city.IsActive)
                                {
                                    city.IsSynced = true;

                                    insertCommand = AddCreateParameters(insertCommand, city);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from Cities WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from Cities WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
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

        public CityResponse Create(CityViewModel city)
        {
            CityResponse response = new CityResponse();

            using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
            {
                db.Open();

                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, city);
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

        public CityResponse Delete(Guid identifier)
        {
            CityResponse response = new CityResponse();

            using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM Cities WHERE Identifier = @Identifier";
                insertCommand.Parameters.AddWithValue("@Identifier", identifier);
                try
                {
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

        public CityResponse DeleteAll()
        {
            CityResponse response = new CityResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM Cities";
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