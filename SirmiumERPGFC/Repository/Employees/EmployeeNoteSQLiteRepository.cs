using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;

namespace SirmiumERPGFC.Repository.Employees
{
    public class EmployeeNoteSQLiteRepository
    {
        #region SQL

        public static string EmployeeNoteTableCreatePart =
                  "CREATE TABLE IF NOT EXISTS EmployeeNotes " +
                  "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                  "ServerId INTEGER NULL, " +
                  "Identifier GUID, " +
                  "EmployeeId INTEGER NULL, " +
                  "EmployeeIdentifier GUID NULL, " +
                  "EmployeeCode NVARCHAR(48) NULL, " +
                  "EmployeeName NVARCHAR(2048) NULL, " +
                  "EmployeeInternalCode NVARCHAR(48) NULL, " +
                  "Note NVARCHAR(2048), " +
                  "NoteDate DATETIME NULL, " +
                  "IsSynced BOOL NULL, " +
                  "UpdatedAt DATETIME NULL, " +
                  "CreatedById INTEGER NULL, " +
                  "CreatedByName NVARCHAR(2048) NULL, " +
                  "CompanyId INTEGER NULL, " +
                  "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, EmployeeId, EmployeeIdentifier, " +
            "EmployeeCode, EmployeeName, EmployeeInternalCode, Note, NoteDate, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO EmployeeNotes " +
            "(Id, ServerId, Identifier, EmployeeId, EmployeeIdentifier, " +
            "EmployeeCode, EmployeeName, EmployeeInternalCode, Note, NoteDate, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @EmployeeId, @EmployeeIdentifier, " +
            "@EmployeeCode, @EmployeeName, @EmployeeInternalCode, @Note, @NoteDate, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods

        private EmployeeNoteViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            EmployeeNoteViewModel dbEntry = new EmployeeNoteViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.Employee = SQLiteHelper.GetEmployee(query, ref counter);
            dbEntry.Note = SQLiteHelper.GetString(query, ref counter);
            dbEntry.NoteDate = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);

            return dbEntry;
        }

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, EmployeeNoteViewModel EmployeeNote)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", EmployeeNote.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", EmployeeNote.Identifier);
            insertCommand.Parameters.AddWithValue("@EmployeeId", ((object)EmployeeNote.Employee.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmployeeIdentifier", ((object)EmployeeNote.Employee.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmployeeCode", ((object)EmployeeNote.Employee.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmployeeName", ((object)EmployeeNote.Employee.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmployeeInternalCode", ((object)EmployeeNote.Employee.EmployeeCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Note", EmployeeNote.Note);
            insertCommand.Parameters.AddWithValue("@NoteDate", ((object)EmployeeNote.NoteDate) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsSynced", EmployeeNote.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)EmployeeNote.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read

        public EmployeeNoteListResponse GetEmployeeNotesByEmployee(int companyId, Guid EmployeeIdentifier)
        {
            EmployeeNoteListResponse response = new EmployeeNoteListResponse();
            List<EmployeeNoteViewModel> EmployeeNotes = new List<EmployeeNoteViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM EmployeeNotes " +
                        "WHERE EmployeeIdentifier = @EmployeeIdentifier " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC;", db);
                    
                    selectCommand.Parameters.AddWithValue("@EmployeeIdentifier", EmployeeIdentifier);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                        EmployeeNotes.Add(Read(query));
                    

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.EmployeeNotes = new List<EmployeeNoteViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.EmployeeNotes = EmployeeNotes;
            return response;
        }

        public EmployeeNoteResponse GetEmployeeNote(Guid identifier)
        {
            EmployeeNoteResponse response = new EmployeeNoteResponse();
            EmployeeNoteViewModel EmployeeNote = new EmployeeNoteViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM EmployeeNotes " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                        EmployeeNote = Read(query);
                    
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.EmployeeNote = new EmployeeNoteViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.EmployeeNote = EmployeeNote;
            return response;
        }

        public EmployeeNoteListResponse GetUnSyncedNotes(int companyId)
        {
            EmployeeNoteListResponse response = new EmployeeNoteListResponse();
            List<EmployeeNoteViewModel> EmployeeNotes = new List<EmployeeNoteViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM  EmployeeNotes " +
                        "WHERE CompanyId = @CompanyId AND IsSynced = 0 " +
                        "ORDER BY Id DESC;", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                        EmployeeNotes.Add(Read(query));
                    

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.EmployeeNotes = new List<EmployeeNoteViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.EmployeeNotes = EmployeeNotes;
            return response;
        }

        #endregion

        #region Sync

        public void Sync(IEmployeeNoteService EmployeeNoteService, Action<int, int> callback = null)
        {
            try
            {
                SyncEmployeeNoteRequest request = new SyncEmployeeNoteRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                EmployeeNoteListResponse response = EmployeeNoteService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.EmployeeNotes?.Count ?? 0;
                    List<EmployeeNoteViewModel> employeeNotesFromDB = response.EmployeeNotes;

                    using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM EmployeeNotes WHERE Identifier = @Identifier";

                            SqliteCommand insertCommand = db.CreateCommand();
                            insertCommand.CommandText = SqlCommandInsertPart;

                            foreach (var employeeNote in employeeNotesFromDB)
                            {
                                deleteCommand.Parameters.AddWithValue("@Identifier", employeeNote.Identifier);
                                deleteCommand.ExecuteNonQuery();
                                deleteCommand.Parameters.Clear();

                                if (employeeNote.IsActive)
                                {
                                    employeeNote.IsSynced = true;

                                    insertCommand = AddCreateParameters(insertCommand, employeeNote);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from EmployeeNotes WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from EmployeeNotes WHERE CompanyId = @CompanyId", db);
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

        public EmployeeNoteResponse Create(EmployeeNoteViewModel EmployeeNote)
        {
            EmployeeNoteResponse response = new EmployeeNoteResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, EmployeeNote);
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

        public EmployeeNoteResponse Delete(Guid identifier)
        {
            EmployeeNoteResponse response = new EmployeeNoteResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM EmployeeNotes WHERE Identifier = @Identifier";
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

        public EmployeeNoteResponse DeleteAll()
        {
            EmployeeNoteResponse response = new EmployeeNoteResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM EmployeeNotes";
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
