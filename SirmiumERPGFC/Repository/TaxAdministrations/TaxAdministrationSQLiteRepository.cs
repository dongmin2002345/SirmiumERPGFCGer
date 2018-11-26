﻿using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.TaxAdministrations;
using ServiceInterfaces.Messages.Common.TaxAdministrations;
using ServiceInterfaces.ViewModels.Common.TaxAdministrations;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.TaxAdministrations
{
    public class TaxAdministrationSQLiteRepository
    {
        public static string TaxAdministrationTableCreatePart =
          "CREATE TABLE IF NOT EXISTS TaxAdministrations " +
          "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
          "ServerId INTEGER NULL, " +
          "Identifier GUID, " +
          "Code NVARCHAR(48) NULL, " +
          "SecondCode NVARCHAR(48) NULL, " +
          "Name NVARCHAR(48) NULL, " +
          "Address1 NVARCHAR(48) NULL, " +
          "Address2 NVARCHAR(48) NULL, " +
          "Address3 NVARCHAR(48) NULL, " +
          "CityId INTEGER NULL, " +
          "CityIdentifier GUID NULL, " +
          "CityCode NVARCHAR(48) NULL, " +
          "CityName NVARCHAR(48) NULL, " +
          "BankId1 INTEGER NULL, " +
          "BankIdentifier1 GUID NULL, " +
          "BankCode1 NVARCHAR(48) NULL, " +
          "BankName1 NVARCHAR(48) NULL, " +
          "BankId2 INTEGER NULL, " +
          "BankIdentifier2 GUID NULL, " +
          "BankCode2 NVARCHAR(48) NULL, " +
          "BankName2 NVARCHAR(48) NULL, " +
          "IBAN1 INTEGER NULL, " +
          "SWIFT INTEGER NULL, " +
          "IsSynced BOOL NULL, " +
          "UpdatedAt DATETIME NULL, " +
          "CreatedById INTEGER NULL, " +
          "CreatedByName NVARCHAR(2048) NULL, " +
          "CompanyId INTEGER NULL, " +
          "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, Code, SecondCode, Name, Address1, Address2, Address3,  " +
            "CityId, CityIdentifier, CityCode, CityName, " +
            "BankId1, BankIdentifier1, BankCode1, BankName1, " +
            "BankId2, BankIdentifier2, BankCode2, BankName2, " +
            "IBAN1, SWIFT, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO TaxAdministrations " +
            "(Id, ServerId, Identifier, Code, SecondCode, Name, Address1, Address2, Address3,  " +
            "CityId, CityIdentifier, CityCode, CityName, " +
            "BankId1, BankIdentifier1, BankCode1, BankName1, " +
            "BankId2, BankIdentifier2, BankCode2, BankName2, " +
            "IBAN1, SWIFT, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @Code, @SecondCode, @Name, @Address1, @Address2, @Address3, " +
            "@CityId, @CityIdentifier, @CityCode, @CityName, " +
            "@BankId1, @BankIdentifier1, @BankCode1, @BankName1, " +
            "@BankId2, @BankIdentifier2, @BankCode2, @BankName2, " +
            "@IBAN1, @SWIFT, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        public TaxAdministrationListResponse GetTaxAdministrationsByPage(int companyId, TaxAdministrationViewModel TaxAdministrationSearchObject, int currentPage = 1, int itemsPerPage = 50)
        {
            TaxAdministrationListResponse response = new TaxAdministrationListResponse();
            List<TaxAdministrationViewModel> TaxAdministrations = new List<TaxAdministrationViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM TaxAdministrations " +
                        "WHERE (@Code IS NULL OR @Code = '' OR Code LIKE @Code) " +
                        "AND (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND (@CityName IS NULL OR @CityName = '' OR CityName LIKE @CityName) " +
                        "AND (@BankName1 IS NULL OR @BankName1 = '' OR BankName1 LIKE @BankName1) " +
                        "AND (@IBAN1 IS NULL OR @IBAN1 = '' OR IBAN1 LIKE @IBAN1) " +
                        "AND (@SWIFT IS NULL OR @SWIFT = '' OR SWIFT LIKE @SWIFT) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage OFFSET @Offset;", db);
                    selectCommand.Parameters.AddWithValue("@Code", ((object)TaxAdministrationSearchObject.SearchBy_Code) != null ? "%" + TaxAdministrationSearchObject.SearchBy_Code + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Name", ((object)TaxAdministrationSearchObject.SearchBy_Name) != null ? "%" + TaxAdministrationSearchObject.SearchBy_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CityName", ((object)TaxAdministrationSearchObject.SearchBy_City) != null ? "%" + TaxAdministrationSearchObject.SearchBy_City + "%" : "");
                    selectCommand.Parameters.AddWithValue("@BankName1", ((object)TaxAdministrationSearchObject.SearchBy_Bank1) != null ? "%" + TaxAdministrationSearchObject.SearchBy_Bank1 + "%" : "");
                    selectCommand.Parameters.AddWithValue("@IBAN1", ((object)TaxAdministrationSearchObject.SearchBy_IBAN1) != null ? "%" + TaxAdministrationSearchObject.SearchBy_IBAN1 + "%" : "");
                    selectCommand.Parameters.AddWithValue("@SWIFT", ((object)TaxAdministrationSearchObject.SearchBy_SWIFT) != null ? "%" + TaxAdministrationSearchObject.SearchBy_SWIFT + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
                    selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        TaxAdministrationViewModel dbEntry = new TaxAdministrationViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.SecondCode = SQLiteHelper.GetString(query, ref counter);

                        dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Address1 = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Address2 = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Address3 = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.City = SQLiteHelper.GetCity(query, ref counter);
                        dbEntry.Bank1 = SQLiteHelper.GetBank(query, ref counter);
                        dbEntry.Bank2 = SQLiteHelper.GetBank(query, ref counter);
                        dbEntry.IBAN1 = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.SWIFT = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        TaxAdministrations.Add(dbEntry);
                    }


                    selectCommand = new SqliteCommand(
                        "SELECT Count(*) " +
                        "FROM TaxAdministrations " +
                        "WHERE (@Code IS NULL OR @Code = '' OR Code LIKE @Code) " +
                        "AND (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND (@CityName IS NULL OR @CityName = '' OR CityName LIKE @CityName) " +
                        "AND (@BankName1 IS NULL OR @BankName1 = '' OR BankName1 LIKE @BankName1) " +
                        "AND (@IBAN1 IS NULL OR @IBAN1 = '' OR IBAN1 LIKE @IBAN1) " +
                        "AND (@SWIFT IS NULL OR @SWIFT = '' OR SWIFT LIKE @SWIFT) " +
                        "AND CompanyId = @CompanyId;", db);
                    selectCommand.Parameters.AddWithValue("@Code", ((object)TaxAdministrationSearchObject.SearchBy_Code) != null ? "%" + TaxAdministrationSearchObject.SearchBy_Code + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Name", ((object)TaxAdministrationSearchObject.SearchBy_Name) != null ? "%" + TaxAdministrationSearchObject.SearchBy_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CityName", ((object)TaxAdministrationSearchObject.SearchBy_City) != null ? "%" + TaxAdministrationSearchObject.SearchBy_City + "%" : "");
                    selectCommand.Parameters.AddWithValue("@BankName1", ((object)TaxAdministrationSearchObject.SearchBy_Bank1) != null ? "%" + TaxAdministrationSearchObject.SearchBy_Bank1 + "%" : "");
                    selectCommand.Parameters.AddWithValue("@IBAN1", ((object)TaxAdministrationSearchObject.SearchBy_IBAN1) != null ? "%" + TaxAdministrationSearchObject.SearchBy_IBAN1 + "%" : "");
                    selectCommand.Parameters.AddWithValue("@SWIFT", ((object)TaxAdministrationSearchObject.SearchBy_SWIFT) != null ? "%" + TaxAdministrationSearchObject.SearchBy_SWIFT + "%" : "");
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
                    response.TaxAdministrations = new List<TaxAdministrationViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.TaxAdministrations = TaxAdministrations;
            return response;
        }

        public TaxAdministrationListResponse TaxAdministrationsForPopup(int companyId, string filterString)
        {
            TaxAdministrationListResponse response = new TaxAdministrationListResponse();
            List<TaxAdministrationViewModel> TaxAdministrations = new List<TaxAdministrationViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM TaxAdministrations " +
                        "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name OR Name LIKE @Name) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage;", db);
                    selectCommand.Parameters.AddWithValue("@Name", ((object)filterString) != null ? "%" + filterString + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", ((object)filterString) != null ? companyId : 0);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", 100);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        TaxAdministrationViewModel dbEntry = new TaxAdministrationViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.SecondCode = SQLiteHelper.GetString(query, ref counter);

                        dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Address1 = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Address2 = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Address3 = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.City = SQLiteHelper.GetCity(query, ref counter);
                        dbEntry.Bank1 = SQLiteHelper.GetBank(query, ref counter);
                        dbEntry.Bank2 = SQLiteHelper.GetBank(query, ref counter);
                        dbEntry.IBAN1 = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.SWIFT = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        TaxAdministrations.Add(dbEntry);
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.TaxAdministrations = new List<TaxAdministrationViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.TaxAdministrations = TaxAdministrations;
            return response;
        }

        public TaxAdministrationResponse GetTaxAdministration(Guid identifier)
        {
            TaxAdministrationResponse response = new TaxAdministrationResponse();
            TaxAdministrationViewModel TaxAdministration = new TaxAdministrationViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM TaxAdministrations " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        int counter = 0;
                        TaxAdministrationViewModel dbEntry = new TaxAdministrationViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.SecondCode = SQLiteHelper.GetString(query, ref counter);

                        dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Address1 = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Address2 = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Address3 = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.City = SQLiteHelper.GetCity(query, ref counter);
                        dbEntry.Bank1 = SQLiteHelper.GetBank(query, ref counter);
                        dbEntry.Bank2 = SQLiteHelper.GetBank(query, ref counter);
                        dbEntry.IBAN1 = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.SWIFT = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        TaxAdministration = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.TaxAdministration = new TaxAdministrationViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.TaxAdministration = TaxAdministration;
            return response;
        }

        public void Sync(ITaxAdministrationService taxAdministrationService)
        {
            SyncTaxAdministrationRequest request = new SyncTaxAdministrationRequest();
            request.CompanyId = MainWindow.CurrentCompanyId;
            request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

            TaxAdministrationListResponse response = taxAdministrationService.Sync(request);
            if (response.Success)
            {
                List<TaxAdministrationViewModel> taxAdministrationsFromDB = response.TaxAdministrations;
                foreach (var taxAdministration in taxAdministrationsFromDB.OrderBy(x => x.Id))
                {
                    Delete(taxAdministration.Identifier);
                    taxAdministration.IsSynced = true;
                    Create(taxAdministration);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from TaxAdministrations WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from TaxAdministrations WHERE CompanyId = @CompanyId", db);
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

        public TaxAdministrationResponse Create(TaxAdministrationViewModel taxAdministration)
        {
            TaxAdministrationResponse response = new TaxAdministrationResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = SqlCommandInsertPart;

                insertCommand.Parameters.AddWithValue("@ServerId", taxAdministration.Id);
                insertCommand.Parameters.AddWithValue("@Identifier", taxAdministration.Identifier);
                insertCommand.Parameters.AddWithValue("@Code", ((object)taxAdministration.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@SecondCode", ((object)taxAdministration.SecondCode) ?? DBNull.Value);

                insertCommand.Parameters.AddWithValue("@Name", ((object)taxAdministration.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Address1", ((object)taxAdministration.Address1) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Address2", ((object)taxAdministration.Address2) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Address3", ((object)taxAdministration.Address3) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CityId", ((object)taxAdministration.City?.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CityIdentifier", ((object)taxAdministration.City?.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CityCode", ((object)taxAdministration.City?.ZipCode) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CityName", ((object)taxAdministration.City?.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BankId1", ((object)taxAdministration.Bank1?.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BankIdentifier1", ((object)taxAdministration.Bank1?.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BankCode1", ((object)taxAdministration.Bank1?.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BankName1", ((object)taxAdministration.Bank1?.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BankId2", ((object)taxAdministration.Bank2?.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BankIdentifier2", ((object)taxAdministration.Bank2?.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BankCode2", ((object)taxAdministration.Bank2?.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BankName2", ((object)taxAdministration.Bank2?.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IBAN1", ((object)taxAdministration.IBAN1) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@SWIFT", ((object)taxAdministration.SWIFT) ?? DBNull.Value);

                insertCommand.Parameters.AddWithValue("@IsSynced", taxAdministration.IsSynced);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)taxAdministration.UpdatedAt) ?? DBNull.Value);
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

        public TaxAdministrationResponse UpdateSyncStatus(Guid identifier, string code, DateTime? updatedAt, int serverId, bool isSynced)
        {
            TaxAdministrationResponse response = new TaxAdministrationResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "UPDATE TaxAdministrations SET " +
                    "IsSynced = @IsSynced, " +
                    "Code = @Code, " +
                    "UpdatedAt = @UpdatedAt, " +
                    "ServerId = @ServerId " +
                    "WHERE Identifier = @Identifier ";

                insertCommand.Parameters.AddWithValue("@IsSynced", isSynced);
                insertCommand.Parameters.AddWithValue("@Code", code);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", updatedAt);
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

        public TaxAdministrationResponse Delete(Guid identifier)
        {
            TaxAdministrationResponse response = new TaxAdministrationResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "DELETE FROM TaxAdministrations WHERE Identifier = @Identifier";
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

        public TaxAdministrationResponse DeleteAll()
        {
            TaxAdministrationResponse response = new TaxAdministrationResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM TaxAdministrations";
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