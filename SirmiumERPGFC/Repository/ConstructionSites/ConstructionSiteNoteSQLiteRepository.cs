using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.ConstructionSites;
using ServiceInterfaces.Messages.ConstructionSites;
using ServiceInterfaces.ViewModels.ConstructionSites;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.ConstructionSites
{
    public class ConstructionSiteNoteSQLiteRepository
    {
        public static string ConstructionSiteNoteTableCreatePart =
                     "CREATE TABLE IF NOT EXISTS ConstructionSiteNotes " +
                     "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                     "ServerId INTEGER NULL, " +
                     "Identifier GUID, " +
                     "ConstructionSiteId INTEGER NULL, " +
                     "ConstructionSiteIdentifier GUID NULL, " +
                     "ConstructionSiteCode NVARCHAR(48) NULL, " +
                     "ConstructionSiteName NVARCHAR(48) NULL, " +
                     "Note NVARCHAR(2048), " +
                     "NoteDate DATETIME NULL, " +
                     "IsSynced BOOL NULL, " +
                     "UpdatedAt DATETIME NULL, " +
                     "CreatedById INTEGER NULL, " +
                     "CreatedByName NVARCHAR(2048) NULL, " +
                     "CompanyId INTEGER NULL, " +
                     "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, ConstructionSiteId, ConstructionSiteIdentifier, " +
            "ConstructionSiteCode, ConstructionSiteName, Note, NoteDate, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO ConstructionSiteNotes " +
            "(Id, ServerId, Identifier, ConstructionSiteId, ConstructionSiteIdentifier, " +
            "ConstructionSiteCode, ConstructionSiteName, Note, NoteDate, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @ConstructionSiteId, @ConstructionSiteIdentifier, " +
            "@ConstructionSiteCode, @ConstructionSiteName, @Note, @NoteDate, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        public ConstructionSiteNoteListResponse GetConstructionSiteNotesByConstructionSite(int companyId, Guid ConstructionSiteIdentifier)
        {
            ConstructionSiteNoteListResponse response = new ConstructionSiteNoteListResponse();
            List<ConstructionSiteNoteViewModel> ConstructionSiteNotes = new List<ConstructionSiteNoteViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM ConstructionSiteNotes " +
                        "WHERE ConstructionSiteIdentifier = @ConstructionSiteIdentifier " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC;", db);
                    selectCommand.Parameters.AddWithValue("@ConstructionSiteIdentifier", ConstructionSiteIdentifier);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        ConstructionSiteNoteViewModel dbEntry = new ConstructionSiteNoteViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.ConstructionSite = SQLiteHelper.GetConstructionSite(query, ref counter);
                        dbEntry.Note = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.NoteDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        ConstructionSiteNotes.Add(dbEntry);
                    }

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.ConstructionSiteNotes = new List<ConstructionSiteNoteViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.ConstructionSiteNotes = ConstructionSiteNotes;
            return response;
        }

        public ConstructionSiteNoteResponse GetConstructionSiteNote(Guid identifier)
        {
            ConstructionSiteNoteResponse response = new ConstructionSiteNoteResponse();
            ConstructionSiteNoteViewModel ConstructionSiteNote = new ConstructionSiteNoteViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM ConstructionSiteNotes " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        int counter = 0;
                        ConstructionSiteNoteViewModel dbEntry = new ConstructionSiteNoteViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.ConstructionSite = SQLiteHelper.GetConstructionSite(query, ref counter);
                        dbEntry.Note = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.NoteDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        ConstructionSiteNote = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.ConstructionSiteNote = new ConstructionSiteNoteViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.ConstructionSiteNote = ConstructionSiteNote;
            return response;
        }

        public ConstructionSiteNoteListResponse GetUnSyncedNotes(int companyId)
        {
            ConstructionSiteNoteListResponse response = new ConstructionSiteNoteListResponse();
            List<ConstructionSiteNoteViewModel> viewModels = new List<ConstructionSiteNoteViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM  ConstructionSiteNotes " +
                        "WHERE CompanyId = @CompanyId AND IsSynced = 0 " +
                        "ORDER BY Id DESC;", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        ConstructionSiteNoteViewModel dbEntry = new ConstructionSiteNoteViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.ConstructionSite = SQLiteHelper.GetConstructionSite(query, ref counter);
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
                    response.ConstructionSiteNotes = new List<ConstructionSiteNoteViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.ConstructionSiteNotes = viewModels;
            return response;
        }

        public void Sync(IConstructionSiteNoteService ConstructionSiteNoteService)
        {
            var unSynced = GetUnSyncedNotes(MainWindow.CurrentCompanyId);
            SyncConstructionSiteNoteRequest request = new SyncConstructionSiteNoteRequest();
            request.CompanyId = MainWindow.CurrentCompanyId;
            request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

            ConstructionSiteNoteListResponse response = ConstructionSiteNoteService.Sync(request);
            if (response.Success)
            {
                List<ConstructionSiteNoteViewModel> ConstructionSiteNotesFromDB = response.ConstructionSiteNotes;
                foreach (var ConstructionSiteNote in ConstructionSiteNotesFromDB.OrderBy(x => x.Id))
                {
                    Delete(ConstructionSiteNote.Identifier);
                    if (ConstructionSiteNote.IsActive)
                    {
                        ConstructionSiteNote.IsSynced = true;
                        Create(ConstructionSiteNote);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from ConstructionSiteNotes WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from ConstructionSiteNotes WHERE CompanyId = @CompanyId", db);
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

        public ConstructionSiteNoteResponse Create(ConstructionSiteNoteViewModel ConstructionSiteNote)
        {
            ConstructionSiteNoteResponse response = new ConstructionSiteNoteResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = SqlCommandInsertPart;

                insertCommand.Parameters.AddWithValue("@ServerId", ConstructionSiteNote.Id);
                insertCommand.Parameters.AddWithValue("@Identifier", ConstructionSiteNote.Identifier);
                insertCommand.Parameters.AddWithValue("@ConstructionSiteId", ((object)ConstructionSiteNote.ConstructionSite.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ConstructionSiteIdentifier", ((object)ConstructionSiteNote.ConstructionSite.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ConstructionSiteCode", ((object)ConstructionSiteNote.ConstructionSite.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@ConstructionSiteName", ((object)ConstructionSiteNote.ConstructionSite.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Note", ConstructionSiteNote.Note);
                insertCommand.Parameters.AddWithValue("@NoteDate", ((object)ConstructionSiteNote.NoteDate) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IsSynced", ConstructionSiteNote.IsSynced);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)ConstructionSiteNote.UpdatedAt) ?? DBNull.Value);
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

        public ConstructionSiteNoteResponse UpdateSyncStatus(Guid identifier, string code, DateTime? updatedAt, int serverId, bool isSynced)
        {
            ConstructionSiteNoteResponse response = new ConstructionSiteNoteResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "UPDATE ConstructionSiteNotes SET " +
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

        public ConstructionSiteNoteResponse Delete(Guid identifier)
        {
            ConstructionSiteNoteResponse response = new ConstructionSiteNoteResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "DELETE FROM ConstructionSiteNotes WHERE Identifier = @Identifier";
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

        public ConstructionSiteNoteResponse DeleteAll()
        {
            ConstructionSiteNoteResponse response = new ConstructionSiteNoteResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM ConstructionSiteNotes";
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
