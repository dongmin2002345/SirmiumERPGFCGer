using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.Prices;
using ServiceInterfaces.Messages.Common.Prices;
using ServiceInterfaces.ViewModels.Common.Prices;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;

namespace SirmiumERPGFC.Repository.Prices
{
    public class ServiceDeliverySQLiteRepository
    {
        #region SQL

        public static string ServiceDeliveryTableCreatePart =
           "CREATE TABLE IF NOT EXISTS ServiceDeliverys " +
           "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
           "ServerId INTEGER NULL, " +
           "Identifier GUID, " +
           "Code NVARCHAR(2048) NULL, " +
            "InternalCode NVARCHAR(2048) NULL, " +
           "Name NVARCHAR(2048) NULL, " +
           "URL NVARCHAR(2048) NULL, " +
           "IsSynced BOOL NULL, " +
           "UpdatedAt DATETIME NULL, " +
           "CreatedById INTEGER NULL, " +
           "CreatedByName NVARCHAR(2048) NULL, " +
           "CompanyId INTEGER NULL, " +
           "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, Code, InternalCode, Name, URL, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO ServiceDeliverys " +
            "(Id, ServerId, Identifier, Code, InternalCode, Name, URL, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @Code, @InternalCode, @Name, @URL, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods

        private ServiceDeliveryViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            ServiceDeliveryViewModel dbEntry = new ServiceDeliveryViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
            dbEntry.InternalCode = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
            dbEntry.URL = SQLiteHelper.GetString(query, ref counter);
            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);

            return dbEntry;
        }

        public SqliteCommand AddCreateParameters(SqliteCommand insertCommand, ServiceDeliveryViewModel serviceDelivery)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", serviceDelivery.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", serviceDelivery.Identifier);
            insertCommand.Parameters.AddWithValue("@Code", ((object)serviceDelivery.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@InternalCode", ((object)serviceDelivery.InternalCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Name", ((object)serviceDelivery.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@URL", ((object)serviceDelivery.URL) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsSynced", serviceDelivery.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)serviceDelivery.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read 

        public ServiceDeliveryListResponse GetServiceDeliverysByPage(int companyId, ServiceDeliveryViewModel serviceDeliverySearchObject, int currentPage = 1, int itemsPerPage = 50)
        {
            ServiceDeliveryListResponse response = new ServiceDeliveryListResponse();
            List<ServiceDeliveryViewModel> serviceDeliverys = new List<ServiceDeliveryViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM ServiceDeliverys " +
                        "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, ServerId " +
                        "LIMIT @ItemsPerPage OFFSET @Offset;", db);
                   
                    selectCommand.Parameters.AddWithValue("@Name", ((object)serviceDeliverySearchObject.Search_Name) != null ? "%" + serviceDeliverySearchObject.Search_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
                    selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        ServiceDeliveryViewModel dbEntry = Read(query);
                        serviceDeliverys.Add(dbEntry);
                    }

                    selectCommand = new SqliteCommand(
                        "SELECT Count(*) " +
                        "FROM ServiceDeliverys " +
                        "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND CompanyId = @CompanyId;", db);
                    
                    selectCommand.Parameters.AddWithValue("@Name", ((object)serviceDeliverySearchObject.Search_Name) != null ? "%" + serviceDeliverySearchObject.Search_Name + "%" : "");
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
                    response.ServiceDeliverys = new List<ServiceDeliveryViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.ServiceDeliverys = serviceDeliverys;
            return response;
        }

        public ServiceDeliveryListResponse GetServiceDeliverysForPopup(int companyId, string filterString)
        {
            ServiceDeliveryListResponse response = new ServiceDeliveryListResponse();
            List<ServiceDeliveryViewModel> serviceDeliverys = new List<ServiceDeliveryViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM ServiceDeliverys " +
                        "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name OR Code LIKE @Name) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage;", db);
                    
                    selectCommand.Parameters.AddWithValue("@Name", ((object)filterString) != null ? "%" + filterString + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", ((object)filterString) != null ? companyId : 0);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", 100);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        ServiceDeliveryViewModel dbEntry = Read(query);
                        serviceDeliverys.Add(dbEntry);
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.ServiceDeliverys = new List<ServiceDeliveryViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.ServiceDeliverys = serviceDeliverys;
            return response;
        }

        public ServiceDeliveryResponse GetServiceDelivery(Guid identifier)
        {
            ServiceDeliveryResponse response = new ServiceDeliveryResponse();
            ServiceDeliveryViewModel serviceDelivery = null;

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM ServiceDeliverys " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        ServiceDeliveryViewModel dbEntry = Read(query);
                        serviceDelivery = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.ServiceDelivery = new ServiceDeliveryViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.ServiceDelivery = serviceDelivery;
            return response;
        }

        #endregion

        #region Sync

        public void Sync(IServiceDeliveryService serviceDeliveryService, Action<int, int> callback = null)
        {
            try
            {
                SyncServiceDeliveryRequest request = new SyncServiceDeliveryRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                ServiceDeliveryListResponse response = serviceDeliveryService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.ServiceDeliverys?.Count ?? 0;
                    List<ServiceDeliveryViewModel> serviceDeliverysFromDB = response.ServiceDeliverys;

                    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM ServiceDeliverys WHERE Identifier = @Identifier";

                            SqliteCommand insertCommand = db.CreateCommand();
                            insertCommand.CommandText = SqlCommandInsertPart;

                            foreach (var serviceDelivery in serviceDeliverysFromDB)
                            {
                                deleteCommand.Parameters.AddWithValue("@Identifier", serviceDelivery.Identifier);
                                deleteCommand.ExecuteNonQuery();
                                deleteCommand.Parameters.Clear();

                                if (serviceDelivery.IsActive)
                                {
                                    serviceDelivery.IsSynced = true;

                                    insertCommand = AddCreateParameters(insertCommand, serviceDelivery);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from ServiceDeliverys WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from ServiceDeliverys WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
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

        public ServiceDeliveryResponse Create(ServiceDeliveryViewModel serviceDelivery)
        {
            ServiceDeliveryResponse response = new ServiceDeliveryResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, serviceDelivery);
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

        public ServiceDeliveryResponse Delete(Guid identifier)
        {
            ServiceDeliveryResponse response = new ServiceDeliveryResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM ServiceDeliverys WHERE Identifier = @Identifier";
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
