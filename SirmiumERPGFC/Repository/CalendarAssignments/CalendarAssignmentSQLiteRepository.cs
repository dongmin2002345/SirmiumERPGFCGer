using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.CalendarAssignments;
using ServiceInterfaces.Messages.CalendarAssignments;
using ServiceInterfaces.ViewModels.CalendarAssignments;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;

namespace SirmiumERPGFC.Repository.CalendarAssignments
{
    public class CalendarAssignmentSQLiteRepository
    {
        #region SQL
        public static string CalendarAssignmentTableCreatePart =
            "CREATE TABLE IF NOT EXISTS CalendarAssignments " +
            "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
            "ServerId INTEGER NULL, " +
            "Identifier GUID, " +
            "Name NVARCHAR(2048) NULL, " +
            "Description NVARCHAR(2048) NULL, " +
            "Date DATETIME NOT NULL, " +

            "IsSynced BOOL NULL, " +
            "UpdatedAt DATETIME NULL, " +
            "AssignedToId INTEGER NULL, " +
            "AssignedToName NVARCHAR(2048) NULL, " +
            "CreatedById INTEGER NULL, " +
            "CreatedByName NVARCHAR(2048) NULL, " +
            "CompanyId INTEGER NULL, " +
            "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, " +
            "Name, Description, Date, " +
            "IsSynced, UpdatedAt, AssignedToId, AssignedToName, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO CalendarAssignments " +
            "(Id, ServerId, Identifier, " +
            "Name, Description, Date, " +
            "IsSynced, UpdatedAt, AssignedToId, AssignedToName, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, " +
            "@Name, @Description, @Date, " +
            "@IsSynced, @UpdatedAt, @AssignedToId, @AssignedToName, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods
        private static CalendarAssignmentViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            CalendarAssignmentViewModel dbEntry = new CalendarAssignmentViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Description = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Date = SQLiteHelper.GetDateTime(query, ref counter);

            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.AssignedTo = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
            return dbEntry;
        }

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, CalendarAssignmentViewModel CalendarAssignment)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", CalendarAssignment.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", CalendarAssignment.Identifier);
            insertCommand.Parameters.AddWithValue("@Name", ((object)CalendarAssignment.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Description", ((object)CalendarAssignment.Description) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Date", ((object)CalendarAssignment.Date) ?? DBNull.Value);

            insertCommand.Parameters.AddWithValue("@IsSynced", CalendarAssignment.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)CalendarAssignment.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@AssignedToId", ((object)CalendarAssignment?.AssignedTo?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@AssignedToName", (object)(CalendarAssignment?.AssignedTo?.FirstName + " " + CalendarAssignment?.AssignedTo?.LastName) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read

        public CalendarAssignmentListResponse GetCalendarAssignmentsByDate(int companyId, DateTime? filterDate)
        {
            CalendarAssignmentListResponse response = new CalendarAssignmentListResponse();
            List<CalendarAssignmentViewModel> CalendarAssignments = new List<CalendarAssignmentViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM CalendarAssignments assignment " +
                        "WHERE (@Date IS NULL OR @Date = '' OR DATE(Date) = DATE(@Date)) " +
                        "AND assignment.CompanyId = @CompanyId " +
                        "ORDER BY assignment.IsSynced, assignment.Id DESC ", db);

                    selectCommand.Parameters.AddWithValue("@Date", ((object)filterDate) ?? DBNull.Value);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        CalendarAssignmentViewModel dbEntry = Read(query);
                        CalendarAssignments.Add(dbEntry);
                    }
                    response.CalendarAssignments = CalendarAssignments;

                    selectCommand = new SqliteCommand(
                        "SELECT Count(*) " +
                        "FROM CalendarAssignments assignment " +
                        "WHERE (@Date IS NULL OR @Date = '' OR DATE(Date) = DATE(@Date)) " +
                        "AND assignment.CompanyId = @CompanyId " +
                        "ORDER BY assignment.IsSynced, assignment.Id DESC;", db);

                    selectCommand.Parameters.AddWithValue("@Date", ((object)filterDate) ?? DBNull.Value);
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
                    response.CalendarAssignments = new List<CalendarAssignmentViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.CalendarAssignments = CalendarAssignments;
            return response;
        }

        public List<DateTime> GetAssignedDates(int companyId, DateTime dateFrom, DateTime dateTo)
        {
            List<DateTime> datesWithEntries = new List<DateTime>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        "SELECT DISTINCT Date " +
                        "FROM CalendarAssignments assignment " +
                        "WHERE DATE(@DateFrom) <= DATE(Date) " +
                        "AND DATE(@DateTo) >= DATE(Date) " +
                        "AND assignment.CompanyId = @CompanyId " +
                        "ORDER BY Date ASC ", db);

                    selectCommand.Parameters.AddWithValue("@DateFrom", ((object)dateFrom) ?? DBNull.Value);
                    selectCommand.Parameters.AddWithValue("@DateTo", ((object)dateTo) ?? DBNull.Value);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        DateTime dbEntry = query.GetDateTime(0);
                        datesWithEntries.Add(dbEntry);
                    }
                }
                catch (SqliteException error)
                {
                    return new List<DateTime>();
                }
                db.Close();
            }
            return datesWithEntries;
        }

        #endregion

        #region Sync

        public void Sync(ICalendarAssignmentService CalendarAssignmentService, Action<int, int> callback = null)
        {
            try
            {
                SyncCalendarAssignmentRequest request = new SyncCalendarAssignmentRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                CalendarAssignmentListResponse response = CalendarAssignmentService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.CalendarAssignments?.Count ?? 0;
                    List<CalendarAssignmentViewModel> employeesFromDB = response.CalendarAssignments;

                    using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM CalendarAssignments WHERE Identifier = @Identifier";

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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from CalendarAssignments WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from CalendarAssignments WHERE CompanyId = @CompanyId", db);
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

        public CalendarAssignmentResponse Create(CalendarAssignmentViewModel CalendarAssignment)
        {
            CalendarAssignmentResponse response = new CalendarAssignmentResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, CalendarAssignment);
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

        public CalendarAssignmentResponse Delete(Guid identifier)
        {
            CalendarAssignmentResponse response = new CalendarAssignmentResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM CalendarAssignments WHERE Identifier = @Identifier";
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

        public CalendarAssignmentResponse DeleteAll()
        {
            CalendarAssignmentResponse response = new CalendarAssignmentResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM CalendarAssignments";
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
