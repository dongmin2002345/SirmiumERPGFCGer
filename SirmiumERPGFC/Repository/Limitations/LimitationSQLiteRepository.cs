using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Limitations;
using ServiceInterfaces.Messages.Limitations;
using ServiceInterfaces.ViewModels.Limitations;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;

namespace SirmiumERPGFC.Repository.Limitations
{
    public class LimitationSQLiteRepository
    {
        #region SQL

        public static string LimitationTableCreatePart =
            "CREATE TABLE IF NOT EXISTS Limitations " +
             "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
             "ServerId INTEGER NULL, " +
             "Identifier GUID, " +

             "ConstructionSiteLimit INTEGER NULL, " +
             "BusinessPartnerConstructionSiteLimit INTEGER NULL, " +
             "EmployeeConstructionSiteLimit INTEGER NULL, " +
             "EmployeeBusinessPartnerLimit INTEGER NULL, " +
             "EmployeeBirthdayLimit INTEGER NULL, " +

             "EmployeePassportLimit INTEGER NULL, " +
             "EmployeeEmbasyLimit INTEGER NULL, " +
             "EmployeeVisaTakeOffLimit INTEGER NULL, " +
             "EmployeeVisaLimit INTEGER NULL, " +
             "EmployeeWorkLicenceLimit INTEGER NULL, " +
             "EmployeeDriveLicenceLimit INTEGER NULL, " +
             "EmployeeEmbasyFamilyLimit INTEGER NULL, " +

             "PersonPassportLimit INTEGER NULL, " +
             "PersonEmbasyLimit INTEGER NULL, " +
             "PersonVisaTakeOffLimit INTEGER NULL, " +
             "PersonVisaLimit INTEGER NULL, " +
             "PersonWorkLicenceLimit INTEGER NULL, " +
             "PersonDriveLicenceLimit INTEGER NULL, " +
             "PersonEmbasyFamilyLimit INTEGER NULL, " +

             "IsSynced BOOL NULL, " +
             "UpdatedAt DATETIME NULL, " +
             "CreatedById INTEGER NULL, " +
             "CreatedByName NVARCHAR(2048) NULL, " +
             "CompanyId INTEGER NULL, " +
             "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, " +
            "ConstructionSiteLimit, BusinessPartnerConstructionSiteLimit, EmployeeConstructionSiteLimit, EmployeeBusinessPartnerLimit, EmployeeBirthdayLimit, " +
            "EmployeePassportLimit, EmployeeEmbasyLimit, EmployeeVisaTakeOffLimit, EmployeeVisaLimit, EmployeeWorkLicenceLimit, EmployeeDriveLicenceLimit, EmployeeEmbasyFamilyLimit, " +
            "PersonPassportLimit, PersonEmbasyLimit, PersonVisaTakeOffLimit, PersonVisaLimit, PersonWorkLicenceLimit, PersonDriveLicenceLimit, PersonEmbasyFamilyLimit, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO Limitations " +
            "(Id, ServerId, Identifier, " +
            "ConstructionSiteLimit, BusinessPartnerConstructionSiteLimit, EmployeeConstructionSiteLimit, EmployeeBusinessPartnerLimit, EmployeeBirthdayLimit, " +
            "EmployeePassportLimit, EmployeeEmbasyLimit, EmployeeVisaTakeOffLimit, EmployeeVisaLimit, EmployeeWorkLicenceLimit, EmployeeDriveLicenceLimit, EmployeeEmbasyFamilyLimit, " +
            "PersonPassportLimit, PersonEmbasyLimit, PersonVisaTakeOffLimit, PersonVisaLimit, PersonWorkLicenceLimit, PersonDriveLicenceLimit, PersonEmbasyFamilyLimit, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, " +
            "@ConstructionSiteLimit, @BusinessPartnerConstructionSiteLimit, @EmployeeConstructionSiteLimit, @EmployeeBusinessPartnerLimit, @EmployeeBirthdayLimit, " +
            "@EmployeePassportLimit, @EmployeeEmbasyLimit, @EmployeeVisaTakeOffLimit, @EmployeeVisaLimit, @EmployeeWorkLicenceLimit, @EmployeeDriveLicenceLimit, @EmployeeEmbasyFamilyLimit, " +
            "@PersonPassportLimit, @PersonEmbasyLimit, @PersonVisaTakeOffLimit, @PersonVisaLimit, @PersonWorkLicenceLimit, @PersonDriveLicenceLimit, @PersonEmbasyFamilyLimit, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods

        private LimitationViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            LimitationViewModel dbEntry = new LimitationViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.ConstructionSiteLimit = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.BusinessPartnerConstructionSiteLimit = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.EmployeeConstructionSiteLimit = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.EmployeeBusinessPartnerLimit = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.EmployeeBirthdayLimit = SQLiteHelper.GetInt(query, ref counter);

            dbEntry.EmployeePassportLimit = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.EmployeeEmbasyLimit = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.EmployeeVisaTakeOffLimit = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.EmployeeVisaLimit = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.EmployeeWorkLicenceLimit = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.EmployeeDriveLicenceLimit = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.EmployeeEmbasyFamilyLimit = SQLiteHelper.GetInt(query, ref counter);

            dbEntry.PersonPassportLimit = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.PersonEmbasyLimit = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.PersonVisaTakeOffLimit = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.PersonVisaLimit = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.PersonWorkLicenceLimit = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.PersonDriveLicenceLimit = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.PersonEmbasyFamilyLimit = SQLiteHelper.GetInt(query, ref counter);

            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);

            return dbEntry;
        }

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, LimitationViewModel limitation)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", limitation.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", limitation.Identifier);
            insertCommand.Parameters.AddWithValue("@ConstructionSiteLimit", ((object)limitation.ConstructionSiteLimit) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerConstructionSiteLimit", ((object)limitation.BusinessPartnerConstructionSiteLimit) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmployeeConstructionSiteLimit", ((object)limitation.EmployeeConstructionSiteLimit) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmployeeBusinessPartnerLimit", ((object)limitation.EmployeeBusinessPartnerLimit) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmployeeBirthdayLimit", ((object)limitation.EmployeeBirthdayLimit) ?? DBNull.Value);

            insertCommand.Parameters.AddWithValue("@EmployeePassportLimit", ((object)limitation.EmployeePassportLimit) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmployeeEmbasyLimit", ((object)limitation.EmployeeEmbasyLimit) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmployeeVisaTakeOffLimit", ((object)limitation.EmployeeVisaTakeOffLimit) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmployeeVisaLimit", ((object)limitation.EmployeeVisaLimit) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmployeeWorkLicenceLimit", ((object)limitation.EmployeeWorkLicenceLimit) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmployeeDriveLicenceLimit", ((object)limitation.EmployeeDriveLicenceLimit) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EmployeeEmbasyFamilyLimit", ((object)limitation.EmployeeEmbasyFamilyLimit) ?? DBNull.Value);

            insertCommand.Parameters.AddWithValue("@PersonPassportLimit", ((object)limitation.PersonPassportLimit) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PersonEmbasyLimit", ((object)limitation.PersonEmbasyLimit) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PersonVisaTakeOffLimit", ((object)limitation.PersonVisaTakeOffLimit) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PersonVisaLimit", ((object)limitation.PersonVisaLimit) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PersonWorkLicenceLimit", ((object)limitation.PersonWorkLicenceLimit) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PersonDriveLicenceLimit", ((object)limitation.PersonDriveLicenceLimit) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PersonEmbasyFamilyLimit", ((object)limitation.PersonEmbasyFamilyLimit) ?? DBNull.Value);

            insertCommand.Parameters.AddWithValue("@IsSynced", limitation.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)limitation.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read

        public LimitationResponse GetLimitation(int companyId)
        {
            LimitationResponse response = new LimitationResponse();
            LimitationViewModel limitation = new LimitationViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Limitations " +
                        "WHERE CompanyId = @CompanyId;", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                        limitation = Read(query);
                    
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.Limitation = new LimitationViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Limitation = limitation;
            return response;
        }

        #endregion

        #region Sync

        public void Sync(ILimitationService limitationService, Action<int, int> callback = null)
        {
            try
            {
                SyncLimitationRequest request = new SyncLimitationRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                LimitationListResponse response = limitationService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.Limitations?.Count ?? 0;
                    List<LimitationViewModel> limitationsFromDB = response.Limitations;

                    using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM Limitations WHERE Identifier = @Identifier";

                            SqliteCommand insertCommand = db.CreateCommand();
                            insertCommand.CommandText = SqlCommandInsertPart;

                            foreach (var limitation in limitationsFromDB)
                            {
                                deleteCommand.Parameters.AddWithValue("@Identifier", limitation.Identifier);
                                deleteCommand.ExecuteNonQuery();
                                deleteCommand.Parameters.Clear();

                                if (limitation.IsActive)
                                {
                                    limitation.IsSynced = true;

                                    insertCommand = AddCreateParameters(insertCommand, limitation);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from Limitations WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from Limitations WHERE CompanyId = @CompanyId", db);
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

        public LimitationResponse Create(LimitationViewModel limitation)
        {
            LimitationResponse response = new LimitationResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, limitation);
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

        public LimitationResponse Delete(Guid identifier)
        {
            LimitationResponse response = new LimitationResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM Limitations WHERE Identifier = @Identifier";
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

        public LimitationResponse DeleteAll()
        {
            LimitationResponse response = new LimitationResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM Limitations";
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
