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
    public class EmployeeByBusinessPartnerSQLiteRepository
    {
        public static string EmployeeByBusinessPartnerTableCreatePart =
               "CREATE TABLE IF NOT EXISTS EmployeeByBusinessPartners " +
               "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
               "ServerId INTEGER NULL, " +
               "Identifier GUID, " +
               "Code NVARCHAR(48) NULL, " +
               "StartDate DATETIME NULL, " +
               "EndDate DATETIME NULL, " +
               "EmployeeId INTEGER NULL, " +
               "EmployeeIdentifier GUID NULL, " +
               "EmployeeCode INTEGER NULL, " +
               "EmployeeName NVARCHAR(2048) NULL, " +
               "BusinessPartnerId INTEGER NULL, " +
               "BusinessPartnerIdentifier GUID NULL, " +
               "BusinessPartnerCode INTEGER NULL, " +
               "BusinessPartnerName NVARCHAR(2048) NULL, " +
               "IsSynced BOOL NULL, " +
               "UpdatedAt DATETIME NULL, " +
               "CreatedById INTEGER NULL, " +
               "CreatedByName NVARCHAR(2048) NULL, " +
               "CompanyId INTEGER NULL, " +
               "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
           "SELECT ServerId, Identifier, Code, StartDate, EndDate, " +
           "EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeName,  " +
           "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName,  " +
           "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO EmployeeByBusinessPartners " +
           "(Id, ServerId, Identifier, Code, StartDate, EndDate, " +
           "EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeName,  " +
           "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName,  " +
           "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

           "VALUES (NULL, @ServerId, @Identifier, @Code, @StartDate, @EndDate, " +
           "@EmployeeId, @EmployeeIdentifier, @EmployeeCode, @EmployeeName,  " +
           "@BusinessPartnerId, @BusinessPartnerIdentifier, @BusinessPartnerCode, @BusinessPartnerName,  " +
           "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";


        public EmployeeByBusinessPartnerListResponse GetByBusinessPartner(Guid businessPartnerIdentifier)
        {
            EmployeeByBusinessPartnerListResponse response = new EmployeeByBusinessPartnerListResponse();
            List<EmployeeByBusinessPartnerViewModel> employeeByBusinessPartners = new List<EmployeeByBusinessPartnerViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        "SELECT ebp.ServerId, ebp.Identifier, ebp.Code, ebp.StartDate, ebp.EndDate, " +
                        "ebp.EmployeeId, ebp.EmployeeIdentifier, ebp.EmployeeCode, ebp.EmployeeName, e.SurName, e.Passport,  " +
                        "ebp.BusinessPartnerId, ebp.BusinessPartnerIdentifier, ebp.BusinessPartnerCode, ebp.BusinessPartnerName,  " +
                        "ebp.IsSynced, ebp.UpdatedAt, ebp.CreatedById, ebp.CreatedByName, ebp.CompanyId, ebp.CompanyName " +
                        "FROM EmployeeByBusinessPartners ebp, Employees e " +
                        "WHERE ebp.BusinessPartnerIdentifier = @BusinessPartnerIdentifier " + 
                        "AND ebp.EmployeeIdentifier = e.Identifier", db);
                    selectCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", businessPartnerIdentifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        EmployeeByBusinessPartnerViewModel dbEntry = new EmployeeByBusinessPartnerViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.StartDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.EndDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.Employee = SQLiteHelper.GetEmployee(query, ref counter);
                        dbEntry.Employee.SurName = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Employee.Passport = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.BusinessPartner = SQLiteHelper.GetBusinessPartner(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        employeeByBusinessPartners.Add(dbEntry);
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.EmployeeByBusinessPartners = new List<EmployeeByBusinessPartnerViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.EmployeeByBusinessPartners = employeeByBusinessPartners;
            return response;
        }

        public void Sync(IEmployeeByBusinessPartnerService employeeByBusinessPartnerService)
        {
            SyncEmployeeByBusinessPartnerRequest request = new SyncEmployeeByBusinessPartnerRequest();
            request.CompanyId = MainWindow.CurrentCompanyId;
            request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

            EmployeeByBusinessPartnerListResponse response = employeeByBusinessPartnerService.Sync(request);
            if (response.Success)
            {
                List<EmployeeByBusinessPartnerViewModel> employeeByBusinessPartnerFromDB = response.EmployeeByBusinessPartners;
                foreach (var employeeByBusinessPartner in employeeByBusinessPartnerFromDB.OrderBy(x => x.Id))
                {
                    Delete(employeeByBusinessPartner.Employee.Identifier, employeeByBusinessPartner.BusinessPartner.Identifier);
                    if (employeeByBusinessPartner.IsActive)
                    {
                        employeeByBusinessPartner.IsSynced = true;
                        Create(employeeByBusinessPartner);
                    }
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from EmployeeByBusinessPartners WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from EmployeeByBusinessPartners WHERE CompanyId = @CompanyId", db);
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

        public EmployeeByBusinessPartnerResponse Create(EmployeeByBusinessPartnerViewModel employeeByBusinessPartner)
        {
            EmployeeByBusinessPartnerResponse response = new EmployeeByBusinessPartnerResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = SqlCommandInsertPart;

                insertCommand.Parameters.AddWithValue("@ServerId", employeeByBusinessPartner.Id);
                insertCommand.Parameters.AddWithValue("@Identifier", employeeByBusinessPartner.Identifier);
                insertCommand.Parameters.AddWithValue("@Code", ((object)employeeByBusinessPartner.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@StartDate", ((object)employeeByBusinessPartner.StartDate) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@EndDate", ((object)employeeByBusinessPartner.EndDate) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@EmployeeId", ((object)employeeByBusinessPartner.Employee?.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@EmployeeIdentifier", ((object)employeeByBusinessPartner.Employee?.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@EmployeeCode", ((object)employeeByBusinessPartner.Employee?.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@EmployeeName", ((object)employeeByBusinessPartner.Employee?.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerId", ((object)employeeByBusinessPartner.BusinessPartner?.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", ((object)employeeByBusinessPartner.BusinessPartner?.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerCode", ((object)employeeByBusinessPartner.BusinessPartner?.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerName", ((object)employeeByBusinessPartner.BusinessPartner?.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IsSynced", employeeByBusinessPartner.IsSynced);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", employeeByBusinessPartner.UpdatedAt);
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

        public EmployeeByBusinessPartnerResponse UpdateSyncStatus(Guid identifier, int serverId, bool isSynced)
        {
            EmployeeByBusinessPartnerResponse response = new EmployeeByBusinessPartnerResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "UPDATE EmployeeByBusinessPartners SET " +
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

        public EmployeeByBusinessPartnerResponse Delete(Guid employeeIdentifier, Guid businessPartnerIdentifier)
        {
            EmployeeByBusinessPartnerResponse response = new EmployeeByBusinessPartnerResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "DELETE FROM EmployeeByBusinessPartners WHERE EmployeeIdentifier = @EmployeeIdentifier AND BusinessPartnerIdentifier = @BusinessPartnerIdentifier";
                insertCommand.Parameters.AddWithValue("@EmployeeIdentifier", employeeIdentifier);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", businessPartnerIdentifier);
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

        public EmployeeByBusinessPartnerResponse DeleteAll()
        {
            EmployeeByBusinessPartnerResponse response = new EmployeeByBusinessPartnerResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM EmployeeByBusinessPartners";
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
