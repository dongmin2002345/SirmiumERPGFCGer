using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.Shipments;
using ServiceInterfaces.Messages.Common.Shipments;
using ServiceInterfaces.ViewModels.Common.Shipments;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.Shipments
{
    public class ShipmentSQLiteRepository
    {
        #region SQL

        public static string ShipmentTableCreatePart =
          "CREATE TABLE IF NOT EXISTS Shipments " +
          "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
          "ServerId INTEGER NULL, " +
          "Identifier GUID, " +
          "Code NVARCHAR(48) NULL, " +
          "ShipmentDate DATETIME NULL, " +
          "Address NVARCHAR(48) NULL, " +
          "ServiceDeliveryId INTEGER NULL, " +
          "ServiceDeliveryIdentifier GUID NULL, " +
          "ServiceDeliveryCode NVARCHAR(48) NULL, " +
          "ServiceDeliveryName NVARCHAR(2048) NULL, " +

          "ShipmentNumber NVARCHAR(48) NULL, " +
          "Acceptor NVARCHAR(48) NULL, " +

          "DeliveryDate DATETIME NULL, " +

          "ReturnReceipt NVARCHAR(48) NULL, " +
          "DocumentName NVARCHAR(48) NULL, " +
          "Note NVARCHAR(48) NULL, " +
          
          "IsSynced BOOL NULL, " +
          "UpdatedAt DATETIME NULL, " +
          "CreatedById INTEGER NULL, " +
          "CreatedByName NVARCHAR(2048) NULL, " +
          "CompanyId INTEGER NULL, " +
          "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, Code, ShipmentDate, Address, ServiceDeliveryId, ServiceDeliveryIdentifier, ServiceDeliveryCode, ServiceDeliveryName, " +
            "ShipmentNumber, Acceptor, DeliveryDate, ReturnReceipt, DocumentName, Note, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO Shipments " +
            "(Id, ServerId, Identifier,Code, ShipmentDate, Address, ServiceDeliveryId, ServiceDeliveryIdentifier, ServiceDeliveryCode, ServiceDeliveryName, " +
            "ShipmentNumber, Acceptor, DeliveryDate, ReturnReceipt, DocumentName, Note, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @Code, @ShipmentDate, @Address, @ServiceDeliveryId, @ServiceDeliveryIdentifier, @ServiceDeliveryCode, @ServiceDeliveryName, " +
            "@ShipmentNumber, @Acceptor, @DeliveryDate, @ReturnReceipt, @DocumentName, @Note, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods

        private static ShipmentViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            ShipmentViewModel dbEntry = new ShipmentViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
            dbEntry.ShipmentDate = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.Address = SQLiteHelper.GetString(query, ref counter);
            dbEntry.ServiceDelivery = SQLiteHelper.GetServiceDelivery(query, ref counter);
            dbEntry.ShipmentNumber = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Acceptor = SQLiteHelper.GetString(query, ref counter);
            dbEntry.DeliveryDate = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.ReturnReceipt = SQLiteHelper.GetString(query, ref counter);
            dbEntry.DocumentName = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Note = SQLiteHelper.GetString(query, ref counter);
            
            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);

            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
            return dbEntry;
        }
        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, ShipmentViewModel Shipment)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", Shipment.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", Shipment.Identifier);
            insertCommand.Parameters.AddWithValue("@Code", ((object)Shipment.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ShipmentDate", ((object)Shipment.ShipmentDate) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Address", ((object)Shipment.Address) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ServiceDeliveryId", ((object)Shipment.ServiceDelivery?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ServiceDeliveryIdentifier", ((object)Shipment.ServiceDelivery?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ServiceDeliveryCode", ((object)Shipment.ServiceDelivery?.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ServiceDeliveryName", ((object)Shipment.ServiceDelivery?.Name) ?? DBNull.Value);
            
            insertCommand.Parameters.AddWithValue("@ShipmentNumber", ((object)Shipment.ShipmentNumber) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Acceptor", ((object)Shipment.Acceptor) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@DeliveryDate", ((object)Shipment.DeliveryDate) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ReturnReceipt", ((object)Shipment.ReturnReceipt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@DocumentName", ((object)Shipment.DocumentName) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Note", ((object)Shipment.Note) ?? DBNull.Value);
            

            insertCommand.Parameters.AddWithValue("@IsSynced", Shipment.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)Shipment.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read

        public ShipmentListResponse GetShipmentsByPage(int companyId, ShipmentViewModel ShipmentSearchObject, int currentPage = 1, int itemsPerPage = 50)
        {
            ShipmentListResponse response = new ShipmentListResponse();
            List<ShipmentViewModel> Shipments = new List<ShipmentViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Shipments " +
                        "WHERE (@Address IS NULL OR @Address = '' OR Address LIKE @Address) " +
                        "AND (@Acceptor IS NULL OR @Acceptor = '' OR Acceptor LIKE @Acceptor)  " +
                        "AND (@ShipmentNumber IS NULL OR @ShipmentNumber = '' OR ShipmentNumber LIKE @ShipmentNumber)  " +
                        
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage OFFSET @Offset;", db);

                    selectCommand.Parameters.AddWithValue("@Address", ((object)ShipmentSearchObject.SearchBy_Address) != null ? "%" + ShipmentSearchObject.SearchBy_Address + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Acceptor", ((object)ShipmentSearchObject.SearchBy_Acceptor) != null ? "%" + ShipmentSearchObject.SearchBy_Acceptor + "%" : "");

                    selectCommand.Parameters.AddWithValue("@ShipmentNumber", ((object)ShipmentSearchObject.SearchBy_ShipmentNumber) != null ? "%" + ShipmentSearchObject.SearchBy_ShipmentNumber + "%" : "");
                    
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
                    selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {

                        ShipmentViewModel dbEntry = Read(query);
                        Shipments.Add(dbEntry);
                    }


                    selectCommand = new SqliteCommand(
                        "SELECT Count(*) " +
                        "FROM Shipments " +
                        "WHERE (@Address IS NULL OR @Address = '' OR Address LIKE @Address) " +
                        "AND (@Acceptor IS NULL OR @Acceptor = '' OR Acceptor LIKE @Acceptor)  " +
                        "AND (@ShipmentNumber IS NULL OR @ShipmentNumber = '' OR ShipmentNumber LIKE @ShipmentNumber)  " +
                        "AND CompanyId = @CompanyId;", db);

                    selectCommand.Parameters.AddWithValue("@Address", ((object)ShipmentSearchObject.SearchBy_Address) != null ? "%" + ShipmentSearchObject.SearchBy_Address + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Acceptor", ((object)ShipmentSearchObject.SearchBy_Acceptor) != null ? "%" + ShipmentSearchObject.SearchBy_Acceptor + "%" : "");

                    selectCommand.Parameters.AddWithValue("@ShipmentNumber", ((object)ShipmentSearchObject.SearchBy_ShipmentNumber) != null ? "%" + ShipmentSearchObject.SearchBy_ShipmentNumber + "%" : "");

                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    query = selectCommand.ExecuteReader();

                    if (query.Read())
                        response.TotalItems = query.GetInt32(0);
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.Shipments = new List<ShipmentViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Shipments = Shipments;
            return response;
        }

        public ShipmentListResponse GetShipmentsForPopup(int companyId, string filterString)
        {
            ShipmentListResponse response = new ShipmentListResponse();
            List<ShipmentViewModel> Shipments = new List<ShipmentViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Shipments " +
                        "WHERE (@Code IS NULL OR @Code = '' OR Code LIKE @Code OR Code LIKE @Code) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage;", db);

                    selectCommand.Parameters.AddWithValue("@Code", ((object)filterString) != null ? "%" + filterString + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", ((object)filterString) != null ? companyId : 0);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", 100);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        ShipmentViewModel dbEntry = Read(query);
                        Shipments.Add(dbEntry);
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.Shipments = new List<ShipmentViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Shipments = Shipments;
            return response;
        }

        public ShipmentResponse GetShipment(Guid identifier)
        {
            ShipmentResponse response = new ShipmentResponse();
            ShipmentViewModel Shipment = new ShipmentViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Shipments " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        ShipmentViewModel dbEntry = Read(query);
                        Shipment = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.Shipment = new ShipmentViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Shipment = Shipment;
            return response;
        }

        #endregion

        #region Sync

        public void Sync(IShipmentService shipmentService, Action<int, int> callback = null)
        {
            try
            {
                SyncShipmentRequest request = new SyncShipmentRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                ShipmentListResponse response = shipmentService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.Shipments?.Count ?? 0;
                    var items = new List<ShipmentViewModel>(response.Shipments);

                    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM Shipments WHERE Identifier = @Identifier";

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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from Shipments WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from Shipments WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
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

        public ShipmentResponse Create(ShipmentViewModel Shipment)
        {
            ShipmentResponse response = new ShipmentResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;



                try
                {
                    insertCommand = AddCreateParameters(insertCommand, Shipment);
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

        public ShipmentResponse Delete(Guid identifier)
        {
            ShipmentResponse response = new ShipmentResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM Shipments WHERE Identifier = @Identifier";
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
