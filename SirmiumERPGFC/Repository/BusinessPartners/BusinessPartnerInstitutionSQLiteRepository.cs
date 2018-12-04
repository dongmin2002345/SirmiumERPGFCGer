using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
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
    public class BusinessPartnerInstitutionSQLiteRepository
    {
        public static string BusinessPartnerInstitutionTableCreatePart =
               "CREATE TABLE IF NOT EXISTS BusinessPartnerInstitutions " +
               "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
               "ServerId INTEGER NULL, " +
               "Identifier GUID, " +
               "BusinessPartnerId INTEGER NULL, " +
               "BusinessPartnerIdentifier GUID NULL, " +
               "BusinessPartnerCode NVARCHAR(2048) NULL, " +
               "BusinessPartnerName NVARCHAR(2048) NULL, " +
               "Institution NVARCHAR(2048) NULL, " +
               "Username NVARCHAR(2048) NULL, " +
               "Password NVARCHAR(2048) NULL, " +
               "ContactPerson NVARCHAR(2048) NULL, " +
               "Phone NVARCHAR(2048) NULL, " +
               "Fax NVARCHAR(2048) NULL, " +
               "Email NVARCHAR(2048) NULL, " +
               "IsSynced BOOL NULL, " +
               "UpdatedAt DATETIME NULL, " +
               "CreatedById INTEGER NULL, " +
               "CreatedByName NVARCHAR(2048) NULL, " +
               "CompanyId INTEGER NULL, " +
               "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
            "Institution, Username, Password, ContactPerson, Phone, Fax, Email, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO BusinessPartnerInstitutions " +
            "(Id, ServerId, Identifier, BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
            "Institution, Username, Password, ContactPerson, Phone, Fax, Email, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @BusinessPartnerId, @BusinessPartnerIdentifier, @BusinessPartnerCode, @BusinessPartnerName, " +
            "@Institution, @Username, @Password, @ContactPerson, @Phone, @Fax, @Email, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        public BusinessPartnerInstitutionListResponse GetBusinessPartnerInstitutionsByBusinessPartner(int companyId, Guid businessPartnerIdentifier)
        {
            BusinessPartnerInstitutionListResponse response = new BusinessPartnerInstitutionListResponse();
            List<BusinessPartnerInstitutionViewModel> businessPartnerInstitutions = new List<BusinessPartnerInstitutionViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM BusinessPartnerInstitutions " +
                        "WHERE BusinessPartnerIdentifier = @BusinessPartnerIdentifier " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC;", db);
                    selectCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", businessPartnerIdentifier);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        BusinessPartnerInstitutionViewModel dbEntry = new BusinessPartnerInstitutionViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.BusinessPartner = SQLiteHelper.GetBusinessPartner(query, ref counter);
                        dbEntry.Institution = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Username = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Password = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.ContactPerson = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Phone = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Fax = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Email = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        businessPartnerInstitutions.Add(dbEntry);
                    }

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.BusinessPartnerInstitutions = new List<BusinessPartnerInstitutionViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.BusinessPartnerInstitutions = businessPartnerInstitutions;
            return response;
        }

        public BusinessPartnerInstitutionResponse GetBusinessPartnerInstitution(Guid identifier)
        {
            BusinessPartnerInstitutionResponse response = new BusinessPartnerInstitutionResponse();
            BusinessPartnerInstitutionViewModel businessPartnerInstitution = new BusinessPartnerInstitutionViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM BusinessPartnerInstitutions " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        int counter = 0;
                        BusinessPartnerInstitutionViewModel dbEntry = new BusinessPartnerInstitutionViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.BusinessPartner = SQLiteHelper.GetBusinessPartner(query, ref counter);
                        dbEntry.Institution = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Username = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Password = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.ContactPerson = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Phone = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Fax = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Email = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        businessPartnerInstitution = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.BusinessPartnerInstitution = new BusinessPartnerInstitutionViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.BusinessPartnerInstitution = businessPartnerInstitution;
            return response;
        }

        public void Sync(IBusinessPartnerInstitutionService BusinessPartnerInstitutionService)
        {
            SyncBusinessPartnerInstitutionRequest request = new SyncBusinessPartnerInstitutionRequest();
            request.CompanyId = MainWindow.CurrentCompanyId;
            request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

            BusinessPartnerInstitutionListResponse response = BusinessPartnerInstitutionService.Sync(request);
            if (response.Success)
            {
                List<BusinessPartnerInstitutionViewModel> BusinessPartnerInstitutionsFromDB = response.BusinessPartnerInstitutions;
                foreach (var BusinessPartnerInstitution in BusinessPartnerInstitutionsFromDB.OrderBy(x => x.Id))
                {
                    Delete(BusinessPartnerInstitution.Identifier);
                    if (BusinessPartnerInstitution.IsActive)
                    {
                        BusinessPartnerInstitution.IsSynced = true;
                        Create(BusinessPartnerInstitution);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from BusinessPartnerInstitutions WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from BusinessPartnerInstitutions WHERE CompanyId = @CompanyId", db);
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

        public BusinessPartnerInstitutionResponse Create(BusinessPartnerInstitutionViewModel businessPartnerInstitution)
        {
            BusinessPartnerInstitutionResponse response = new BusinessPartnerInstitutionResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = SqlCommandInsertPart;

                insertCommand.Parameters.AddWithValue("@ServerId", businessPartnerInstitution.Id);
                insertCommand.Parameters.AddWithValue("@Identifier", businessPartnerInstitution.Identifier);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerId", ((object)businessPartnerInstitution.BusinessPartner.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", ((object)businessPartnerInstitution.BusinessPartner.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerCode", ((object)businessPartnerInstitution.BusinessPartner.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerName", ((object)businessPartnerInstitution.BusinessPartner.Name) ?? businessPartnerInstitution.BusinessPartner.NameGer);
                insertCommand.Parameters.AddWithValue("@Institution", ((object)businessPartnerInstitution.Institution) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Username", ((object)businessPartnerInstitution.Username) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Password", ((object)businessPartnerInstitution.Password) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ContactPerson", ((object)businessPartnerInstitution.ContactPerson) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Phone", ((object)businessPartnerInstitution.Phone) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Fax", ((object)businessPartnerInstitution.Fax) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Email", ((object)businessPartnerInstitution.Email) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IsSynced", businessPartnerInstitution.IsSynced);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)businessPartnerInstitution.UpdatedAt) ?? DBNull.Value);
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

        public BusinessPartnerInstitutionResponse UpdateSyncStatus(Guid identifier, int serverId, string code, DateTime? updatedAt, bool isSynced)
        {
            BusinessPartnerInstitutionResponse response = new BusinessPartnerInstitutionResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "UPDATE BusinessPartnerInstitutions SET " +
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

        public BusinessPartnerInstitutionResponse Delete(Guid identifier)
        {
            BusinessPartnerInstitutionResponse response = new BusinessPartnerInstitutionResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "DELETE FROM BusinessPartnerInstitutions WHERE Identifier = @Identifier";
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

        public BusinessPartnerInstitutionResponse DeleteAll()
        {
            BusinessPartnerInstitutionResponse response = new BusinessPartnerInstitutionResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM BusinessPartnerInstitutions";
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
