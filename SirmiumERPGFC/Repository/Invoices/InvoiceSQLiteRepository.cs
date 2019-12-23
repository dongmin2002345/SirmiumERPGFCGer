using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.Invoices;
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
    public class InvoiceSQLiteRepository
    {
        #region SQL

        public static string InvoiceTableCreatePart =
            "CREATE TABLE IF NOT EXISTS Invoices " +
            "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
            "ServerId INTEGER NULL, " +
            "Identifier GUID, " +
            "Code NVARCHAR(48) NULL, " +
            "InvoiceNumber NVARCHAR(48) NULL, " +

            "BuyerId INTEGER NULL, " +
            "BuyerIdentifier GUID NULL, " +
            "BuyerCode NVARCHAR(48) NULL, " +
            "BuyerName NVARCHAR(48) NULL, " +
            "BuyerInternalCode NVARCHAR(2048) NULL, " +
            "BuyerNameGer NVARCHAR(2048) NULL, " +
            "EnteredBuyerName NVARCHAR(2048) NULL, " +

            "Address NVARCHAR(2048) NULL, " +
            "InvoiceDate DATETIME NULL, " +
            "DueDate DATETIME NULL, " +
            "DateOfPayment DATETIME NULL, " +

            "Status INTEGER NULL, " +
            "StatusDate DATETIME NULL, " +
            
            "Description NVARCHAR(2048) NULL, " +
            "CurrencyCode NVARCHAR(2048) NULL, " +
            "CurrencyExchangeRate DECIMAL(18, 2) NULL, " +


            "CityId INTEGER NULL, " +
            "CityIdentifier GUID NULL, " +
            "CityCode NVARCHAR(48) NULL, " +
            "CityName NVARCHAR(48) NULL, " +

            "MunicipalityId INTEGER NULL, " +
            "MunicipalityIdentifier GUID NULL, " +
            "MunicipalityCode NVARCHAR(48) NULL, " +
            "MunicipalityName NVARCHAR(48) NULL, " +

            "DiscountId INTEGER NULL, " +
            "DiscountIdentifier GUID NULL, " +
            "DiscountCode NVARCHAR(48) NULL, " +
            "DiscountName NVARCHAR(48) NULL, " +
            "DiscountAmount DECIMAL NULL, " +

            "VatId INTEGER NULL, " +
            "VatIdentifier GUID NULL, " +
            "VatCode NVARCHAR(48) NULL, " +
            "VatDescription NVARCHAR(48) NULL, " +
            "VatAmount DECIMAL NULL, " +

            "PdvType INTEGER NULL, " +
            "IsSynced BOOL NULL, " +
            "UpdatedAt DATETIME NULL, " +
            "CreatedById INTEGER NULL, " +
            "CreatedByName NVARCHAR(2048) NULL, " +
            "CompanyId INTEGER NULL, " +
            "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, Code, InvoiceNumber, " +
            "BuyerId, BuyerIdentifier, BuyerCode, BuyerName, " +
            "BuyerInternalCode, BuyerNameGer, EnteredBuyerName, " +

            "Address, InvoiceDate, DueDate, DateOfPayment, " +
            "Status, StatusDate, Description, CurrencyCode, CurrencyExchangeRate, " +

            "CityId, CityIdentifier, CityCode, CityName, " +
            "MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +

            "DiscountId, DiscountIdentifier, DiscountCode, DiscountName, DiscountAmount, " +

            "VatId, VatIdentifier, VatCode, VatDescription, VatAmount,  " +

            "PdvType, IsSynced, UpdatedAt, CreatedById, CreatedByName, " +
            "CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO Invoices " +
            "(Id, ServerId, Identifier, Code, InvoiceNumber, " +
            "BuyerId, BuyerIdentifier, BuyerCode, BuyerName, " +
            "BuyerInternalCode, BuyerNameGer, EnteredBuyerName, " +

            "Address, InvoiceDate, DueDate, DateOfPayment, " +
            "Status, StatusDate, Description, CurrencyCode, CurrencyExchangeRate, " +

            "CityId, CityIdentifier, CityCode, CityName, " +
            "MunicipalityId, MunicipalityIdentifier, MunicipalityCode, MunicipalityName, " +

            "DiscountId, DiscountIdentifier, DiscountCode, DiscountName, DiscountAmount, " +

            "VatId, VatIdentifier, VatCode, VatDescription, VatAmount,  " +

            "PdvType, IsSynced, UpdatedAt, CreatedById, CreatedByName, " +
            "CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @Code, @InvoiceNumber, " +
            "@BuyerId, @BuyerIdentifier, @BuyerCode, @BuyerName, " +
            "@BuyerInternalCode, @BuyerNameGer, @EnteredBuyerName, " +

            "@Address, @InvoiceDate, @DueDate, @DateOfPayment, " +
            "@Status, @StatusDate, @Description, @CurrencyCode, @CurrencyExchangeRate, " +

            "@CityId, @CityIdentifier, @CityCode, @CityName, " +
            "@MunicipalityId, @MunicipalityIdentifier, @MunicipalityCode, @MunicipalityName, " +

            "@DiscountId, @DiscountIdentifier, @DiscountCode, @DiscountName, @DiscountAmount, " +

            "@VatId, @VatIdentifier, @VatCode, @VatDescription, @VatAmount,  " +

            "@PdvType, @IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, " +
            "@CompanyId, @CompanyName)";

        #endregion

        #region Helper methods

        private static InvoiceViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            InvoiceViewModel dbEntry = new InvoiceViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
            dbEntry.InvoiceNumber = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Buyer = SQLiteHelper.GetBusinessPartner(query, ref counter);
            dbEntry.BuyerName = SQLiteHelper.GetString(query, ref counter);

            dbEntry.Address = SQLiteHelper.GetString(query, ref counter);
            dbEntry.InvoiceDate = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.DueDate = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.DateOfPayment = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.Status = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.StatusDate = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.Description = SQLiteHelper.GetString(query, ref counter);
            dbEntry.CurrencyCode = SQLiteHelper.GetString(query, ref counter);
            dbEntry.CurrencyExchangeRate = SQLiteHelper.GetDoubleNullable(query, ref counter);

            dbEntry.City = SQLiteHelper.GetCity(query, ref counter);
            dbEntry.Municipality = SQLiteHelper.GetMunicipality(query, ref counter);
            dbEntry.Discount = SQLiteHelper.GetDiscount(query, ref counter);
            dbEntry.Vat = SQLiteHelper.GetVat(query, ref counter);

            dbEntry.PdvType = SQLiteHelper.GetIntNullable(query, ref counter);

            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
            return dbEntry;
        }
        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, InvoiceViewModel Invoice)
        {


            insertCommand.Parameters.AddWithValue("@ServerId", Invoice.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", Invoice.Identifier);
            insertCommand.Parameters.AddWithValue("@Code", ((object)Invoice.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@InvoiceNumber", ((object)Invoice.InvoiceNumber) ?? DBNull.Value);

            insertCommand.Parameters.AddWithValue("@BuyerId", ((object)Invoice.Buyer?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BuyerIdentifier", ((object)Invoice.Buyer?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BuyerCode", ((object)Invoice.Buyer?.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BuyerName", ((object)Invoice.Buyer?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BuyerInternalCode", ((object)Invoice.Buyer?.InternalCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BuyerNameGer", ((object)Invoice.Buyer?.NameGer) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@EnteredBuyerName", ((object)Invoice.BuyerName) ?? DBNull.Value);

            insertCommand.Parameters.AddWithValue("@Address", ((object)Invoice.Address) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@InvoiceDate", ((object)Invoice.InvoiceDate) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@DueDate", ((object)Invoice.DueDate) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@DateOfPayment", ((object)Invoice.DateOfPayment) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Status", ((object)Invoice.Status) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@StatusDate", ((object)Invoice.StatusDate) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Description", ((object)Invoice.Description) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CurrencyCode", ((object)Invoice.CurrencyCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CurrencyExchangeRate", ((object)Invoice.CurrencyExchangeRate) ?? DBNull.Value);

            insertCommand.Parameters.AddWithValue("@CityId", ((object)Invoice.City?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CityIdentifier", ((object)Invoice.City?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CityCode", ((object)Invoice.City?.ZipCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CityName", ((object)Invoice.City?.Name) ?? DBNull.Value);

            insertCommand.Parameters.AddWithValue("@MunicipalityId", ((object)Invoice.Municipality?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@MunicipalityIdentifier", ((object)Invoice.Municipality?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@MunicipalityCode", ((object)Invoice.Municipality?.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@MunicipalityName", ((object)Invoice.Municipality?.Name) ?? DBNull.Value);

            insertCommand.Parameters.AddWithValue("@DiscountId", ((object)Invoice.Discount?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@DiscountIdentifier", ((object)Invoice.Discount?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@DiscountCode", ((object)Invoice.Discount?.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@DiscountName", ((object)Invoice.Discount?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@DiscountAmount", ((object)Invoice.Discount?.Amount) ?? DBNull.Value);

            insertCommand.Parameters.AddWithValue("@VatId", ((object)Invoice.Vat?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@VatIdentifier", ((object)Invoice.Vat?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@VatCode", ((object)Invoice.Vat?.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@VatDescription", ((object)Invoice.Vat?.Description) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@VatAmount", ((object)Invoice.Vat?.Amount) ?? DBNull.Value);

            insertCommand.Parameters.AddWithValue("@PdvType", ((object)Invoice.PdvType) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsSynced", Invoice.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)Invoice.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read

        public InvoiceListResponse GetInvoicesByPage(int companyId, InvoiceViewModel InvoiceSearchObject, int currentPage = 1, int itemsPerPage = 50)
        {
            InvoiceListResponse response = new InvoiceListResponse();
            List<InvoiceViewModel> Invoices = new List<InvoiceViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {

                    SqliteCommand selectCommand = new SqliteCommand(
                         SqlCommandSelectPart +
                         "FROM Invoices " +
                         "WHERE (@BuyerName IS NULL OR @BuyerName = '' OR BuyerName LIKE @BuyerName) " +
                         "AND (@InvoiceNumber IS NULL OR @InvoiceNumber = '' OR InvoiceNumber LIKE @InvoiceNumber) " +
                         "AND (@DateTo IS NULL OR @DateTo = '' OR DATE(InvoiceDate) <= DATE(@DateTo)) " +
                         "AND (@DateFrom IS NULL OR @DateFrom = '' OR DATE(InvoiceDate) >= DATE(@DateFrom)) " +
                         "AND CompanyId = @CompanyId " +
                         "ORDER BY IsSynced, Id DESC " +
                         "LIMIT @ItemsPerPage OFFSET @Offset;", db);

                    selectCommand.Parameters.AddWithValue("@BuyerName", ((object)InvoiceSearchObject.SearchBy_BusinessPartner) != null ? "%" + InvoiceSearchObject.SearchBy_BusinessPartner + "%" : "");
                    selectCommand.Parameters.AddWithValue("@InvoiceNumber", ((object)InvoiceSearchObject.SearchBy_InvoiceNumber) != null ? "%" + InvoiceSearchObject.SearchBy_InvoiceNumber + "%" : "");
                    selectCommand.Parameters.AddWithValue("@DateFrom", ((object)InvoiceSearchObject.SearchBy_InvoiceDateFrom) ?? "");
                    selectCommand.Parameters.AddWithValue("@DateTo", ((object)InvoiceSearchObject.SearchBy_InvoiceDateTo) ?? "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
                    selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        InvoiceViewModel dbEntry = Read(query);
                        Invoices.Add(dbEntry);
                    }


                    selectCommand = new SqliteCommand(
                        "SELECT Count(*) " +
                        "FROM Invoices " +
                        "WHERE(@BuyerName IS NULL OR @BuyerName = '' OR BuyerName LIKE @BuyerName) " +
                        "AND (@InvoiceNumber IS NULL OR @InvoiceNumber = '' OR InvoiceNumber LIKE @InvoiceNumber) " +
                        "AND CompanyId = @CompanyId;", db);

                    selectCommand.Parameters.AddWithValue("@BuyerName", ((object)InvoiceSearchObject.SearchBy_BusinessPartner) != null ? "%" + InvoiceSearchObject.SearchBy_BusinessPartner + "%" : "");
                    selectCommand.Parameters.AddWithValue("@InvoiceNumber", ((object)InvoiceSearchObject.SearchBy_InvoiceNumber) != null ? "%" + InvoiceSearchObject.SearchBy_InvoiceNumber + "%" : "");
                    selectCommand.Parameters.AddWithValue("@DateFrom", ((object)InvoiceSearchObject.SearchBy_InvoiceDateFrom) ?? "");
                    selectCommand.Parameters.AddWithValue("@DateTo", ((object)InvoiceSearchObject.SearchBy_InvoiceDateTo) ?? "");
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
                    response.Invoices = new List<InvoiceViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Invoices = Invoices;
            return response;
        }

        public InvoiceListResponse GetInvoicesForPopup(int companyId, string filterString)
        {
            InvoiceListResponse response = new InvoiceListResponse();
            List<InvoiceViewModel> Invoices = new List<InvoiceViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM Invoices " +
                        "WHERE (@InvoiceNumber IS NULL OR @InvoiceNumber = '' OR InvoiceNumber LIKE @InvoiceNumber) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage;", db);

                    selectCommand.Parameters.AddWithValue("@InvoiceNumber", ((object)filterString) != null ? "%" + filterString + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", ((object)filterString) != null ? companyId : 0);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", 100);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        InvoiceViewModel dbEntry = Read(query);
                        Invoices.Add(dbEntry);
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.Invoices = new List<InvoiceViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.Invoices = Invoices;
            return response;
        }

        #endregion

        #region Sync

        public void Sync(IInvoiceService invoiceService, Action<int, int> callback = null)
        {
            try
            {
                SyncInvoiceRequest request = new SyncInvoiceRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                InvoiceListResponse response = invoiceService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.Invoices?.Count ?? 0;
                    var items = new List<InvoiceViewModel>(response.Invoices);

                    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM Invoices WHERE Identifier = @Identifier";

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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from Invoices WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from Invoices WHERE CompanyId = @CompanyId", db);
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

        public InvoiceResponse Create(InvoiceViewModel invoice)
        {
            InvoiceResponse response = new InvoiceResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, invoice);
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

        public InvoiceResponse Delete(Guid identifier)
        {
            InvoiceResponse response = new InvoiceResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM Invoices WHERE Identifier = @Identifier";
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
