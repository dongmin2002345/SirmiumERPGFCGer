using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.ConstructionSites;
using ServiceInterfaces.Messages.ConstructionSites;
using ServiceInterfaces.ViewModels.ConstructionSites;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.ConstructionSites
{
    public class ConstructionSiteCalculationSQLiteRepository
    {
        public static string ConstructionSiteCalculationTableCreatePart =
                  "CREATE TABLE IF NOT EXISTS ConstructionSiteCalculations " +
                  "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                  "ServerId INTEGER NULL, " +
                  "Identifier GUID, " +
                  "ConstructionSiteId INTEGER NULL, " +
                  "ConstructionSiteIdentifier GUID NULL, " +
                  "ConstructionSiteCode NVARCHAR(48) NULL, " +
                  "ConstructionSiteName NVARCHAR(48) NULL, " +
                  "NumOfEmployees INTEGER NULL, " +
                  "EmployeePrice DECIMAL NULL, " +
                  "NumOfMonths INTEGER NULL, " +
                  "OldValue DECIMAL NULL, " +
                  "NewValue DECIMAL NULL, " +
                  "ValueDifference DECIMAL NULL, " +
                  "PlusMinus NVARCHAR(48) NULL, " +
                  "IsSynced BOOL NULL, " +
                  "UpdatedAt DATETIME NULL, " +
                  "CreatedById INTEGER NULL, " +
                  "CreatedByName NVARCHAR(2048) NULL, " +
                  "CompanyId INTEGER NULL, " +
                  "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, " + 
            "ConstructionSiteId, ConstructionSiteIdentifier, ConstructionSiteCode, ConstructionSiteName, " +
            "NumOfEmployees, EmployeePrice, NumOfMonths, OldValue, NewValue, ValueDifference, PlusMinus, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO ConstructionSiteCalculations " +
            "(Id, ServerId, Identifier, " +
            "ConstructionSiteId, ConstructionSiteIdentifier, ConstructionSiteCode, ConstructionSiteName, " +
            "NumOfEmployees, EmployeePrice, NumOfMonths, OldValue, NewValue, ValueDifference, PlusMinus, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, " +
            "@ConstructionSiteId, @ConstructionSiteIdentifier, @ConstructionSiteCode, @ConstructionSiteName, " +
            "@NumOfEmployees, @EmployeePrice, @NumOfMonths, @OldValue, @NewValue, @ValueDifference, @PlusMinus, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        public ConstructionSiteCalculationListResponse GetConstructionSiteCalculationsByConstructionSite(int companyId, Guid ConstructionSiteIdentifier)
        {
            ConstructionSiteCalculationListResponse response = new ConstructionSiteCalculationListResponse();
            List<ConstructionSiteCalculationViewModel> ConstructionSiteCalculations = new List<ConstructionSiteCalculationViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM ConstructionSiteCalculations " +
                        "WHERE ConstructionSiteIdentifier = @ConstructionSiteIdentifier " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC;", db);
                    selectCommand.Parameters.AddWithValue("@ConstructionSiteIdentifier", ConstructionSiteIdentifier);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        ConstructionSiteCalculationViewModel dbEntry = new ConstructionSiteCalculationViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.ConstructionSite = SQLiteHelper.GetConstructionSite(query, ref counter);
                        dbEntry.NumOfEmployees = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.EmployeePrice = SQLiteHelper.GetDecimal(query, ref counter);
                        dbEntry.NumOfMonths = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.OldValue = SQLiteHelper.GetDecimal(query, ref counter);
                        dbEntry.NewValue = SQLiteHelper.GetDecimal(query, ref counter);
                        dbEntry.ValueDifference = SQLiteHelper.GetDecimal(query, ref counter);
                        dbEntry.PlusMinus = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        ConstructionSiteCalculations.Add(dbEntry);
                    }

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.ConstructionSiteCalculations = new List<ConstructionSiteCalculationViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.ConstructionSiteCalculations = ConstructionSiteCalculations;
            return response;
        }

        public ConstructionSiteCalculationResponse GetConstructionSiteCalculation(Guid identifier)
        {
            ConstructionSiteCalculationResponse response = new ConstructionSiteCalculationResponse();
            ConstructionSiteCalculationViewModel ConstructionSiteCalculation = new ConstructionSiteCalculationViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM ConstructionSiteCalculations " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        int counter = 0;
                        ConstructionSiteCalculationViewModel dbEntry = new ConstructionSiteCalculationViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.ConstructionSite = SQLiteHelper.GetConstructionSite(query, ref counter);
                        dbEntry.NumOfEmployees = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.EmployeePrice = SQLiteHelper.GetDecimal(query, ref counter);
                        dbEntry.NumOfMonths = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.OldValue = SQLiteHelper.GetDecimal(query, ref counter);
                        dbEntry.NewValue = SQLiteHelper.GetDecimal(query, ref counter);
                        dbEntry.ValueDifference = SQLiteHelper.GetDecimal(query, ref counter);
                        dbEntry.PlusMinus = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        ConstructionSiteCalculation = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.ConstructionSiteCalculation = new ConstructionSiteCalculationViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.ConstructionSiteCalculation = ConstructionSiteCalculation;
            return response;
        }

        public ConstructionSiteCalculationListResponse GetUnSyncedItems(int companyId)
        {
            ConstructionSiteCalculationListResponse response = new ConstructionSiteCalculationListResponse();
            List<ConstructionSiteCalculationViewModel> viewModels = new List<ConstructionSiteCalculationViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM  ConstructionSiteCalculations " +
                        "WHERE CompanyId = @CompanyId AND IsSynced = 0 " +
                        "ORDER BY Id DESC;", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        ConstructionSiteCalculationViewModel dbEntry = new ConstructionSiteCalculationViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.ConstructionSite = SQLiteHelper.GetConstructionSite(query, ref counter);
                        dbEntry.NumOfEmployees = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.EmployeePrice = SQLiteHelper.GetDecimal(query, ref counter);
                        dbEntry.NumOfMonths = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.OldValue = SQLiteHelper.GetDecimal(query, ref counter);
                        dbEntry.NewValue = SQLiteHelper.GetDecimal(query, ref counter);
                        dbEntry.ValueDifference = SQLiteHelper.GetDecimal(query, ref counter);
                        dbEntry.PlusMinus = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        viewModels.Add(dbEntry);
                    }

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.ConstructionSiteCalculations = new List<ConstructionSiteCalculationViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.ConstructionSiteCalculations = viewModels;
            return response;
        }

        public void Sync(IConstructionSiteCalculationService ConstructionSiteCalculationService)
        {
            var unSynced = GetUnSyncedItems(MainWindow.CurrentCompanyId);
            SyncConstructionSiteCalculationRequest request = new SyncConstructionSiteCalculationRequest();
            request.CompanyId = MainWindow.CurrentCompanyId;
            request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);


            ConstructionSiteCalculationListResponse response = ConstructionSiteCalculationService.Sync(request);
            if (response.Success)
            {
                List<ConstructionSiteCalculationViewModel> ConstructionSiteCalculationsFromDB = response.ConstructionSiteCalculations;
                foreach (var ConstructionSiteCalculation in ConstructionSiteCalculationsFromDB.OrderBy(x => x.Id))
                {
                    Delete(ConstructionSiteCalculation.Identifier);
                    if (ConstructionSiteCalculation.IsActive)
                    {
                        ConstructionSiteCalculation.IsSynced = true;
                        Create(ConstructionSiteCalculation);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from ConstructionSiteCalculations WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from ConstructionSiteCalculations WHERE CompanyId = @CompanyId", db);
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

        public ConstructionSiteCalculationResponse Create(ConstructionSiteCalculationViewModel ConstructionSiteCalculation)
        {
            ConstructionSiteCalculationResponse response = new ConstructionSiteCalculationResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = SqlCommandInsertPart;

                insertCommand.Parameters.AddWithValue("@ServerId", ConstructionSiteCalculation.Id);
                insertCommand.Parameters.AddWithValue("@Identifier", ConstructionSiteCalculation.Identifier);
                insertCommand.Parameters.AddWithValue("@ConstructionSiteId", ((object)ConstructionSiteCalculation.ConstructionSite.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ConstructionSiteIdentifier", ((object)ConstructionSiteCalculation.ConstructionSite.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ConstructionSiteCode", ((object)ConstructionSiteCalculation.ConstructionSite.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ConstructionSiteName", ((object)ConstructionSiteCalculation.ConstructionSite.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@NumOfEmployees", ((object)ConstructionSiteCalculation.NumOfEmployees) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@EmployeePrice", ((object)ConstructionSiteCalculation.EmployeePrice) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@NumOfMonths", ((object)ConstructionSiteCalculation.NumOfMonths) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@OldValue", ((object)ConstructionSiteCalculation.OldValue) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@NewValue", ((object)ConstructionSiteCalculation.NewValue) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ValueDifference", ((object)ConstructionSiteCalculation.ValueDifference) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@PlusMinus", ((object)ConstructionSiteCalculation.PlusMinus) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IsSynced", ConstructionSiteCalculation.IsSynced);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)ConstructionSiteCalculation.UpdatedAt) ?? DBNull.Value);
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

        public ConstructionSiteCalculationResponse UpdateSyncStatus(Guid identifier, DateTime? updatedAt, int serverId, decimal valueDifference, decimal newValue, bool isSynced)
        {
            ConstructionSiteCalculationResponse response = new ConstructionSiteCalculationResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "UPDATE ConstructionSiteCalculations SET " +
                    "IsSynced = @IsSynced, " +
                    "UpdatedAt = @UpdatedAt, " +
                    "NewValue = @NewValue, " +
                    "ValueDifference = @ValueDifference, " +
                    "ServerId = @ServerId " +
                    "WHERE Identifier = @Identifier ";

                insertCommand.Parameters.AddWithValue("@IsSynced", isSynced);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", updatedAt);
                insertCommand.Parameters.AddWithValue("@NewValue", newValue);
                insertCommand.Parameters.AddWithValue("@ValueDifference", valueDifference);
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

        public ConstructionSiteCalculationResponse Delete(Guid identifier)
        {
            ConstructionSiteCalculationResponse response = new ConstructionSiteCalculationResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "DELETE FROM ConstructionSiteCalculations WHERE Identifier = @Identifier";
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

        public ConstructionSiteCalculationResponse DeleteAll()
        {
            ConstructionSiteCalculationResponse response = new ConstructionSiteCalculationResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM ConstructionSiteCalculations";
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
