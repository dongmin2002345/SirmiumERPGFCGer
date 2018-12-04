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
    public class EmployeeProfessionItemSQLiteRepository
    {
        public static string EmployeeItemTableCreatePart =
               "CREATE TABLE IF NOT EXISTS EmployeeProfessionItems " +
               "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
               "ServerId INTEGER NULL, " +
               "Identifier GUID, " +
               "EmployeeId INTEGER NULL, " +
               "EmployeeIdentifier GUID NULL, " +
               "EmployeeCode NVARCHAR(48) NULL, " +
               "EmployeeName NVARCHAR(48) NULL, " +
               "ProfessionId INTEGER NULL, " +
               "ProfessionIdentifier GUID NULL, " +
               "ProfessionCode NVARCHAR(48) NULL, " +
               "ProfessionName NVARCHAR(48) NULL, " +
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
            "SELECT ServerId, Identifier, EmployeeId, EmployeeIdentifier, " +
            "EmployeeCode, EmployeeName, ProfessionId, ProfessionIdentifier, " +
            "ProfessionCode, ProfessionName, CountryId, CountryIdentifier, " +
            "CountryCode, CountryName, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO EmployeeProfessionItems " +
            "(Id, ServerId, Identifier, EmployeeId, EmployeeIdentifier, " +
            "EmployeeCode, EmployeeName, ProfessionId, ProfessionIdentifier, " +
            "ProfessionCode, ProfessionName, CountryId, CountryIdentifier, " +
            "CountryCode, CountryName, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @EmployeeId, @EmployeeIdentifier, " +
            "@EmployeeCode, @EmployeeName, @ProfessionId, @ProfessionIdentifier, " +
            "@ProfessionCode, @ProfessionName, @CountryId, @CountryIdentifier, " +
            "@CountryCode, @CountryName, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";


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
                        EmployeeProfessionItems.Add(dbEntry);
                    }

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
                        EmployeeItem = dbEntry;
                    }
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

        public void Sync(IEmployeeProfessionService EmployeeItemService)
        {
            SyncEmployeeProfessionItemRequest request = new SyncEmployeeProfessionItemRequest();
            request.CompanyId = MainWindow.CurrentCompanyId;
            request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);


            EmployeeProfessionItemListResponse response = EmployeeItemService.Sync(request);
            if (response.Success)
            {
                List<EmployeeProfessionItemViewModel> EmployeeItemsFromDB = response.EmployeeProfessionItems;
                foreach (var EmployeeItem in EmployeeItemsFromDB.OrderBy(x => x.Id))
                {
                    Delete(EmployeeItem.Identifier);
                    if (EmployeeItem.IsActive)
                    {
                        EmployeeItem.IsSynced = true;
                        Create(EmployeeItem);
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

        public EmployeeProfessionItemResponse Create(EmployeeProfessionItemViewModel EmployeeItem)
        {
            EmployeeProfessionItemResponse response = new EmployeeProfessionItemResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = SqlCommandInsertPart;

                insertCommand.Parameters.AddWithValue("@ServerId", EmployeeItem.Id);
                insertCommand.Parameters.AddWithValue("@Identifier", EmployeeItem.Identifier);
                insertCommand.Parameters.AddWithValue("@EmployeeId", ((object)EmployeeItem.Employee?.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@EmployeeIdentifier", ((object)EmployeeItem.Employee?.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@EmployeeCode", ((object)EmployeeItem.Employee?.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@EmployeeName", ((object)EmployeeItem.Employee?.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ProfessionId", ((object)EmployeeItem.Profession?.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ProfessionIdentifier", ((object)EmployeeItem.Profession?.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ProfessionCode", ((object)EmployeeItem.Profession?.SecondCode) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ProfessionName", ((object)EmployeeItem.Profession?.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CountryId", ((object)EmployeeItem.Country?.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CountryIdentifier", ((object)EmployeeItem.Country?.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CountryCode", ((object)EmployeeItem.Country?.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@CountryName", ((object)EmployeeItem.Country?.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IsSynced", EmployeeItem.IsSynced);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)EmployeeItem.UpdatedAt) ?? DBNull.Value);
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

        public EmployeeProfessionItemResponse UpdateSyncStatus(Guid identifier, string code, DateTime? updatedAt, int serverId, bool isSynced)
        {
            EmployeeProfessionItemResponse response = new EmployeeProfessionItemResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "UPDATE EmployeeProfessionItems SET " +
                    "IsSynced = @IsSynced, " +
                    "Code = @Code, " +
                    "UpdatedAt = @UpdatedAt, " +
                    "ServerId = @ServerId " +
                    "WHERE Identifier = @Identifier ";

                insertCommand.Parameters.AddWithValue("@IsSynced", isSynced);
                insertCommand.Parameters.AddWithValue("@Code", code);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", updatedAt);
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

        public EmployeeProfessionItemResponse Delete(Guid identifier)
        {
            EmployeeProfessionItemResponse response = new EmployeeProfessionItemResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "DELETE FROM  EmployeeProfessionItems WHERE Identifier = @Identifier";
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
