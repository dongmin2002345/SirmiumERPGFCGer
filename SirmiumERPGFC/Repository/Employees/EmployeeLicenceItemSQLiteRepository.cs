using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Gloabals;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;

namespace SirmiumERPGFC.Repository.Employees
{
    public class EmployeeLicenceItemSQLiteRepository
    {
        #region SQL

        public static string EmployeeItemTableCreatePart =
               "CREATE TABLE IF NOT EXISTS EmployeeLicenceItems " +
               "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
               "ServerId INTEGER NULL, " +
               "Identifier GUID, " +
               "EmployeeId INTEGER NULL, " +
               "EmployeeIdentifier GUID NULL, " +
               "EmployeeCode NVARCHAR(48) NULL, " +
               "EmployeeName NVARCHAR(2048) NULL, " +
               "EmployeeInternalCode NVARCHAR(48) NULL, " +
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
               "ItemStatus INTEGER NOT NULL, " +
               "IsSynced BOOL NULL, " +
               "UpdatedAt DATETIME NULL, " +
               "CreatedById INTEGER NULL, " +
               "CreatedByName NVARCHAR(2048) NULL, " +
               "CompanyId INTEGER NULL, " +
               "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, EmployeeId, EmployeeIdentifier, " +
            "EmployeeCode, EmployeeName, EmployeeInternalCode, LicenceId, LicenceIdentifier, " +
            "LicenceCode, LicenceCategory, LicenceDescription, ValidFrom, ValidTo, CountryId, CountryIdentifier, " +
            "CountryCode, CountryName, ItemStatus, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO EmployeeLicenceItems " +
            "(Id, ServerId, Identifier, EmployeeId, EmployeeIdentifier, " +
            "EmployeeCode, EmployeeName, EmployeeInternalCode, LicenceId, LicenceIdentifier, " +
            "LicenceCode, LicenceCategory, LicenceDescription, ValidFrom, ValidTo, CountryId, CountryIdentifier, " +
            "CountryCode, CountryName, ItemStatus, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @EmployeeId, @EmployeeIdentifier, " +
            "@EmployeeCode, @EmployeeName, @EmployeeInternalCode, @LicenceId, @LicenceIdentifier, " +
            "@LicenceCode, @LicenceCategory, @LicenceDescription, @ValidFrom, @ValidTo, @CountryId, @CountryIdentifier, " +
            "@CountryCode, @CountryName, @ItemStatus, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods
        private static EmployeeLicenceItemViewModel Read(SqliteDataReader query)
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
            dbEntry.ItemStatus = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
            return dbEntry;
        }

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, EmployeeLicenceItemViewModel EmployeeItem)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", EmployeeItem.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", EmployeeItem.Identifier);
            insertCommand.Parameters.AddWithValue("@EmployeeId", ((object)EmployeeItem.Employee.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmployeeIdentifier", ((object)EmployeeItem.Employee.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmployeeCode", ((object)EmployeeItem.Employee.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmployeeName", ((object)EmployeeItem.Employee.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmployeeInternalCode", ((object)EmployeeItem.Employee.EmployeeCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@LicenceId", ((object)EmployeeItem.Licence.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@LicenceIdentifier", ((object)EmployeeItem.Licence.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@LicenceCode", ((object)EmployeeItem.Licence.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@LicenceCategory", ((object)EmployeeItem.Licence.Category) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@LicenceDescription", ((object)EmployeeItem.Licence.Description) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ValidFrom", ((object)EmployeeItem.ValidFrom) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ValidTo", ((object)EmployeeItem.ValidTo) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryId", ((object)EmployeeItem.Country?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryIdentifier", ((object)EmployeeItem.Country?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryCode", ((object)EmployeeItem.Country?.Mark) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryName", ((object)EmployeeItem.Country?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ItemStatus", ((object)EmployeeItem.ItemStatus) ?? DBNull.Value);

            insertCommand.Parameters.AddWithValue("@IsSynced", EmployeeItem.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)EmployeeItem.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read

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
                        EmployeeLicenceItemViewModel dbEntry = Read(query);
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
                        EmployeeLicenceItemViewModel dbEntry = Read(query);
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

        #endregion

        #region Sync

        public void Sync(IEmployeeLicenceService EmployeeItemService, Action<int, int> callback = null)
        {
            try
            {
                SyncEmployeeLicenceItemRequest request = new SyncEmployeeLicenceItemRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                EmployeeLicenceItemListResponse response = EmployeeItemService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.EmployeeLicenceItems?.Count ?? 0;
                    List<EmployeeLicenceItemViewModel> employeeLicenceItemsFromDB = response.EmployeeLicenceItems;

                    using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM EmployeeLicenceItems WHERE Identifier = @Identifier";

                            SqliteCommand insertCommand = db.CreateCommand();
                            insertCommand.CommandText = SqlCommandInsertPart;

                            foreach (var employeeLicenceItem in employeeLicenceItemsFromDB)
                            {
                                deleteCommand.Parameters.AddWithValue("@Identifier", employeeLicenceItem.Identifier);
                                deleteCommand.ExecuteNonQuery();
                                deleteCommand.Parameters.Clear();

                                if (employeeLicenceItem.IsActive)
                                {
                                    employeeLicenceItem.IsSynced = true;

                                    insertCommand = AddCreateParameters(insertCommand, employeeLicenceItem);
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

        public EmployeeLicenceItemResponse Create(EmployeeLicenceItemViewModel EmployeeItem)
        {
            EmployeeLicenceItemResponse response = new EmployeeLicenceItemResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, EmployeeItem);
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

        public EmployeeLicenceItemResponse Delete(Guid identifier)
        {
            EmployeeLicenceItemResponse response = new EmployeeLicenceItemResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM  EmployeeLicenceItems WHERE Identifier = @Identifier";
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

        public EmployeeLicenceItemResponse SetStatusDeleted(Guid identifier)
        {
            EmployeeLicenceItemResponse response = new EmployeeLicenceItemResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "UPDATE EmployeeLicenceItems SET ItemStatus = @ItemStatus WHERE Identifier = @Identifier";
                insertCommand.Parameters.AddWithValue("@ItemStatus", ItemStatus.Deleted);
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
        #endregion
    }
}
