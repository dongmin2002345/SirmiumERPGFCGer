using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.Phonebook;
using ServiceInterfaces.Gloabals;
using ServiceInterfaces.Messages.Common.Phonebooks;
using ServiceInterfaces.ViewModels.Common.Phonebooks;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.Phonebooks
{
    public class PhonebookDocumentSQLiteRepository
    {
        #region SQL

        public static string PhonebookDocumentTableCreatePart =
                 "CREATE TABLE IF NOT EXISTS PhonebookDocuments " +
                 "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                 "ServerId INTEGER NULL, " +
                 "Identifier GUID, " +
                 "PhonebookId INTEGER NULL, " +
                 "PhonebookIdentifier GUID NULL, " +
                 "PhonebookCode NVARCHAR(48) NULL, " +
                 "PhonebookName NVARCHAR(48) NULL, " +
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
            "SELECT ServerId, Identifier, PhonebookId, PhonebookIdentifier, " +
            "PhonebookCode, PhonebookName, Name, CreateDate, Path, ItemStatus, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO PhonebookDocuments " +
            "(Id, ServerId, Identifier, PhonebookId, PhonebookIdentifier, " +
            "PhonebookCode, PhonebookName, Name, CreateDate, Path, ItemStatus, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @PhonebookId, @PhonebookIdentifier, " +
            "@PhonebookCode, @PhonebookName,@Name, @CreateDate, @Path, @ItemStatus, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods
        private static PhonebookDocumentViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            PhonebookDocumentViewModel dbEntry = new PhonebookDocumentViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.Phonebook = SQLiteHelper.GetPhonebook(query, ref counter);
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

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, PhonebookDocumentViewModel PhonebookDocument)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", PhonebookDocument.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", PhonebookDocument.Identifier);
            insertCommand.Parameters.AddWithValue("@PhonebookId", ((object)PhonebookDocument.Phonebook.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PhonebookIdentifier", ((object)PhonebookDocument.Phonebook.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PhonebookCode", ((object)PhonebookDocument.Phonebook.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PhonebookName", ((object)PhonebookDocument.Phonebook.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Name", ((object)PhonebookDocument.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreateDate", ((object)PhonebookDocument.CreateDate) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Path", ((object)PhonebookDocument.Path) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ItemStatus", ((object)PhonebookDocument.ItemStatus) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsSynced", PhonebookDocument.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)PhonebookDocument.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read

        public PhonebookDocumentListResponse GetPhonebookDocumentsByPhonebook(int companyId, Guid PhonebookIdentifier)
        {
            PhonebookDocumentListResponse response = new PhonebookDocumentListResponse();
            List<PhonebookDocumentViewModel> PhonebookDocuments = new List<PhonebookDocumentViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM PhonebookDocuments " +
                        "WHERE PhonebookIdentifier = @PhonebookIdentifier " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC;", db);

                    selectCommand.Parameters.AddWithValue("@PhonebookIdentifier", PhonebookIdentifier);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {

                        PhonebookDocumentViewModel dbEntry = Read(query);
                        PhonebookDocuments.Add(dbEntry);
                    }

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.PhonebookDocuments = new List<PhonebookDocumentViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.PhonebookDocuments = PhonebookDocuments;
            return response;
        }

        public PhonebookDocumentResponse GetPhonebookDocument(Guid identifier)
        {
            PhonebookDocumentResponse response = new PhonebookDocumentResponse();
            PhonebookDocumentViewModel PhonebookDocument = new PhonebookDocumentViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM PhonebookDocuments " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        PhonebookDocumentViewModel dbEntry = Read(query);
                        PhonebookDocument = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.PhonebookDocument = new PhonebookDocumentViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.PhonebookDocument = PhonebookDocument;
            return response;
        }

        public void Sync(IPhonebookDocumentService PhonebookDocumentService, Action<int, int> callback = null)
        {
            try
            {
                SyncPhonebookDocumentRequest request = new SyncPhonebookDocumentRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                PhonebookDocumentListResponse response = PhonebookDocumentService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.PhonebookDocuments?.Count ?? 0;
                    List<PhonebookDocumentViewModel> items = response.PhonebookDocuments;

                    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM PhonebookDocuments WHERE Identifier = @Identifier";

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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from PhonebookDocuments WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from PhonebookDocuments WHERE CompanyId = @CompanyId", db);
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

        public PhonebookDocumentResponse Create(PhonebookDocumentViewModel PhonebookDocument)
        {
            PhonebookDocumentResponse response = new PhonebookDocumentResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, PhonebookDocument);
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

        public PhonebookDocumentResponse Delete(Guid identifier)
        {
            PhonebookDocumentResponse response = new PhonebookDocumentResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM PhonebookDocuments WHERE Identifier = @Identifier";
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

        public PhonebookDocumentResponse SetStatusDeleted(Guid identifier)
        {
            PhonebookDocumentResponse response = new PhonebookDocumentResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "UPDATE PhonebookDocuments SET ItemStatus = @ItemStatus WHERE Identifier = @Identifier";
                insertCommand.Parameters.AddWithValue("@ItemStatus", ItemStatus.Deleted);
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

        
        #endregion
    }
}
