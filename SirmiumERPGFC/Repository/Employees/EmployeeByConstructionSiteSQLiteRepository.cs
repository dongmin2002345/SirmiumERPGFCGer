﻿using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.ConstructionSites;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.Employees
{
    public class EmployeeByConstructionSiteSQLiteRepository
    {
        public static string EmployeeByConstructionSiteTableCreatePart =
               "CREATE TABLE IF NOT EXISTS EmployeeByConstructionSites " +
               "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
               "ServerId INTEGER NULL, " +
               "Identifier GUID, " +
               "Code NVARCHAR(48) NULL, " +
               "StartDate DATETIME NULL, " +
               "EmployeeId INTEGER NULL, " +
               "EmployeeIdentifier GUID NULL, " +
               "EmployeeCode INTEGER NULL, " +
               "EmployeeName NVARCHAR(2048) NULL, " +
               "ConstructionSiteId INTEGER NULL, " +
               "ConstructionSiteIdentifier GUID NULL, " +
               "ConstructionSiteCode INTEGER NULL, " +
               "ConstructionSiteName NVARCHAR(2048) NULL, " +
               "IsSynced BOOL NULL, " +
               "UpdatedAt DATETIME NULL, " +
               "CreatedById INTEGER NULL, " +
               "CreatedByName NVARCHAR(2048) NULL, " +
               "CompanyId INTEGER NULL, " +
               "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
           "SELECT ServerId, Identifier, Code, StartDate, " +
           "EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeName,  " +
           "ConstructionSiteId, ConstructionSiteIdentifier, ConstructionSiteCode, ConstructionSiteName,  " +
           "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO EmployeeByConstructionSites " +
           "(Id, ServerId, Identifier, Code, StartDate, " +
           "EmployeeId, EmployeeIdentifier, EmployeeCode, EmployeeName,  " +
           "ConstructionSiteId, ConstructionSiteIdentifier, ConstructionSiteCode, ConstructionSiteName,  " +
           "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

           "VALUES (NULL, @ServerId, @Identifier, @Code, @StartDate, " +
           "@EmployeeId, @EmployeeIdentifier, @EmployeeCode, @EmployeeName,  " +
           "@ConstructionSiteId, @ConstructionSiteIdentifier, @ConstructionSiteCode, @ConstructionSiteName,  " +
           "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";


        public EmployeeByConstructionSiteListResponse GetByConstructionSite(Guid constructionSiteIdentifier)
        {
            EmployeeByConstructionSiteListResponse response = new EmployeeByConstructionSiteListResponse();
            List<EmployeeByConstructionSiteViewModel> employeeByConstructionSites = new List<EmployeeByConstructionSiteViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM EmployeeByConstructionSites " +
                        "WHERE ConstructionSiteIdentifier = @ConstructionSiteIdentifier;", db);
                    selectCommand.Parameters.AddWithValue("@ConstructionSiteIdentifier", constructionSiteIdentifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        int counter = 0;
                        EmployeeByConstructionSiteViewModel dbEntry = new EmployeeByConstructionSiteViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.StartDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.Employee = SQLiteHelper.GetEmployee(query, ref counter);
                        dbEntry.ConstructionSite = SQLiteHelper.GetConstructionSite(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        employeeByConstructionSites.Add(dbEntry);
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.EmployeeByConstructionSites = new List<EmployeeByConstructionSiteViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.EmployeeByConstructionSites = employeeByConstructionSites;
            return response;
        }

        public void Sync(IEmployeeByConstructionSiteService employeeByConstructionSiteService)
        {
            SyncEmployeeByConstructionSiteRequest request = new SyncEmployeeByConstructionSiteRequest();
            request.CompanyId = MainWindow.CurrentCompanyId;
            request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

            EmployeeByConstructionSiteListResponse response = employeeByConstructionSiteService.Sync(request);
            if (response.Success)
            {
                List<EmployeeByConstructionSiteViewModel> employeeByConstructionSiteFromDB = response.EmployeeByConstructionSites;
                foreach (var employeeByConstructionSite in employeeByConstructionSiteFromDB.OrderBy(x => x.Id))
                {
                    Delete(employeeByConstructionSite.Employee.Identifier, employeeByConstructionSite.ConstructionSite.Identifier);
                    employeeByConstructionSite.IsSynced = true;
                    Create(employeeByConstructionSite);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from EmployeeByConstructionSites WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from EmployeeByConstructionSites WHERE CompanyId = @CompanyId", db);
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

        public EmployeeByConstructionSiteResponse Create(EmployeeByConstructionSiteViewModel employeeByConstructionSite)
        {
            EmployeeByConstructionSiteResponse response = new EmployeeByConstructionSiteResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = SqlCommandInsertPart;

                insertCommand.Parameters.AddWithValue("@ServerId", employeeByConstructionSite.Id);
                insertCommand.Parameters.AddWithValue("@Identifier", employeeByConstructionSite.Identifier);
                insertCommand.Parameters.AddWithValue("@Code", ((object)employeeByConstructionSite.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@StartDate", ((object)employeeByConstructionSite.StartDate) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@EmployeeId", ((object)employeeByConstructionSite.Employee?.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@EmployeeIdentifier", ((object)employeeByConstructionSite.Employee?.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@EmployeeCode", ((object)employeeByConstructionSite.Employee?.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@EmployeeName", ((object)employeeByConstructionSite.Employee?.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ConstructionSiteId", ((object)employeeByConstructionSite.ConstructionSite?.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ConstructionSiteIdentifier", ((object)employeeByConstructionSite.ConstructionSite?.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ConstructionSiteCode", ((object)employeeByConstructionSite.ConstructionSite?.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ConstructionSiteName", ((object)employeeByConstructionSite.ConstructionSite?.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IsSynced", employeeByConstructionSite.IsSynced);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", employeeByConstructionSite.UpdatedAt);
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

        public EmployeeByConstructionSiteResponse UpdateSyncStatus(Guid identifier, int serverId, bool isSynced)
        {
            EmployeeByConstructionSiteResponse response = new EmployeeByConstructionSiteResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "UPDATE EmployeeByConstructionSites SET " +
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

        public EmployeeByConstructionSiteResponse Delete(Guid employeeIdentifier, Guid constructionSiteIdentifier)
        {
            EmployeeByConstructionSiteResponse response = new EmployeeByConstructionSiteResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "DELETE FROM EmployeeByConstructionSites WHERE EmployeeIdentifier = @EmployeeIdentifier AND ConstructionSiteIdentifier = @ConstructionSiteIdentifier";
                insertCommand.Parameters.AddWithValue("@EmployeeIdentifier", employeeIdentifier);
                insertCommand.Parameters.AddWithValue("@ConstructionSiteIdentifier", constructionSiteIdentifier);
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

        public EmployeeByConstructionSiteResponse DeleteAll()
        {
            EmployeeByConstructionSiteResponse response = new EmployeeByConstructionSiteResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM EmployeeByConstructionSites";
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
