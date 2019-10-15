using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Gloabals;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;

namespace SirmiumERPGFC.Repository.BusinessPartners
{
    public class BusinessPartnerPhoneSQLiteRepository
    {
        #region SQL

        public static string BusinessPartnerPhoneTableCreatePart =
            "CREATE TABLE IF NOT EXISTS BusinessPartnerPhones " +
            "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
            "ServerId INTEGER NULL, " +
            "Identifier GUID, " +
            "BusinessPartnerId INTEGER NULL, " +
            "BusinessPartnerIdentifier GUID NULL, " +
            "BusinessPartnerCode NVARCHAR(2048) NULL, " +
            "BusinessPartnerName NVARCHAR(2048) NULL, " +
            "BusinessPartnerInternalCode NVARCHAR(2048) NULL, " +
            "BusinessPartnerNameGer NVARCHAR(2048) NULL, " +
            "Phone NVARCHAR(2048) NULL, " +
            "Mobile NVARCHAR(2048) NULL, " +
            "Fax NVARCHAR(2048) NULL, " +
            "Email NVARCHAR(2048) NULL, " +
            "ContactPersonFirstName NVARCHAR(2048) NULL, " +
            "ContactPersonLastName NVARCHAR(2048) NULL, " +
            "Birthday DATETIME NULL, " + 
            "Description NVARCHAR(2048) NULL, " +
            "Path NVARCHAR(2048) NULL, " +
            "ItemStatus INTEGER NOT NULL, " +
            "IsSynced BOOL NULL, " +
            "UpdatedAt DATETIME NULL, " +
            "CreatedById INTEGER NULL, " +
            "CreatedByName NVARCHAR(2048) NULL, " +
            "CompanyId INTEGER NULL, " +
            "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, BusinessPartnerInternalCode, BusinessPartnerNameGer, " +
            "Phone, Mobile, Fax, Email, ContactPersonFirstName, ContactPersonLastName, Birthday, Description, Path, ItemStatus, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO BusinessPartnerPhones " +
            "(Id, ServerId, Identifier, BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, BusinessPartnerInternalCode, BusinessPartnerNameGer, " +
            "Phone, Mobile, Fax, Email, ContactPersonFirstName, ContactPersonLastName, Birthday, Description, Path, ItemStatus, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @BusinessPartnerId, @BusinessPartnerIdentifier, @BusinessPartnerCode, @BusinessPartnerName, @BusinessPartnerInternalCode, @BusinessPartnerNameGer, " +
            "@Phone, @Mobile, @Fax, @Email, @ContactPersonFirstName, @ContactPersonLastName, @Birthday, @Description, @Path, @ItemStatus, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods

        private BusinessPartnerPhoneViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            BusinessPartnerPhoneViewModel dbEntry = new BusinessPartnerPhoneViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.BusinessPartner = SQLiteHelper.GetBusinessPartner(query, ref counter);
            dbEntry.Phone = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Mobile = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Fax = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Email = SQLiteHelper.GetString(query, ref counter);
            dbEntry.ContactPersonFirstName = SQLiteHelper.GetString(query, ref counter);
            dbEntry.ContactPersonLastName = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Birthday = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.Description = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Path = SQLiteHelper.GetString(query, ref counter);
            dbEntry.ItemStatus = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);

            return dbEntry;
        }

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, BusinessPartnerPhoneViewModel businessPartnerPhone)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", businessPartnerPhone.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", businessPartnerPhone.Identifier);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerId", ((object)businessPartnerPhone.BusinessPartner.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", ((object)businessPartnerPhone.BusinessPartner.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerCode", ((object)businessPartnerPhone.BusinessPartner.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerName", ((object)businessPartnerPhone.BusinessPartner.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerInternalCode", ((object)businessPartnerPhone.BusinessPartner.InternalCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerNameGer", ((object)businessPartnerPhone.BusinessPartner.NameGer) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Phone", ((object)businessPartnerPhone.Phone) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Mobile", ((object)businessPartnerPhone.Mobile) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Fax", ((object)businessPartnerPhone.Fax) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Email", ((object)businessPartnerPhone.Email) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ContactPersonFirstName", ((object)businessPartnerPhone.ContactPersonFirstName) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ContactPersonLastName", ((object)businessPartnerPhone.ContactPersonLastName) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Birthday", ((object)businessPartnerPhone.Birthday) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Description", ((object)businessPartnerPhone.Description) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Path", ((object)businessPartnerPhone.Path) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ItemStatus", ((object)businessPartnerPhone.ItemStatus) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsSynced", businessPartnerPhone.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)businessPartnerPhone.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read

        public BusinessPartnerPhoneListResponse GetBusinessPartnerPhonesByBusinessPartner(int companyId, Guid businessPartnerIdentifier)
        {
            BusinessPartnerPhoneListResponse response = new BusinessPartnerPhoneListResponse();
            List<BusinessPartnerPhoneViewModel> businessPartnerPhones = new List<BusinessPartnerPhoneViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM BusinessPartnerPhones " +
                        "WHERE BusinessPartnerIdentifier = @BusinessPartnerIdentifier " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC;", db);
                    
                    selectCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", businessPartnerIdentifier);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        BusinessPartnerPhoneViewModel dbEntry = Read(query);
                        businessPartnerPhones.Add(dbEntry);

                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.BusinessPartnerPhones = new List<BusinessPartnerPhoneViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.BusinessPartnerPhones = businessPartnerPhones;
            return response;
        }

        public BusinessPartnerPhoneResponse GetBusinessPartnerPhone(Guid identifier)
        {
            BusinessPartnerPhoneResponse response = new BusinessPartnerPhoneResponse();
            BusinessPartnerPhoneViewModel businessPartnerPhone = new BusinessPartnerPhoneViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM BusinessPartnerPhones " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        BusinessPartnerPhoneViewModel dbEntry = Read(query);
                        businessPartnerPhone = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.BusinessPartnerPhone = new BusinessPartnerPhoneViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.BusinessPartnerPhone = businessPartnerPhone;
            return response;
        }

        #endregion

        #region Sync

        public void Sync(IBusinessPartnerPhoneService BusinessPartnerPhoneService, Action<int, int> callback = null)
        {
            try
            {
                SyncBusinessPartnerPhoneRequest request = new SyncBusinessPartnerPhoneRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                BusinessPartnerPhoneListResponse response = BusinessPartnerPhoneService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.BusinessPartnerPhones?.Count ?? 0;
                    List<BusinessPartnerPhoneViewModel> businessPartnerPhonesFromDB = response.BusinessPartnerPhones;

                    using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM BusinessPartnerPhones WHERE Identifier = @Identifier";

                            SqliteCommand insertCommand = db.CreateCommand();
                            insertCommand.CommandText = SqlCommandInsertPart;

                            foreach (var businessPartnerPhone in businessPartnerPhonesFromDB)
                            {
                                deleteCommand.Parameters.AddWithValue("@Identifier", businessPartnerPhone.Identifier);
                                deleteCommand.ExecuteNonQuery();
                                deleteCommand.Parameters.Clear();

                                if (businessPartnerPhone.IsActive)
                                {
                                    businessPartnerPhone.IsSynced = true;

                                    insertCommand = AddCreateParameters(insertCommand, businessPartnerPhone);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from BusinessPartnerPhones WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from BusinessPartnerPhones WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
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

        public BusinessPartnerPhoneResponse Create(BusinessPartnerPhoneViewModel businessPartnerPhone)
        {
            BusinessPartnerPhoneResponse response = new BusinessPartnerPhoneResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, businessPartnerPhone);
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

        public BusinessPartnerPhoneResponse Delete(Guid identifier)
        {
            BusinessPartnerPhoneResponse response = new BusinessPartnerPhoneResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM BusinessPartnerPhones WHERE Identifier = @Identifier";
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

        public BusinessPartnerPhoneResponse SetStatusDeleted(Guid identifier)
        {
            BusinessPartnerPhoneResponse response = new BusinessPartnerPhoneResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "UPDATE BusinessPartnerPhones SET ItemStatus = @ItemStatus WHERE Identifier = @Identifier";
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
