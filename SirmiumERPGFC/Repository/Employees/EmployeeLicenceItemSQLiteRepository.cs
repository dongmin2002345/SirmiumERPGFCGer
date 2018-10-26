using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.Employees
{
    public class EmployeeLicenceItemSQLiteRepository
    {
        public static string EmployeeItemTableCreatePart =
               "CREATE TABLE IF NOT EXISTS EmployeeLicenceItems " +
               "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
               "ServerId INTEGER NULL, " +
               "Identifier GUID, " +
               "EmployeeId INTEGER NULL, " +
               "EmployeeIdentifier GUID NULL, " +
               "EmployeeCode NVARCHAR(48) NULL, " +
               "EmployeeName NVARCHAR(48) NULL, " +
               "LicenceId INTEGER NULL, " +
               "LicenceIdentifier GUID NULL, " +
               "LicenceCode NVARCHAR(48) NULL, " +
               "LicenceCategory NVARCHAR(48) NULL, " +
               "LicenceDescription NVARCHAR(48) NULL, " +
               "ValidFrom DATETIME NULL, " +
               "ValidTo DATETIME NULL, " +
               "CountryId INTEGER NULL, " +
               "CountryIdentifier GUID NULL, " +
               "CountryCode NVARCHAR(48) NULL, " +
               "CountryName NVARCHAR(48) NULL, " +
               "IsSynced BOOL NULL, " +
               "UpdatedAt DATETIME NULL, " +
               "CreatedById INTEGER NULL, " +
               "CreatedByName NVARCHAR(2048) NULL, " +
               "CompanyId INTEGER NULL, " +
               "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, EmployeeId, EmployeeIdentifier, " +
            "EmployeeCode, EmployeeName, LicenceId, LicenceIdentifier, " +
            "LicenceCode, LicenceCategory, LicenceDescription, ValidFrom, ValidTo, CountryId, CountryIdentifier, " +
            "CountryCode, CountryName, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO EmployeeLicenceItems " +
            "(Id, ServerId, Identifier, EmployeeId, EmployeeIdentifier, " +
            "EmployeeCode, EmployeeName, LicenceId, LicenceIdentifier, " +
            "LicenceCode, LicenceCategory, LicenceDescription, ValidFrom, ValidTo, CountryId, CountryIdentifier, " +
            "CountryCode, CountryName, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @EmployeeId, @EmployeeIdentifier, " +
            "@EmployeeCode, @EmployeeName, @LicenceId, @LicenceIdentifier, " +
            "@LicenceCode, @LicenceCategory, @LicenceDescription, @ValidFrom, @ValidTo, @CountryId, @CountryIdentifier, " +
            "@CountryCode, @CountryName, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";


        public EmployeeLicenceItemListResponse GetEmployeeLicencesByEmployee(int companyId, Guid EmployeeIdentifier)
        {
            EmployeeLicenceItemListResponse response = new EmployeeLicenceItemListResponse();
            List<EmployeeLicenceItemViewModel> EmployeeLicenceItems = new List<EmployeeLicenceItemViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM  EmployeeLicenceItems " +
                        "WHERE EmployeeIdentifier = @EmployeeIdentifier " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC;", db);
                    selectCommand.Parameters.AddWithValue("@EmployeeIdentifier", EmployeeIdentifier);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        EmployeeLicenceItemViewModel dbEntry = new EmployeeLicenceItemViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Employee = SQLiteHelper.GetEmployee(query, ref counter);
                        dbEntry.Licence = SQLiteHelper.GetLicence(query, ref counter);
                        dbEntry.ValidFrom = SQLiteHelper.GetDateTimeNullable(query, ref counter);
                        dbEntry.ValidTo = SQLiteHelper.GetDateTimeNullable(query, ref counter);
                        dbEntry.Country = SQLiteHelper.GetCountry(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        EmployeeLicenceItems.Add(dbEntry);
                    }

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.EmployeeLicenceItems = new List<EmployeeLicenceItemViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.EmployeeLicenceItems = EmployeeLicenceItems;
            return response;
        }

        public EmployeeLicenceItemResponse GetEmployeeLicenceItem(Guid identifier)
        {
            EmployeeLicenceItemResponse response = new EmployeeLicenceItemResponse();
            EmployeeLicenceItemViewModel EmployeeItem = new EmployeeLicenceItemViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM  EmployeeLicenceItems " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        int counter = 0;
                        EmployeeLicenceItemViewModel dbEntry = new EmployeeLicenceItemViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Employee = SQLiteHelper.GetEmployee(query, ref counter);
                        dbEntry.Licence = SQLiteHelper.GetLicence(query, ref counter);
                        dbEntry.ValidFrom = SQLiteHelper.GetDateTimeNullable(query, ref counter);
                        dbEntry.ValidTo = SQLiteHelper.GetDateTimeNullable(query, ref counter);
                        dbEntry.Country = SQLiteHelper.GetCountry(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        EmployeeItem = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.EmployeeLicenceItem = new EmployeeLicenceItemViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.EmployeeLicenceItem = EmployeeItem;
            return response;
        }

        public EmployeeLicenceItemListResponse GetUnSyncedItems(int companyId)
        {
            EmployeeLicenceItemListResponse response = new EmployeeLicenceItemListResponse();
            List<EmployeeLicenceItemViewModel> viewModels = new List<EmployeeLicenceItemViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM  EmployeeLicenceItems " +
                        "WHERE CompanyId = @CompanyId AND IsSynced = 0 " +
                        "ORDER BY Id DESC;", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        EmployeeLicenceItemViewModel dbEntry = new EmployeeLicenceItemViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Employee = SQLiteHelper.GetEmployee(query, ref counter);
                        dbEntry.Licence = SQLiteHelper.GetLicence(query, ref counter);
                        dbEntry.ValidFrom = SQLiteHelper.GetDateTimeNullable(query, ref counter);
                        dbEntry.ValidTo = SQLiteHelper.GetDateTimeNullable(query, ref counter);
                        dbEntry.Country = SQLiteHelper.GetCountry(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        viewModels.Add(dbEntry);
                    }

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.EmployeeLicenceItems = new List<EmployeeLicenceItemViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.EmployeeLicenceItems = viewModels;
            return response;
        }

        public void Sync(IEmployeeLicenceService EmployeeItemService)
        {
            var unSynced = GetUnSyncedItems(MainWindow.CurrentCompanyId);
            SyncEmployeeLicenceItemRequest request = new SyncEmployeeLicenceItemRequest();
            request.CompanyId = MainWindow.CurrentCompanyId;
            request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);
            request.UnSyncedEmployeeLicenceItems = unSynced?.EmployeeLicenceItems ?? new List<EmployeeLicenceItemViewModel>();

            EmployeeLicenceItemListResponse response = EmployeeItemService.Sync(request);
            if (response.Success)
            {
                List<EmployeeLicenceItemViewModel> EmployeeItemsFromDB = response.EmployeeLicenceItems;
                foreach (var EmployeeItem in EmployeeItemsFromDB.OrderBy(x => x.Id))
                {
                    Delete(EmployeeItem.Identifier);
                    EmployeeItem.IsSynced = true;
                    Create(EmployeeItem);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from  EmployeeLicenceItems WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from  EmployeeLicenceItems WHERE CompanyId = @CompanyId", db);
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

        public EmployeeLicenceItemResponse Create(EmployeeLicenceItemViewModel EmployeeItem)
        {
            EmployeeLicenceItemResponse response = new EmployeeLicenceItemResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = SqlCommandInsertPart;

                insertCommand.Parameters.AddWithValue("@ServerId", EmployeeItem.Id);
                insertCommand.Parameters.AddWithValue("@Identifier", EmployeeItem.Identifier);
                insertCommand.Parameters.AddWithValue("@EmployeeId", ((object)EmployeeItem.Employee.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@EmployeeIdentifier", ((object)EmployeeItem.Employee.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@EmployeeCode", ((object)EmployeeItem.Employee.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@EmployeeName", ((object)EmployeeItem.Employee.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@LicenceId", ((object)EmployeeItem.Licence.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@LicenceIdentifier", ((object)EmployeeItem.Licence.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@LicenceCode", ((object)EmployeeItem.Licence.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@LicenceCategory", ((object)EmployeeItem.Licence.Category) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@LicenceDescription", ((object)EmployeeItem.Licence.Description) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CountryId", ((object)EmployeeItem.Country.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CountryIdentifier", ((object)EmployeeItem.Country.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CountryCode", ((object)EmployeeItem.Country.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CountryName", ((object)EmployeeItem.Country.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ValidFrom", ((object)EmployeeItem.ValidFrom) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ValidTo", ((object)EmployeeItem.ValidTo) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IsSynced", EmployeeItem.IsSynced);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", EmployeeItem.UpdatedAt);
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

        public EmployeeLicenceItemResponse UpdateSyncStatus(Guid identifier, int serverId, bool isSynced)
        {
            EmployeeLicenceItemResponse response = new EmployeeLicenceItemResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "UPDATE EmployeeLicenceItems SET " +
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

        public EmployeeLicenceItemResponse Delete(Guid identifier)
        {
            EmployeeLicenceItemResponse response = new EmployeeLicenceItemResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "DELETE FROM  EmployeeLicenceItems WHERE Identifier = @Identifier";
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

        public EmployeeLicenceItemResponse DeleteAll()
        {
            EmployeeLicenceItemResponse response = new EmployeeLicenceItemResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM EmployeeLicenceItems";
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
