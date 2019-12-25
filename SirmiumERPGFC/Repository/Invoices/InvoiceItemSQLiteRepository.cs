using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.Invoices;
using ServiceInterfaces.Gloabals;
using ServiceInterfaces.Messages.Common.Invoices;
using ServiceInterfaces.ViewModels.Common.Invoices;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.Invoices
{
    public class InvoiceItemSQLiteRepository
    {
        #region SQL

        public static string InvoiceItemTableCreatePart =
                           "CREATE TABLE IF NOT EXISTS InvoiceItems " +
                           "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                           "ServerId INTEGER NULL, " +
                           "Identifier GUID, " +
                           "InvoiceId INTEGER NULL, " +
                           "InvoiceIdentifier GUID NULL, " +
                           "InvoiceCode NVARCHAR(48) NULL, " +
                           "Code NVARCHAR(2048), " +
                           "Name NVARCHAR(48) NULL, " +
                           "UnitOfMeasure NVARCHAR(48) NULL, " +
                           "Quantity DECIMAL(18, 3) NULL, " +
                           "PriceWithPDV DECIMAL(18, 3) NULL, " +
                           "PriceWithoutPDV DECIMAL(18, 3) NULL, " +
                           "Discount DECIMAL(18, 3) NULL, " +
                           "PDVPercent DECIMAL(18, 3) NULL, " +
                           "PDV DECIMAL(18, 3) NULL, " +
                           "Amount DECIMAL(18, 3) NULL, " +
                           "ItemStatus INTEGER NOT NULL, " +
                           "CurrencyCode NVARCHAR(2048) NULL, " +
                           "ExchangeRate DECIMAL(18, 4) NULL, " +
                           "CurrencyPriceWithPDV DECIMAL(18, 4) NULL, " +
                           "IsSynced BOOL NULL, " +
                           "UpdatedAt DATETIME NULL, " +
                           "CreatedById INTEGER NULL, " +
                           "CreatedByName NVARCHAR(2048) NULL, " +
                           "CompanyId INTEGER NULL, " +
                           "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, InvoiceId, InvoiceIdentifier, " +
            "InvoiceCode, Code, Name, UnitOfMeasure, Quantity,  " +
            "PriceWithPDV, PriceWithoutPDV, Discount, PDVPercent, PDV,  " +
            "Amount, ItemStatus, CurrencyCode, ExchangeRate, CurrencyPriceWithPDV, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO InvoiceItems " +
            "(Id, ServerId, Identifier, InvoiceId, InvoiceIdentifier, " +
            "InvoiceCode, Code, Name, UnitOfMeasure, Quantity,  " +
            "PriceWithPDV, PriceWithoutPDV, Discount, PDVPercent, PDV,  " +
            "Amount, ItemStatus, CurrencyCode, ExchangeRate, CurrencyPriceWithPDV, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @InvoiceId, @InvoiceIdentifier, " +
            "@InvoiceCode, @Code, @Name, @UnitOfMeasure, @Quantity,  " +
            "@PriceWithPDV, @PriceWithoutPDV, @Discount, @PDVPercent, @PDV, " +
            "@Amount, @ItemStatus, @CurrencyCode, @ExchangeRate, @CurrencyPriceWithPDV, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods
        private static InvoiceItemViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            InvoiceItemViewModel dbEntry = new InvoiceItemViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.Invoice = SQLiteHelper.GetInvoice(query, ref counter);
            dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
            dbEntry.UnitOfMeasure = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Quantity = SQLiteHelper.GetDecimal(query, ref counter);
            dbEntry.PriceWithPDV = SQLiteHelper.GetDecimal(query, ref counter);
            dbEntry.PriceWithoutPDV = SQLiteHelper.GetDecimal(query, ref counter);
            dbEntry.Discount = SQLiteHelper.GetDecimal(query, ref counter);
            dbEntry.PDVPercent = SQLiteHelper.GetDecimal(query, ref counter);
            dbEntry.PDV = SQLiteHelper.GetDecimal(query, ref counter);
            dbEntry.Amount = SQLiteHelper.GetDecimal(query, ref counter);
            dbEntry.ItemStatus = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.CurrencyCode = SQLiteHelper.GetString(query, ref counter);
            dbEntry.ExchangeRate = SQLiteHelper.GetDoubleNullable(query, ref counter);
            dbEntry.CurrencyPriceWithPDV = SQLiteHelper.GetDoubleNullable(query, ref counter);

            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
            return dbEntry;
        }

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, InvoiceItemViewModel InvoiceItem)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", InvoiceItem.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", InvoiceItem.Identifier);
            insertCommand.Parameters.AddWithValue("@InvoiceId", ((object)InvoiceItem.Invoice?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@InvoiceIdentifier", ((object)InvoiceItem.Invoice?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@InvoiceCode", ((object)InvoiceItem.Invoice?.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Code", ((object)InvoiceItem.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Name", ((object)InvoiceItem.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@UnitOfMeasure", ((object)InvoiceItem.UnitOfMeasure) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Quantity", ((object)InvoiceItem.Quantity) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PriceWithPDV", ((object)InvoiceItem.PriceWithPDV) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PriceWithoutPDV", ((object)InvoiceItem.PriceWithoutPDV) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Discount", ((object)InvoiceItem.Discount) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PDVPercent", ((object)InvoiceItem.PDVPercent) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PDV", ((object)InvoiceItem.PDV) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Amount", ((object)InvoiceItem.Amount) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ItemStatus", ((object)InvoiceItem.ItemStatus) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CurrencyCode", ((object)InvoiceItem.CurrencyCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ExchangeRate", ((object)InvoiceItem.ExchangeRate) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CurrencyPriceWithPDV", ((object)InvoiceItem.CurrencyPriceWithPDV) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsSynced", InvoiceItem.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)InvoiceItem.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read

        public InvoiceItemListResponse GetInvoiceItemsByInvoice(int companyId, Guid InvoiceIdentifier)
        {
            InvoiceItemListResponse response = new InvoiceItemListResponse();
            List<InvoiceItemViewModel> InvoiceItems = new List<InvoiceItemViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM InvoiceItems " +
                        "WHERE InvoiceIdentifier = @InvoiceIdentifier " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC;", db);

                    selectCommand.Parameters.AddWithValue("@InvoiceIdentifier", InvoiceIdentifier);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        InvoiceItemViewModel dbEntry = Read(query);
                        InvoiceItems.Add(dbEntry);
                    }

                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.InvoiceItems = new List<InvoiceItemViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.InvoiceItems = InvoiceItems;
            return response;
        }

        public InvoiceItemResponse GetInvoiceItem(Guid identifier)
        {
            InvoiceItemResponse response = new InvoiceItemResponse();
            InvoiceItemViewModel InvoiceItem = new InvoiceItemViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM InvoiceItems " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        InvoiceItemViewModel dbEntry = Read(query);
                        InvoiceItem = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.InvoiceItem = new InvoiceItemViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.InvoiceItem = InvoiceItem;
            return response;
        }

        #endregion

        #region Sync

        public void Sync(IInvoiceItemService InvoiceItemService, Action<int, int> callback = null)
        {
            try
            {
                SyncInvoiceItemRequest request = new SyncInvoiceItemRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                InvoiceItemListResponse response = InvoiceItemService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.InvoiceItems?.Count ?? 0;
                    List<InvoiceItemViewModel> invoiceItemsFromDB = response.InvoiceItems;

                    using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM InvoiceItems WHERE Identifier = @Identifier";

                            SqliteCommand insertCommand = db.CreateCommand();
                            insertCommand.CommandText = SqlCommandInsertPart;

                            foreach (var invoiceItem in invoiceItemsFromDB)
                            {
                                deleteCommand.Parameters.AddWithValue("@Identifier", invoiceItem.Identifier);
                                deleteCommand.ExecuteNonQuery();
                                deleteCommand.Parameters.Clear();

                                if (invoiceItem.IsActive)
                                {
                                    invoiceItem.IsSynced = true;

                                    insertCommand = AddCreateParameters(insertCommand, invoiceItem);
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from InvoiceItems WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from InvoiceItems WHERE CompanyId = @CompanyId", db);
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

        public InvoiceItemResponse Create(InvoiceItemViewModel InvoiceItem)
        {
            InvoiceItemResponse response = new InvoiceItemResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, InvoiceItem);
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

        public InvoiceItemResponse Delete(Guid identifier)
        {
            InvoiceItemResponse response = new InvoiceItemResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM InvoiceItems WHERE Identifier = @Identifier";
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
        public InvoiceItemResponse SetStatusDeleted(Guid identifier)
        {
            InvoiceItemResponse response = new InvoiceItemResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "UPDATE InvoiceItems SET ItemStatus = @ItemStatus WHERE Identifier = @Identifier";
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
