using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Gloabals;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.Employees
{
    public class PhysicalPersonLicenceSQLiteRepository
    {
        public static string PhysicalPersonItemTableCreatePart =
                  "CREATE TABLE IF NOT EXISTS PhysicalPersonLicences " +
                  "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                  "ServerId INTEGER NULL, " +
                  "Identifier GUID, " +
                  "PhysicalPersonId INTEGER NULL, " +
                  "PhysicalPersonIdentifier GUID NULL, " +
                  "PhysicalPersonCode NVARCHAR(48) NULL, " +
                  "PhysicalPersonName NVARCHAR(48) NULL, " +
                  "LicenceId INTEGER NULL, " +
                  "LicenceIdentifier GUID NULL, " +
                  "LicenceCode NVARCHAR(48) NULL, " +
                  "LicenceCategory NVARCHAR(48) NULL, " +
                  "LicenceDescription NVARCHAR(48) NULL, " +
                  "ValidFrom DATETIME NULL, " +
                  "ValidTo DATETIME NULL, " +
                  "CountryId INTEGER NULL, " +
                  "CountryIdentifier GUID NULL, " +
                  "CountryCode NVARCHAR(48) NULL, " +
                  "CountryName NVARCHAR(48) NULL, " +
                  "ItemStatus INTEGER NOT NULL, " +
                  "IsSynced BOOL NULL, " +
                  "UpdatedAt DATETIME NULL, " +
                  "CreatedById INTEGER NULL, " +
                  "CreatedByName NVARCHAR(2048) NULL, " +
                  "CompanyId INTEGER NULL, " +
                  "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, PhysicalPersonId, PhysicalPersonIdentifier, " +
            "PhysicalPersonCode, PhysicalPersonName, LicenceId, LicenceIdentifier, " +
            "LicenceCode, LicenceCategory, LicenceDescription, ValidFrom, ValidTo, ItemStatus, CountryId, CountryIdentifier, " +
            "CountryCode, CountryName, "+
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO PhysicalPersonLicences " +
            "(Id, ServerId, Identifier, PhysicalPersonId, PhysicalPersonIdentifier, " +
            "PhysicalPersonCode, PhysicalPersonName, LicenceId, LicenceIdentifier, " +
            "LicenceCode, LicenceCategory, LicenceDescription, ValidFrom, ValidTo, ItemStatus, CountryId, CountryIdentifier, " +
            "CountryCode, CountryName, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @PhysicalPersonId, @PhysicalPersonIdentifier, " +
            "@PhysicalPersonCode, @PhysicalPersonName, @LicenceId, @LicenceIdentifier, " +
            "@LicenceCode, @LicenceCategory, @LicenceDescription, @ValidFrom, @ValidTo, @ItemStatus, @CountryId, @CountryIdentifier, " +
            "@CountryCode, @CountryName," +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #region Helper methods
        private static PhysicalPersonLicenceViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            PhysicalPersonLicenceViewModel dbEntry = new PhysicalPersonLicenceViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.PhysicalPerson = SQLiteHelper.GetPhysicalPerson(query, ref counter);
            dbEntry.Licence = SQLiteHelper.GetLicence(query, ref counter);
            dbEntry.ValidFrom = SQLiteHelper.GetDateTimeNullable(query, ref counter);
            dbEntry.ValidTo = SQLiteHelper.GetDateTimeNullable(query, ref counter);
            dbEntry.ItemStatus = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Country = SQLiteHelper.GetCountry(query, ref counter);
            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
            return dbEntry;
        }

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, PhysicalPersonLicenceViewModel PhysicalPersonItem)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", PhysicalPersonItem.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", PhysicalPersonItem.Identifier);
            insertCommand.Parameters.AddWithValue("@PhysicalPersonId", ((object)PhysicalPersonItem.PhysicalPerson.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PhysicalPersonIdentifier", ((object)PhysicalPersonItem.PhysicalPerson.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PhysicalPersonCode", ((object)PhysicalPersonItem.PhysicalPerson.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PhysicalPersonName", ((object)PhysicalPersonItem.PhysicalPerson.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@LicenceId", ((object)PhysicalPersonItem.Licence.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@LicenceIdentifier", ((object)PhysicalPersonItem.Licence.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@LicenceCode", ((object)PhysicalPersonItem.Licence.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@LicenceCategory", ((object)PhysicalPersonItem.Licence.Category) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@LicenceDescription", ((object)PhysicalPersonItem.Licence.Description) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryId", ((object)PhysicalPersonItem.Country.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryIdentifier", ((object)PhysicalPersonItem.Country.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryCode", ((object)PhysicalPersonItem.Country.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryName", ((object)PhysicalPersonItem.Country.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ValidFrom", ((object)PhysicalPersonItem.ValidFrom) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ValidTo", ((object)PhysicalPersonItem.ValidTo) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ItemStatus", PhysicalPersonItem.ItemStatus);
            insertCommand.Parameters.AddWithValue("@IsSynced", PhysicalPersonItem.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)PhysicalPersonItem.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        public PhysicalPersonLicenceListResponse GetPhysicalPersonLicencesByPhysicalPerson(int companyId, Guid PhysicalPersonIdentifier)
        {
            PhysicalPersonLicenceListResponse response = new PhysicalPersonLicenceListResponse();
            List<PhysicalPersonLicenceViewModel> PhysicalPersonLicences = new List<PhysicalPersonLicenceViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM  PhysicalPersonLicences " +
                        "WHERE PhysicalPersonIdentifier = @PhysicalPersonIdentifier " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC;", db);
                    selectCommand.Parameters.AddWithValue("@PhysicalPersonIdentifier", PhysicalPersonIdentifier);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        PhysicalPersonLicenceViewModel dbEntry = Read(query);
                        PhysicalPersonLicences.Add(dbEntry);
                    }

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.PhysicalPersonLicences = new List<PhysicalPersonLicenceViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.PhysicalPersonLicences = PhysicalPersonLicences;
            return response;
        }

        public PhysicalPersonLicenceResponse GetPhysicalPersonLicence(Guid identifier)
        {
            PhysicalPersonLicenceResponse response = new PhysicalPersonLicenceResponse();
            PhysicalPersonLicenceViewModel PhysicalPersonItem = new PhysicalPersonLicenceViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM  PhysicalPersonLicences " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        PhysicalPersonLicenceViewModel dbEntry = Read(query);
                        PhysicalPersonItem = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.PhysicalPersonLicence = new PhysicalPersonLicenceViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.PhysicalPersonLicence = PhysicalPersonItem;
            return response;
        }

        public void Sync(IPhysicalPersonLicenceService physicalPersonItemService, Action<int, int> callback = null)
        {
            try
            {
                SyncPhysicalPersonLicenceRequest request = new SyncPhysicalPersonLicenceRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                PhysicalPersonLicenceListResponse response = physicalPersonItemService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.PhysicalPersonLicences?.Count ?? 0;
                    List<PhysicalPersonLicenceViewModel> items = response.PhysicalPersonLicences;

                    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM PhysicalPersonLicences WHERE Identifier = @Identifier";

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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from  PhysicalPersonLicences WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from  PhysicalPersonLicences WHERE CompanyId = @CompanyId", db);
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

        public PhysicalPersonLicenceResponse Create(PhysicalPersonLicenceViewModel PhysicalPersonItem)
        {
            PhysicalPersonLicenceResponse response = new PhysicalPersonLicenceResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, PhysicalPersonItem);
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

        //public PhysicalPersonLicenceResponse UpdateSyncStatus(Guid identifier, string code, DateTime? updatedAt, int serverId, bool isSynced)
        //{
        //    PhysicalPersonLicenceResponse response = new PhysicalPersonLicenceResponse();

        //    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
        //    {
        //        db.Open();

        //        SqliteCommand insertCommand = new SqliteCommand();
        //        insertCommand.Connection = db;

        //        insertCommand.CommandText = "UPDATE PhysicalPersonLicences SET " +
        //            "IsSynced = @IsSynced, " +
        //            "Code = @Code, " +
        //            "UpdatedAt = @UpdatedAt, " +
        //            "ServerId = @ServerId " +
        //            "WHERE Identifier = @Identifier ";

        //        insertCommand.Parameters.AddWithValue("@IsSynced", isSynced);
        //        insertCommand.Parameters.AddWithValue("@Code", code);
        //        insertCommand.Parameters.AddWithValue("@UpdatedAt", updatedAt);
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

        public PhysicalPersonLicenceResponse Delete(Guid identifier)
        {
            PhysicalPersonLicenceResponse response = new PhysicalPersonLicenceResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "DELETE FROM  PhysicalPersonLicences WHERE Identifier = @Identifier";
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

        //public PhysicalPersonLicenceResponse DeleteAll()
        //{
        //    PhysicalPersonLicenceResponse response = new PhysicalPersonLicenceResponse();

        //    try
        //    {
        //        using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
        //        {
        //            db.Open();
        //            db.EnableExtensions(true);

        //            SqliteCommand insertCommand = new SqliteCommand();
        //            insertCommand.Connection = db;

        //            //Use parameterized query to prevent SQL injection attacks
        //            insertCommand.CommandText = "DELETE FROM PhysicalPersonLicences";
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

        public PhysicalPersonLicenceResponse SetStatusDeleted(Guid identifier)
        {
            PhysicalPersonLicenceResponse response = new PhysicalPersonLicenceResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "UPDATE PhysicalPersonLicences SET ItemStatus = @ItemStatus WHERE Identifier = @Identifier";
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
    }
}
