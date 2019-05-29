using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.InputInvoices;
using ServiceInterfaces.Messages.Common.InputInvoices;
using ServiceInterfaces.ViewModels.Common.InputInvoices;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.InputInvoices
{
    public class InputInvoiceNoteSQLiteRepository
    {
        public static string InputInvoiceNoteTableCreatePart =
                        "CREATE TABLE IF NOT EXISTS InputInvoiceNotes " +
                        "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                        "ServerId INTEGER NULL, " +
                        "Identifier GUID, " +
                        "InputInvoiceId INTEGER NULL, " +
                        "InputInvoiceIdentifier GUID NULL, " +
                        "InputInvoiceCode NVARCHAR(48) NULL, " +
                        "Note NVARCHAR(2048), " +
                        "NoteDate DATETIME NULL, " +
                        "IsSynced BOOL NULL, " +
                        "UpdatedAt DATETIME NULL, " +
                        "CreatedById INTEGER NULL, " +
                        "CreatedByName NVARCHAR(2048) NULL, " +
                        "CompanyId INTEGER NULL, " +
                        "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, InputInvoiceId, InputInvoiceIdentifier, " +
            "InputInvoiceCode, Note, NoteDate, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO InputInvoiceNotes " +
            "(Id, ServerId, Identifier, InputInvoiceId, InputInvoiceIdentifier, " +
            "InputInvoiceCode, Note, NoteDate, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @InputInvoiceId, @InputInvoiceIdentifier, " +
            "@InputInvoiceCode, @Note, @NoteDate, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        public InputInvoiceNoteListResponse GetInputInvoiceNotesByInputInvoice(int companyId, Guid InputInvoiceIdentifier)
        {
            InputInvoiceNoteListResponse response = new InputInvoiceNoteListResponse();
            List<InputInvoiceNoteViewModel> InputInvoiceNotes = new List<InputInvoiceNoteViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM InputInvoiceNotes " +
                        "WHERE InputInvoiceIdentifier = @InputInvoiceIdentifier " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC;", db);
                    selectCommand.Parameters.AddWithValue("@InputInvoiceIdentifier", InputInvoiceIdentifier);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        InputInvoiceNoteViewModel dbEntry = new InputInvoiceNoteViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.InputInvoice = SQLiteHelper.GetInputInvoice(query, ref counter);
                        dbEntry.Note = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.NoteDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        InputInvoiceNotes.Add(dbEntry);
                    }

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.InputInvoiceNotes = new List<InputInvoiceNoteViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.InputInvoiceNotes = InputInvoiceNotes;
            return response;
        }

        public InputInvoiceNoteResponse GetInputInvoiceNote(Guid identifier)
        {
            InputInvoiceNoteResponse response = new InputInvoiceNoteResponse();
            InputInvoiceNoteViewModel InputInvoiceNote = new InputInvoiceNoteViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM InputInvoiceNotes " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        int counter = 0;
                        InputInvoiceNoteViewModel dbEntry = new InputInvoiceNoteViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.InputInvoice = SQLiteHelper.GetInputInvoice(query, ref counter);
                        dbEntry.Note = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.NoteDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        InputInvoiceNote = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.InputInvoiceNote = new InputInvoiceNoteViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.InputInvoiceNote = InputInvoiceNote;
            return response;
        }

        public InputInvoiceNoteListResponse GetUnSyncedNotes(int companyId)
        {
            InputInvoiceNoteListResponse response = new InputInvoiceNoteListResponse();
            List<InputInvoiceNoteViewModel> viewModels = new List<InputInvoiceNoteViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM  InputInvoiceNotes " +
                        "WHERE CompanyId = @CompanyId AND IsSynced = 0 " +
                        "ORDER BY Id DESC;", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        InputInvoiceNoteViewModel dbEntry = new InputInvoiceNoteViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.InputInvoice = SQLiteHelper.GetInputInvoice(query, ref counter);
                        dbEntry.Note = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.NoteDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        viewModels.Add(dbEntry);
                    }

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.InputInvoiceNotes = new List<InputInvoiceNoteViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.InputInvoiceNotes = viewModels;
            return response;
        }

        public void Sync(IInputInvoiceNoteService InputInvoiceNoteService)
        {
            var unSynced = GetUnSyncedNotes(MainWindow.CurrentCompanyId);
            SyncInputInvoiceNoteRequest request = new SyncInputInvoiceNoteRequest();
            request.CompanyId = MainWindow.CurrentCompanyId;
            request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

            InputInvoiceNoteListResponse response = InputInvoiceNoteService.Sync(request);
            if (response.Success)
            {
                List<InputInvoiceNoteViewModel> InputInvoiceNotesFromDB = response.InputInvoiceNotes;
                foreach (var InputInvoiceNote in InputInvoiceNotesFromDB.OrderBy(x => x.Id))
                {
                    Delete(InputInvoiceNote.Identifier);
                    if (InputInvoiceNote.IsActive)
                    {
                        InputInvoiceNote.IsSynced = true;
                        Create(InputInvoiceNote);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from InputInvoiceNotes WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from InputInvoiceNotes WHERE CompanyId = @CompanyId", db);
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

        public InputInvoiceNoteResponse Create(InputInvoiceNoteViewModel InputInvoiceNote)
        {
            InputInvoiceNoteResponse response = new InputInvoiceNoteResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = SqlCommandInsertPart;

                insertCommand.Parameters.AddWithValue("@ServerId", InputInvoiceNote.Id);
                insertCommand.Parameters.AddWithValue("@Identifier", InputInvoiceNote.Identifier);
                insertCommand.Parameters.AddWithValue("@InputInvoiceId", ((object)InputInvoiceNote.InputInvoice.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@InputInvoiceIdentifier", ((object)InputInvoiceNote.InputInvoice.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@InputInvoiceCode", ((object)InputInvoiceNote.InputInvoice.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Note", InputInvoiceNote.Note);
                insertCommand.Parameters.AddWithValue("@NoteDate", ((object)InputInvoiceNote.NoteDate) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IsSynced", InputInvoiceNote.IsSynced);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)InputInvoiceNote.UpdatedAt) ?? DBNull.Value);
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

        public InputInvoiceNoteResponse UpdateSyncStatus(Guid identifier, string code, DateTime? updatedAt, int serverId, bool isSynced)
        {
            InputInvoiceNoteResponse response = new InputInvoiceNoteResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "UPDATE InputInvoiceNotes SET " +
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

        public InputInvoiceNoteResponse Delete(Guid identifier)
        {
            InputInvoiceNoteResponse response = new InputInvoiceNoteResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "DELETE FROM InputInvoiceNotes WHERE Identifier = @Identifier";
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

        public InputInvoiceNoteResponse DeleteAll()
        {
            InputInvoiceNoteResponse response = new InputInvoiceNoteResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM InputInvoiceNotes";
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
