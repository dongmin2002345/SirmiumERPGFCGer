using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;

namespace SirmiumERPGFC.Repository.BusinessPartners
{
    public class BusinessPartnerTypeSQLiteRepository
    {
        #region SQL

        public static string BusinessPartnerTypeTableCreatePart =
            "CREATE TABLE IF NOT EXISTS BusinessPartnerTypes " +
            "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
            "ServerId INTEGER, " +
            "Identifier GUID, " +
            "Code NVARCHAR(48), " +
            "Name NVARCHAR(2048), " +
            "IsBuyer BOOL NULL, " +
            "IsSupplier BOOL NULL, " +
            "IsSynced BOOL NULL, " +
            "UpdatedAt DATETIME NULL, " +
            "CreatedById INTEGER NULL, " +
            "CreatedByName NVARCHAR(2048) NULL, " +
            "CompanyId INTEGER NULL, " +
            "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, Code, Name, IsBuyer, IsSupplier, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, " +
            "CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO BusinessPartnerTypes " +
            "(Id, ServerId, Identifier, Code, Name, IsBuyer, IsSupplier, IsSynced, UpdatedAt, " +
            "CreatedById, CreatedByName, CompanyId, CompanyName) " +
            "VALUES (NULL, @ServerId, @Identifier,  @Code, @Name, @IsBuyer, @IsSupplier, @IsSynced, @UpdatedAt, " +
            "@CreatedById, @CreatedByName, @CompanyId, @CompanyName);";

        #endregion

        #region Helper methods

        private BusinessPartnerTypeViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            BusinessPartnerTypeViewModel dbEntry = new BusinessPartnerTypeViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
            dbEntry.IsBuyer = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.IsSupplier = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);

            return dbEntry;
        }

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, BusinessPartnerTypeViewModel businessPartnerType)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", businessPartnerType.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", businessPartnerType.Identifier);
            insertCommand.Parameters.AddWithValue("@Code", ((object)businessPartnerType.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Name", ((object)businessPartnerType.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsBuyer", ((object)businessPartnerType.IsBuyer) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsSupplier", ((object)businessPartnerType.IsSupplier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsSynced", businessPartnerType.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)businessPartnerType.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read

        public BusinessPartnerTypeListResponse GetBusinessPartnerTypes(int companyId)
        {
            BusinessPartnerTypeListResponse response = new BusinessPartnerTypeListResponse();
            List<BusinessPartnerTypeViewModel> businessPartnerTypes = new List<BusinessPartnerTypeViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM BusinessPartnerTypes " +
                        "WHERE CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC;", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                        businessPartnerTypes.Add(Read(query));
                    

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.BusinessPartnerTypes = new List<BusinessPartnerTypeViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.BusinessPartnerTypes = businessPartnerTypes;
            return response;
        }

        public BusinessPartnerTypeListResponse GetBusinessPartnerTypesByPage(int companyId, BusinessPartnerTypeViewModel businessPartnerTypeSearchObject, int currentPage = 1, int itemsPerPage = 50)
        {
            BusinessPartnerTypeListResponse response = new BusinessPartnerTypeListResponse();
            List<BusinessPartnerTypeViewModel> BusinessPartnerTypes = new List<BusinessPartnerTypeViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM BusinessPartnerTypes " +
                        "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage OFFSET @Offset;", db);
                    
                    selectCommand.Parameters.AddWithValue("@Name", ((object)businessPartnerTypeSearchObject.Search_Name) != null ? "%" + businessPartnerTypeSearchObject.Search_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
                    selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                        BusinessPartnerTypes.Add(Read(query));
                    

                    selectCommand = new SqliteCommand(
                        "SELECT Count(*) " +
                        "FROM BusinessPartnerTypes " +
                        "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND CompanyId = @CompanyId;", db);
                    selectCommand.Parameters.AddWithValue("@Name", ((object)businessPartnerTypeSearchObject.Search_Name) != null ? "%" + businessPartnerTypeSearchObject.Search_Name + "%" : "");
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
                    response.BusinessPartnerTypes = new List<BusinessPartnerTypeViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.BusinessPartnerTypes = BusinessPartnerTypes;
            return response;
        }

        public BusinessPartnerTypeListResponse GetBusinessPartnerTypesByBusinessPartner(int companyId, Guid businessPartnerIdentifier)
        {
            BusinessPartnerTypeListResponse response = new BusinessPartnerTypeListResponse();
            List<BusinessPartnerTypeViewModel> businessPartnerTypes = new List<BusinessPartnerTypeViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM BusinessPartnerTypes " +
                        "WHERE Identifier IN (SELECT BusinessPartnerTypeIdentifier from BusinessPartnerBusinessPartnerTypes where BusinessPartnerIdentifier = @BusinessPartnerIdentifier) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC;", db);
                    
                    selectCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", businessPartnerIdentifier);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                        businessPartnerTypes.Add(Read(query));
                    

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.BusinessPartnerTypes = new List<BusinessPartnerTypeViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.BusinessPartnerTypes = businessPartnerTypes;
            return response;
        }

        public BusinessPartnerTypeResponse GetBusinessPartnerType(Guid identifier)
        {
            BusinessPartnerTypeResponse response = new BusinessPartnerTypeResponse();
            BusinessPartnerTypeViewModel businessPartnerType = new BusinessPartnerTypeViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM BusinessPartnerTypes " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                        businessPartnerType = Read(query);
                    
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.BusinessPartnerType = new BusinessPartnerTypeViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.BusinessPartnerType = businessPartnerType;
            return response;
        }

        #endregion

        #region Sync

        public void Sync(IBusinessPartnerTypeService businessPartnerTypeService, Action<int, int> callback = null)
        {
            try
            {
                SyncBusinessPartnerTypeRequest request = new SyncBusinessPartnerTypeRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                BusinessPartnerTypeListResponse response = businessPartnerTypeService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.BusinessPartnerTypes?.Count ?? 0;
                    List<BusinessPartnerTypeViewModel> businessPartnerTypesFromDB = response.BusinessPartnerTypes;

                    using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM BusinessPartnerTypes WHERE Identifier = @Identifier";

                            SqliteCommand insertCommand = db.CreateCommand();
                            insertCommand.CommandText = SqlCommandInsertPart;

                            foreach (var businessPartnerType in businessPartnerTypesFromDB)
                            {
                                deleteCommand.Parameters.AddWithValue("@Identifier", businessPartnerType.Identifier);
                                deleteCommand.ExecuteNonQuery();
                                deleteCommand.Parameters.Clear();

                                if (businessPartnerType.IsActive)
                                {
                                    businessPartnerType.IsSynced = true;

                                    insertCommand = AddCreateParameters(insertCommand, businessPartnerType);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from BusinessPartnerTypes WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from BusinessPartnerTypes WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
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

        public BusinessPartnerTypeResponse Create(BusinessPartnerTypeViewModel businessPartnerType)
        {
            BusinessPartnerTypeResponse response = new BusinessPartnerTypeResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, businessPartnerType);
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

        public BusinessPartnerTypeResponse Delete(Guid identifier)
        {
            BusinessPartnerTypeResponse response = new BusinessPartnerTypeResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM BusinessPartnerTypes WHERE Identifier = @Identifier";
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

        public BusinessPartnerTypeResponse DeleteAll()
        {
            BusinessPartnerTypeResponse response = new BusinessPartnerTypeResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM BusinessPartnerTypes";
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
