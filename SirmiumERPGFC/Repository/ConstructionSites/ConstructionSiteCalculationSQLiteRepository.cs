using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.ConstructionSites;
using ServiceInterfaces.Gloabals;
using ServiceInterfaces.Messages.ConstructionSites;
using ServiceInterfaces.ViewModels.ConstructionSites;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;

namespace SirmiumERPGFC.Repository.ConstructionSites
{
    public class ConstructionSiteCalculationSQLiteRepository
    {
        #region SQL

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
                  "ItemStatus INTEGER NOT NULL, " +
                  "IsSynced BOOL NULL, " +
                  "UpdatedAt DATETIME NULL, " +
                  "CreatedById INTEGER NULL, " +
                  "CreatedByName NVARCHAR(2048) NULL, " +
                  "CompanyId INTEGER NULL, " +
                  "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, " + 
            "ConstructionSiteId, ConstructionSiteIdentifier, ConstructionSiteCode, ConstructionSiteName, " +
            "NumOfEmployees, EmployeePrice, NumOfMonths, OldValue, NewValue, ValueDifference, PlusMinus, ItemStatus, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO ConstructionSiteCalculations " +
            "(Id, ServerId, Identifier, " +
            "ConstructionSiteId, ConstructionSiteIdentifier, ConstructionSiteCode, ConstructionSiteName, " +
            "NumOfEmployees, EmployeePrice, NumOfMonths, OldValue, NewValue, ValueDifference, PlusMinus, ItemStatus, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, " +
            "@ConstructionSiteId, @ConstructionSiteIdentifier, @ConstructionSiteCode, @ConstructionSiteName, " +
            "@NumOfEmployees, @EmployeePrice, @NumOfMonths, @OldValue, @NewValue, @ValueDifference, @PlusMinus, @ItemStatus, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods
        private static ConstructionSiteCalculationViewModel Read(SqliteDataReader query)
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
            dbEntry.ItemStatus = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
            return dbEntry;
        }

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, ConstructionSiteCalculationViewModel constructionSiteCalculation)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", constructionSiteCalculation.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", constructionSiteCalculation.Identifier);
            insertCommand.Parameters.AddWithValue("@ConstructionSiteId", ((object)constructionSiteCalculation.ConstructionSite.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ConstructionSiteIdentifier", ((object)constructionSiteCalculation.ConstructionSite.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ConstructionSiteCode", ((object)constructionSiteCalculation.ConstructionSite.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ConstructionSiteName", ((object)constructionSiteCalculation.ConstructionSite.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@NumOfEmployees", ((object)constructionSiteCalculation.NumOfEmployees) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmployeePrice", ((object)constructionSiteCalculation.EmployeePrice) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@NumOfMonths", ((object)constructionSiteCalculation.NumOfMonths) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@OldValue", ((object)constructionSiteCalculation.OldValue) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@NewValue", ((object)constructionSiteCalculation.NewValue) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ValueDifference", ((object)constructionSiteCalculation.ValueDifference) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PlusMinus", ((object)constructionSiteCalculation.PlusMinus) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ItemStatus", ((object)constructionSiteCalculation.ItemStatus) ?? DBNull.Value);

            insertCommand.Parameters.AddWithValue("@IsSynced", constructionSiteCalculation.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)constructionSiteCalculation.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read

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
                        ConstructionSiteCalculationViewModel dbEntry = Read(query);
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
                        ConstructionSiteCalculationViewModel dbEntry = Read(query);
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

        //public ConstructionSiteCalculationListResponse GetUnSyncedItems(int companyId)
        //{
        //    ConstructionSiteCalculationListResponse response = new ConstructionSiteCalculationListResponse();
        //    List<ConstructionSiteCalculationViewModel> viewModels = new List<ConstructionSiteCalculationViewModel>();

        //    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
        //    {
        //        db.Open();
        //        try
        //        {
        //            SqliteCommand selectCommand = new SqliteCommand(
        //                SqlCommandSelectPart +
        //                "FROM  ConstructionSiteCalculations " +
        //                "WHERE CompanyId = @CompanyId AND IsSynced = 0 " +
        //                "ORDER BY Id DESC;", db);
        //            selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

        //            SqliteDataReader query = selectCommand.ExecuteReader();

        //            while (query.Read())
        //            {
        //                int counter = 0;
        //                ConstructionSiteCalculationViewModel dbEntry = new ConstructionSiteCalculationViewModel();
        //                dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
        //                dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
        //                dbEntry.ConstructionSite = SQLiteHelper.GetConstructionSite(query, ref counter);
        //                dbEntry.NumOfEmployees = SQLiteHelper.GetInt(query, ref counter);
        //                dbEntry.EmployeePrice = SQLiteHelper.GetDecimal(query, ref counter);
        //                dbEntry.NumOfMonths = SQLiteHelper.GetInt(query, ref counter);
        //                dbEntry.OldValue = SQLiteHelper.GetDecimal(query, ref counter);
        //                dbEntry.NewValue = SQLiteHelper.GetDecimal(query, ref counter);
        //                dbEntry.ValueDifference = SQLiteHelper.GetDecimal(query, ref counter);
        //                dbEntry.PlusMinus = SQLiteHelper.GetString(query, ref counter);
        //                dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
        //                dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
        //                dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
        //                dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
        //                viewModels.Add(dbEntry);
        //            }

        //        }
        //        catch (SqliteException error)
        //        {
        //            MainWindow.ErrorMessage = error.Message;
        //            response.Success = false;
        //            response.Message = error.Message;
        //            response.ConstructionSiteCalculations = new List<ConstructionSiteCalculationViewModel>();
        //            return response;
        //        }
        //        db.Close();
        //    }
        //    response.Success = true;
        //    response.ConstructionSiteCalculations = viewModels;
        //    return response;
        //}

        #endregion

        #region Sync

        public void Sync(IConstructionSiteCalculationService ConstructionSiteCalculationService, Action<int, int> callback = null)
        {
            try
            {
                SyncConstructionSiteCalculationRequest request = new SyncConstructionSiteCalculationRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                ConstructionSiteCalculationListResponse response = ConstructionSiteCalculationService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.ConstructionSiteCalculations?.Count ?? 0;
                    List<ConstructionSiteCalculationViewModel> items = response.ConstructionSiteCalculations;

                    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM ConstructionSiteCalculations WHERE Identifier = @Identifier";

                            SqliteCommand insertCommand = db.CreateCommand();
                            insertCommand.CommandText = SqlCommandInsertPart;

                            foreach (var item in items)
                            {
                                deleteCommand.Parameters.AddWithValue("@Identifier", item.Identifier);
                                deleteCommand.ExecuteNonQuery();
                                deleteCommand.Parameters.Clear();

                                if (item.IsActive)
                                {
                                    item.IsSynced = true;

                                    insertCommand = AddCreateParameters(insertCommand, item);
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

        public ConstructionSiteCalculationResponse Create(ConstructionSiteCalculationViewModel constructionSiteCalculation)
        {
            ConstructionSiteCalculationResponse response = new ConstructionSiteCalculationResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, constructionSiteCalculation);
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

        public ConstructionSiteCalculationResponse Delete(Guid identifier)
        {
            ConstructionSiteCalculationResponse response = new ConstructionSiteCalculationResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM ConstructionSiteCalculations WHERE Identifier = @Identifier";
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

        //public ConstructionSiteCalculationResponse DeleteAll()
        //{
        //    ConstructionSiteCalculationResponse response = new ConstructionSiteCalculationResponse();

        //    try
        //    {
        //        using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
        //        {
        //            db.Open();
        //            db.EnableExtensions(true);

        //            SqliteCommand insertCommand = new SqliteCommand();
        //            insertCommand.Connection = db;

        //            //Use parameterized query to prevent SQL injection attacks
        //            insertCommand.CommandText = "DELETE FROM ConstructionSiteCalculations";
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

        public ConstructionSiteCalculationResponse SetStatusDeleted(Guid identifier)
        {
            ConstructionSiteCalculationResponse response = new ConstructionSiteCalculationResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =  "UPDATE ConstructionSiteCalculations SET ItemStatus = @ItemStatus WHERE Identifier = @Identifier";
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
