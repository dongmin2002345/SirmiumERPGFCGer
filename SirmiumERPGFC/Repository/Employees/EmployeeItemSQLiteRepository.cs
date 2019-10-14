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
    public class EmployeeItemSQLiteRepository
    {
        #region SQL

        public static string EmployeeItemTableCreatePart =
               "CREATE TABLE IF NOT EXISTS EmployeeItems " +
               "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
               "ServerId INTEGER NULL, " +
               "Identifier GUID, " +
               "EmployeeId INTEGER NULL, " +
               "EmployeeIdentifier GUID NULL, " +
               "EmployeeCode NVARCHAR(48) NULL, " +
               "EmployeeName NVARCHAR(2048) NULL, " +
               "EmployeeInternalCode NVARCHAR(48) NULL, " +
               "FamilyMemberId INTEGER NULL, " +
               "FamilyMemberIdentifier GUID NULL, " +
               "FamilyMemberCode NVARCHAR(48) NULL, " +
               "FamilyMemberName NVARCHAR(48) NULL, " +
               "Name NVARCHAR(2048), " +
               "DateOfBirth DATETIME NULL, " +
               "EmbassyDate DATETIME NULL, " +
               "Passport NVARCHAR(2048) NULL, " +
                "ItemStatus INTEGER NOT NULL, " +
               "IsSynced BOOL NULL, " +
               "UpdatedAt DATETIME NULL, " +
               "CreatedById INTEGER NULL, " +
               "CreatedByName NVARCHAR(2048) NULL, " +
               "CompanyId INTEGER NULL, " +
               "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, EmployeeId, EmployeeIdentifier, " +
            "EmployeeCode, EmployeeName, EmployeeInternalCode, FamilyMemberId, FamilyMemberIdentifier, " +
            "FamilyMemberCode, FamilyMemberName, Name, DateOfBirth, EmbassyDate, Passport, ItemStatus, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO EmployeeItems " +
            "(Id, ServerId, Identifier, EmployeeId, EmployeeIdentifier, " +
            "EmployeeCode, EmployeeName, EmployeeInternalCode, FamilyMemberId, FamilyMemberIdentifier, " +
            "FamilyMemberCode, FamilyMemberName, Name, DateOfBirth, EmbassyDate, Passport, ItemStatus, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @EmployeeId, @EmployeeIdentifier, " +
            "@EmployeeCode, @EmployeeName, @EmployeeInternalCode, @FamilyMemberId, @FamilyMemberIdentifier, " +
            "@FamilyMemberCode, @FamilyMemberName, @Name, @DateOfBirth, @EmbassyDate, @Passport, @ItemStatus, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #region Helper methods
        private static EmployeeItemViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            EmployeeItemViewModel dbEntry = new EmployeeItemViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.Employee = SQLiteHelper.GetEmployee(query, ref counter);
            dbEntry.FamilyMember = SQLiteHelper.GetFamilyMember(query, ref counter);
            dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
            dbEntry.DateOfBirth = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.EmbassyDate = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.Passport = SQLiteHelper.GetString(query, ref counter);
            dbEntry.ItemStatus = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
            return dbEntry;
        }

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, EmployeeItemViewModel EmployeeItem)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", EmployeeItem.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", EmployeeItem.Identifier);
            insertCommand.Parameters.AddWithValue("@EmployeeId", ((object)EmployeeItem.Employee.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmployeeIdentifier", ((object)EmployeeItem.Employee.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmployeeCode", ((object)EmployeeItem.Employee.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmployeeName", ((object)EmployeeItem.Employee.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmployeeInternalCode", ((object)EmployeeItem.Employee.EmployeeCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@FamilyMemberId", ((object)EmployeeItem.FamilyMember.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@FamilyMemberIdentifier", ((object)EmployeeItem.Employee.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@FamilyMemberCode", ((object)EmployeeItem.FamilyMember.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@FamilyMemberName", ((object)EmployeeItem.FamilyMember.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Name", EmployeeItem.Name);
            insertCommand.Parameters.AddWithValue("@DateOfBirth", ((object)EmployeeItem.DateOfBirth) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmbassyDate", ((object)EmployeeItem.EmbassyDate) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Passport", ((object)EmployeeItem.Passport) ?? DBNull.Value);
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

        public EmployeeItemListResponse GetEmployeeItemsByEmployee(int companyId, Guid EmployeeIdentifier)
        {
            EmployeeItemListResponse response = new EmployeeItemListResponse();
            List<EmployeeItemViewModel> EmployeeItems = new List<EmployeeItemViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM EmployeeItems " +
                        "WHERE EmployeeIdentifier = @EmployeeIdentifier " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC;", db);
                    
                    selectCommand.Parameters.AddWithValue("@EmployeeIdentifier", EmployeeIdentifier);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                        EmployeeItems.Add(Read(query));
                    

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.EmployeeItems = new List<EmployeeItemViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.EmployeeItems = EmployeeItems;
            return response;
        }

        public EmployeeItemResponse GetEmployeeItem(Guid identifier)
        {
            EmployeeItemResponse response = new EmployeeItemResponse();
            EmployeeItemViewModel EmployeeItem = new EmployeeItemViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM EmployeeItems " +
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
                    response.EmployeeItem = new EmployeeItemViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.EmployeeItem = EmployeeItem;
            return response;
        }

        public EmployeeItemListResponse GetUnSyncedItems(int companyId)
        {
            EmployeeItemListResponse response = new EmployeeItemListResponse();
            List<EmployeeItemViewModel> EmployeeItems = new List<EmployeeItemViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM  EmployeeItems " +
                        "WHERE CompanyId = @CompanyId AND IsSynced = 0 " +
                        "ORDER BY Id DESC;", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                        EmployeeItems.Add(Read(query));
                    

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.EmployeeItems = new List<EmployeeItemViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.EmployeeItems = EmployeeItems;
            return response;
        }

        #endregion

        #region Sync

        public void Sync(IEmployeeItemService EmployeeItemService, Action<int, int> callback = null)
        {
            try
            {
                SyncEmployeeItemRequest request = new SyncEmployeeItemRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                EmployeeItemListResponse response = EmployeeItemService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.EmployeeItems?.Count ?? 0;
                    List<EmployeeItemViewModel> employeeItemsFromDB = response.EmployeeItems;

                    using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM EmployeeItems WHERE Identifier = @Identifier";

                            SqliteCommand insertCommand = db.CreateCommand();
                            insertCommand.CommandText = SqlCommandInsertPart;

                            foreach (var employeeItem in employeeItemsFromDB)
                            {
                                deleteCommand.Parameters.AddWithValue("@Identifier", employeeItem.Identifier);
                                deleteCommand.ExecuteNonQuery();
                                deleteCommand.Parameters.Clear();

                                if (employeeItem.IsActive)
                                {
                                    employeeItem.IsSynced = true;

                                    insertCommand = AddCreateParameters(insertCommand, employeeItem);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from EmployeeItems WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from EmployeeItems WHERE CompanyId = @CompanyId", db);
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

        public EmployeeItemResponse Create(EmployeeItemViewModel EmployeeItem)
        {
            EmployeeItemResponse response = new EmployeeItemResponse();

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

        public EmployeeItemResponse Delete(Guid identifier)
        {
            EmployeeItemResponse response = new EmployeeItemResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM EmployeeItems WHERE Identifier = @Identifier";
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

        public EmployeeItemResponse SetStatusDeleted(Guid identifier)
        {
            EmployeeItemResponse response = new EmployeeItemResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "UPDATE EmployeeItems SET ItemStatus = @ItemStatus WHERE Identifier = @Identifier";
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
    }
}
