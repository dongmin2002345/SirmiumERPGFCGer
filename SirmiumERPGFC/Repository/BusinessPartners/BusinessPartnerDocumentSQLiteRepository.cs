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
    public class BusinessPartnerDocumentSQLiteRepository
    {
        #region SQL

        public static string BusinessPartnerDocumentTableCreatePart =
                     "CREATE TABLE IF NOT EXISTS BusinessPartnerDocuments " +
                     "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                     "ServerId INTEGER NULL, " +
                     "Identifier GUID, " +
                     "BusinessPartnerId INTEGER NULL, " +
                     "BusinessPartnerIdentifier GUID NULL, " +
                     "BusinessPartnerCode NVARCHAR(48) NULL, " +
                     "BusinessPartnerName NVARCHAR(48) NULL, " +
                     "BusinessPartnerInternalCode NVARCHAR(2048) NULL, " +
                     "BusinessPartnerNameGer NVARCHAR(2048) NULL, " +
                     "Name NVARCHAR(2048), " +
                     "CreateDate DATETIME NULL, " +
                     "Path NVARCHAR(2048) NULL, " +
                     "ItemStatus INTEGER NOT NULL, " +
                     "IsSynced BOOL NULL, " +
                     "UpdatedAt DATETIME NULL, " +
                     "CreatedById INTEGER NULL, " +
                     "CreatedByName NVARCHAR(2048) NULL, " +
                     "CompanyId INTEGER NULL, " +
                     "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, BusinessPartnerId, BusinessPartnerIdentifier, " +
            "BusinessPartnerCode, BusinessPartnerName, BusinessPartnerInternalCode, BusinessPartnerNameGer, Name, CreateDate, Path, ItemStatus, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO BusinessPartnerDocuments " +
            "(Id, ServerId, Identifier, BusinessPartnerId, BusinessPartnerIdentifier, " +
            "BusinessPartnerCode, BusinessPartnerName, BusinessPartnerInternalCode, BusinessPartnerNameGer, Name, CreateDate, Path, ItemStatus, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @BusinessPartnerId, @BusinessPartnerIdentifier, " +
            "@BusinessPartnerCode, @BusinessPartnerName, @BusinessPartnerInternalCode, @BusinessPartnerNameGer, @Name, @CreateDate, @Path, @ItemStatus, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods

        private BusinessPartnerDocumentViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            BusinessPartnerDocumentViewModel dbEntry = new BusinessPartnerDocumentViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.BusinessPartner = SQLiteHelper.GetBusinessPartner(query, ref counter);
            dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
            dbEntry.CreateDate = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.Path = SQLiteHelper.GetString(query, ref counter);
            dbEntry.ItemStatus = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);

            return dbEntry;
        }

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, BusinessPartnerDocumentViewModel BusinessPartnerDocument)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", BusinessPartnerDocument.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", BusinessPartnerDocument.Identifier);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerId", ((object)BusinessPartnerDocument.BusinessPartner.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", ((object)BusinessPartnerDocument.BusinessPartner.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerCode", ((object)BusinessPartnerDocument.BusinessPartner.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerName", ((object)BusinessPartnerDocument.BusinessPartner.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerInternalCode", ((object)BusinessPartnerDocument.BusinessPartner.InternalCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerNameGer", ((object)BusinessPartnerDocument.BusinessPartner.NameGer) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Name", BusinessPartnerDocument.Name);
            insertCommand.Parameters.AddWithValue("@CreateDate", ((object)BusinessPartnerDocument.CreateDate) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Path", ((object)BusinessPartnerDocument.Path) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ItemStatus", ((object)BusinessPartnerDocument.ItemStatus) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsSynced", BusinessPartnerDocument.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)BusinessPartnerDocument.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read

        public BusinessPartnerDocumentListResponse GetBusinessPartnerDocumentsByBusinessPartner(int companyId, Guid BusinessPartnerIdentifier)
        {
            BusinessPartnerDocumentListResponse response = new BusinessPartnerDocumentListResponse();
            List<BusinessPartnerDocumentViewModel> BusinessPartnerDocuments = new List<BusinessPartnerDocumentViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM BusinessPartnerDocuments " +
                        "WHERE BusinessPartnerIdentifier = @BusinessPartnerIdentifier " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC;", db);
                    
                    selectCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", BusinessPartnerIdentifier);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {

                        BusinessPartnerDocumentViewModel dbEntry = Read(query);
                        BusinessPartnerDocuments.Add(dbEntry);
                    }

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.BusinessPartnerDocuments = new List<BusinessPartnerDocumentViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.BusinessPartnerDocuments = BusinessPartnerDocuments;
            return response;
        }

        public BusinessPartnerDocumentResponse GetBusinessPartnerDocument(Guid identifier)
        {
            BusinessPartnerDocumentResponse response = new BusinessPartnerDocumentResponse();
            BusinessPartnerDocumentViewModel BusinessPartnerDocument = new BusinessPartnerDocumentViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM BusinessPartnerDocuments " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        BusinessPartnerDocumentViewModel dbEntry = Read(query);
                        BusinessPartnerDocument = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.BusinessPartnerDocument = new BusinessPartnerDocumentViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.BusinessPartnerDocument = BusinessPartnerDocument;
            return response;
        }

        #endregion

        #region Sync

        public void Sync(IBusinessPartnerDocumentService BusinessPartnerDocumentService, Action<int, int> callback = null)
        {
            try
            {
                SyncBusinessPartnerDocumentRequest request = new SyncBusinessPartnerDocumentRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                BusinessPartnerDocumentListResponse response = BusinessPartnerDocumentService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.BusinessPartnerDocuments?.Count ?? 0;
                    List<BusinessPartnerDocumentViewModel> businessPartnerDocumentsFromDB = response.BusinessPartnerDocuments;

                    using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM BusinessPartnerDocuments WHERE Identifier = @Identifier";

                            SqliteCommand insertCommand = db.CreateCommand();
                            insertCommand.CommandText = SqlCommandInsertPart;

                            foreach (var businessPartnerDocument in businessPartnerDocumentsFromDB)
                            {
                                deleteCommand.Parameters.AddWithValue("@Identifier", businessPartnerDocument.Identifier);
                                deleteCommand.ExecuteNonQuery();
                                deleteCommand.Parameters.Clear();

                                if (businessPartnerDocument.IsActive)
                                {
                                    businessPartnerDocument.IsSynced = true;

                                    insertCommand = AddCreateParameters(insertCommand, businessPartnerDocument);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from BusinessPartnerDocuments WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from BusinessPartnerDocuments WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
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

        public BusinessPartnerDocumentResponse Create(BusinessPartnerDocumentViewModel BusinessPartnerDocument)
        {
            BusinessPartnerDocumentResponse response = new BusinessPartnerDocumentResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, BusinessPartnerDocument);
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

        public BusinessPartnerDocumentResponse Delete(Guid identifier)
        {
            BusinessPartnerDocumentResponse response = new BusinessPartnerDocumentResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM BusinessPartnerDocuments WHERE Identifier = @Identifier";
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

        public BusinessPartnerDocumentResponse SetStatusDeleted(Guid identifier)
        {
            BusinessPartnerDocumentResponse response = new BusinessPartnerDocumentResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "UPDATE BusinessPartnerDocuments SET ItemStatus = @ItemStatus WHERE Identifier = @Identifier";
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
