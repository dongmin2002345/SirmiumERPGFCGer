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
    public class EmployeeAttachmentSQLiteRepository
    {
        #region SQL

        public static string EmployeeAttachmentsTableCreatePart =
            @"CREATE TABLE IF NOT EXISTS EmployeeAttachments  
                (Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                ServerId INTEGER NULL, 
                Identifier GUID, 
                Code NVARCHAR(2048) NULL, 
                OK BOOL NULL, 

                EmployeeId INTEGER NULL, 
                EmployeeIdentifier GUID NULL, 
                EmployeeCode NVARCHAR(48) NULL, 
                EmployeeName NVARCHAR(2048) NULL, 
                EmployeeInternalCode NVARCHAR(48) NULL, 

                IsSynced BOOL NULL, 
                UpdatedAt DATETIME NULL, 
                CreatedById INTEGER NULL, 
                CreatedByName NVARCHAR(2048) NULL, 
                CompanyId INTEGER NULL, 
                CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            @"SELECT ServerId, Identifier, 
                Code, OK, EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeName, EmployeeInternalCode,            
                IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = @"
            INSERT INTO EmployeeAttachments (Id, ServerId, Identifier, Code, OK, EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeName, EmployeeInternalCode, 
                IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) 
            VALUES (NULL, @ServerId, @Identifier, @Code, @OK, @EmployeeId, @EmployeeIdentifier, @EmployeeCode, @EmployeeName, @EmployeeInternalCode, 
                @IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";
        #endregion

        #region Helper methods
        private static EmployeeAttachmentViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            EmployeeAttachmentViewModel dbEntry = new EmployeeAttachmentViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
            dbEntry.OK = SQLiteHelper.GetBoolean(query, ref counter);

            dbEntry.Employee = SQLiteHelper.GetEmployee(query, ref counter);

            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
            return dbEntry;
        }

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, EmployeeAttachmentViewModel EmployeeAttachment)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", EmployeeAttachment.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", EmployeeAttachment.Identifier);
            insertCommand.Parameters.AddWithValue("@Code", ((object)EmployeeAttachment.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@OK", ((object)EmployeeAttachment.OK) ?? DBNull.Value);

            
            insertCommand.Parameters.AddWithValue("@EmployeeId", ((object)EmployeeAttachment.Employee?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmployeeIdentifier", ((object)EmployeeAttachment.Employee?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmployeeCode", ((object)EmployeeAttachment.Employee?.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmployeeName", ((object)EmployeeAttachment.Employee?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmployeeInternalCode", ((object)EmployeeAttachment.Employee?.EmployeeCode) ?? DBNull.Value);

            insertCommand.Parameters.AddWithValue("@IsSynced", EmployeeAttachment.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)EmployeeAttachment.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read

        public EmployeeAttachmentListResponse GetEmployeeAttachmentsByEmployee(int companyId, Guid employeeIdentifier)
        {
            EmployeeAttachmentListResponse response = new EmployeeAttachmentListResponse();
            List<EmployeeAttachmentViewModel> EmployeeAttachments = new List<EmployeeAttachmentViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        @"FROM EmployeeAttachments att 
                            WHERE (@EmployeeIdentifier IS NULL OR @EmployeeIdentifier = '' OR att.EmployeeIdentifier LIKE @EmployeeIdentifier) 
                            AND att.CompanyId = @CompanyId 
                            ORDER BY att.Id ASC ", db);

                    selectCommand.Parameters.AddWithValue("@EmployeeIdentifier", ((object)employeeIdentifier) ?? DBNull.Value);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        EmployeeAttachmentViewModel dbEntry = Read(query);
                        EmployeeAttachments.Add(dbEntry);

                    }
                    response.EmployeeAttachments = EmployeeAttachments;

                    selectCommand = new SqliteCommand(
                        @"SELECT Count(*) 
                            FROM EmployeeAttachments att
                            WHERE(@EmployeeIdentifier IS NULL OR @EmployeeIdentifier = '' OR att.EmployeeIdentifier LIKE @EmployeeIdentifier)
                            AND att.CompanyId = @CompanyId
                            ORDER BY att.IsSynced, att.Id DESC", db);

                    selectCommand.Parameters.AddWithValue("@EmployeeIdentifier", ((object)employeeIdentifier) ?? DBNull.Value);
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
                    response.EmployeeAttachments = new List<EmployeeAttachmentViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.EmployeeAttachments = EmployeeAttachments;
            return response;
        }

        #endregion

        #region Sync

        public void Sync(IEmployeeAttachmentService EmployeeAttachmentService, Action<int, int> callback = null)
        {
            try
            {
                SyncEmployeeAttachmentRequest request = new SyncEmployeeAttachmentRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                EmployeeAttachmentListResponse response = EmployeeAttachmentService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.EmployeeAttachments?.Count ?? 0;
                    List<EmployeeAttachmentViewModel> employeesFromDB = response.EmployeeAttachments;

                    using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM EmployeeAttachments WHERE Identifier = @Identifier";

                            SqliteCommand insertCommand = db.CreateCommand();
                            insertCommand.CommandText = SqlCommandInsertPart;

                            foreach (var employee in employeesFromDB)
                            {
                                deleteCommand.Parameters.AddWithValue("@Identifier", employee.Identifier);
                                deleteCommand.ExecuteNonQuery();
                                deleteCommand.Parameters.Clear();

                                if (employee.IsActive)
                                {
                                    employee.IsSynced = true;

                                    insertCommand = AddCreateParameters(insertCommand, employee);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from EmployeeAttachments WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from EmployeeAttachments WHERE CompanyId = @CompanyId", db);
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

        public EmployeeResponse Create(EmployeeAttachmentViewModel attachment)
        {
            EmployeeResponse response = new EmployeeResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, attachment);
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

        public EmployeeAttachmentResponse Delete(Guid identifier)
        {
            EmployeeAttachmentResponse response = new EmployeeAttachmentResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM EmployeeAttachments WHERE Identifier = @Identifier";
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

        public EmployeeAttachmentResponse DeleteAll()
        {
            EmployeeAttachmentResponse response = new EmployeeAttachmentResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM EmployeeAttachments ";
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
