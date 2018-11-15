using Microsoft.Data.Sqlite;
using ServiceInterfaces.Messages.Common.OutputInvoices;
using ServiceInterfaces.ViewModels.Common.OutputInvoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
          "Description NVARCHAR(48) NULL, " +
          "IsSynced BOOL NULL, " +
          "UpdatedAt DATETIME NULL, " +
          "CreatedById INTEGER NULL, " +
          "CreatedByName NVARCHAR(2048) NULL, " +
          "CompanyId INTEGER NULL, " +
          "CompanyName NVARCHAR(2048) NULL)";

        public string SqlCommandSelectPart =
            "SELECT ServerId, Identifier, Code, " +
            "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
            "Supplier, Address, InvoiceNumber, InvoiceDate, AmountNet, PdvPercent, Pdv, AmountGross, Currency, DateOfPayment, Status, StatusDate, Description, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

        public string SqlCommandInsertPart = "INSERT INTO OutputInvoices " +
            "(Id, ServerId, Identifier, Code, " +
            "BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, " +
            "Supplier, Address, InvoiceNumber, InvoiceDate, AmountNet, PdvPercent, Pdv, AmountGross, Currency, DateOfPayment, Status, StatusDate, Description, " +
            "IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @Code, " +
            "@BusinessPartnerId, @BusinessPartnerIdentifier, @BusinessPartnerCode, @BusinessPartnerName, " +
            "@Supplier, @Address, @InvoiceNumber, @InvoiceDate, @AmountNet, @PdvPercent, @Pdv, @AmountGross, @Currency, @DateOfPayment, @Status, @StatusDate, @Description,  " +
            "@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        //public OutputInvoiceListResponse GetOutputInvoicesByPage(int companyId, OutputInvoiceViewModel OutputInvoiceSearchObject, int currentPage = 1, int itemsPerPage = 50)
        //{
        //    OutputInvoiceListResponse response = new OutputInvoiceListResponse();
        //    List<OutputInvoiceViewModel> OutputInvoices = new List<OutputInvoiceViewModel>();

        //    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
        //    {
        //        db.Open();
        //        try
        //        {
        //            SqliteCommand selectCommand = new SqliteCommand(
        //                SqlCommandSelectPart +
        //                "FROM OutputInvoices " +
        //                "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
        //                "AND (@DateTo IS NULL OR @DateTo = '' OR DATE(InvoiceDate) <= DATE(@DateTo)) " +
        //                "AND (@DateFrom IS NULL OR @DateFrom = '' OR DATE(InvoiceDate) >= DATE(@DateFrom)) " +
        //                "AND (@DateOfPaymentTo IS NULL OR @DateOfPaymentTo = '' OR DATE(InvoiceDate) <= DATE(@DateOfPaymentTo)) " +
        //                "AND (@DateOfPaymentFrom IS NULL OR @DateOfPaymentFrom = '' OR DATE(InvoiceDate) >= DATE(@DateOfPaymentFrom)) " +
        //                "AND CompanyId = @CompanyId " +
        //                "ORDER BY IsSynced, Id DESC " +
        //                "LIMIT @ItemsPerPage OFFSET @Offset;", db);
        //            selectCommand.Parameters.AddWithValue("@Supplier", ((object)OutputInvoiceSearchObject.SearchBy_Supplier) != null ? "%" + OutputInvoiceSearchObject.SearchBy_Supplier + "%" : "");
        //            selectCommand.Parameters.AddWithValue("@BusinessPartnerName", ((object)OutputInvoiceSearchObject.SearchBy_BusinessPartner) != null ? "%" + OutputInvoiceSearchObject.SearchBy_BusinessPartner + "%" : "");
        //            selectCommand.Parameters.AddWithValue("@InvoiceNumber", ((object)OutputInvoiceSearchObject.SearchBy_InvoiceNumber) != null ? "%" + OutputInvoiceSearchObject.SearchBy_InvoiceNumber + "%" : "");
        //            selectCommand.Parameters.AddWithValue("@DateFrom", ((object)OutputInvoiceSearchObject.SearchBy_InvoiceDateFrom) != null ? "%" + OutputInvoiceSearchObject.SearchBy_InvoiceDateFrom + "%" : "");
        //            selectCommand.Parameters.AddWithValue("@DateTo", ((object)OutputInvoiceSearchObject.SearchBy_InvoiceDateTo) != null ? "%" + OutputInvoiceSearchObject.SearchBy_InvoiceDateTo + "%" : "");
        //            selectCommand.Parameters.AddWithValue("@DateOfPaymentFrom", ((object)OutputInvoiceSearchObject.SearchBy_DateOfPaymentFrom) != null ? "%" + OutputInvoiceSearchObject.SearchBy_DateOfPaymentFrom + "%" : "");
        //            selectCommand.Parameters.AddWithValue("@DateOfPaymentTo", ((object)OutputInvoiceSearchObject.SearchBy_DateOfPaymentTo) != null ? "%" + OutputInvoiceSearchObject.SearchBy_DateOfPaymentTo + "%" : "");
        //            selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
        //            selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
        //            selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

        //            SqliteDataReader query = selectCommand.ExecuteReader();

        //            while (query.Read())
        //            {
        //                int counter = 0;
        //                FavorViewModel dbEntry = new FavorViewModel();
        //                dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
        //                dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
        //                dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
        //                dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
        //                dbEntry.Pdv = SQLiteHelper.GetInt(query, ref counter);
        //                dbEntry.UnitOfMeasurement = SQLiteHelper.GetUnitOfMeasurement(query, ref counter);
        //                dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
        //                dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
        //                dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
        //                dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
        //                Favors.Add(dbEntry);
        //            }


        //            selectCommand = new SqliteCommand(
        //                "SELECT Count(*) " +
        //                "FROM Favors " +
        //                "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
        //                "AND CompanyId = @CompanyId;", db);
        //            selectCommand.Parameters.AddWithValue("@Name", ((object)FavorSearchObject.Search_Name) != null ? "%" + FavorSearchObject.Search_Name + "%" : "");
        //            selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

        //            query = selectCommand.ExecuteReader();

        //            if (query.Read())
        //                response.TotalItems = query.GetInt32(0);
        //        }
        //        catch (SqliteException error)
        //        {
        //            MainWindow.ErrorMessage = error.Message;
        //            response.Success = false;
        //            response.Message = error.Message;
        //            response.Favors = new List<FavorViewModel>();
        //            return response;
        //        }
        //        db.Close();
        //    }
        //    response.Success = true;
        //    response.Favors = Favors;
        //    return response;
        //}

        //public FavorListResponse GetFavorsForPopup(int companyId, string filterString)
        //{
        //    FavorListResponse response = new FavorListResponse();
        //    List<FavorViewModel> Favors = new List<FavorViewModel>();

        //    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPFinancialDB.db"))
        //    {
        //        db.Open();
        //        try
        //        {
        //            SqliteCommand selectCommand = new SqliteCommand(
        //                SqlCommandSelectPart +
        //                "FROM Favors " +
        //                "WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name OR Code LIKE @Name) " +
        //                "AND CompanyId = @CompanyId " +
        //                "ORDER BY IsSynced, Id DESC " +
        //                "LIMIT @ItemsPerPage;", db);
        //            selectCommand.Parameters.AddWithValue("@Name", ((object)filterString) != null ? "%" + filterString + "%" : "");
        //            selectCommand.Parameters.AddWithValue("@CompanyId", ((object)filterString) != null ? companyId : 0);
        //            selectCommand.Parameters.AddWithValue("@ItemsPerPage", 100);

        //            SqliteDataReader query = selectCommand.ExecuteReader();

        //            while (query.Read())
        //            {
        //                int counter = 0;
        //                FavorViewModel dbEntry = new FavorViewModel();
        //                dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
        //                dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
        //                dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
        //                dbEntry.BussinesPartner = SQLiteHelper.GetBussinesPartner(query, ref counter);
        //                dbEntry.Supplier = SQLiteHelper.GetString(query, ref counter);
        //                dbEntry.Address = SQLiteHelper.GetString(query, ref counter);
        //                dbEntry.InvoiceNumber = SQLiteHelper.GetString(query, ref counter);
        //                dbEntry.InvoiceDate = SQLiteHelper.GetDateTime(query, ref counter);
        //                dbEntry.AmountNet = SQLiteHelper.GetDecimal(query, ref counter);
        //                dbEntry.PdvPercent = SQLiteHelper.GetInt(query, ref counter);
        //                dbEntry.Address = SQLiteHelper.GetString(query, ref counter);
        //                dbEntry.Address = SQLiteHelper.GetString(query, ref counter);

        //                dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
        //                dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
        //                dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
        //                dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
        //                Favors.Add(dbEntry);
        //            }
        //        }
        //        catch (SqliteException error)
        //        {
        //            MainWindow.ErrorMessage = error.Message;
        //            response.Success = false;
        //            response.Message = error.Message;
        //            response.Favors = new List<FavorViewModel>();
        //            return response;
        //        }
        //        db.Close();
        //    }
        //    response.Success = true;
        //    response.Favors = Favors;
        //    return response;
        //}

        //public FavorResponse GetFavor(Guid identifier)
        //{
        //    FavorResponse response = new FavorResponse();
        //    FavorViewModel Favor = new FavorViewModel();

        //    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPFinancialDB.db"))
        //    {
        //        db.Open();
        //        try
        //        {
        //            SqliteCommand selectCommand = new SqliteCommand(
        //                SqlCommandSelectPart +
        //                "FROM Favors " +
        //                "WHERE Identifier = @Identifier;", db);
        //            selectCommand.Parameters.AddWithValue("@Identifier", identifier);

        //            SqliteDataReader query = selectCommand.ExecuteReader();

        //            if (query.Read())
        //            {
        //                int counter = 0;
        //                FavorViewModel dbEntry = new FavorViewModel();
        //                dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
        //                dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
        //                dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
        //                dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
        //                dbEntry.Pdv = SQLiteHelper.GetInt(query, ref counter);
        //                dbEntry.UnitOfMeasurement = SQLiteHelper.GetUnitOfMeasurement(query, ref counter);
        //                dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
        //                dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
        //                dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
        //                dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
        //                Favor = dbEntry;
        //            }
        //        }
        //        catch (SqliteException error)
        //        {
        //            MainWindow.ErrorMessage = error.Message;
        //            response.Success = false;
        //            response.Message = error.Message;
        //            response.Favor = new FavorViewModel();
        //            return response;
        //        }
        //        db.Close();
        //    }
        //    response.Success = true;
        //    response.Favor = Favor;
        //    return response;
        //}

        //public void Sync(IFavorService favorService)
        //{
        //    SyncFavorRequest request = new SyncFavorRequest();
        //    request.CompanyId = MainWindow.CurrentCompanyId;
        //    request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

        //    FavorListResponse response = favorService.Sync(request);
        //    if (response.Success)
        //    {
        //        List<FavorViewModel> favorsFromDB = response.Favors;
        //        foreach (var favor in favorsFromDB.OrderBy(x => x.Id))
        //        {
        //            Delete(favor.Identifier);
        //            favor.IsSynced = true;
        //            Create(favor);
        //        }
        //    }
        //}

        //public DateTime? GetLastUpdatedAt(int companyId)
        //{
        //    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPFinancialDB.db"))
        //    {
        //        db.Open();
        //        try
        //        {
        //            SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from Favors WHERE CompanyId = @CompanyId", db);
        //            selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
        //            SqliteDataReader query = selectCommand.ExecuteReader();
        //            int count = query.Read() ? query.GetInt32(0) : 0;

        //            if (count == 0)
        //                return null;
        //            else
        //            {
        //                selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from Favors WHERE CompanyId = @CompanyId", db);
        //                selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
        //                query = selectCommand.ExecuteReader();
        //                if (query.Read())
        //                {
        //                    return query.GetDateTime(0);
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MainWindow.ErrorMessage = ex.Message;
        //        }
        //        db.Close();
        //    }
        //    return null;
        //}

        //public FavorResponse Create(FavorViewModel Favor)
        //{
        //    FavorResponse response = new FavorResponse();

        //    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPFinancialDB.db"))
        //    {
        //        db.Open();

        //        SqliteCommand insertCommand = new SqliteCommand();
        //        insertCommand.Connection = db;

        //        //Use parameterized query to prevent SQL injection attacks
        //        insertCommand.CommandText = SqlCommandInsertPart;

        //        insertCommand.Parameters.AddWithValue("@ServerId", Favor.Id);
        //        insertCommand.Parameters.AddWithValue("@Identifier", Favor.Identifier);
        //        insertCommand.Parameters.AddWithValue("@Code", ((object)Favor.Code) ?? DBNull.Value);
        //        insertCommand.Parameters.AddWithValue("@Name", ((object)Favor.Name) ?? DBNull.Value);
        //        insertCommand.Parameters.AddWithValue("@Pdv", ((object)Favor.Pdv) ?? DBNull.Value);
        //        insertCommand.Parameters.AddWithValue("@UnitOfMeasurementId", ((object)Favor.UnitOfMeasurement?.Id) ?? DBNull.Value);
        //        insertCommand.Parameters.AddWithValue("@UnitOfMeasurementIdentifier", ((object)Favor.UnitOfMeasurement?.Identifier) ?? DBNull.Value);
        //        insertCommand.Parameters.AddWithValue("@UnitOfMeasurementCode", ((object)Favor.UnitOfMeasurement?.Code) ?? DBNull.Value);
        //        insertCommand.Parameters.AddWithValue("@UnitOfMeasurementName", ((object)Favor.UnitOfMeasurement?.ShortName) ?? DBNull.Value);
        //        insertCommand.Parameters.AddWithValue("@IsSynced", Favor.IsSynced);
        //        insertCommand.Parameters.AddWithValue("@UpdatedAt", Favor.UpdatedAt);
        //        insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
        //        insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
        //        insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
        //        insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

        //        try
        //        {
        //            insertCommand.ExecuteReader();
        //        }
        //        catch (SqliteException error)
        //        {
        //            MainWindow.ErrorMessage = error.Message;
        //            response.Success = false;
        //            response.Message = error.Message;
        //            return response;
        //        }
        //        db.Close();

        //        response.Success = true;
        //        return response;
        //    }
        //}

        //public FavorResponse UpdateSyncStatus(Guid identifier, int serverId, bool isSynced)
        //{
        //    FavorResponse response = new FavorResponse();

        //    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPFinancialDB.db"))
        //    {
        //        db.Open();

        //        SqliteCommand insertCommand = new SqliteCommand();
        //        insertCommand.Connection = db;

        //        insertCommand.CommandText = "UPDATE Favors SET " +
        //            "IsSynced = @IsSynced, " +
        //            "ServerId = @ServerId " +
        //            "WHERE Identifier = @Identifier ";

        //        insertCommand.Parameters.AddWithValue("@IsSynced", isSynced);
        //        insertCommand.Parameters.AddWithValue("@Identifier", identifier);
        //        insertCommand.Parameters.AddWithValue("@ServerId", serverId);

        //        try
        //        {
        //            insertCommand.ExecuteReader();
        //        }
        //        catch (SqliteException error)
        //        {
        //            MainWindow.ErrorMessage = error.Message;
        //            response.Success = false;
        //            response.Message = error.Message;
        //            return response;
        //        }
        //        db.Close();

        //        response.Success = true;
        //        return response;
        //    }
        //}

        //public FavorResponse Delete(Guid identifier)
        //{
        //    FavorResponse response = new FavorResponse();

        //    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPFinancialDB.db"))
        //    {
        //        db.Open();

        //        SqliteCommand insertCommand = new SqliteCommand();
        //        insertCommand.Connection = db;

        //        //Use parameterized query to prevent SQL injection attacks
        //        insertCommand.CommandText =
        //            "DELETE FROM Favors WHERE Identifier = @Identifier";
        //        insertCommand.Parameters.AddWithValue("@Identifier", identifier);
        //        try
        //        {
        //            insertCommand.ExecuteReader();
        //        }
        //        catch (SqliteException error)
        //        {
        //            MainWindow.ErrorMessage = error.Message;
        //            response.Success = false;
        //            response.Message = error.Message;
        //            return response;
        //        }
        //        db.Close();

        //        response.Success = true;
        //        return response;
        //    }
        //}

        //public FavorResponse DeleteAll()
        //{
        //    FavorResponse response = new FavorResponse();

        //    try
        //    {
        //        using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPFinancialDB.db"))
        //        {
        //            db.Open();
        //            db.EnableExtensions(true);

        //            SqliteCommand insertCommand = new SqliteCommand();
        //            insertCommand.Connection = db;

        //            //Use parameterized query to prevent SQL injection attacks
        //            insertCommand.CommandText = "DELETE FROM Favors";
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
    }
}
