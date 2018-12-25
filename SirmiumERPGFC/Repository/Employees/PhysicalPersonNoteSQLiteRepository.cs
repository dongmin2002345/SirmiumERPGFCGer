﻿using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.Employees
{
    public class PhysicalPersonNoteSQLiteRepository
    {
        public static string PhysicalPersonNoteTableCreatePart =
                       "CREATE TABLE IF NOT EXISTS PhysicalPersonNotes " +
                       "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                       "ServerId INTEGER NULL, " +
                       "Identifier GUID, " +
                       "PhysicalPersonId INTEGER NULL, " +
                       "PhysicalPersonIdentifier GUID NULL, " +
                       "PhysicalPersonCode NVARCHAR(48) NULL, " +
                       "PhysicalPersonName NVARCHAR(48) NULL, " +
                       "Note NVARCHAR(2048), " +
                       "NoteDate DATETIME NULL, " +
                       "IsSynced BOOL NULL, " +
                       "UpdatedAt DATETIME NULL, " +
                       "CreatedById INTEGER NULL, " +
                       "CreatedByName NVARCHAR(2048) NULL, " +
                       "CompanyId INTEGER NULL, " +
                       "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, PhysicalPersonId, PhysicalPersonIdentifier, " +
            "PhysicalPersonCode, PhysicalPersonName, Note, NoteDate, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO PhysicalPersonNotes " +
            "(Id, ServerId, Identifier, PhysicalPersonId, PhysicalPersonIdentifier, " +
            "PhysicalPersonCode, PhysicalPersonName, Note, NoteDate, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @PhysicalPersonId, @PhysicalPersonIdentifier, " +
            "@PhysicalPersonCode, @PhysicalPersonName, @Note, @NoteDate, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        public PhysicalPersonNoteListResponse GetPhysicalPersonNotesByPhysicalPerson(int companyId, Guid PhysicalPersonIdentifier)
        {
            PhysicalPersonNoteListResponse response = new PhysicalPersonNoteListResponse();
            List<PhysicalPersonNoteViewModel> PhysicalPersonNotes = new List<PhysicalPersonNoteViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM PhysicalPersonNotes " +
                        "WHERE PhysicalPersonIdentifier = @PhysicalPersonIdentifier " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC;", db);
                    selectCommand.Parameters.AddWithValue("@PhysicalPersonIdentifier", PhysicalPersonIdentifier);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        PhysicalPersonNoteViewModel dbEntry = new PhysicalPersonNoteViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.PhysicalPerson = SQLiteHelper.GetPhysicalPerson(query, ref counter);
                        dbEntry.Note = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.NoteDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        PhysicalPersonNotes.Add(dbEntry);
                    }

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.PhysicalPersonNotes = new List<PhysicalPersonNoteViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.PhysicalPersonNotes = PhysicalPersonNotes;
            return response;
        }

        public PhysicalPersonNoteResponse GetPhysicalPersonNote(Guid identifier)
        {
            PhysicalPersonNoteResponse response = new PhysicalPersonNoteResponse();
            PhysicalPersonNoteViewModel PhysicalPersonNote = new PhysicalPersonNoteViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM PhysicalPersonNotes " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        int counter = 0;
                        PhysicalPersonNoteViewModel dbEntry = new PhysicalPersonNoteViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.PhysicalPerson = SQLiteHelper.GetPhysicalPerson(query, ref counter);
                        dbEntry.Note = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.NoteDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        PhysicalPersonNote = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.PhysicalPersonNote = new PhysicalPersonNoteViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.PhysicalPersonNote = PhysicalPersonNote;
            return response;
        }

        public PhysicalPersonNoteListResponse GetUnSyncedNotes(int companyId)
        {
            PhysicalPersonNoteListResponse response = new PhysicalPersonNoteListResponse();
            List<PhysicalPersonNoteViewModel> viewModels = new List<PhysicalPersonNoteViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM  PhysicalPersonNotes " +
                        "WHERE CompanyId = @CompanyId AND IsSynced = 0 " +
                        "ORDER BY Id DESC;", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        int counter = 0;
                        PhysicalPersonNoteViewModel dbEntry = new PhysicalPersonNoteViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.PhysicalPerson = SQLiteHelper.GetPhysicalPerson(query, ref counter);
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
                    response.PhysicalPersonNotes = new List<PhysicalPersonNoteViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.PhysicalPersonNotes = viewModels;
            return response;
        }

        public void Sync(IPhysicalPersonNoteService PhysicalPersonNoteService)
        {
            var unSynced = GetUnSyncedNotes(MainWindow.CurrentCompanyId);
            SyncPhysicalPersonNoteRequest request = new SyncPhysicalPersonNoteRequest();
            request.CompanyId = MainWindow.CurrentCompanyId;
            request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

            PhysicalPersonNoteListResponse response = PhysicalPersonNoteService.Sync(request);
            if (response.Success)
            {
                List<PhysicalPersonNoteViewModel> PhysicalPersonNotesFromDB = response.PhysicalPersonNotes;
                foreach (var PhysicalPersonNote in PhysicalPersonNotesFromDB.OrderBy(x => x.Id))
                {
                    Delete(PhysicalPersonNote.Identifier);
                    if (PhysicalPersonNote.IsActive)
                    {
                        PhysicalPersonNote.IsSynced = true;
                        Create(PhysicalPersonNote);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from PhysicalPersonNotes WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from PhysicalPersonNotes WHERE CompanyId = @CompanyId", db);
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

        public PhysicalPersonNoteResponse Create(PhysicalPersonNoteViewModel PhysicalPersonNote)
        {
            PhysicalPersonNoteResponse response = new PhysicalPersonNoteResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = SqlCommandInsertPart;

                insertCommand.Parameters.AddWithValue("@ServerId", PhysicalPersonNote.Id);
                insertCommand.Parameters.AddWithValue("@Identifier", PhysicalPersonNote.Identifier);
                insertCommand.Parameters.AddWithValue("@PhysicalPersonId", ((object)PhysicalPersonNote.PhysicalPerson.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@PhysicalPersonIdentifier", ((object)PhysicalPersonNote.PhysicalPerson.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@PhysicalPersonCode", ((object)PhysicalPersonNote.PhysicalPerson.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@PhysicalPersonName", ((object)PhysicalPersonNote.PhysicalPerson.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Note", PhysicalPersonNote.Note);
                insertCommand.Parameters.AddWithValue("@NoteDate", ((object)PhysicalPersonNote.NoteDate) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@IsSynced", PhysicalPersonNote.IsSynced);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)PhysicalPersonNote.UpdatedAt) ?? DBNull.Value);
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

        public PhysicalPersonNoteResponse UpdateSyncStatus(Guid identifier, string code, DateTime? updatedAt, int serverId, bool isSynced)
        {
            PhysicalPersonNoteResponse response = new PhysicalPersonNoteResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "UPDATE PhysicalPersonNotes SET " +
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

        public PhysicalPersonNoteResponse Delete(Guid identifier)
        {
            PhysicalPersonNoteResponse response = new PhysicalPersonNoteResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "DELETE FROM PhysicalPersonNotes WHERE Identifier = @Identifier";
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

        public PhysicalPersonNoteResponse DeleteAll()
        {
            PhysicalPersonNoteResponse response = new PhysicalPersonNoteResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM PhysicalPersonNotes";
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