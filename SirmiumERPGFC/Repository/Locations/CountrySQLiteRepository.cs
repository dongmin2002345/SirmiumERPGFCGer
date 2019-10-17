using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.Locations;
using ServiceInterfaces.Messages.Common.Locations;
using ServiceInterfaces.ViewModels.Common.Locations;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;

namespace SirmiumERPGFC.Repository.Locations
{
    public class CountrySQLiteRepository
    {
        #region SQL

        public static string CountryTableCreatePart =
           "CREATE TABLE IF NOT EXISTS Countries " +
           "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
           "ServerId INTEGER NULL, " +
           "Identifier GUID, " +
           "Code NVARCHAR(2048) NULL, " +
           "AlfaCode NVARCHAR(2048) NULL, " +
           "NumericCode NVARCHAR(2048) NULL, " +
           "Mark NVARCHAR(2048) NULL, " +
           "Name NVARCHAR(2048) NULL, " +
           "IsSynced BOOL NULL, " +
           "UpdatedAt DATETIME NULL, " +
           "CreatedById INTEGER NULL, " +
           "CreatedByName NVARCHAR(2048) NULL, " +
           "CompanyId INTEGER NULL, " +
           "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, Code, AlfaCode, NumericCode, Mark, Name, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO Countries " +
            "(Id, ServerId, Identifier, Code, AlfaCode, NumericCode, Mark, Name," +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @Code, @AlfaCode, @NumericCode, @Mark, @Name, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods

        private CountryViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            CountryViewModel dbEntry = new CountryViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
            dbEntry.AlfaCode = SQLiteHelper.GetString(query, ref counter);
            dbEntry.NumericCode = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Mark = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);

            return dbEntry;
        }

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, CountryViewModel country)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", country.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", country.Identifier);
            insertCommand.Parameters.AddWithValue("@Code", ((object)country.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@AlfaCode", ((object)country.AlfaCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@NumericCode", ((object)country.NumericCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Mark", ((object)country.Mark) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Name", ((object)country.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsSynced", country.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)country.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read

        public CountryListResponse GetCountriesByPage(int companyId, CountryViewModel countrySearchObject, int currentPage = 1, int itemsPerPage = 50)
        {
            CountryListResponse response = new CountryListResponse();
            List<CountryViewModel> Countries = new List<CountryViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Countries " +
                        "WHERE (@AlfaCode IS NULL OR @AlfaCode = '' OR AlfaCode LIKE @AlfaCode)  " +
                        "AND (@NumericCode IS NULL OR @NumericCode = '' OR NumericCode LIKE @NumericCode) " +
                        "AND (@Mark IS NULL OR @Mark = '' OR Mark LIKE @Mark) " +
                        "AND (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced DESC, Name ASC " +
                        "LIMIT @ItemsPerPage OFFSET @Offset;", db);
                    
                    selectCommand.Parameters.AddWithValue("@Name", ((object)countrySearchObject.Search_Name) != null ? "%" + countrySearchObject.Search_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@AlfaCode", ((object)countrySearchObject.Search_AlfaCode) != null ? "%" + countrySearchObject.Search_AlfaCode + "%" : "");
                    selectCommand.Parameters.AddWithValue("@NumericCode", ((object)countrySearchObject.Search_NumericCode) != null ? "%" + countrySearchObject.Search_NumericCode + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Mark", ((object)countrySearchObject.Search_Mark) != null ? "%" + countrySearchObject.Search_Mark + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
                    selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        CountryViewModel dbEntry = Read(query);
                        Countries.Add(dbEntry);
                    }


                    selectCommand = new SqliteCommand(
                        "SELECT Count(*) " +
                        "FROM Countries " +
                        "WHERE (@AlfaCode IS NULL OR @AlfaCode = '' OR AlfaCode LIKE @AlfaCode)  " +
                        "AND (@NumericCode IS NULL OR @NumericCode = '' OR NumericCode LIKE @NumericCode) " +
                        "AND (@Mark IS NULL OR @Mark = '' OR Mark LIKE @Mark) " +
                        "AND (@Name IS NULL OR @Name = '' OR Name = @Name) " +
                        "AND CompanyId = @CompanyId;", db);
                    
                    selectCommand.Parameters.AddWithValue("@Name", ((object)countrySearchObject.Search_Name) != null ? "%" + countrySearchObject.Search_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@AlfaCode", ((object)countrySearchObject.Search_AlfaCode) != null ? "%" + countrySearchObject.Search_AlfaCode + "%" : "");
                    selectCommand.Parameters.AddWithValue("@NumericCode", ((object)countrySearchObject.NumericCode) != null ? "%" + countrySearchObject.NumericCode + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Mark", ((object)countrySearchObject.Mark) != null ? "%" + countrySearchObject.Mark + "%" : "");
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
                    response.Countries = new List<CountryViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Countries = Countries;
            return response;
        }

        public CountryListResponse GetCountriesForPopup(int companyId, string filterString)
        {
            CountryListResponse response = new CountryListResponse();
            List<CountryViewModel> countries = new List<CountryViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Countries " +
                        "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name OR Code LIKE @Name) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage;", db);
                    
                    selectCommand.Parameters.AddWithValue("@Name", ((object)filterString) != null ? "%" + filterString + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", ((object)filterString) != null ? companyId : 0);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", 100);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        CountryViewModel dbEntry = Read(query);
                        countries.Add(dbEntry);
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.Countries = new List<CountryViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Countries = countries;
            return response;
        }

        public CountryResponse GetCountry(Guid identifier)
        {
            CountryResponse response = new CountryResponse();
            CountryViewModel Country = new CountryViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Countries " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        CountryViewModel dbEntry = Read(query);
                        Country = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.Country = new CountryViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Country = Country;
            return response;
        }

        #endregion

        #region Sync

        public void Sync(ICountryService countryService, Action<int, int> callback = null)
        {
            try
            {
                SyncCountryRequest request = new SyncCountryRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                CountryListResponse response = countryService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.Countries?.Count ?? 0;
                    List<CountryViewModel> countrysFromDB = response.Countries;

                    using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM Countries WHERE Identifier = @Identifier";

                            SqliteCommand insertCommand = db.CreateCommand();
                            insertCommand.CommandText = SqlCommandInsertPart;

                            foreach (var country in countrysFromDB)
                            {
                                deleteCommand.Parameters.AddWithValue("@Identifier", country.Identifier);
                                deleteCommand.ExecuteNonQuery();
                                deleteCommand.Parameters.Clear();

                                if (country.IsActive)
                                {
                                    country.IsSynced = true;

                                    insertCommand = AddCreateParameters(insertCommand, country);
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
            using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from Countries WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from Countries WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
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

        public CountryResponse Create(CountryViewModel country)
        {
            CountryResponse response = new CountryResponse();

            using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
            {
                db.Open();

                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, country);
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

        public CountryResponse Delete(Guid identifier)
        {
            CountryResponse response = new CountryResponse();

            using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM Countries WHERE Identifier = @Identifier";
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

        public CountryResponse DeleteAll()
        {
            CountryResponse response = new CountryResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM Countries";
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
