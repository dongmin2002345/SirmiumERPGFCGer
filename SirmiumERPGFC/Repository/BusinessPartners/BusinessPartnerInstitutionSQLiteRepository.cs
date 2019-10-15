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
    public class BusinessPartnerInstitutionSQLiteRepository
    {
        #region SQL

        public static string BusinessPartnerInstitutionTableCreatePart =
               "CREATE TABLE IF NOT EXISTS BusinessPartnerInstitutions " +
               "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
               "ServerId INTEGER NULL, " +
               "Identifier GUID, " +
               "BusinessPartnerId INTEGER NULL, " +
               "BusinessPartnerIdentifier GUID NULL, " +
               "BusinessPartnerCode NVARCHAR(2048) NULL, " +
               "BusinessPartnerName NVARCHAR(2048) NULL, " +
               "BusinessPartnerInternalCode NVARCHAR(2048) NULL, " +
               "BusinessPartnerNameGer NVARCHAR(2048) NULL, " +
                "Code NVARCHAR(2048) NULL, " +
               "Institution NVARCHAR(2048) NULL, " +
               "Username NVARCHAR(2048) NULL, " +
               "Password NVARCHAR(2048) NULL, " +
               "ContactPerson NVARCHAR(2048) NULL, " +
               "Phone NVARCHAR(2048) NULL, " +
               "Fax NVARCHAR(2048) NULL, " +
               "Email NVARCHAR(2048) NULL, " +
               "Note NVARCHAR(2048) NULL, " +
                "ItemStatus INTEGER NOT NULL, " +
               "IsSynced BOOL NULL, " +
               "UpdatedAt DATETIME NULL, " +
               "CreatedById INTEGER NULL, " +
               "CreatedByName NVARCHAR(2048) NULL, " +
               "CompanyId INTEGER NULL, " +
               "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, BusinessPartnerInternalCode, BusinessPartnerNameGer, " +
            "Code, Institution, Username, Password, ContactPerson, Phone, Fax, Email, Note, ItemStatus, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO BusinessPartnerInstitutions " +
            "(Id, ServerId, Identifier, BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, BusinessPartnerInternalCode, BusinessPartnerNameGer, " +
            "Code, Institution, Username, Password, ContactPerson, Phone, Fax, Email, Note, ItemStatus, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @BusinessPartnerId, @BusinessPartnerIdentifier, @BusinessPartnerCode, @BusinessPartnerName, @BusinessPartnerInternalCode, @BusinessPartnerNameGer, " +
            "@Code, @Institution, @Username, @Password, @ContactPerson, @Phone, @Fax, @Email, @Note, @ItemStatus, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods

        private BusinessPartnerInstitutionViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            BusinessPartnerInstitutionViewModel dbEntry = new BusinessPartnerInstitutionViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.BusinessPartner = SQLiteHelper.GetBusinessPartner(query, ref counter);
            dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Institution = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Username = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Password = SQLiteHelper.GetString(query, ref counter);
            dbEntry.ContactPerson = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Phone = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Fax = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Email = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Note = SQLiteHelper.GetString(query, ref counter);
            dbEntry.ItemStatus = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);

            return dbEntry;
        }

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, BusinessPartnerInstitutionViewModel businessPartnerInstitution)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", businessPartnerInstitution.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", businessPartnerInstitution.Identifier);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerId", ((object)businessPartnerInstitution.BusinessPartner.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", ((object)businessPartnerInstitution.BusinessPartner.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerCode", ((object)businessPartnerInstitution.BusinessPartner.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerName", ((object)businessPartnerInstitution.BusinessPartner.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerInternalCode", ((object)businessPartnerInstitution.BusinessPartner.InternalCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerNameGer", ((object)businessPartnerInstitution.BusinessPartner.NameGer) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Code", ((object)businessPartnerInstitution.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Institution", ((object)businessPartnerInstitution.Institution) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Username", ((object)businessPartnerInstitution.Username) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Password", ((object)businessPartnerInstitution.Password) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ContactPerson", ((object)businessPartnerInstitution.ContactPerson) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Phone", ((object)businessPartnerInstitution.Phone) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Fax", ((object)businessPartnerInstitution.Fax) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Email", ((object)businessPartnerInstitution.Email) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Note", ((object)businessPartnerInstitution.Note) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ItemStatus", ((object)businessPartnerInstitution.ItemStatus) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsSynced", businessPartnerInstitution.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)businessPartnerInstitution.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read

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
                        BusinessPartnerInstitutionViewModel dbEntry = Read(query);
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
                        BusinessPartnerInstitutionViewModel dbEntry = Read(query);
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

        #endregion

        #region Sync

        public void Sync(IBusinessPartnerInstitutionService BusinessPartnerInstitutionService, Action<int, int> callback = null)
        {
            try
            {
                SyncBusinessPartnerInstitutionRequest request = new SyncBusinessPartnerInstitutionRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                BusinessPartnerInstitutionListResponse response = BusinessPartnerInstitutionService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.BusinessPartnerInstitutions?.Count ?? 0;
                    List<BusinessPartnerInstitutionViewModel> businessPartnerInstitutionsFromDB = response.BusinessPartnerInstitutions;

                    using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM BusinessPartnerInstitutions WHERE Identifier = @Identifier";

                            SqliteCommand insertCommand = db.CreateCommand();
                            insertCommand.CommandText = SqlCommandInsertPart;

                            foreach (var businessPartnerInstitution in businessPartnerInstitutionsFromDB)
                            {
                                deleteCommand.Parameters.AddWithValue("@Identifier", businessPartnerInstitution.Identifier);
                                deleteCommand.ExecuteNonQuery();
                                deleteCommand.Parameters.Clear();

                                if (businessPartnerInstitution.IsActive)
                                {
                                    businessPartnerInstitution.IsSynced = true;

                                    insertCommand = AddCreateParameters(insertCommand, businessPartnerInstitution);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from BusinessPartnerInstitutions WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from BusinessPartnerInstitutions WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
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

        public BusinessPartnerInstitutionResponse Create(BusinessPartnerInstitutionViewModel businessPartnerInstitution)
        {
            BusinessPartnerInstitutionResponse response = new BusinessPartnerInstitutionResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, businessPartnerInstitution);
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

        public BusinessPartnerInstitutionResponse SetStatusDeleted(Guid identifier)
        {
            BusinessPartnerInstitutionResponse response = new BusinessPartnerInstitutionResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "UPDATE BusinessPartnerInstitutions SET ItemStatus = @ItemStatus WHERE Identifier = @Identifier";
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
