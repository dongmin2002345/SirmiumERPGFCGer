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
    public class EmployeeDocumentSQLiteRepository
    {
        #region SQL

        public static string EmployeeDocumentTableCreatePart =
                  "CREATE TABLE IF NOT EXISTS EmployeeDocuments " +
                  "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                  "ServerId INTEGER NULL, " +
                  "Identifier GUID, " +
                  "EmployeeId INTEGER NULL, " +
                  "EmployeeIdentifier GUID NULL, " +
                  "EmployeeCode NVARCHAR(48) NULL, " +
                  "EmployeeName NVARCHAR(2048) NULL, " +
                  "EmployeeInternalCode NVARCHAR(48) NULL, " +
                  "Name NVARCHAR(2048), " +
                  "CreateDate DATETIME NULL, " +
                  "Path NVARCHAR(2048) NULL, " +
                  "ItemStatus INTEGER NOT NULL, " +
                  "IsSynced BOOL NULL, " +
                  "UpdatedAt DATETIME NULL, " +
                  "CreatedById INTEGER NULL, " +
                  "CreatedByName NVARCHAR(2048) NULL, " +
                  "CompanyId INTEGER NULL, " +
                  "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, EmployeeId, EmployeeIdentifier, " +
            "EmployeeCode, EmployeeName, EmployeeInternalCode, Name, CreateDate, Path, ItemStatus, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO EmployeeDocuments " +
            "(Id, ServerId, Identifier, EmployeeId, EmployeeIdentifier, " +
            "EmployeeCode, EmployeeName, EmployeeInternalCode, Name, CreateDate, Path, ItemStatus, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @EmployeeId, @EmployeeIdentifier, " +
            "@EmployeeCode, @EmployeeName, @EmployeeInternalCode, @Name, @CreateDate, @Path, @ItemStatus, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #region Helper methods
        private static EmployeeDocumentViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            EmployeeDocumentViewModel dbEntry = new EmployeeDocumentViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.Employee = SQLiteHelper.GetEmployee(query, ref counter);
            dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
            dbEntry.CreateDate = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.Path = SQLiteHelper.GetString(query, ref counter);
            dbEntry.ItemStatus = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
            return dbEntry;
        }

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, EmployeeDocumentViewModel EmployeeDocument)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", EmployeeDocument.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", EmployeeDocument.Identifier);
            insertCommand.Parameters.AddWithValue("@EmployeeId", ((object)EmployeeDocument.Employee.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmployeeIdentifier", ((object)EmployeeDocument.Employee.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmployeeCode", ((object)EmployeeDocument.Employee.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmployeeName", ((object)EmployeeDocument.Employee.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmployeeInternalCode", ((object)EmployeeDocument.Employee.EmployeeCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Name", EmployeeDocument.Name);
            insertCommand.Parameters.AddWithValue("@CreateDate", ((object)EmployeeDocument.CreateDate) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Path", ((object)EmployeeDocument.Path) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ItemStatus", ((object)EmployeeDocument.ItemStatus) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsSynced", EmployeeDocument.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)EmployeeDocument.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        public EmployeeDocumentListResponse GetEmployeeDocumentsByEmployee(int companyId, Guid EmployeeIdentifier)
        {
            EmployeeDocumentListResponse response = new EmployeeDocumentListResponse();
            List<EmployeeDocumentViewModel> EmployeeDocuments = new List<EmployeeDocumentViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM EmployeeDocuments " +
                        "WHERE EmployeeIdentifier = @EmployeeIdentifier " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC;", db);
                    
                    selectCommand.Parameters.AddWithValue("@EmployeeIdentifier", EmployeeIdentifier);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                        EmployeeDocuments.Add(Read(query));
                    

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.EmployeeDocuments = new List<EmployeeDocumentViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.EmployeeDocuments = EmployeeDocuments;
            return response;
        }

        public EmployeeDocumentResponse GetEmployeeDocument(Guid identifier)
        {
            EmployeeDocumentResponse response = new EmployeeDocumentResponse();
            EmployeeDocumentViewModel EmployeeDocument = new EmployeeDocumentViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM EmployeeDocuments " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                        EmployeeDocument = Read(query);
                    
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.EmployeeDocument = new EmployeeDocumentViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.EmployeeDocument = EmployeeDocument;
            return response;
        }

        #endregion

        #region Sync

        public void Sync(IEmployeeDocumentService EmployeeDocumentService, Action<int, int> callback = null)
        {
            try
            {
                SyncEmployeeDocumentRequest request = new SyncEmployeeDocumentRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                EmployeeDocumentListResponse response = EmployeeDocumentService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.EmployeeDocuments?.Count ?? 0;
                    List<EmployeeDocumentViewModel> employeeDocumentsFromDB = response.EmployeeDocuments;

                    using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM EmployeeDocuments WHERE Identifier = @Identifier";

                            SqliteCommand insertCommand = db.CreateCommand();
                            insertCommand.CommandText = SqlCommandInsertPart;

                            foreach (var employeeDocument in employeeDocumentsFromDB)
                            {
                                deleteCommand.Parameters.AddWithValue("@Identifier", employeeDocument.Identifier);
                                deleteCommand.ExecuteNonQuery();
                                deleteCommand.Parameters.Clear();

                                if (employeeDocument.IsActive)
                                {
                                    employeeDocument.IsSynced = true;

                                    insertCommand = AddCreateParameters(insertCommand, employeeDocument);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from EmployeeDocuments WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from EmployeeDocuments WHERE CompanyId = @CompanyId", db);
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

        public EmployeeDocumentResponse Create(EmployeeDocumentViewModel EmployeeDocument)
        {
            EmployeeDocumentResponse response = new EmployeeDocumentResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, EmployeeDocument);
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

        public EmployeeDocumentResponse Delete(Guid identifier)
        {
            EmployeeDocumentResponse response = new EmployeeDocumentResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM EmployeeDocuments WHERE Identifier = @Identifier";
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

        public EmployeeDocumentResponse SetStatusDeleted(Guid identifier)
        {
            EmployeeDocumentResponse response = new EmployeeDocumentResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "UPDATE EmployeeDocuments SET ItemStatus = @ItemStatus WHERE Identifier = @Identifier";
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
