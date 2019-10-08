using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;

namespace SirmiumERPGFC.Repository.BusinessPartners
{
    public class BusinessPartnerNoteSQLiteRepository
    {
        #region SQL

        public static string BusinessPartnerNoteTableCreatePart =
                     "CREATE TABLE IF NOT EXISTS BusinessPartnerNotes " +
                     "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                     "ServerId INTEGER NULL, " +
                     "Identifier GUID, " +
                     "BusinessPartnerId INTEGER NULL, " +
                     "BusinessPartnerIdentifier GUID NULL, " +
                     "BusinessPartnerCode NVARCHAR(48) NULL, " +
                     "BusinessPartnerName NVARCHAR(48) NULL, " +
                     "BusinessPartnerInternalCode NVARCHAR(2048) NULL, " +
                     "BusinessPartnerNameGer NVARCHAR(2048) NULL, " +
                     "Note NVARCHAR(2048), " +
                     "NoteDate DATETIME NULL, " +
                     "IsSynced BOOL NULL, " +
                     "UpdatedAt DATETIME NULL, " +
                     "CreatedById INTEGER NULL, " +
                     "CreatedByName NVARCHAR(2048) NULL, " +
                     "CompanyId INTEGER NULL, " +
                     "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, BusinessPartnerId, BusinessPartnerIdentifier, " +
            "BusinessPartnerCode, BusinessPartnerName, BusinessPartnerInternalCode, BusinessPartnerNameGer, Note, NoteDate, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO BusinessPartnerNotes " +
            "(Id, ServerId, Identifier, BusinessPartnerId, BusinessPartnerIdentifier, " +
            "BusinessPartnerCode, BusinessPartnerName, BusinessPartnerInternalCode, BusinessPartnerNameGer, Note, NoteDate, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @BusinessPartnerId, @BusinessPartnerIdentifier, " +
            "@BusinessPartnerCode, @BusinessPartnerName, @BusinessPartnerInternalCode, @BusinessPartnerNameGer, @Note, @NoteDate, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods

        private BusinessPartnerNoteViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            BusinessPartnerNoteViewModel dbEntry = new BusinessPartnerNoteViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.BusinessPartner = SQLiteHelper.GetBusinessPartner(query, ref counter);
            dbEntry.Note = SQLiteHelper.GetString(query, ref counter);
            dbEntry.NoteDate = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);

            return dbEntry;
        }

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, BusinessPartnerNoteViewModel BusinessPartnerNote)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", BusinessPartnerNote.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", BusinessPartnerNote.Identifier);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerId", ((object)BusinessPartnerNote.BusinessPartner.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", ((object)BusinessPartnerNote.BusinessPartner.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerCode", ((object)BusinessPartnerNote.BusinessPartner.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerName", ((object)BusinessPartnerNote.BusinessPartner.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerInternalCode", ((object)BusinessPartnerNote.BusinessPartner.InternalCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerNameGer", ((object)BusinessPartnerNote.BusinessPartner.NameGer) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Note", BusinessPartnerNote.Note);
            insertCommand.Parameters.AddWithValue("@NoteDate", ((object)BusinessPartnerNote.NoteDate) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsSynced", BusinessPartnerNote.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)BusinessPartnerNote.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read

        public BusinessPartnerNoteListResponse GetBusinessPartnerNotesByBusinessPartner(int companyId, Guid BusinessPartnerIdentifier)
        {
            BusinessPartnerNoteListResponse response = new BusinessPartnerNoteListResponse();
            List<BusinessPartnerNoteViewModel> BusinessPartnerNotes = new List<BusinessPartnerNoteViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM BusinessPartnerNotes " +
                        "WHERE BusinessPartnerIdentifier = @BusinessPartnerIdentifier " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC;", db);
                    
                    selectCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", BusinessPartnerIdentifier);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                        BusinessPartnerNotes.Add(Read(query));
                    

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.BusinessPartnerNotes = new List<BusinessPartnerNoteViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.BusinessPartnerNotes = BusinessPartnerNotes;
            return response;
        }

        public BusinessPartnerNoteResponse GetBusinessPartnerNote(Guid identifier)
        {
            BusinessPartnerNoteResponse response = new BusinessPartnerNoteResponse();
            BusinessPartnerNoteViewModel BusinessPartnerNote = new BusinessPartnerNoteViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM BusinessPartnerNotes " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                        BusinessPartnerNote = Read(query);
                    
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.BusinessPartnerNote = new BusinessPartnerNoteViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.BusinessPartnerNote = BusinessPartnerNote;
            return response;
        }

        public BusinessPartnerNoteListResponse GetUnSyncedNotes(int companyId)
        {
            BusinessPartnerNoteListResponse response = new BusinessPartnerNoteListResponse();
            List<BusinessPartnerNoteViewModel> viewModels = new List<BusinessPartnerNoteViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM  BusinessPartnerNotes " +
                        "WHERE CompanyId = @CompanyId AND IsSynced = 0 " +
                        "ORDER BY Id DESC;", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                        viewModels.Add(Read(query));
                    

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.BusinessPartnerNotes = new List<BusinessPartnerNoteViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.BusinessPartnerNotes = viewModels;
            return response;
        }

        #endregion

        #region Sync

        public void Sync(IBusinessPartnerNoteService BusinessPartnerNoteService, Action<int, int> callback = null)
        {
            try
            {
                SyncBusinessPartnerNoteRequest request = new SyncBusinessPartnerNoteRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                BusinessPartnerNoteListResponse response = BusinessPartnerNoteService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.BusinessPartnerNotes?.Count ?? 0;
                    List<BusinessPartnerNoteViewModel> businessPartnerNotesFromDB = response.BusinessPartnerNotes;

                    using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM BusinessPartnerNotes WHERE Identifier = @Identifier";

                            SqliteCommand insertCommand = db.CreateCommand();
                            insertCommand.CommandText = SqlCommandInsertPart;

                            foreach (var businessPartnerNote in businessPartnerNotesFromDB)
                            {
                                deleteCommand.Parameters.AddWithValue("@Identifier", businessPartnerNote.Identifier);
                                deleteCommand.ExecuteNonQuery();
                                deleteCommand.Parameters.Clear();

                                if (businessPartnerNote.IsActive)
                                {
                                    businessPartnerNote.IsSynced = true;

                                    insertCommand = AddCreateParameters(insertCommand, businessPartnerNote);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from BusinessPartnerNotes WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from BusinessPartnerNotes WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
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

        public BusinessPartnerNoteResponse Create(BusinessPartnerNoteViewModel BusinessPartnerNote)
        {
            BusinessPartnerNoteResponse response = new BusinessPartnerNoteResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, BusinessPartnerNote);
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

        public BusinessPartnerNoteResponse Delete(Guid identifier)
        {
            BusinessPartnerNoteResponse response = new BusinessPartnerNoteResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM BusinessPartnerNotes WHERE Identifier = @Identifier";
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

        public BusinessPartnerNoteResponse DeleteAll()
        {
            BusinessPartnerNoteResponse response = new BusinessPartnerNoteResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM BusinessPartnerNotes";
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
