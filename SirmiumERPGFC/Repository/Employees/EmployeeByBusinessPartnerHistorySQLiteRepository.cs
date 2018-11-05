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
    public class EmployeeByBusinessPartnerHistorySQLiteRepository
    {
        //public static string EmployeeByBusinessPartnerHistoryTableCreatePart =
        //       "CREATE TABLE IF NOT EXISTS EmployeeByBusinessPartnerHistories " +
        //       "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
        //       "ServerId INTEGER NULL, " +
        //       "Identifier GUID, " +
        //       "Code NVARCHAR(48) NULL, " +
        //       "StartDate DATETIME NULL, " +
        //       "EmployeeId INTEGER NULL, " +
        //       "EmployeeIdentifier GUID NULL, " +
        //       "EmployeeCode INTEGER NULL, " +
        //       "EmployeeName NVARCHAR(2048) NULL, " +
        //       "BusinessPartnerId INTEGER NULL, " +
        //       "BusinessPartnerIdentifier GUID NULL, " +
        //       "BusinessPartnerCode INTEGER NULL, " +
        //       "BusinessPartnerName NVARCHAR(2048) NULL, " +
        //       "IsSynced BOOL NULL, " +
        //       "UpdatedAt DATETIME NULL, " +
        //       "CreatedById INTEGER NULL, " +
        //       "CreatedByName NVARCHAR(2048) NULL, " +
        //       "CompanyId INTEGER NULL, " +
        //       "CompanyName NVARCHAR(2048) NULL)";

        //public string SqlCommandSelectPart =
        //   "SELECT ServerId, Identifier, Code, StartDate, " +
        //   "EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeName,  " +
        //   "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName,  " +
        //   "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        //public string SqlCommandInsertPart = "INSERT INTO EmployeeByBusinessPartnerHistories " +
        //   "(Id, ServerId, Identifier, Code, StartDate, " +
        //   "EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeName,  " +
        //   "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName,  " +
        //   "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

        //   "VALUES (NULL, @ServerId, @Identifier, @Code, @StartDate, " +
        //   "@EmployeeId, @EmployeeIdentifier, @EmployeeCode, @EmployeeName,  " +
        //   "@BusinessPartnerId, @BusinessPartnerIdentifier, @BusinessPartnerCode, @BusinessPartnerName,  " +
        //   "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";


        //public EmployeeByBusinessPartnerHistoryListResponse GetByBusinessPartner(Guid businessPartnerIdentifier)
        //{
        //    EmployeeByBusinessPartnerHistoryListResponse response = new EmployeeByBusinessPartnerHistoryListResponse();
        //    List<EmployeeByBusinessPartnerHistoryViewModel> employeeByBusinessPartnerHistories = new List<EmployeeByBusinessPartnerHistoryViewModel>();

        //    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
        //    {
        //        db.Open();
        //        try
        //        {
        //            SqliteCommand selectCommand = new SqliteCommand(
        //                SqlCommandSelectPart +
        //                "FROM EmployeeByBusinessPartnerHistories " +
        //                "WHERE BusinessPartnerIdentifier = @BusinessPartnerIdentifier;", db);
        //            selectCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", businessPartnerIdentifier);

        //            SqliteDataReader query = selectCommand.ExecuteReader();

        //            if (query.Read())
        //            {
        //                int counter = 0;
        //                EmployeeByBusinessPartnerHistoryViewModel dbEntry = new EmployeeByBusinessPartnerHistoryViewModel();
        //                dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
        //                dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
        //                dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
        //                dbEntry.StartDate = SQLiteHelper.GetDateTime(query, ref counter);
        //                dbEntry.Employee = SQLiteHelper.GetEmployee(query, ref counter);
        //                dbEntry.BusinessPartner = SQLiteHelper.GetBusinessPartner(query, ref counter);
        //                dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
        //                dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
        //                dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
        //                dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
        //                employeeByBusinessPartnerHistories.Add(dbEntry);
        //            }
        //        }
        //        catch (SqliteException error)
        //        {
        //            MainWindow.ErrorMessage = error.Message;
        //            response.Success = false;
        //            response.Message = error.Message;
        //            response.EmployeeByBusinessPartnerHistories = new List<EmployeeByBusinessPartnerHistoryViewModel>();
        //            return response;
        //        }
        //        db.Close();
        //    }
        //    response.Success = true;
        //    response.EmployeeByBusinessPartnerHistories = employeeByBusinessPartnerHistories;
        //    return response;
        //}

        //public void Sync(IEmployeeByBusinessPartnerHistoryService employeeByBusinessPartnerHistoryService)
        //{
        //    SyncEmployeeByBusinessPartnerHistoryRequest request = new SyncEmployeeByBusinessPartnerHistoryRequest();
        //    request.CompanyId = MainWindow.CurrentCompanyId;
        //    request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

        //    EmployeeByBusinessPartnerHistoryListResponse response = employeeByBusinessPartnerHistoryService.Sync(request);
        //    if (response.Success)
        //    {
        //        List<EmployeeByBusinessPartnerHistoryViewModel> employeeByBusinessPartnerHistoryFromDB = response.EmployeeByBusinessPartnerHistories;
        //        foreach (var employeeByBusinessPartnerHistory in employeeByBusinessPartnerHistoryFromDB.OrderBy(x => x.Id))
        //        {
        //            Delete(employeeByBusinessPartnerHistory.Employee.Identifier, employeeByBusinessPartnerHistory.BusinessPartner.Identifier);
        //            employeeByBusinessPartnerHistory.IsSynced = true;
        //            Create(employeeByBusinessPartnerHistory);
        //        }
        //    }
        //}

        //public DateTime? GetLastUpdatedAt(int companyId)
        //{
        //    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
        //    {
        //        db.Open();
        //        try
        //        {
        //            SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from EmployeeByBusinessPartnerHistories WHERE CompanyId = @CompanyId", db);
        //            selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
        //            SqliteDataReader query = selectCommand.ExecuteReader();
        //            int count = query.Read() ? query.GetInt32(0) : 0;

        //            if (count == 0)
        //                return null;
        //            else
        //            {
        //                selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from EmployeeByBusinessPartnerHistories WHERE CompanyId = @CompanyId", db);
        //                selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
        //                query = selectCommand.ExecuteReader();
        //                if (query.Read())
        //                {
        //                    return query.GetDateTime(0);
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MainWindow.ErrorMessage = ex.Message;
        //        }
        //        db.Close();
        //    }
        //    return null;
        //}

        //public EmployeeByBusinessPartnerHistoryResponse Create(EmployeeByBusinessPartnerHistoryViewModel employeeByBusinessPartnerHistory)
        //{
        //    EmployeeByBusinessPartnerHistoryResponse response = new EmployeeByBusinessPartnerHistoryResponse();

        //    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
        //    {
        //        db.Open();

        //        SqliteCommand insertCommand = new SqliteCommand();
        //        insertCommand.Connection = db;

        //        //Use parameterized query to prevent SQL injection attacks
        //        insertCommand.CommandText = SqlCommandInsertPart;

        //        insertCommand.Parameters.AddWithValue("@ServerId", employeeByBusinessPartnerHistory.Id);
        //        insertCommand.Parameters.AddWithValue("@Identifier", employeeByBusinessPartnerHistory.Identifier);
        //        insertCommand.Parameters.AddWithValue("@Code", ((object)employeeByBusinessPartnerHistory.Code) ?? DBNull.Value);
        //        insertCommand.Parameters.AddWithValue("@StartDate", ((object)employeeByBusinessPartnerHistory.StartDate) ?? DBNull.Value);
        //        insertCommand.Parameters.AddWithValue("@EmployeeId", ((object)employeeByBusinessPartnerHistory.Employee?.Id) ?? DBNull.Value);
        //        insertCommand.Parameters.AddWithValue("@EmployeeIdentifier", ((object)employeeByBusinessPartnerHistory.Employee?.Identifier) ?? DBNull.Value);
        //        insertCommand.Parameters.AddWithValue("@EmployeeCode", ((object)employeeByBusinessPartnerHistory.Employee?.Code) ?? DBNull.Value);
        //        insertCommand.Parameters.AddWithValue("@EmployeeName", ((object)employeeByBusinessPartnerHistory.Employee?.Name) ?? DBNull.Value);
        //        insertCommand.Parameters.AddWithValue("@BusinessPartnerId", ((object)employeeByBusinessPartnerHistory.BusinessPartner?.Id) ?? DBNull.Value);
        //        insertCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", ((object)employeeByBusinessPartnerHistory.BusinessPartner?.Identifier) ?? DBNull.Value);
        //        insertCommand.Parameters.AddWithValue("@BusinessPartnerCode", ((object)employeeByBusinessPartnerHistory.BusinessPartner?.Code) ?? DBNull.Value);
        //        insertCommand.Parameters.AddWithValue("@BusinessPartnerName", ((object)employeeByBusinessPartnerHistory.BusinessPartner?.Name) ?? DBNull.Value);
        //        insertCommand.Parameters.AddWithValue("@IsSynced", employeeByBusinessPartnerHistory.IsSynced);
        //        insertCommand.Parameters.AddWithValue("@UpdatedAt", employeeByBusinessPartnerHistory.UpdatedAt);
        //        insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
        //        insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
        //        insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
        //        insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

        //        try
        //        {
        //            insertCommand.ExecuteReader();
        //        }
        //        catch (SqliteException error)
        //        {
        //            MainWindow.ErrorMessage = error.Message;
        //            response.Success = false;
        //            response.Message = error.Message;
        //            return response;
        //        }
        //        db.Close();

        //        response.Success = true;
        //        return response;
        //    }
        //}

        //public EmployeeByBusinessPartnerHistoryResponse UpdateSyncStatus(Guid identifier, int serverId, bool isSynced)
        //{
        //    EmployeeByBusinessPartnerHistoryResponse response = new EmployeeByBusinessPartnerHistoryResponse();

        //    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
        //    {
        //        db.Open();

        //        SqliteCommand insertCommand = new SqliteCommand();
        //        insertCommand.Connection = db;

        //        insertCommand.CommandText = "UPDATE EmployeeByBusinessPartnerHistories SET " +
        //            "IsSynced = @IsSynced, " +
        //            "ServerId = @ServerId " +
        //            "WHERE Identifier = @Identifier ";

        //        insertCommand.Parameters.AddWithValue("@IsSynced", isSynced);
        //        insertCommand.Parameters.AddWithValue("@ServerId", serverId);
        //        insertCommand.Parameters.AddWithValue("@Identifier", identifier);

        //        try
        //        {
        //            insertCommand.ExecuteReader();
        //        }
        //        catch (SqliteException error)
        //        {
        //            MainWindow.ErrorMessage = error.Message;
        //            response.Success = false;
        //            response.Message = error.Message;
        //            return response;
        //        }
        //        db.Close();

        //        response.Success = true;
        //        return response;
        //    }
        //}

        //public EmployeeByBusinessPartnerHistoryResponse Delete(Guid employeeIdentifier, Guid businessPartnerIdentifier)
        //{
        //    EmployeeByBusinessPartnerHistoryResponse response = new EmployeeByBusinessPartnerHistoryResponse();

        //    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
        //    {
        //        db.Open();

        //        SqliteCommand insertCommand = new SqliteCommand();
        //        insertCommand.Connection = db;

        //        //Use parameterized query to prevent SQL injection attacks
        //        insertCommand.CommandText =
        //            "DELETE FROM EmployeeByBusinessPartnerHistories WHERE EmployeeIdentifier = @EmployeeIdentifier AND BusinessPartnerIdentifier = @BusinessPartnerIdentifier";
        //        insertCommand.Parameters.AddWithValue("@EmployeeIdentifier", employeeIdentifier);
        //        insertCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", businessPartnerIdentifier);
        //        try
        //        {
        //            insertCommand.ExecuteReader();
        //        }
        //        catch (SqliteException error)
        //        {
        //            MainWindow.ErrorMessage = error.Message;
        //            response.Success = false;
        //            response.Message = error.Message;
        //            return response;
        //        }
        //        db.Close();

        //        response.Success = true;
        //        return response;
        //    }
        //}

        //public EmployeeByBusinessPartnerHistoryResponse DeleteAll()
        //{
        //    EmployeeByBusinessPartnerHistoryResponse response = new EmployeeByBusinessPartnerHistoryResponse();

        //    try
        //    {
        //        using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
        //        {
        //            db.Open();
        //            db.EnableExtensions(true);

        //            SqliteCommand insertCommand = new SqliteCommand();
        //            insertCommand.Connection = db;

        //            //Use parameterized query to prevent SQL injection attacks
        //            insertCommand.CommandText = "DELETE FROM EmployeeByBusinessPartnerHistories";
        //            try
        //            {
        //                insertCommand.ExecuteReader();
        //            }
        //            catch (SqliteException error)
        //            {
        //                response.Success = false;
        //                response.Message = error.Message;

        //                MainWindow.ErrorMessage = error.Message;
        //                return response;
        //            }
        //            db.Close();
        //        }
        //    }
        //    catch (SqliteException error)
        //    {
        //        response.Success = false;
        //        response.Message = error.Message;
        //        return response;
        //    }

        //    response.Success = true;
        //    return response;
        //}
    }
}
