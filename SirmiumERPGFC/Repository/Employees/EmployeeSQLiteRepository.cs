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
    public class EmployeeSQLiteRepository
    {
        public static string EmployeeTableCreatePart =
            "CREATE TABLE IF NOT EXISTS Employees " +
            "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
            "ServerId INTEGER NULL, " +
            "Identifier GUID, " +
            "Code NVARCHAR(2048) NULL, " +
            "EmployeeCode NVARCHAR(2048) NULL, " +
            "Name NVARCHAR(2048) NULL, " +
            "SurName NVARCHAR(2048) NULL, " +
            "DateOfBirth DATETIME NULL, " +
            "Address NVARCHAR(2048) NULL, " +
            "Passport NVARCHAR(2048) NULL, " +
            "Interest NVARCHAR(2048) NULL, " +
            "License NVARCHAR(2048) NULL, " +
            "EmbassyDate DATETIME NULL, " +
            "VisaFrom DATETIME NULL, " +
            "VisaTo DATETIME NULL, " +
            "WorkPermitFrom DATETIME NULL, " +
            "WorkPermitTo DATETIME NULL, " +
            "IsSynced BOOL NULL, " +
            "UpdatedAt DATETIME NULL, " +
            "CreatedById INTEGER NULL, " +
            "CreatedByName NVARCHAR(2048) NULL, " +
            "CompanyId INTEGER NULL, " +
            "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, Code, EmployeeCode, " +
            "Name, SurName, DateOfBirth, Address, " +
            "Passport, Interest, License, EmbassyDate, " +
            "VisaFrom, VisaTo, WorkPermitFrom, WorkPermitTo, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO Employees " +
            "(Id, ServerId, Identifier, Code, EmployeeCode, " +
            "Name, SurName, DateOfBirth, Address, " +
            "Passport, Interest, License, EmbassyDate, " +
            "VisaFrom, VisaTo, WorkPermitFrom, WorkPermitTo, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @Code, @EmployeeCode, " +
            "@Name, @SurName, @DateOfBirth, @Address, " +
            "@Passport, @Interest, @License, @EmbassyDate, " +
            "@VisaFrom, @VisaTo, @WorkPermitFrom, @WorkPermitTo, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        public EmployeeListResponse GetEmployeesByPage(int companyId, EmployeeViewModel EmployeeSearchObject, int currentPage = 1, int itemsPerPage = 50)
        {
            EmployeeListResponse response = new EmployeeListResponse();
            List<EmployeeViewModel> Employees = new List<EmployeeViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Employees " +
                        "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND (@SurName IS NULL OR @SurName = '' OR SurName LIKE @SurName) " +
                        "AND (@Passport IS NULL OR @Passport = '' OR Passport LIKE @Passport) " +
                        "AND (@Interest IS NULL OR @Interest = '' OR Interest LIKE @Interest) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage OFFSET @Offset;", db);
                    selectCommand.Parameters.AddWithValue("@Name", ((object)EmployeeSearchObject.SearchBy_Name) != null ? "%" + EmployeeSearchObject.SearchBy_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@SurName", ((object)EmployeeSearchObject.SearchBy_SurName) != null ? "%" + EmployeeSearchObject.SearchBy_SurName + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Passport", ((object)EmployeeSearchObject.SearchBy_Passport) != null ? "%" + EmployeeSearchObject.SearchBy_Passport + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Interest", ((object)EmployeeSearchObject.SearchBy_Interest) != null ? "%" + EmployeeSearchObject.SearchBy_Interest + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
                    selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        EmployeeViewModel dbEntry = new EmployeeViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.EmployeeCode = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.SurName = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.DateOfBirth = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.Address = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Passport = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Interest = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.License = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.EmbassyDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.VisaFrom = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.VisaTo = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.WorkPermitFrom = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.WorkPermitTo = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        Employees.Add(dbEntry);
                    }


                    selectCommand = new SqliteCommand(
                        "SELECT Count(*) " +
                        "FROM Employees " +
                       "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND (@SurName IS NULL OR @SurName = '' OR SurName LIKE @SurName) " +
                        "AND (@Passport IS NULL OR @Passport = '' OR Passport LIKE @Passport) " +
                        "AND (@Interest IS NULL OR @Interest = '' OR Interest LIKE @Interest) " +
                        "AND CompanyId = @CompanyId;", db);
                    selectCommand.Parameters.AddWithValue("@Name", ((object)EmployeeSearchObject.SearchBy_Name) != null ? "%" + EmployeeSearchObject.SearchBy_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@SurName", ((object)EmployeeSearchObject.SearchBy_SurName) != null ? "%" + EmployeeSearchObject.SearchBy_SurName + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Passport", ((object)EmployeeSearchObject.SearchBy_Passport) != null ? "%" + EmployeeSearchObject.SearchBy_Passport + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Interest", ((object)EmployeeSearchObject.SearchBy_Interest) != null ? "%" + EmployeeSearchObject.SearchBy_Interest + "%" : "");
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
                    response.Employees = new List<EmployeeViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Employees = Employees;
            return response;
        }

        public EmployeeResponse GetEmployee(Guid identifier)
        {
            EmployeeResponse response = new EmployeeResponse();
            EmployeeViewModel Employee = new EmployeeViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Employees " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        int counter = 0;
                        EmployeeViewModel dbEntry = new EmployeeViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.EmployeeCode = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.SurName = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.DateOfBirth = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.Address = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Passport = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Interest = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.License = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.EmbassyDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.VisaFrom = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.VisaTo = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.WorkPermitFrom = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.WorkPermitTo = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        Employee = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.Employee = new EmployeeViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Employee = Employee;
            return response;
        }

        public void Sync(IEmployeeService EmployeeService)
        {
            SyncEmployeeRequest request = new SyncEmployeeRequest();
            request.CompanyId = MainWindow.CurrentCompanyId;
            request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

            EmployeeListResponse response = EmployeeService.Sync(request);
            if (response.Success)
            {
                List<EmployeeViewModel> EmployeesFromDB = response.Employees;
                foreach (var Employee in EmployeesFromDB.OrderBy(x => x.Id))
                {
                    Delete(Employee.Identifier);
                    Employee.IsSynced = true;
                    Create(Employee);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from Employees WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from Employees WHERE CompanyId = @CompanyId", db);
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

        public EmployeeResponse Create(EmployeeViewModel Employee)
        {
            EmployeeResponse response = new EmployeeResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = SqlCommandInsertPart;

                insertCommand.Parameters.AddWithValue("@ServerId", Employee.Id);
                insertCommand.Parameters.AddWithValue("@Identifier", Employee.Identifier);
                insertCommand.Parameters.AddWithValue("@Code", ((object)Employee.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@EmployeeCode", ((object)Employee.EmployeeCode) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Name", ((object)Employee.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@SurName", ((object)Employee.SurName) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@DateOfBirth", ((object)Employee.DateOfBirth) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Address", ((object)Employee.Address) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Passport", ((object)Employee.Passport) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Interest", ((object)Employee.Interest) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@License", ((object)Employee.License) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@EmbassyDate", ((object)Employee.EmbassyDate) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@VisaFrom", ((object)Employee.VisaFrom) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@VisaTo", ((object)Employee.VisaTo) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@WorkPermitFrom", ((object)Employee.WorkPermitFrom) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@WorkPermitTo", ((object)Employee.WorkPermitTo) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IsSynced", Employee.IsSynced);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", Employee.UpdatedAt);
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

        public EmployeeResponse UpdateSyncStatus(Guid identifier, int serverId, bool isSynced)
        {
            EmployeeResponse response = new EmployeeResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "UPDATE Employees SET " +
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

        public EmployeeResponse Delete(Guid identifier)
        {
            EmployeeResponse response = new EmployeeResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "DELETE FROM Employees WHERE Identifier = @Identifier";
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

        public EmployeeResponse DeleteAll()
        {
            EmployeeResponse response = new EmployeeResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM Employees";
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
