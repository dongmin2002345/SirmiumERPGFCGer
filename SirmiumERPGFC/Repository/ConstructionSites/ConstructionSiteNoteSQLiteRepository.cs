﻿using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.ConstructionSites;
using ServiceInterfaces.Gloabals;
using ServiceInterfaces.Messages.ConstructionSites;
using ServiceInterfaces.ViewModels.ConstructionSites;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;

namespace SirmiumERPGFC.Repository.ConstructionSites
{
    public class ConstructionSiteNoteSQLiteRepository
    {
        #region SQL

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
                     "Path NVARCHAR(2048) NULL, " +
                     "ItemStatus INTEGER NOT NULL, " +
                     "IsSynced BOOL NULL, " +
                     "UpdatedAt DATETIME NULL, " +
                     "CreatedById INTEGER NULL, " +
                     "CreatedByName NVARCHAR(2048) NULL, " +
                     "CompanyId INTEGER NULL, " +
                     "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, ConstructionSiteId, ConstructionSiteIdentifier, " +
            "ConstructionSiteCode, ConstructionSiteName, Note, NoteDate, Path, ItemStatus, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO ConstructionSiteNotes " +
            "(Id, ServerId, Identifier, ConstructionSiteId, ConstructionSiteIdentifier, " +
            "ConstructionSiteCode, ConstructionSiteName, Note, NoteDate, Path, ItemStatus, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @ConstructionSiteId, @ConstructionSiteIdentifier, " +
            "@ConstructionSiteCode, @ConstructionSiteName, @Note, @NoteDate, @Path, @ItemStatus, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods
        private static ConstructionSiteNoteViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            ConstructionSiteNoteViewModel dbEntry = new ConstructionSiteNoteViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.ConstructionSite = SQLiteHelper.GetConstructionSite(query, ref counter);
            dbEntry.Note = SQLiteHelper.GetString(query, ref counter);
            dbEntry.NoteDate = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.Path = SQLiteHelper.GetString(query, ref counter);
            dbEntry.ItemStatus = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
            return dbEntry;
        }

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, ConstructionSiteNoteViewModel constructionSiteNote)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", constructionSiteNote.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", constructionSiteNote.Identifier);
            insertCommand.Parameters.AddWithValue("@ConstructionSiteId", ((object)constructionSiteNote.ConstructionSite.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ConstructionSiteIdentifier", ((object)constructionSiteNote.ConstructionSite.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ConstructionSiteCode", ((object)constructionSiteNote.ConstructionSite.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ConstructionSiteName", ((object)constructionSiteNote.ConstructionSite.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Note", constructionSiteNote.Note);
            insertCommand.Parameters.AddWithValue("@NoteDate", ((object)constructionSiteNote.NoteDate) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Path", ((object)constructionSiteNote.Path) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ItemStatus", constructionSiteNote.ItemStatus);
            insertCommand.Parameters.AddWithValue("@IsSynced", constructionSiteNote.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)constructionSiteNote.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read

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
                        ConstructionSiteNoteViewModel dbEntry = Read(query);
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
                        ConstructionSiteNoteViewModel dbEntry = Read(query);
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

        //public ConstructionSiteNoteListResponse GetUnSyncedNotes(int companyId)
        //{
        //    ConstructionSiteNoteListResponse response = new ConstructionSiteNoteListResponse();
        //    List<ConstructionSiteNoteViewModel> viewModels = new List<ConstructionSiteNoteViewModel>();

        //    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
        //    {
        //        db.Open();
        //        try
        //        {
        //            SqliteCommand selectCommand = new SqliteCommand(
        //                SqlCommandSelectPart +
        //                "FROM  ConstructionSiteNotes " +
        //                "WHERE CompanyId = @CompanyId AND IsSynced = 0 " +
        //                "ORDER BY Id DESC;", db);
        //            selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

        //            SqliteDataReader query = selectCommand.ExecuteReader();

        //            while (query.Read())
        //            {
        //                int counter = 0;
        //                ConstructionSiteNoteViewModel dbEntry = new ConstructionSiteNoteViewModel();
        //                dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
        //                dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
        //                dbEntry.ConstructionSite = SQLiteHelper.GetConstructionSite(query, ref counter);
        //                dbEntry.Note = SQLiteHelper.GetString(query, ref counter);
        //                dbEntry.NoteDate = SQLiteHelper.GetDateTime(query, ref counter);
        //                dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
        //                dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
        //                dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
        //                dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
        //                viewModels.Add(dbEntry);
        //            }

        //        }
        //        catch (SqliteException error)
        //        {
        //            MainWindow.ErrorMessage = error.Message;
        //            response.Success = false;
        //            response.Message = error.Message;
        //            response.ConstructionSiteNotes = new List<ConstructionSiteNoteViewModel>();
        //            return response;
        //        }
        //        db.Close();
        //    }
        //    response.Success = true;
        //    response.ConstructionSiteNotes = viewModels;
        //    return response;
        //}

        #endregion

        #region Sync

        public void Sync(IConstructionSiteNoteService ConstructionSiteNoteService, Action<int, int> callback = null)
        {
            try
            {
                SyncConstructionSiteNoteRequest request = new SyncConstructionSiteNoteRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                ConstructionSiteNoteListResponse response = ConstructionSiteNoteService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.ConstructionSiteNotes?.Count ?? 0;
                    List<ConstructionSiteNoteViewModel> items = response.ConstructionSiteNotes;

                    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM ConstructionSiteNotes WHERE Identifier = @Identifier";

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

        public ConstructionSiteNoteResponse Create(ConstructionSiteNoteViewModel constructionSiteNote)
        {
            ConstructionSiteNoteResponse response = new ConstructionSiteNoteResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, constructionSiteNote);
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

        public ConstructionSiteNoteResponse Delete(Guid identifier)
        {
            ConstructionSiteNoteResponse response = new ConstructionSiteNoteResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM ConstructionSiteNotes WHERE Identifier = @Identifier";
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

        //public ConstructionSiteNoteResponse DeleteAll()
        //{
        //    ConstructionSiteNoteResponse response = new ConstructionSiteNoteResponse();

        //    try
        //    {
        //        using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
        //        {
        //            db.Open();
        //            db.EnableExtensions(true);

        //            SqliteCommand insertCommand = new SqliteCommand();
        //            insertCommand.Connection = db;

        //            //Use parameterized query to prevent SQL injection attacks
        //            insertCommand.CommandText = "DELETE FROM ConstructionSiteNotes";
        //            try
        //            {
        //                insertCommand.ExecuteReader();
        //            }
        //            catch (SqliteException error)
        //            {
        //                response.Success = false;
        //                response.Message = error.Message;

        //                MainWindow.ErrorMessage = error.Message;
        //                return response;
        //            }
        //            db.Close();
        //        }
        //    }
        //    catch (SqliteException error)
        //    {
        //        response.Success = false;
        //        response.Message = error.Message;
        //        return response;
        //    }

        //    response.Success = true;
        //    return response;
        //}

        public ConstructionSiteNoteResponse SetStatusDeleted(Guid identifier)
        {
            ConstructionSiteNoteResponse response = new ConstructionSiteNoteResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "UPDATE ConstructionSiteNotes SET ItemStatus = @ItemStatus WHERE Identifier = @Identifier";
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
