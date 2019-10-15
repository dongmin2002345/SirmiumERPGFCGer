using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.Prices;
using ServiceInterfaces.Messages.Common.Prices;
using ServiceInterfaces.ViewModels.Common.Prices;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.Prices
{
    public class DiscountSQLiteRepository
    {
        #region SQL

        public static string DiscountTableCreatePart =
           "CREATE TABLE IF NOT EXISTS Discounts " +
           "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
           "ServerId INTEGER NULL, " +
           "Identifier GUID, " +
           "Code NVARCHAR(2048) NULL, " +
           "Name NVARCHAR(2048) NULL, " +
           "Amount DECIMAL NULL, " +
           "IsSynced BOOL NULL, " +
           "UpdatedAt DATETIME NULL, " +
           "CreatedById INTEGER NULL, " +
           "CreatedByName NVARCHAR(2048) NULL, " +
           "CompanyId INTEGER NULL, " +
           "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, Code, Name, Amount, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO Discounts " +
            "(Id, ServerId, Identifier, Code, Name, Amount, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @Code, @Name, @Amount, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods

        private DiscountViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            DiscountViewModel dbEntry = new DiscountViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Amount = SQLiteHelper.GetDecimal(query, ref counter);
            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);

            return dbEntry;
        }

        public SqliteCommand AddCreateParameters(SqliteCommand insertCommand, DiscountViewModel discount)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", discount.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", discount.Identifier);
            insertCommand.Parameters.AddWithValue("@Code", ((object)discount.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Name", ((object)discount.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Amount", ((object)discount.Amount) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsSynced", discount.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)discount.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read 

        public DiscountListResponse GetDiscountsByPage(int companyId, DiscountViewModel discountSearchObject, int currentPage = 1, int itemsPerPage = 50)
        {
            DiscountListResponse response = new DiscountListResponse();
            List<DiscountViewModel> discounts = new List<DiscountViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Discounts " +
                        "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, ServerId " +
                        "LIMIT @ItemsPerPage OFFSET @Offset;", db);
                    selectCommand.Parameters.AddWithValue("@Name", ((object)discountSearchObject.Search_Name) != null ? "%" + discountSearchObject.Search_Name + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
                    selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                        discounts.Add(Read(query));

                    selectCommand = new SqliteCommand(
                        "SELECT Count(*) " +
                        "FROM Discounts " +
                        "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND CompanyId = @CompanyId;", db);
                    selectCommand.Parameters.AddWithValue("@Name", ((object)discountSearchObject.Search_Name) != null ? "%" + discountSearchObject.Search_Name + "%" : "");
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
                    response.Discounts = new List<DiscountViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Discounts = discounts;
            return response;
        }

        public DiscountListResponse GetDiscountsForPopup(int companyId, string filterString)
        {
            DiscountListResponse response = new DiscountListResponse();
            List<DiscountViewModel> discounts = new List<DiscountViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Discounts " +
                        "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name OR Code LIKE @Name) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage;", db);
                    selectCommand.Parameters.AddWithValue("@Name", ((object)filterString) != null ? "%" + filterString + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", ((object)filterString) != null ? companyId : 0);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", 100);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                        discounts.Add(Read(query));
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.Discounts = new List<DiscountViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Discounts = discounts;
            return response;
        }

        public DiscountResponse GetDiscount(Guid identifier)
        {
            DiscountResponse response = new DiscountResponse();
            DiscountViewModel discount = null;

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Discounts " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                        discount = Read(query);
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.Discount = new DiscountViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Discount = discount;
            return response;
        }

        #endregion

        #region Sync

        public void Sync(IDiscountService discountService, Action<int, int> callback = null)
        {
            try
            {
                SyncDiscountRequest request = new SyncDiscountRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                DiscountListResponse response = discountService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.Discounts?.Count ?? 0;
                    List<DiscountViewModel> workOrderFinalProductsFromDB = response.Discounts;

                    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM Discounts WHERE Identifier = @Identifier";

                            SqliteCommand insertCommand = db.CreateCommand();
                            insertCommand.CommandText = SqlCommandInsertPart;

                            foreach (var workOrderFinalProduct in workOrderFinalProductsFromDB)
                            {
                                deleteCommand.Parameters.AddWithValue("@Identifier", workOrderFinalProduct.Identifier);
                                deleteCommand.ExecuteNonQuery();
                                deleteCommand.Parameters.Clear();

                                if (workOrderFinalProduct.IsActive)
                                {
                                    workOrderFinalProduct.IsSynced = true;

                                    insertCommand = AddCreateParameters(insertCommand, workOrderFinalProduct);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from Discounts WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from Discounts WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
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

        public DiscountResponse UpdateSyncStatus(Guid identifier, int serverId, bool isSynced, DateTime? lastUpdate, string code)
        {
            DiscountResponse response = new DiscountResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "UPDATE Discounts SET " +
                    "IsSynced = @IsSynced, " +
                    "ServerId = @ServerId, " +
                    "UpdatedAt = @UpdatedAt, " +
                    "Code = @Code " +
                    "WHERE Identifier = @Identifier ";

                insertCommand.Parameters.AddWithValue("@IsSynced", isSynced);
                insertCommand.Parameters.AddWithValue("@ServerId", serverId);
                insertCommand.Parameters.AddWithValue("@Code", code);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)lastUpdate) ?? DBNull.Value);
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

        #region Create

        public DiscountResponse Create(DiscountViewModel discount)
        {
            DiscountResponse response = new DiscountResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, discount);
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

        public DiscountResponse Delete(Guid identifier)
        {
            DiscountResponse response = new DiscountResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "DELETE FROM Discounts WHERE Identifier = @Identifier";
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
