using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.Locations;
using ServiceInterfaces.Messages.Common.Locations;
using ServiceInterfaces.ViewModels.Common.Locations;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;

namespace SirmiumERPGFC.Repository.Locations
{
    public class RegionSQLiteRepository
    {
        #region SQL

        public static string RegionTableCreatePart =
                "CREATE TABLE IF NOT EXISTS Regions " +
                "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                "ServerId INTEGER NULL, " +
                "Identifier GUID, " +
                "Code NVARCHAR(48) NULL, " +
                "RegionCode NVARCHAR(48) NULL, " +
                "Name NVARCHAR(48) NULL, " +
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
            "SELECT ServerId, Identifier, Code, RegionCode, Name,  " +
              "CountryId, CountryIdentifier, CountryCode, CountryName, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO Regions " +
            "(Id, ServerId, Identifier, Code, RegionCode, Name,  " +
              "CountryId, CountryIdentifier, CountryCode, CountryName, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @Code, @RegionCode, @Name,  " +
              "@CountryId, @CountryIdentifier, @CountryCode, @CountryName, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods

        private RegionViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            RegionViewModel dbEntry = new RegionViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
            dbEntry.RegionCode = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Country = SQLiteHelper.GetCountry(query, ref counter);
            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);

            return dbEntry;
        }

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, RegionViewModel region)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", region.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", region.Identifier);
            insertCommand.Parameters.AddWithValue("@Code", ((object)region.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@RegionCode", ((object)region.RegionCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Name", ((object)region.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryId", ((object)region.Country?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryIdentifier", ((object)region.Country?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryCode", ((object)region.Country?.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryName", ((object)region.Country?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsSynced", region.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)region.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read

        public RegionListResponse GetRegionsByPage(int companyId, RegionViewModel regionSearchObject, int currentPage = 1, int itemsPerPage = 50)
        {
            RegionListResponse response = new RegionListResponse();
            List<RegionViewModel> Regions = new List<RegionViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Regions " +
                        "WHERE (@RegionCode IS NULL OR @RegionCode = '' OR RegionCode LIKE @RegionCode) " +
                        "AND (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND (@CountryName IS NULL OR @CountryName = '' OR CountryName LIKE @CountryName) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage OFFSET @Offset;", db);
                   
                    selectCommand.Parameters.AddWithValue("@RegionCode", ((object)regionSearchObject.Search_RegionCode) != null ? "%" + regionSearchObject.Search_RegionCode + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Name", ((object)regionSearchObject.Search_Name) != null ? "%" + regionSearchObject.Search_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CountryName", ((object)regionSearchObject.Search_Country) != null ? "%" + regionSearchObject.Search_Country + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
                    selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        RegionViewModel dbEntry = Read(query);
                        Regions.Add(dbEntry);
                    }

                    selectCommand = new SqliteCommand(
                        "SELECT Count(*) " +
                        "FROM Regions " +
                        "WHERE (@RegionCode IS NULL OR @RegionCode = '' OR RegionCode LIKE @RegionCode) " +
                        "AND (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND (@CountryName IS NULL OR @CountryName = '' OR CountryName LIKE @CountryName) " +
                        "AND CompanyId = @CompanyId;", db);
                    
                    selectCommand.Parameters.AddWithValue("@RegionCode", ((object)regionSearchObject.Search_RegionCode) != null ? "%" + regionSearchObject.Search_RegionCode + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Name", ((object)regionSearchObject.Search_Name) != null ? "%" + regionSearchObject.Search_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CountryName", ((object)regionSearchObject.Search_Country) != null ? "%" + regionSearchObject.Search_Country + "%" : "");
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
                    response.Regions = new List<RegionViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Regions = Regions;
            return response;
        }

        public RegionListResponse GetRegionsForPopup(int companyId, Guid countryIdentifier, string filterString)
        {
            RegionListResponse response = new RegionListResponse();
            List<RegionViewModel> Regions = new List<RegionViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Regions " +
                        "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "OR (@RegionCode IS NULL OR @RegionCode = '' OR RegionCode LIKE @RegionCode) " +
                        "OR (@CountryName IS NULL OR @CountryName = '' OR CountryName LIKE @CountryName) " +
                        "AND CountryIdentifier = @CountryIdentifier " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage;", db);
                    
                    selectCommand.Parameters.AddWithValue("@Name", ((object)filterString) != null ? "%" + filterString + "%" : "");
                    selectCommand.Parameters.AddWithValue("@RegionCode", ((object)filterString) != null ? "%" + filterString + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CountryName", ((object)filterString) != null ? "%" + filterString + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CountryIdentifier", countryIdentifier);
                    selectCommand.Parameters.AddWithValue("@CompanyId", ((object)filterString) != null ? companyId : 0);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", 100);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        RegionViewModel dbEntry = Read(query);
                        Regions.Add(dbEntry);
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.Regions = new List<RegionViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Regions = Regions;
            return response;
        }

        public RegionResponse GetRegion(Guid identifier)
        {
            RegionResponse response = new RegionResponse();
            RegionViewModel Region = new RegionViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Regions " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        RegionViewModel dbEntry = Read(query);
                        Region = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.Region = new RegionViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Region = Region;
            return response;
        }

        #endregion

        #region Sync

        public void Sync(IRegionService regionService, Action<int, int> callback = null)
        {
            try
            {
                SyncRegionRequest request = new SyncRegionRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                RegionListResponse response = regionService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.Regions?.Count ?? 0;
                    List<RegionViewModel> regionsFromDB = response.Regions;

                    using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM Regions WHERE Identifier = @Identifier";

                            SqliteCommand insertCommand = db.CreateCommand();
                            insertCommand.CommandText = SqlCommandInsertPart;

                            foreach (var region in regionsFromDB)
                            {
                                deleteCommand.Parameters.AddWithValue("@Identifier", region.Identifier);
                                deleteCommand.ExecuteNonQuery();
                                deleteCommand.Parameters.Clear();

                                if (region.IsActive)
                                {
                                    region.IsSynced = true;

                                    insertCommand = AddCreateParameters(insertCommand, region);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from Regions WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from Regions WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
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

        public RegionResponse Create(RegionViewModel region)
        {
            RegionResponse response = new RegionResponse();

            using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
            {
                db.Open();

                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, region);
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

        public RegionResponse Delete(Guid identifier)
        {
            RegionResponse response = new RegionResponse();

            using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM Regions WHERE Identifier = @Identifier";
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

        public RegionResponse DeleteAll()
        {
            RegionResponse response = new RegionResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM Regions";
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
