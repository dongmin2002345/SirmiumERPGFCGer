using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;

namespace SirmiumERPGFC.Repository.Employees
{
    public class EmployeeProfessionItemSQLiteRepository
    {
        #region SQL

        public static string EmployeeItemTableCreatePart =
               "CREATE TABLE IF NOT EXISTS EmployeeProfessionItems " +
               "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
               "ServerId INTEGER NULL, " +
               "Identifier GUID, " +
               "EmployeeId INTEGER NULL, " +
               "EmployeeIdentifier GUID NULL, " +
               "EmployeeCode NVARCHAR(48) NULL, " +
               "EmployeeName NVARCHAR(2048) NULL, " +
               "EmployeeInternalCode NVARCHAR(48) NULL, " +
               "ProfessionId INTEGER NULL, " +
               "ProfessionIdentifier GUID NULL, " +
               "ProfessionCode NVARCHAR(48) NULL, " +
               "ProfessionName NVARCHAR(2048) NULL, " +
               "ProfessionSecond NVARCHAR(48) NULL, " +
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
            "SELECT ServerId, Identifier, " +
            "EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeName, EmployeeInternalCode, " +
            "ProfessionId, ProfessionIdentifier, ProfessionCode, ProfessionName, ProfessionSecondCode, " +
            "CountryId, CountryIdentifier, CountryCode, CountryName, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO EmployeeProfessionItems " +
            "(Id, ServerId, Identifier, " +
            "EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeName, EmployeeInternalCode, " +
            "ProfessionId, ProfessionIdentifier, ProfessionCode, ProfessionName, ProfessionSecondCode, " +
            "CountryId, CountryIdentifier, CountryCode, CountryName, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, " +
            "@EmployeeId, @EmployeeIdentifier, @EmployeeCode, @EmployeeName, @EmployeeInternalCode, " +
            "@ProfessionId, @ProfessionIdentifier, @ProfessionCode, @ProfessionName, @ProfessionSecondCode, " +
            "@CountryId, @CountryIdentifier, @CountryCode, @CountryName, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods

        private EmployeeProfessionItemViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            EmployeeProfessionItemViewModel dbEntry = new EmployeeProfessionItemViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.Employee = SQLiteHelper.GetEmployee(query, ref counter);
            dbEntry.Profession = SQLiteHelper.GetProfession(query, ref counter);
            dbEntry.Country = SQLiteHelper.GetCountry(query, ref counter);
            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);

            return dbEntry;
        }

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, EmployeeProfessionItemViewModel EmployeeProfessionItem)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", EmployeeProfessionItem.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", EmployeeProfessionItem.Identifier);
            insertCommand.Parameters.AddWithValue("@EmployeeId", ((object)EmployeeProfessionItem.Employee?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmployeeIdentifier", ((object)EmployeeProfessionItem.Employee?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmployeeCode", ((object)EmployeeProfessionItem.Employee?.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmployeeName", ((object)EmployeeProfessionItem.Employee?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmployeeInternalCode", ((object)EmployeeProfessionItem.Employee?.EmployeeCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ProfessionId", ((object)EmployeeProfessionItem.Profession?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ProfessionIdentifier", ((object)EmployeeProfessionItem.Profession?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ProfessionCode", ((object)EmployeeProfessionItem.Profession?.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ProfessionName", ((object)EmployeeProfessionItem.Profession?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ProfessionSecondCode", ((object)EmployeeProfessionItem.Profession?.SecondCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryId", ((object)EmployeeProfessionItem.Country?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryIdentifier", ((object)EmployeeProfessionItem.Country?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryCode", ((object)EmployeeProfessionItem.Country?.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryName", ((object)EmployeeProfessionItem.Country?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsSynced", EmployeeProfessionItem.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)EmployeeProfessionItem.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read

        public EmployeeProfessionItemListResponse GetEmployeeProfessionsByEmployee(int companyId, Guid EmployeeIdentifier)
        {
            EmployeeProfessionItemListResponse response = new EmployeeProfessionItemListResponse();
            List<EmployeeProfessionItemViewModel>  EmployeeProfessionItems = new List<EmployeeProfessionItemViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM  EmployeeProfessionItems " +
                        "WHERE EmployeeIdentifier = @EmployeeIdentifier " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC;", db);
                    
                    selectCommand.Parameters.AddWithValue("@EmployeeIdentifier", EmployeeIdentifier);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                        EmployeeProfessionItems.Add(Read(query));
                    

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.EmployeeProfessionItems = new List<EmployeeProfessionItemViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.EmployeeProfessionItems = EmployeeProfessionItems;
            return response;
        }

        public EmployeeProfessionItemResponse GetEmployeeProfessionItem(Guid identifier)
        {
            EmployeeProfessionItemResponse response = new EmployeeProfessionItemResponse();
            EmployeeProfessionItemViewModel EmployeeItem = new EmployeeProfessionItemViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM  EmployeeProfessionItems " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                        EmployeeItem = Read(query);
                    
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.EmployeeProfessionItem = new EmployeeProfessionItemViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.EmployeeProfessionItem = EmployeeItem;
            return response;
        }

        #endregion

        #region Sync

        public void Sync(IEmployeeProfessionService EmployeeItemService, Action<int, int> callback = null)
        {
            try
            {
                SyncEmployeeProfessionItemRequest request = new SyncEmployeeProfessionItemRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                EmployeeProfessionItemListResponse response = EmployeeItemService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.EmployeeProfessionItems?.Count ?? 0;
                    List<EmployeeProfessionItemViewModel> employeeProfessionItemsFromDB = response.EmployeeProfessionItems;

                    using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM EmployeeProfessionItems WHERE Identifier = @Identifier";

                            SqliteCommand insertCommand = db.CreateCommand();
                            insertCommand.CommandText = SqlCommandInsertPart;

                            foreach (var employeeProfessionItem in employeeProfessionItemsFromDB)
                            {
                                deleteCommand.Parameters.AddWithValue("@Identifier", employeeProfessionItem.Identifier);
                                deleteCommand.ExecuteNonQuery();
                                deleteCommand.Parameters.Clear();

                                if (employeeProfessionItem.IsActive)
                                {
                                    employeeProfessionItem.IsSynced = true;

                                    insertCommand = AddCreateParameters(insertCommand, employeeProfessionItem);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from  EmployeeProfessionItems WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from  EmployeeProfessionItems WHERE CompanyId = @CompanyId", db);
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

        public EmployeeProfessionItemResponse Create(EmployeeProfessionItemViewModel EmployeeProfessionItem)
        {
            EmployeeProfessionItemResponse response = new EmployeeProfessionItemResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, EmployeeProfessionItem);
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

        public EmployeeProfessionItemResponse Delete(Guid identifier)
        {
            EmployeeProfessionItemResponse response = new EmployeeProfessionItemResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM  EmployeeProfessionItems WHERE Identifier = @Identifier";
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

        public EmployeeProfessionItemResponse DeleteAll()
        {
            EmployeeProfessionItemResponse response = new EmployeeProfessionItemResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM EmployeeProfessionItems";
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
