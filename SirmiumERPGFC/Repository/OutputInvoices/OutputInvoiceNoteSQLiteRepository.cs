using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.OutputInvoices;
using ServiceInterfaces.Messages.Common.OutputInvoices;
using ServiceInterfaces.ViewModels.Common.OutputInvoices;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.OutputInvoices
{
    public class OutputInvoiceNoteSQLiteRepository
    {
        public static string OutputInvoiceNoteTableCreatePart =
                           "CREATE TABLE IF NOT EXISTS OutputInvoiceNotes " +
                           "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                           "ServerId INTEGER NULL, " +
                           "Identifier GUID, " +
                           "OutputInvoiceId INTEGER NULL, " +
                           "OutputInvoiceIdentifier GUID NULL, " +
                           "OutputInvoiceCode NVARCHAR(48) NULL, " +
                           "Note NVARCHAR(2048), " +
                           "NoteDate DATETIME NULL, " +
                           "IsSynced BOOL NULL, " +
                           "UpdatedAt DATETIME NULL, " +
                           "CreatedById INTEGER NULL, " +
                           "CreatedByName NVARCHAR(2048) NULL, " +
                           "CompanyId INTEGER NULL, " +
                           "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, OutputInvoiceId, OutputInvoiceIdentifier, " +
            "OutputInvoiceCode, Note, NoteDate, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO OutputInvoiceNotes " +
            "(Id, ServerId, Identifier, OutputInvoiceId, OutputInvoiceIdentifier, " +
            "OutputInvoiceCode, Note, NoteDate, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @OutputInvoiceId, @OutputInvoiceIdentifier, " +
            "@OutputInvoiceCode, @Note, @NoteDate, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        public OutputInvoiceNoteListResponse GetOutputInvoiceNotesByOutputInvoice(int companyId, Guid OutputInvoiceIdentifier)
        {
            OutputInvoiceNoteListResponse response = new OutputInvoiceNoteListResponse();
            List<OutputInvoiceNoteViewModel> OutputInvoiceNotes = new List<OutputInvoiceNoteViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM OutputInvoiceNotes " +
                        "WHERE OutputInvoiceIdentifier = @OutputInvoiceIdentifier " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC;", db);
                    selectCommand.Parameters.AddWithValue("@OutputInvoiceIdentifier", OutputInvoiceIdentifier);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        OutputInvoiceNoteViewModel dbEntry = new OutputInvoiceNoteViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.OutputInvoice = SQLiteHelper.GetOutputInvoice(query, ref counter);
                        dbEntry.Note = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.NoteDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        OutputInvoiceNotes.Add(dbEntry);
                    }

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.OutputInvoiceNotes = new List<OutputInvoiceNoteViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.OutputInvoiceNotes = OutputInvoiceNotes;
            return response;
        }

        public OutputInvoiceNoteResponse GetOutputInvoiceNote(Guid identifier)
        {
            OutputInvoiceNoteResponse response = new OutputInvoiceNoteResponse();
            OutputInvoiceNoteViewModel OutputInvoiceNote = new OutputInvoiceNoteViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM OutputInvoiceNotes " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        int counter = 0;
                        OutputInvoiceNoteViewModel dbEntry = new OutputInvoiceNoteViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.OutputInvoice = SQLiteHelper.GetOutputInvoice(query, ref counter);
                        dbEntry.Note = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.NoteDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        OutputInvoiceNote = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.OutputInvoiceNote = new OutputInvoiceNoteViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.OutputInvoiceNote = OutputInvoiceNote;
            return response;
        }

        public OutputInvoiceNoteListResponse GetUnSyncedNotes(int companyId)
        {
            OutputInvoiceNoteListResponse response = new OutputInvoiceNoteListResponse();
            List<OutputInvoiceNoteViewModel> viewModels = new List<OutputInvoiceNoteViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM  OutputInvoiceNotes " +
                        "WHERE CompanyId = @CompanyId AND IsSynced = 0 " +
                        "ORDER BY Id DESC;", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        OutputInvoiceNoteViewModel dbEntry = new OutputInvoiceNoteViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.OutputInvoice = SQLiteHelper.GetOutputInvoice(query, ref counter);
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
                    response.OutputInvoiceNotes = new List<OutputInvoiceNoteViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.OutputInvoiceNotes = viewModels;
            return response;
        }

        public void Sync(IOutputInvoiceNoteService OutputInvoiceNoteService)
        {
            var unSynced = GetUnSyncedNotes(MainWindow.CurrentCompanyId);
            SyncOutputInvoiceNoteRequest request = new SyncOutputInvoiceNoteRequest();
            request.CompanyId = MainWindow.CurrentCompanyId;
            request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

            OutputInvoiceNoteListResponse response = OutputInvoiceNoteService.Sync(request);
            if (response.Success)
            {
                List<OutputInvoiceNoteViewModel> OutputInvoiceNotesFromDB = response.OutputInvoiceNotes;
                foreach (var OutputInvoiceNote in OutputInvoiceNotesFromDB.OrderBy(x => x.Id))
                {
                    Delete(OutputInvoiceNote.Identifier);
                    if (OutputInvoiceNote.IsActive)
                    {
                        OutputInvoiceNote.IsSynced = true;
                        Create(OutputInvoiceNote);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from OutputInvoiceNotes WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from OutputInvoiceNotes WHERE CompanyId = @CompanyId", db);
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

        public OutputInvoiceNoteResponse Create(OutputInvoiceNoteViewModel OutputInvoiceNote)
        {
            OutputInvoiceNoteResponse response = new OutputInvoiceNoteResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = SqlCommandInsertPart;

                insertCommand.Parameters.AddWithValue("@ServerId", OutputInvoiceNote.Id);
                insertCommand.Parameters.AddWithValue("@Identifier", OutputInvoiceNote.Identifier);
                insertCommand.Parameters.AddWithValue("@OutputInvoiceId", ((object)OutputInvoiceNote.OutputInvoice.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@OutputInvoiceIdentifier", ((object)OutputInvoiceNote.OutputInvoice.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@OutputInvoiceCode", ((object)OutputInvoiceNote.OutputInvoice.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Note", OutputInvoiceNote.Note);
                insertCommand.Parameters.AddWithValue("@NoteDate", ((object)OutputInvoiceNote.NoteDate) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IsSynced", OutputInvoiceNote.IsSynced);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)OutputInvoiceNote.UpdatedAt) ?? DBNull.Value);
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

        public OutputInvoiceNoteResponse UpdateSyncStatus(Guid identifier, string code, DateTime? updatedAt, int serverId, bool isSynced)
        {
            OutputInvoiceNoteResponse response = new OutputInvoiceNoteResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "UPDATE OutputInvoiceNotes SET " +
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

        public OutputInvoiceNoteResponse Delete(Guid identifier)
        {
            OutputInvoiceNoteResponse response = new OutputInvoiceNoteResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "DELETE FROM OutputInvoiceNotes WHERE Identifier = @Identifier";
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

        public OutputInvoiceNoteResponse DeleteAll()
        {
            OutputInvoiceNoteResponse response = new OutputInvoiceNoteResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM OutputInvoiceNotes";
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
