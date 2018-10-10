using Microsoft.Data.Sqlite;
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
    public class BusinessPartnerOrganizationUnitSQLiteRepository
    {
        public static string BusinessPartnerOrganizationUnitTableCreatePart =
            "CREATE TABLE IF NOT EXISTS BusinessPartnerOrganizationUnits " +
            "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
            "ServerId INTEGER NULL, " +
            "Identifier GUID, " +
            "Code NVARCHAR(48) NULL, " +
            "Name NVARCHAR(48) NULL, " +
            "BusinessPartnerId INTEGER NULL, " +
            "BusinessPartnerIdentifier GUID NULL, " +
            "BusinessPartnerCode NVARCHAR(2048) NULL, " +
            "BusinessPartnerName NVARCHAR(2048) NULL, " +
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
            "ContactPerson NVARCHAR(2048) NULL, " +
            "Phone NVARCHAR(2048) NULL, " +
            "Mobile NVARCHAR(2048) NULL, " +
            "IsSynced BOOL NULL, " +
            "UpdatedAt DATETIME NULL, " +
            "CreatedById INTEGER NULL, " +
            "CreatedByName NVARCHAR(2048) NULL, " +
            "CompanyId INTEGER NULL, " +
            "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, Code, Name, BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
            "Address, CountryId, CountryIdentifier, CountryCode, CountryName, " +
            "CityId, CityIdentifier, CityCode, CityName, " +
            "MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +
            "ContactPerson, Phone, Mobile, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO BusinessPartnerOrganizationUnits " +
            "(Id, ServerId, Identifier, Code, Name, BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
            "Address, CountryId, CountryIdentifier, CountryCode, CountryName, " +
            "CityId, CityIdentifier, CityCode, CityName, " +
            "MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +
            "ContactPerson, Phone, Mobile, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @Code, @Name, @BusinessPartnerId, @BusinessPartnerIdentifier, @BusinessPartnerCode, @BusinessPartnerName, " +
            "@Address, @CountryId, @CountryIdentifier, @CountryCode, @CountryName, " +
            "@CityId, @CityIdentifier, @CityCode, @CityName, " +
            "@MunicipalityId, @MunicipalityIdentifier, @MunicipalityCode, @MunicipalityName, " +
            "@ContactPerson, @Phone, @Mobile, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        public BusinessPartnerOrganizationUnitListResponse GetBusinessPartnerOrganizationUnitsByBusinessPartner(int companyId, Guid businessPartnerIdentifier)
        {
            BusinessPartnerOrganizationUnitListResponse response = new BusinessPartnerOrganizationUnitListResponse();
            List<BusinessPartnerOrganizationUnitViewModel> businessPartnerOrganizationUnits = new List<BusinessPartnerOrganizationUnitViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM BusinessPartnerOrganizationUnits " +
                        "WHERE BusinessPartnerIdentifier = @BusinessPartnerIdentifier " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC;", db);
                    selectCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", businessPartnerIdentifier);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        BusinessPartnerOrganizationUnitViewModel dbEntry = new BusinessPartnerOrganizationUnitViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.BusinessPartner = SQLiteHelper.GetBusinessPartner(query, ref counter);
                        dbEntry.Address = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Country = SQLiteHelper.GetCountry(query, ref counter);
                        dbEntry.City = SQLiteHelper.GetCity(query, ref counter);
                        dbEntry.Municipality = SQLiteHelper.GetMunicipality(query, ref counter);
                        dbEntry.ContactPerson = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Phone = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Mobile = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        businessPartnerOrganizationUnits.Add(dbEntry);
                    }

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.BusinessPartnerOrganizationUnits = new List<BusinessPartnerOrganizationUnitViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.BusinessPartnerOrganizationUnits = businessPartnerOrganizationUnits;
            return response;
        }

        public BusinessPartnerOrganizationUnitListResponse GetUnSyncedBusinessPartnerOrganizationUnits(int companyId)
        {
            BusinessPartnerOrganizationUnitListResponse response = new BusinessPartnerOrganizationUnitListResponse();
            List<BusinessPartnerOrganizationUnitViewModel> businessPartnerOrganizationUnits = new List<BusinessPartnerOrganizationUnitViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM BusinessPartnerOrganizationUnits " +
                        "WHERE CompanyId = @CompanyId AND IsSynced = 0 " +
                        "ORDER BY Id DESC;", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        BusinessPartnerOrganizationUnitViewModel dbEntry = new BusinessPartnerOrganizationUnitViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.BusinessPartner = SQLiteHelper.GetBusinessPartner(query, ref counter);
                        dbEntry.Address = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Country = SQLiteHelper.GetCountry(query, ref counter);
                        dbEntry.City = SQLiteHelper.GetCity(query, ref counter);
                        dbEntry.Municipality = SQLiteHelper.GetMunicipality(query, ref counter);
                        dbEntry.ContactPerson = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Phone = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Mobile = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        businessPartnerOrganizationUnits.Add(dbEntry);
                    }

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.BusinessPartnerOrganizationUnits = new List<BusinessPartnerOrganizationUnitViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.BusinessPartnerOrganizationUnits = businessPartnerOrganizationUnits;
            return response;
        }

        public BusinessPartnerOrganizationUnitResponse GetBusinessPartnerOrganizationUnit(Guid identifier)
        {
            BusinessPartnerOrganizationUnitResponse response = new BusinessPartnerOrganizationUnitResponse();
            BusinessPartnerOrganizationUnitViewModel businessPartnerOrganizationUnit = new BusinessPartnerOrganizationUnitViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM BusinessPartnerOrganizationUnits " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        int counter = 0;
                        BusinessPartnerOrganizationUnitViewModel dbEntry = new BusinessPartnerOrganizationUnitViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.BusinessPartner = SQLiteHelper.GetBusinessPartner(query, ref counter);
                        dbEntry.Address = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Country = SQLiteHelper.GetCountry(query, ref counter);
                        dbEntry.City = SQLiteHelper.GetCity(query, ref counter);
                        dbEntry.Municipality = SQLiteHelper.GetMunicipality(query, ref counter);
                        dbEntry.ContactPerson = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Phone = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Mobile = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        businessPartnerOrganizationUnit = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.BusinessPartnerOrganizationUnit = new BusinessPartnerOrganizationUnitViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.BusinessPartnerOrganizationUnit = businessPartnerOrganizationUnit;
            return response;
        }

        public DateTime? GetLastUpdatedAt(int companyId)
        {
            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from BusinessPartnerOrganizationUnits WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from BusinessPartnerOrganizationUnits WHERE CompanyId = @CompanyId", db);
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

        public BusinessPartnerOrganizationUnitResponse Create(BusinessPartnerOrganizationUnitViewModel businessPartnerOrganizationUnit)
        {
            BusinessPartnerOrganizationUnitResponse response = new BusinessPartnerOrganizationUnitResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = SqlCommandInsertPart;

                insertCommand.Parameters.AddWithValue("@ServerId", businessPartnerOrganizationUnit.Id);
                insertCommand.Parameters.AddWithValue("@Identifier", businessPartnerOrganizationUnit.Identifier);
                insertCommand.Parameters.AddWithValue("@Code", businessPartnerOrganizationUnit.Code);
                insertCommand.Parameters.AddWithValue("@Name", ((object)businessPartnerOrganizationUnit.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerId", ((object)businessPartnerOrganizationUnit.BusinessPartner.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", ((object)businessPartnerOrganizationUnit.BusinessPartner.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerCode", ((object)businessPartnerOrganizationUnit.BusinessPartner.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerName", ((object)businessPartnerOrganizationUnit.BusinessPartner.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Address", ((object)businessPartnerOrganizationUnit.Address) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CountryId", ((object)businessPartnerOrganizationUnit.Country?.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CountryIdentifier", ((object)businessPartnerOrganizationUnit.Country?.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CountryCode", ((object)businessPartnerOrganizationUnit.Country?.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CountryName", ((object)businessPartnerOrganizationUnit.Country?.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CityId", ((object)businessPartnerOrganizationUnit.City?.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CityIdentifier", ((object)businessPartnerOrganizationUnit.City?.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CityCode", ((object)businessPartnerOrganizationUnit.City?.ZipCode) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CityName", ((object)businessPartnerOrganizationUnit.City?.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@MunicipalityId", ((object)businessPartnerOrganizationUnit.Municipality?.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@MunicipalityIdentifier", ((object)businessPartnerOrganizationUnit.Municipality?.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@MunicipalityCode", ((object)businessPartnerOrganizationUnit.Municipality?.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@MunicipalityName", ((object)businessPartnerOrganizationUnit.Municipality?.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ContactPerson", ((object)businessPartnerOrganizationUnit.ContactPerson) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Phone", ((object)businessPartnerOrganizationUnit.Phone) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Mobile", ((object)businessPartnerOrganizationUnit.Mobile) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IsSynced", businessPartnerOrganizationUnit.IsSynced);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", businessPartnerOrganizationUnit.UpdatedAt);
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

        public BusinessPartnerOrganizationUnitResponse UpdateSyncStatus(Guid identifier, int serverId, bool isSynced)
        {
            BusinessPartnerOrganizationUnitResponse response = new BusinessPartnerOrganizationUnitResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "UPDATE BusinessPartnerOrganizationUnits SET " +
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

        public BusinessPartnerOrganizationUnitResponse Delete(Guid identifier)
        {
            BusinessPartnerOrganizationUnitResponse response = new BusinessPartnerOrganizationUnitResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "DELETE FROM BusinessPartnerOrganizationUnits WHERE Identifier = @Identifier";
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

        public BusinessPartnerOrganizationUnitResponse DeleteAll()
        {
            BusinessPartnerOrganizationUnitResponse response = new BusinessPartnerOrganizationUnitResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM BusinessPartnerOrganizationUnits";
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

