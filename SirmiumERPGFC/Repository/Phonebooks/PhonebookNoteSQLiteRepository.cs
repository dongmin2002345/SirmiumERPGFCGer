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
    public class PhonebookNoteSQLiteRepository
    {
        #region SQL
        public static string PhonebookNoteTableCreatePart =
                        "CREATE TABLE IF NOT EXISTS PhonebookNotes " +
                        "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                        "ServerId INTEGER NULL, " +
                        "Identifier GUID, " +
                        "PhonebookId INTEGER NULL, " +
                        "PhonebookIdentifier GUID NULL, " +
                        "PhonebookCode NVARCHAR(48) NULL, " +
                        "PhonebookName NVARCHAR(48) NULL, " +
                        "Note NVARCHAR(2048), " +
                        "NoteDate DATETIME NULL, " +
                        "ItemStatus INTEGER NOT NULL, " +
                        "IsSynced BOOL NULL, " +
                        "UpdatedAt DATETIME NULL, " +
                        "CreatedById INTEGER NULL, " +
                        "CreatedByName NVARCHAR(2048) NULL, " +
                        "CompanyId INTEGER NULL, " +
                        "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, PhonebookId, PhonebookIdentifier, " +
            "PhonebookCode, PhonebookName, Note, NoteDate, ItemStatus, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO PhonebookNotes " +
            "(Id, ServerId, Identifier, PhonebookId, PhonebookIdentifier, " +
            "PhonebookCode, PhonebookName, Note, NoteDate, ItemStatus, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @PhonebookId, @PhonebookIdentifier, " +
            "@PhonebookCode, @PhonebookName, @Note, @NoteDate, @ItemStatus, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods
        private static PhonebookNoteViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            PhonebookNoteViewModel dbEntry = new PhonebookNoteViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.Phonebook = SQLiteHelper.GetPhonebook(query, ref counter);
            dbEntry.Note = SQLiteHelper.GetString(query, ref counter);
            dbEntry.NoteDate = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.ItemStatus = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
            return dbEntry;
        }

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, PhonebookNoteViewModel PhonebookNote)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", PhonebookNote.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", PhonebookNote.Identifier);
            insertCommand.Parameters.AddWithValue("@PhonebookId", ((object)PhonebookNote.Phonebook.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PhonebookIdentifier", ((object)PhonebookNote.Phonebook.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PhonebookCode", ((object)PhonebookNote.Phonebook.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PhonebookName", ((object)PhonebookNote.Phonebook.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Note", ((object)PhonebookNote.Note) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@NoteDate", ((object)PhonebookNote.NoteDate) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ItemStatus", ((object)PhonebookNote.ItemStatus) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsSynced", PhonebookNote.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)PhonebookNote.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read

        public PhonebookNoteListResponse GetPhonebookNotesByPhonebook(int companyId, Guid PhonebookIdentifier)
        {
            PhonebookNoteListResponse response = new PhonebookNoteListResponse();
            List<PhonebookNoteViewModel> PhonebookNotes = new List<PhonebookNoteViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM PhonebookNotes " +
                        "WHERE PhonebookIdentifier = @PhonebookIdentifier " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC;", db);

                    selectCommand.Parameters.AddWithValue("@PhonebookIdentifier", PhonebookIdentifier);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {

                        PhonebookNoteViewModel dbEntry = Read(query);
                        PhonebookNotes.Add(dbEntry);
                    }

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.PhonebookNotes = new List<PhonebookNoteViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.PhonebookNotes = PhonebookNotes;
            return response;
        }

        public PhonebookNoteResponse GetPhonebookNote(Guid identifier)
        {
            PhonebookNoteResponse response = new PhonebookNoteResponse();
            PhonebookNoteViewModel PhonebookNote = new PhonebookNoteViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM PhonebookNotes " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        PhonebookNoteViewModel dbEntry = Read(query);
                        PhonebookNote = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.PhonebookNote = new PhonebookNoteViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.PhonebookNote = PhonebookNote;
            return response;
        }

        #endregion

        #region Sync
        public void Sync(IPhonebookNoteService PhonebookNoteservice, Action<int, int> callback = null)
        {
            try
            {
                SyncPhonebookNoteRequest request = new SyncPhonebookNoteRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                PhonebookNoteListResponse response = PhonebookNoteservice.Sync(request);
                if (response.Success)
                {
                    toSync = response?.PhonebookNotes?.Count ?? 0;
                    List<PhonebookNoteViewModel> items = response.PhonebookNotes;

                    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM PhonebookNotes WHERE Identifier = @Identifier";

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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from PhonebookNotes WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from PhonebookNotes WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
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

        public PhonebookNoteResponse Create(PhonebookNoteViewModel PhonebookNote)
        {
            PhonebookNoteResponse response = new PhonebookNoteResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, PhonebookNote);
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

        public PhonebookNoteResponse Delete(Guid identifier)
        {
            PhonebookNoteResponse response = new PhonebookNoteResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM PhonebookNotes WHERE Identifier = @Identifier";
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

      
        public PhonebookNoteResponse SetStatusDeleted(Guid identifier)
        {
            PhonebookNoteResponse response = new PhonebookNoteResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "UPDATE PhonebookNotes SET ItemStatus = @ItemStatus WHERE Identifier = @Identifier";
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
