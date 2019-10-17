using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.Shipments;
using ServiceInterfaces.Gloabals;
using ServiceInterfaces.Messages.Common.Shipments;
using ServiceInterfaces.ViewModels.Common.Shipments;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;

namespace SirmiumERPGFC.Repository.Shipments
{
    public class ShipmentDocumentSQLiteRepository
    {
        #region SQL

        public static string ShipmentDocumentTableCreatePart =
                 "CREATE TABLE IF NOT EXISTS ShipmentDocuments " +
                 "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                 "ServerId INTEGER NULL, " +
                 "Identifier GUID, " +
                 "ShipmentId INTEGER NULL, " +
                 "ShipmentIdentifier GUID NULL, " +
                 "ShipmentCode NVARCHAR(48) NULL, " +
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
            "SELECT ServerId, Identifier, ShipmentId, ShipmentIdentifier, " +
            "ShipmentCode, Name, CreateDate, Path, ItemStatus, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO ShipmentDocuments " +
            "(Id, ServerId, Identifier, ShipmentId, ShipmentIdentifier, " +
            "ShipmentCode, Name, CreateDate, Path, ItemStatus, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @ShipmentId, @ShipmentIdentifier, " +
            "@ShipmentCode, @Name, @CreateDate, @Path, @ItemStatus, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods
        private static ShipmentDocumentViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            ShipmentDocumentViewModel dbEntry = new ShipmentDocumentViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.Shipment = SQLiteHelper.GetShipment(query, ref counter);
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

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, ShipmentDocumentViewModel ShipmentDocument)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", ShipmentDocument.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", ShipmentDocument.Identifier);
            insertCommand.Parameters.AddWithValue("@ShipmentId", ((object)ShipmentDocument.Shipment.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ShipmentIdentifier", ((object)ShipmentDocument.Shipment.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ShipmentCode", ((object)ShipmentDocument.Shipment.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Name", ShipmentDocument.Name);
            insertCommand.Parameters.AddWithValue("@CreateDate", ((object)ShipmentDocument.CreateDate) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Path", ((object)ShipmentDocument.Path) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ItemStatus", ((object)ShipmentDocument.ItemStatus) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsSynced", ShipmentDocument.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)ShipmentDocument.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read

        public ShipmentDocumentListResponse GetShipmentDocumentsByShipment(int companyId, Guid ShipmentIdentifier)
        {
            ShipmentDocumentListResponse response = new ShipmentDocumentListResponse();
            List<ShipmentDocumentViewModel> ShipmentDocuments = new List<ShipmentDocumentViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM ShipmentDocuments " +
                        "WHERE ShipmentIdentifier = @ShipmentIdentifier " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC;", db);

                    selectCommand.Parameters.AddWithValue("@ShipmentIdentifier", ShipmentIdentifier);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {

                        ShipmentDocumentViewModel dbEntry = Read(query);
                        ShipmentDocuments.Add(dbEntry);
                    }

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.ShipmentDocuments = new List<ShipmentDocumentViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.ShipmentDocuments = ShipmentDocuments;
            return response;
        }

        public ShipmentDocumentResponse GetShipmentDocument(Guid identifier)
        {
            ShipmentDocumentResponse response = new ShipmentDocumentResponse();
            ShipmentDocumentViewModel ShipmentDocument = new ShipmentDocumentViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM ShipmentDocuments " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        ShipmentDocumentViewModel dbEntry = Read(query);
                        ShipmentDocument = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.ShipmentDocument = new ShipmentDocumentViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.ShipmentDocument = ShipmentDocument;
            return response;
        }

        public void Sync(IShipmentDocumentService ShipmentDocumentService, Action<int, int> callback = null)
        {
            try
            {
                SyncShipmentDocumentRequest request = new SyncShipmentDocumentRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                ShipmentDocumentListResponse response = ShipmentDocumentService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.ShipmentDocuments?.Count ?? 0;
                    List<ShipmentDocumentViewModel> items = response.ShipmentDocuments;

                    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM ShipmentDocuments WHERE Identifier = @Identifier";

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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from ShipmentDocuments WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from ShipmentDocuments WHERE CompanyId = @CompanyId", db);
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

        public ShipmentDocumentResponse Create(ShipmentDocumentViewModel ShipmentDocument)
        {
            ShipmentDocumentResponse response = new ShipmentDocumentResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, ShipmentDocument);
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

        public ShipmentDocumentResponse Delete(Guid identifier)
        {
            ShipmentDocumentResponse response = new ShipmentDocumentResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM ShipmentDocuments WHERE Identifier = @Identifier";
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

        public ShipmentDocumentResponse SetStatusDeleted(Guid identifier)
        {
            ShipmentDocumentResponse response = new ShipmentDocumentResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "UPDATE ShipmentDocuments SET ItemStatus = @ItemStatus WHERE Identifier = @Identifier";
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
