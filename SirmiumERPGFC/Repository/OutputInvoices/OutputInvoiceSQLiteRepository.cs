using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.OutputInvoices;
using ServiceInterfaces.Messages.Common.OutputInvoices;
using ServiceInterfaces.ViewModels.Common.OutputInvoices;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.OutputInvoices
{
    public class OutputInvoiceSQLiteRepository
    {
        public static string OutputInvoiceTableCreatePart =
          "CREATE TABLE IF NOT EXISTS OutputInvoices " +
          "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
          "ServerId INTEGER NULL, " +
          "Identifier GUID, " +
          "Code NVARCHAR(48) NULL, " +
          "BusinessPartnerId INTEGER NULL, " +
          "BusinessPartnerIdentifier GUID NULL, " +
          "BusinessPartnerCode NVARCHAR(48) NULL, " +
          "BusinessPartnerName NVARCHAR(48) NULL, " +
               "BusinessPartnerInternalCode NVARCHAR(2048) NULL, " +
               "BusinessPartnerNameGer NVARCHAR(2048) NULL, " +
          "Supplier NVARCHAR(48) NULL, " +
          "Address NVARCHAR(48) NULL, " +
          "InvoiceNumber NVARCHAR(48) NULL, " +
          "InvoiceDate DATETIME NULL, " +
          "AmountNet DECIMAL NULL, " +
          "PdvPercent INTEGER NULL, " +
          "Pdv DECIMAL NULL, " +
          "AmountGross DECIMAL NULL, " +
          "Currency DECIMAL NULL, " +
          "DateOfPayment DATETIME NULL, " +
          "Status NVARCHAR(48) NULL, " +
          "StatusDate DATETIME NULL, " +
          "Description NVARCHAR(2048) NULL, " +
          "Path NVARCHAR(2048) NULL, " +
          "IsSynced BOOL NULL, " +
          "UpdatedAt DATETIME NULL, " +
          "CreatedById INTEGER NULL, " +
          "CreatedByName NVARCHAR(2048) NULL, " +
          "CompanyId INTEGER NULL, " +
          "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, Code, " +
            "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, BusinessPartnerInternalCode, BusinessPartnerNameGer, " +
            "Supplier, Address, InvoiceNumber, InvoiceDate, AmountNet, PdvPercent, Pdv, AmountGross, Currency, DateOfPayment, Status, StatusDate, Description, Path, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO OutputInvoices " +
            "(Id, ServerId, Identifier, Code, " +
            "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, BusinessPartnerInternalCode, BusinessPartnerNameGer, " +
            "Supplier, Address, InvoiceNumber, InvoiceDate, AmountNet, PdvPercent, Pdv, AmountGross, Currency, DateOfPayment, Status, StatusDate, Description, Path, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @Code, " +
            "@BusinessPartnerId, @BusinessPartnerIdentifier, @BusinessPartnerCode, @BusinessPartnerName, @BusinessPartnerInternalCode, @BusinessPartnerNameGer, " +
            "@Supplier, @Address, @InvoiceNumber, @InvoiceDate, @AmountNet, @PdvPercent, @Pdv, @AmountGross, @Currency, @DateOfPayment, @Status, @StatusDate, @Description, @Path, " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #region Helper methods

        private static OutputInvoiceViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            OutputInvoiceViewModel dbEntry = new OutputInvoiceViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
            dbEntry.BusinessPartner = SQLiteHelper.GetBusinessPartner(query, ref counter);
            dbEntry.Supplier = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Address = SQLiteHelper.GetString(query, ref counter);
            dbEntry.InvoiceNumber = SQLiteHelper.GetString(query, ref counter);
            dbEntry.InvoiceDate = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.AmountNet = SQLiteHelper.GetDecimal(query, ref counter);
            dbEntry.PdvPercent = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Pdv = SQLiteHelper.GetDecimal(query, ref counter);
            dbEntry.AmountGross = SQLiteHelper.GetDecimal(query, ref counter);
            dbEntry.Currency = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.DateOfPayment = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.Status = SQLiteHelper.GetString(query, ref counter);
            dbEntry.StatusDate = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.Description = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Path = SQLiteHelper.GetString(query, ref counter);

            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);

            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
            return dbEntry;
        }
        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, OutputInvoiceViewModel OutputInvoice)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", OutputInvoice.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", OutputInvoice.Identifier);
            insertCommand.Parameters.AddWithValue("@Code", ((object)OutputInvoice.Code) ?? DBNull.Value);

            insertCommand.Parameters.AddWithValue("@BusinessPartnerId", ((object)OutputInvoice.BusinessPartner?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", ((object)OutputInvoice.BusinessPartner?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerCode", ((object)OutputInvoice.BusinessPartner?.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerName", ((object)OutputInvoice.BusinessPartner?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerInternalCode", ((object)OutputInvoice.BusinessPartner?.InternalCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@BusinessPartnerNameGer", ((object)OutputInvoice.BusinessPartner?.NameGer) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Supplier", ((object)OutputInvoice.Supplier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Address", ((object)OutputInvoice.Address) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@InvoiceNumber", ((object)OutputInvoice.InvoiceNumber) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@InvoiceDate", ((object)OutputInvoice.InvoiceDate) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@AmountNet", ((object)OutputInvoice.AmountNet) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PDVPercent", ((object)OutputInvoice.PdvPercent) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@PDV", ((object)OutputInvoice.Pdv) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@AmountGross", ((object)OutputInvoice.AmountGross) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Currency", ((object)OutputInvoice.Currency) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@DateOfPaymet", ((object)OutputInvoice.DateOfPayment) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Status", ((object)OutputInvoice.Status) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@StatusDate", ((object)OutputInvoice.StatusDate) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Description", ((object)OutputInvoice.Description) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Path", ((object)OutputInvoice.Path) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsSynced", OutputInvoice.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)OutputInvoice.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion
        public OutputInvoiceListResponse GetOutputInvoicesByPage(int companyId, OutputInvoiceViewModel OutputInvoiceSearchObject, int currentPage = 1, int itemsPerPage = 50)
        {
            OutputInvoiceListResponse response = new OutputInvoiceListResponse();
            List<OutputInvoiceViewModel> OutputInvoices = new List<OutputInvoiceViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {

                   SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM OutputInvoices " +
                        "WHERE (@Supplier IS NULL OR @Supplier = '' OR Supplier LIKE @Supplier) " +
                        "AND (@BusinessPartnerName IS NULL OR @BusinessPartnerName = '' OR BusinessPartnerName LIKE @BusinessPartnerName) " +
                        "AND (@InvoiceNumber IS NULL OR @InvoiceNumber = '' OR InvoiceNumber LIKE @InvoiceNumber) " +
                        "AND (@DateTo IS NULL OR @DateTo = '' OR DATE(InvoiceDate) <= DATE(@DateTo)) " +
                        "AND (@DateFrom IS NULL OR @DateFrom = '' OR DATE(InvoiceDate) >= DATE(@DateFrom)) " +
                        "AND (@DateOfPaymentTo IS NULL OR @DateOfPaymentTo = '' OR DATE(DateOfPayment) <=  DATE(@DateOfPaymentTo)) " +
                        "AND (@DateOfPaymentFrom IS NULL OR @DateOfPaymentFrom = '' OR DATE(DateOfPayment) >= DATE(@DateOfPaymentFrom)) " +
                        "AND CompanyId = @CompanyId " +
                        "ORDER BY IsSynced, Id DESC " +
                        "LIMIT @ItemsPerPage OFFSET @Offset;", db);
                    selectCommand.Parameters.AddWithValue("@Supplier", ((object)OutputInvoiceSearchObject.SearchBy_Supplier) != null ? "%" + OutputInvoiceSearchObject.SearchBy_Supplier + "%" : "");
                    selectCommand.Parameters.AddWithValue("@BusinessPartnerName", ((object)OutputInvoiceSearchObject.SearchBy_BusinessPartner) != null ? "%" + OutputInvoiceSearchObject.SearchBy_BusinessPartner + "%" : "");
                    selectCommand.Parameters.AddWithValue("@InvoiceNumber", ((object)OutputInvoiceSearchObject.SearchBy_InvoiceNumber) != null ? "%" + OutputInvoiceSearchObject.SearchBy_InvoiceNumber + "%" : "");
                    selectCommand.Parameters.AddWithValue("@DateFrom", ((object)OutputInvoiceSearchObject.SearchBy_InvoiceDateFrom) ?? "");
                    selectCommand.Parameters.AddWithValue("@DateTo", ((object)OutputInvoiceSearchObject.SearchBy_InvoiceDateTo) ?? "");
                    selectCommand.Parameters.AddWithValue("@DateOfPaymentFrom", ((object)OutputInvoiceSearchObject.SearchBy_DateOfPaymentFrom) ?? "");
                    selectCommand.Parameters.AddWithValue("@DateOfPaymentTo", ((object)OutputInvoiceSearchObject.SearchBy_DateOfPaymentTo) ?? "");
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
                    selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        OutputInvoiceViewModel dbEntry = Read(query);
                        OutputInvoices.Add(dbEntry);
                    }


                    selectCommand = new SqliteCommand(
                        "SELECT Count(*) " +
                        "FROM OutputInvoices " +
                        "WHERE (@Supplier IS NULL OR @Supplier = '' OR Supplier LIKE @Supplier) " +
                        "AND (@BusinessPartnerName IS NULL OR @BusinessPartnerName = '' OR BusinessPartnerName LIKE @BusinessPartnerName) " +
                        "AND (@InvoiceNumber IS NULL OR @InvoiceNumber = '' OR InvoiceNumber LIKE @InvoiceNumber) " +
                        "AND (@DateFrom IS NULL OR @DateFrom = '' OR DATE(InvoiceDate) >= DATE(@DateFrom)) " +
                        "AND (@DateTo IS NULL OR @DateTo = '' OR DATE(InvoiceDate) <= DATE(@DateTo)) " +
                        "AND (@DateOfPaymentFrom IS NULL OR @DateOfPaymentFrom = '' OR DATE(DateOfPayment) >= DATE(@DateOfPaymentFrom)) " +
                        "AND (@DateOfPaymentTo IS NULL OR @DateOfPaymentTo = '' OR DATE(DateOfPayment) <= DATE(@DateOfPaymentTo)) " +
                        "AND CompanyId = @CompanyId;", db);
                    selectCommand.Parameters.AddWithValue("@Supplier", ((object)OutputInvoiceSearchObject.SearchBy_Supplier) != null ? "%" + OutputInvoiceSearchObject.SearchBy_Supplier + "%" : "");
                    selectCommand.Parameters.AddWithValue("@BusinessPartnerName", ((object)OutputInvoiceSearchObject.SearchBy_BusinessPartner) != null ? "%" + OutputInvoiceSearchObject.SearchBy_BusinessPartner + "%" : "");
                    selectCommand.Parameters.AddWithValue("@InvoiceNumber", ((object)OutputInvoiceSearchObject.SearchBy_InvoiceNumber) != null ? "%" + OutputInvoiceSearchObject.SearchBy_InvoiceNumber + "%" : "");
                    selectCommand.Parameters.AddWithValue("@DateFrom", ((object)OutputInvoiceSearchObject.SearchBy_InvoiceDateFrom) ?? "");
                    selectCommand.Parameters.AddWithValue("@DateTo", ((object)OutputInvoiceSearchObject.SearchBy_InvoiceDateTo) ?? "");
                    selectCommand.Parameters.AddWithValue("@DateOfPaymentFrom", ((object)OutputInvoiceSearchObject.SearchBy_DateOfPaymentFrom) ?? "");
                    selectCommand.Parameters.AddWithValue("@DateOfPaymentTo", ((object)OutputInvoiceSearchObject.SearchBy_DateOfPaymentTo) ?? "");
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
                    response.OutputInvoices = new List<OutputInvoiceViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.OutputInvoices = OutputInvoices;
            return response;
        }

        public OutputInvoiceListResponse GetOutputInvoicesForPopup(int companyId, string filterString)
        {
            OutputInvoiceListResponse response = new OutputInvoiceListResponse();
            List<OutputInvoiceViewModel> OutputInvoices = new List<OutputInvoiceViewModel>();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM OutputInvoices " +
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
                        int counter = 0;
                        OutputInvoiceViewModel dbEntry = new OutputInvoiceViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.BusinessPartner = SQLiteHelper.GetBusinessPartner(query, ref counter);
                        dbEntry.Supplier = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Address = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.InvoiceNumber = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.InvoiceDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.AmountNet = SQLiteHelper.GetDecimal(query, ref counter);
                        dbEntry.PdvPercent = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Pdv = SQLiteHelper.GetDecimal(query, ref counter);
                        dbEntry.AmountGross = SQLiteHelper.GetDecimal(query, ref counter);
                        dbEntry.Currency = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.DateOfPayment = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.Status = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.StatusDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.Description = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Path = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        OutputInvoices.Add(dbEntry);
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.OutputInvoices = new List<OutputInvoiceViewModel>();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.OutputInvoices = OutputInvoices;
            return response;
        }

        public OutputInvoiceResponse GetOutputInvoice(Guid identifier)
        {
            OutputInvoiceResponse response = new OutputInvoiceResponse();
            OutputInvoiceViewModel OutputInvoice = new OutputInvoiceViewModel();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();
                try
                {
                    SqliteCommand selectCommand = new SqliteCommand(
                        SqlCommandSelectPart +
                        "FROM OutputInvoices " +
                        "WHERE Identifier = @Identifier;", db);
                    selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        int counter = 0;
                        OutputInvoiceViewModel dbEntry = new OutputInvoiceViewModel();
                        dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
                        dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.BusinessPartner = SQLiteHelper.GetBusinessPartner(query, ref counter);
                        dbEntry.Supplier = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Address = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.InvoiceNumber = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.InvoiceDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.AmountNet = SQLiteHelper.GetDecimal(query, ref counter);
                        dbEntry.PdvPercent = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.Pdv = SQLiteHelper.GetDecimal(query, ref counter);
                        dbEntry.AmountGross = SQLiteHelper.GetDecimal(query, ref counter);
                        dbEntry.Currency = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.DateOfPayment = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.Status = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.StatusDate = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.Description = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Path = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
                        dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
                        dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
                        dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
                        OutputInvoice = dbEntry;
                    }
                }
                catch (SqliteException error)
                {
                    MainWindow.ErrorMessage = error.Message;
                    response.Success = false;
                    response.Message = error.Message;
                    response.OutputInvoice = new OutputInvoiceViewModel();
                    return response;
                }
                db.Close();
            }
            response.Success = true;
            response.OutputInvoice = OutputInvoice;
            return response;
        }

        public void Sync(IOutputInvoiceService outputInvoiceService, Action<int, int> callback = null)
        {
            try
            {
                SyncOutputInvoiceRequest request = new SyncOutputInvoiceRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                OutputInvoiceListResponse response = outputInvoiceService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.OutputInvoices?.Count ?? 0;
                    List<OutputInvoiceViewModel> outputInvoicesFromDB = response.OutputInvoices;
                    foreach (var outputInvoice in outputInvoicesFromDB.OrderBy(x => x.Id))
                    {
                            Delete(outputInvoice.Identifier);
                            if (outputInvoice.IsActive)
                            {
                                outputInvoice.IsSynced = true;
                                Create(outputInvoice);
                                syncedItems++;
                                callback?.Invoke(syncedItems, toSync);
                            }
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
                    SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from OutputInvoices WHERE CompanyId = @CompanyId", db);
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    int count = query.Read() ? query.GetInt32(0) : 0;

                    if (count == 0)
                        return null;
                    else
                    {
                        selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from OutputInvoices WHERE CompanyId = @CompanyId", db);
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

        public OutputInvoiceResponse Create(OutputInvoiceViewModel outputInvoice)
        {
            OutputInvoiceResponse response = new OutputInvoiceResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = SqlCommandInsertPart;

                insertCommand.Parameters.AddWithValue("@ServerId", outputInvoice.Id);
                insertCommand.Parameters.AddWithValue("@Identifier", outputInvoice.Identifier);
                insertCommand.Parameters.AddWithValue("@Code", ((object)outputInvoice.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerId", ((object)outputInvoice.BusinessPartner?.Id) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", ((object)outputInvoice.BusinessPartner?.Identifier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerCode", ((object)outputInvoice.BusinessPartner?.Code) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerName", ((object)outputInvoice.BusinessPartner?.Name) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerInternalCode", ((object)outputInvoice.BusinessPartner?.InternalCode) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@BusinessPartnerNameGer", ((object)outputInvoice.BusinessPartner?.NameGer) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Supplier", ((object)outputInvoice.Supplier) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Address", ((object)outputInvoice.Address) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@InvoiceNumber", ((object)outputInvoice.InvoiceNumber) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@InvoiceDate", ((object)outputInvoice.InvoiceDate) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@AmountNet", ((object)outputInvoice.AmountNet) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@PdvPercent", ((object)outputInvoice.PdvPercent) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Pdv", ((object)outputInvoice.Pdv) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@AmountGross", ((object)outputInvoice.AmountGross) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Currency", ((object)outputInvoice.Currency) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@DateOfPayment", ((object)outputInvoice.DateOfPayment) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Status", ((object)outputInvoice.Status) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@StatusDate", ((object)outputInvoice.StatusDate) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Description", ((object)outputInvoice.Description) ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@Path", ((object)outputInvoice.Path) ?? DBNull.Value);

                insertCommand.Parameters.AddWithValue("@IsSynced", outputInvoice.IsSynced);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)outputInvoice.UpdatedAt) ?? DBNull.Value);
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

        public OutputInvoiceResponse UpdateSyncStatus(Guid identifier, string code, DateTime? updatedAt, int serverId, bool isSynced)
        {
            OutputInvoiceResponse response = new OutputInvoiceResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "UPDATE OutputInvoices SET " +
                    "IsSynced = @IsSynced, " +
                    "Code = @Code, " +
                    "UpdatedAt = @UpdatedAt, " +
                    "ServerId = @ServerId " +
                    "WHERE Identifier = @Identifier ";

                insertCommand.Parameters.AddWithValue("@IsSynced", isSynced);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", updatedAt);
                insertCommand.Parameters.AddWithValue("@Code", code);
                insertCommand.Parameters.AddWithValue("@Identifier", identifier);
                insertCommand.Parameters.AddWithValue("@ServerId", serverId);

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

        public OutputInvoiceResponse Delete(Guid identifier)
        {
            OutputInvoiceResponse response = new OutputInvoiceResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "DELETE FROM OutputInvoices WHERE Identifier = @Identifier";
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

        public OutputInvoiceResponse DeleteAll()
        {
            OutputInvoiceResponse response = new OutputInvoiceResponse();

            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                {
                    db.Open();
                    db.EnableExtensions(true);

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "DELETE FROM OutputInvoices";
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
