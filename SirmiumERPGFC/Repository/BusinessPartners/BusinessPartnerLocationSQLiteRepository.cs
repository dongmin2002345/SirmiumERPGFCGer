using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;

namespace SirmiumERPGFC.Repository.BusinessPartners
{
    public class BusinessPartnerLocationSQLiteRepository
    {
        #region SQL

        public static string BusinessPartnerLocationTableCreatePart =
            "CREATE TABLE IF NOT EXISTS BusinessPartnerLocations " +
            "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
            "ServerId INTEGER NULL, " +
            "Identifier GUID, " +
            "BusinessPartnerId INTEGER NULL, " +
            "BusinessPartnerIdentifier GUID NULL, " +
            "BusinessPartnerCode NVARCHAR(48) NULL, " +
            "BusinessPartnerName NVARCHAR(2048) NULL, " +
               "BusinessPartnerInternalCode NVARCHAR(2048) NULL, " +
               "BusinessPartnerNameGer NVARCHAR(2048) NULL, " +
            "Address NVARCHAR(2048) NULL, " +
            "CountryId INTEGER NULL, " +
            "CountryIdentifier GUID NULL, " +
            "CountryCode NVARCHAR(48) NULL, " +
            "CountryName NVARCHAR(2048) NULL, " +
            "CityId INTEGER NULL, " +
            "CityIdentifier GUID NULL, " +
            "CityCode NVARCHAR(48) NULL, " +
            "CityName NVARCHAR(2048) NULL, " +
            "MunicipalityId INTEGER NULL, " +
            "MunicipalityIdentifier GUID NULL, " +
            "MunicipalityCode NVARCHAR(48) NULL, " +
            "MunicipalityName NVARCHAR(2048) NULL, " +
               "RegionId INTEGER NULL, " +
            "RegionIdentifier GUID NULL, " +
            "RegionCode NVARCHAR(48) NULL, " +
            "RegionName NVARCHAR(2048) NULL, " +
            "IsSynced BOOL NULL, " +
            "UpdatedAt DATETIME NULL, " +
            "CreatedById INTEGER NULL, " +
            "CreatedByName NVARCHAR(2048) NULL, " +
            "CompanyId INTEGER NULL, " +
            "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, " +
            "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, BusinessPartnerInternalCode, BusinessPartnerNameGer, " +
            "Address, CountryId, CountryIdentifier, CountryCode, CountryName, " +
            "CityId, CityIdentifier, CityCode, CityName, " +
            "MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +
            "RegionId, RegionIdentifier, RegionCode, RegionName, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO BusinessPartnerLocations " +
            "(Id, ServerId, Identifier, " +
            "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, BusinessPartnerInternalCode, BusinessPartnerNameGer, " +
            "Address, CountryId, CountryIdentifier, CountryCode, CountryName, " +
            "CityId, CityIdentifier, CityCode, CityName, " +
            "MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +
            "RegionId, RegionIdentifier, RegionCode, RegionName, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, " +
            "@BusinessPartnerId, @BusinessPartnerIdentifier, @BusinessPartnerCode, @BusinessPartnerName, @BusinessPartnerInternalCode, @BusinessPartnerNameGer, " +
            "@Address, @CountryId, @CountryIdentifier, @CountryCode, @CountryName, " +
            "@CityId, @CityIdentifier, @CityCode, @CityName, " +
            "@MunicipalityId, @MunicipalityIdentifier, @MunicipalityCode, @MunicipalityName, " +
            "@RegionId, @RegionIdentifier, @RegionCode, @RegionName, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods

        private BusinessPartnerLocationViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            BusinessPartnerLocationViewModel dbEntry = new BusinessPartnerLocationViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.BusinessPartner = SQLiteHelper.GetBusinessPartner(query, ref counter);
            dbEntry.Address = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Country = SQLiteHelper.GetCountry(query, ref counter);
            dbEntry.City = SQLiteHelper.GetCity(query, ref counter);
            dbEntry.Municipality = SQLiteHelper.GetMunicipality(query, ref counter);
            dbEntry.Region = SQLiteHelper.GetRegion(query, ref counter);
            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);

            return dbEntry;
        }

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, BusinessPartnerLocationViewModel businessPartnerLocation)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", businessPartnerLocation.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", businessPartnerLocation.Identifier);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerId", ((object)businessPartnerLocation.BusinessPartner.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", ((object)businessPartnerLocation.BusinessPartner.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerCode", ((object)businessPartnerLocation.BusinessPartner.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerName", ((object)businessPartnerLocation.BusinessPartner.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerInternalCode", ((object)businessPartnerLocation.BusinessPartner.InternalCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerNameGer", ((object)businessPartnerLocation.BusinessPartner.NameGer) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Address", ((object)businessPartnerLocation.Address) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryId", ((object)businessPartnerLocation.Country?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryIdentifier", ((object)businessPartnerLocation.Country?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryCode", ((object)businessPartnerLocation.Country?.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryName", ((object)businessPartnerLocation.Country?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CityId", ((object)businessPartnerLocation.City?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CityIdentifier", ((object)businessPartnerLocation.City?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CityCode", ((object)businessPartnerLocation.City?.ZipCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CityName", ((object)businessPartnerLocation.City?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@MunicipalityId", ((object)businessPartnerLocation.Municipality?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@MunicipalityIdentifier", ((object)businessPartnerLocation.Municipality?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@MunicipalityCode", ((object)businessPartnerLocation.Municipality?.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@MunicipalityName", ((object)businessPartnerLocation.Municipality?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@RegionId", ((object)businessPartnerLocation.Region?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@RegionIdentifier", ((object)businessPartnerLocation.Region?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@RegionCode", ((object)businessPartnerLocation.Region?.RegionCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@RegionName", ((object)businessPartnerLocation.Region?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsSynced", businessPartnerLocation.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)businessPartnerLocation.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read

        public BusinessPartnerLocationListResponse GetBusinessPartnerLocationsByBusinessPartner(int companyId, Guid businessPartnerIdentifier)
        {
            BusinessPartnerLocationListResponse response = new BusinessPartnerLocationListResponse();
            List<BusinessPartnerLocationViewModel> businessPartnerLocations = new List<BusinessPartnerLocationViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM BusinessPartnerLocations " +
                        "WHERE BusinessPartnerIdentifier = @BusinessPartnerIdentifier " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC;", db);
                    
                    selectCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", businessPartnerIdentifier);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                        businessPartnerLocations.Add(Read(query));
                    

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.BusinessPartnerLocations = new List<BusinessPartnerLocationViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.BusinessPartnerLocations = businessPartnerLocations;
            return response;
        }

        public BusinessPartnerLocationResponse GetBusinessPartnerLocation(Guid identifier)
        {
            BusinessPartnerLocationResponse response = new BusinessPartnerLocationResponse();
            BusinessPartnerLocationViewModel businessPartnerLocation = new BusinessPartnerLocationViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM BusinessPartnerLocations " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                        businessPartnerLocation = Read(query);
                    
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.BusinessPartnerLocation = new BusinessPartnerLocationViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.BusinessPartnerLocation = businessPartnerLocation;
            return response;
        }

        #endregion

        #region Sync

        public void Sync(IBusinessPartnerLocationService BusinessPartnerLocationService, Action<int, int> callback = null)
        {
            try
            {
                SyncBusinessPartnerLocationRequest request = new SyncBusinessPartnerLocationRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                BusinessPartnerLocationListResponse response = BusinessPartnerLocationService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.BusinessPartnerLocations?.Count ?? 0;
                    List<BusinessPartnerLocationViewModel> businessPartnerLocationsFromDB = response.BusinessPartnerLocations;

                    using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM BusinessPartnerLocations WHERE Identifier = @Identifier";

                            SqliteCommand insertCommand = db.CreateCommand();
                            insertCommand.CommandText = SqlCommandInsertPart;

                            foreach (var businessPartnerLocation in businessPartnerLocationsFromDB)
                            {
                                deleteCommand.Parameters.AddWithValue("@Identifier", businessPartnerLocation.Identifier);
                                deleteCommand.ExecuteNonQuery();
                                deleteCommand.Parameters.Clear();

                                if (businessPartnerLocation.IsActive)
                                {
                                    businessPartnerLocation.IsSynced = true;

                                    insertCommand = AddCreateParameters(insertCommand, businessPartnerLocation);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from BusinessPartnerLocations WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from BusinessPartnerLocations WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
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

        #endregion

        #region Create

        public BusinessPartnerLocationResponse Create(BusinessPartnerLocationViewModel businessPartnerLocation)
        {
            BusinessPartnerLocationResponse response = new BusinessPartnerLocationResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;
                
                try
                {
                    insertCommand = AddCreateParameters(insertCommand, businessPartnerLocation);
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

        public BusinessPartnerLocationResponse Delete(Guid identifier)
        {
            BusinessPartnerLocationResponse response = new BusinessPartnerLocationResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM BusinessPartnerLocations WHERE Identifier = @Identifier";
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

        public BusinessPartnerLocationResponse DeleteAll()
        {
            BusinessPartnerLocationResponse response = new BusinessPartnerLocationResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM BusinessPartnerLocations";
                    
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
