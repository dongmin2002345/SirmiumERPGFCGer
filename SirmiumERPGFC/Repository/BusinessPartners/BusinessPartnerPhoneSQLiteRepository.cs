using Microsoft.Data.Sqlite;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.BusinessPartners
{
    public class BusinessPartnerPhoneSQLiteRepository
    {
        public static string BusinessPartnerPhoneTableCreatePart =
            "CREATE TABLE IF NOT EXISTS BusinessPartnerPhones " +
            "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
            "ServerId INTEGER NULL, " +
            "Identifier GUID, " +
            "BusinessPartnerId INTEGER NULL, " +
            "BusinessPartnerIdentifier GUID NULL, " +
            "BusinessPartnerCode NVARCHAR(2048) NULL, " +
            "BusinessPartnerName NVARCHAR(2048) NULL, " +
            "Phone NVARCHAR(2048) NULL, " +
            "Mobile NVARCHAR(2048) NULL, " +
            "Fax NVARCHAR(2048) NULL, " +
            "Email NVARCHAR(2048) NULL, " +
            "ContactPersonFirstName NVARCHAR(2048) NULL, " +
            "ContactPersonLastName NVARCHAR(2048) NULL, " +
            "Birthday DATETIME NULL, " + 
            "Description NVARCHAR(2048) NULL, " +
            "IsSynced BOOL NULL, " +
            "UpdatedAt DATETIME NULL, " +
            "CreatedById INTEGER NULL, " +
            "CreatedByName NVARCHAR(2048) NULL, " +
            "CompanyId INTEGER NULL, " +
            "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
            "Phone, Mobile, Fax, Email, ContactPersonFirstName, ContactPersonLastName, Birthday, Description, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO BusinessPartnerPhones " +
            "(Id, ServerId, Identifier, BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
            "Phone, Mobile, Fax, Email, ContactPersonFirstName, ContactPersonLastName, Birthday, Description, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @BusinessPartnerId, @BusinessPartnerIdentifier, @BusinessPartnerCode, @BusinessPartnerName, " +
            "@Phone, @Mobile, @Fax, @Email, @ContactPersonFirstName, @ContactPersonLastName, @Birthday, @Description, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

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
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
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

        public BusinessPartnerPhoneListResponse GetUnSyncedBusinessPartnerPhones(int companyId)
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
                        "WHERE CompanyId = @CompanyId AND IsSynced = 0 " +
                        "ORDER BY Id DESC;", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
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
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
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
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
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

        public DateTime? GetLastUpdatedAt(int companyId)
        {
            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from BusinessPartnerPhones WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from BusinessPartnerPhones WHERE CompanyId = @CompanyId", db);
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

        public BusinessPartnerPhoneResponse Create(BusinessPartnerPhoneViewModel businessPartnerPhone)
        {
            BusinessPartnerPhoneResponse response = new BusinessPartnerPhoneResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = SqlCommandInsertPart;

                insertCommand.Parameters.AddWithValue("@ServerId", businessPartnerPhone.Id);
                insertCommand.Parameters.AddWithValue("@Identifier", businessPartnerPhone.Identifier);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerId", ((object)businessPartnerPhone.BusinessPartner.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", ((object)businessPartnerPhone.BusinessPartner.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerCode", ((object)businessPartnerPhone.BusinessPartner.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerName", ((object)businessPartnerPhone.BusinessPartner.Name) ?? businessPartnerPhone.BusinessPartner.NameGer);
                insertCommand.Parameters.AddWithValue("@Phone", ((object)businessPartnerPhone.Phone) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Mobile", ((object)businessPartnerPhone.Mobile) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Fax", ((object)businessPartnerPhone.Fax) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Email", ((object)businessPartnerPhone.Email) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ContactPersonFirstName", ((object)businessPartnerPhone.ContactPersonFirstName) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ContactPersonLastName", ((object)businessPartnerPhone.ContactPersonLastName) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Birthday", ((object)businessPartnerPhone.Birthday) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Description", ((object)businessPartnerPhone.Description) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IsSynced", businessPartnerPhone.IsSynced);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)businessPartnerPhone.UpdatedAt) ?? DBNull.Value);
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

        public BusinessPartnerPhoneResponse UpdateSyncStatus(Guid identifier, int serverId, string code, DateTime? updatedAt, bool isSynced)
        {
            BusinessPartnerPhoneResponse response = new BusinessPartnerPhoneResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "UPDATE BusinessPartnerPhones SET " +
                    "IsSynced = @IsSynced, " +
                    "Code = @Code, " +
                    "UpdatedAT = @UpdatedAt, " +
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

        public BusinessPartnerPhoneResponse Delete(Guid identifier)
        {
            BusinessPartnerPhoneResponse response = new BusinessPartnerPhoneResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "DELETE FROM BusinessPartnerPhones WHERE Identifier = @Identifier";
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

        public BusinessPartnerPhoneResponse DeleteAll()
        {
            BusinessPartnerPhoneResponse response = new BusinessPartnerPhoneResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM BusinessPartnerPhones";
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
