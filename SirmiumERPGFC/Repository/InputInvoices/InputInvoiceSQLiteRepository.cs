using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.InputInvoices;
using ServiceInterfaces.Messages.Common.InputInvoices;
using ServiceInterfaces.ViewModels.Common.InputInvoices;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.InputInvoices
{
	public class InputInvoiceSQLiteRepository
	{
		public static string InputInvoiceTableCreatePart =
		  "CREATE TABLE IF NOT EXISTS InputInvoices " +
		  "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
		  "ServerId INTEGER NULL, " +
		  "Identifier GUID, " +
		  "Code NVARCHAR(48) NULL, " +
		  "BusinessPartnerId INTEGER NULL, " +
		  "BusinessPartnerIdentifier GUID NULL, " +
		  "BusinessPartnerCode NVARCHAR(48) NULL, " +
		  "BusinessPartnerName NVARCHAR(2048) NULL, " +
               "BusinessPartnerInternalCode NVARCHAR(2048) NULL, " +
               "BusinessPartnerNameGer NVARCHAR(2048) NULL, " +
          "Supplier NVARCHAR(48) NULL, " +
		  "Address NVARCHAR(48) NULL, " +
		  "InvoiceNumber NVARCHAR(48) NULL, " +
		  "InvoiceDate DATETIME NULL, " +
		  "AmountNet DECIMAL(18, 2) NULL, " +
		  "PDVPercent INTEGER NULL, " +
		  "PDV DECIMAL(18, 2) NULL, " +
		  "AmountGross DECIMAL(18, 2) NULL, " +
		  "Currency DECIMAL(18, 2) NULL, " +
		  "DateOfPaymet DATETIME NULL, " +
		  "Status INTEGER NULL, " +
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
            "SELECT ServerId, Identifier, Code, BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, BusinessPartnerInternalCode, BusinessPartnerNameGer, " +
			"Supplier, Address, InvoiceNumber, InvoiceDate, AmountNet, PDVPercent, PDV, " +
			"AmountGross, Currency, DateOfPaymet, Status, StatusDate, Description, Path, " +
			"IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

		public string SqlCommandInsertPart = "INSERT INTO InputInvoices " +
            "(Id, ServerId, Identifier, Code, BusinessPartnerId, BusinessPartnerIdentifier, BusinessPartnerCode, BusinessPartnerName, BusinessPartnerInternalCode, BusinessPartnerNameGer, " +
			"Supplier, Address, InvoiceNumber, InvoiceDate, AmountNet, PDVPercent, PDV, " +
			"AmountGross, Currency, DateOfPaymet, Status, StatusDate, Description, Path, " +
			"IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @Code, @BusinessPartnerId, @BusinessPartnerIdentifier, @BusinessPartnerCode, @BusinessPartnerName, @BusinessPartnerInternalCode, @BusinessPartnerNameGer, " +
			"@Supplier, @Address, @InvoiceNumber, @InvoiceDate, @AmountNet, @PDVPercent, @PDV, " +
			"@AmountGross, @Currency, @DateOfPaymet, @Status, @StatusDate, @Description, @Path, " +
			"@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

		public InputInvoiceListResponse GetInputInvoicesByPage(int companyId, InputInvoiceViewModel InputInvoiceSearchObject, int currentPage = 1, int itemsPerPage = 50)
		{
			InputInvoiceListResponse response = new InputInvoiceListResponse();
			List<InputInvoiceViewModel> InputInvoices = new List<InputInvoiceViewModel>();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
				db.Open();
				try
				{
					SqliteCommand selectCommand = new SqliteCommand(
						SqlCommandSelectPart +
						"FROM InputInvoices " +
						"WHERE (@BusinessPartnerName IS NULL OR @BusinessPartnerName = '' OR BusinessPartnerName LIKE @BusinessPartnerName) " +
						"AND (@Supplier IS NULL OR @Supplier = '' OR Supplier LIKE @Supplier)  " +
						"AND (@InvoiceNumber IS NULL OR @InvoiceNumber = '' OR InvoiceNumber LIKE @InvoiceNumber)  " +
						"AND (@InvoiceDateTo IS NULL OR @InvoiceDateTo = '' OR DATE(InvoiceDate) <= DATE(@InvoiceDateTo)) " +
						"AND (@InvoiceDateFrom IS NULL OR @InvoiceDateFrom = '' OR DATE(InvoiceDate) >= DATE(@InvoiceDateFrom)) " +
						"AND (@DateOfPaymetTo IS NULL OR @DateOfPaymetTo = '' OR DATE(DateOfPaymet) <= DATE(@DateOfPaymetTo)) " +
						"AND (@DateOfPaymetFrom IS NULL OR @DateOfPaymetFrom = '' OR DATE(DateOfPaymet) >= DATE(@DateOfPaymetFrom)) " +
						"AND CompanyId = @CompanyId " +
						"ORDER BY IsSynced, Id DESC " +
						"LIMIT @ItemsPerPage OFFSET @Offset;", db);
					selectCommand.Parameters.AddWithValue("@BusinessPartnerName", ((object)InputInvoiceSearchObject.SearchBy_BusinessPartnerName) != null ? "%" + InputInvoiceSearchObject.SearchBy_BusinessPartnerName + "%" : "");
					selectCommand.Parameters.AddWithValue("@Supplier", ((object)InputInvoiceSearchObject.SearchBy_Supplier) != null ? "%" + InputInvoiceSearchObject.SearchBy_Supplier + "%" : "");

					selectCommand.Parameters.AddWithValue("@InvoiceNumber", ((object)InputInvoiceSearchObject.SearchBy_InvoiceNumber) != null ? "%" + InputInvoiceSearchObject.SearchBy_InvoiceNumber + "%" : "");
                    selectCommand.Parameters.AddWithValue("@InvoiceDateFrom", ((object)InputInvoiceSearchObject.SearchBy_InvoiceDateFrom) != null ? (object)InputInvoiceSearchObject.SearchBy_InvoiceDateFrom : DBNull.Value);
					selectCommand.Parameters.AddWithValue("@InvoiceDateTo", ((object)InputInvoiceSearchObject.SearchBy_InvoiceDateTo) != null ? (object)InputInvoiceSearchObject.SearchBy_InvoiceDateTo : DBNull.Value);
					selectCommand.Parameters.AddWithValue("@DateOfPaymetFrom", ((object)InputInvoiceSearchObject.SearchBy_DateOfPaymetFrom) != null ? (object)InputInvoiceSearchObject.SearchBy_DateOfPaymetFrom : DBNull.Value);
					selectCommand.Parameters.AddWithValue("@DateOfPaymetTo", ((object)InputInvoiceSearchObject.SearchBy_DateOfPaymetTo) != null ? (object)InputInvoiceSearchObject.SearchBy_DateOfPaymetTo : DBNull.Value);

					selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
					selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
					selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

					SqliteDataReader query = selectCommand.ExecuteReader();

					while (query.Read())
					{
						int counter = 0;
						InputInvoiceViewModel dbEntry = new InputInvoiceViewModel();
						dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
						dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
						dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
						dbEntry.BusinessPartner = SQLiteHelper.GetBusinessPartner(query, ref counter);
						dbEntry.Supplier = SQLiteHelper.GetString(query, ref counter);
						dbEntry.Address = SQLiteHelper.GetString(query, ref counter);
						dbEntry.InvoiceNumber = SQLiteHelper.GetString(query, ref counter);
						dbEntry.InvoiceDate = SQLiteHelper.GetDateTime(query, ref counter);
						dbEntry.AmountNet = SQLiteHelper.GetDecimal(query, ref counter);
						dbEntry.PDVPercent = SQLiteHelper.GetInt(query, ref counter);
						dbEntry.PDV = SQLiteHelper.GetDecimal(query, ref counter);
						dbEntry.AmountGross = SQLiteHelper.GetDecimal(query, ref counter);
						dbEntry.Currency = SQLiteHelper.GetInt(query, ref counter);
						dbEntry.DateOfPaymet = SQLiteHelper.GetDateTime(query, ref counter);
						dbEntry.Status = SQLiteHelper.GetString(query, ref counter);
						dbEntry.StatusDate = SQLiteHelper.GetDateTime(query, ref counter);
						dbEntry.Description = SQLiteHelper.GetString(query, ref counter);
						dbEntry.Path = SQLiteHelper.GetString(query, ref counter);
						dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
						dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
						dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
						dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
						InputInvoices.Add(dbEntry);
					}


					selectCommand = new SqliteCommand(
						"SELECT Count(*) " +
						"FROM InputInvoices " +
						"WHERE (@BusinessPartnerName IS NULL OR @BusinessPartnerName = '' OR BusinessPartnerName LIKE @BusinessPartnerName) " +
						"AND (@Supplier IS NULL OR @Supplier = '' OR Supplier LIKE @Supplier)  " +
						"AND (@InvoiceNumber IS NULL OR @InvoiceNumber = '' OR InvoiceNumber LIKE @InvoiceNumber)  " +
						"AND (@InvoiceDateTo IS NULL OR @InvoiceDateTo = '' OR DATE(InvoiceDate) <= DATE(@InvoiceDateTo)) " +
						"AND (@InvoiceDateFrom IS NULL OR @InvoiceDateFrom = '' OR DATE(InvoiceDate) >= DATE(@InvoiceDateFrom)) " +
						"AND (@DateOfPaymetTo IS NULL OR @DateOfPaymetTo = '' OR DATE(DateOfPaymet) <= DATE(@DateOfPaymetTo)) " +
						"AND (@DateOfPaymetFrom IS NULL OR @DateOfPaymetFrom = '' OR DATE(DateOfPaymet) >= DATE(@DateOfPaymetFrom)) " +
						"AND CompanyId = @CompanyId;", db);
					selectCommand.Parameters.AddWithValue("@BusinessPartnerName", ((object)InputInvoiceSearchObject.SearchBy_BusinessPartnerName) != null ? "%" + InputInvoiceSearchObject.SearchBy_BusinessPartnerName + "%" : "");
					selectCommand.Parameters.AddWithValue("@Supplier", ((object)InputInvoiceSearchObject.SearchBy_Supplier) != null ? "%" + InputInvoiceSearchObject.SearchBy_Supplier + "%" : "");

					selectCommand.Parameters.AddWithValue("@InvoiceNumber", ((object)InputInvoiceSearchObject.SearchBy_InvoiceNumber) != null ? "%" + InputInvoiceSearchObject.SearchBy_InvoiceNumber + "%" : "");
                    selectCommand.Parameters.AddWithValue("@InvoiceDateFrom", ((object)InputInvoiceSearchObject.SearchBy_InvoiceDateFrom) != null ? (object)InputInvoiceSearchObject.SearchBy_InvoiceDateFrom : DBNull.Value);
                    selectCommand.Parameters.AddWithValue("@InvoiceDateTo", ((object)InputInvoiceSearchObject.SearchBy_InvoiceDateTo) != null ? (object)InputInvoiceSearchObject.SearchBy_InvoiceDateTo : DBNull.Value);
                    selectCommand.Parameters.AddWithValue("@DateOfPaymetFrom", ((object)InputInvoiceSearchObject.SearchBy_DateOfPaymetFrom) != null ? (object)InputInvoiceSearchObject.SearchBy_DateOfPaymetFrom : DBNull.Value);
                    selectCommand.Parameters.AddWithValue("@DateOfPaymetTo", ((object)InputInvoiceSearchObject.SearchBy_DateOfPaymetTo) != null ? (object)InputInvoiceSearchObject.SearchBy_DateOfPaymetTo : DBNull.Value);

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
					response.InputInvoices = new List<InputInvoiceViewModel>();
					return response;
				}
				db.Close();
			}
			response.Success = true;
			response.InputInvoices = InputInvoices;
			return response;
		}

		public InputInvoiceListResponse GetInputInvoicesForPopup(int companyId, string filterString)
		{
			InputInvoiceListResponse response = new InputInvoiceListResponse();
			List<InputInvoiceViewModel> InputInvoices = new List<InputInvoiceViewModel>();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
				db.Open();
				try
				{
					SqliteCommand selectCommand = new SqliteCommand(
						SqlCommandSelectPart +
						"FROM InputInvoices " +
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
						int counter = 0;
						InputInvoiceViewModel dbEntry = new InputInvoiceViewModel();
						dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
						dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
						dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
						dbEntry.BusinessPartner = SQLiteHelper.GetBusinessPartner(query, ref counter);
						dbEntry.Supplier = SQLiteHelper.GetString(query, ref counter);
						dbEntry.Address = SQLiteHelper.GetString(query, ref counter);
						dbEntry.InvoiceNumber = SQLiteHelper.GetString(query, ref counter);
						dbEntry.InvoiceDate = SQLiteHelper.GetDateTime(query, ref counter);
						dbEntry.AmountNet = SQLiteHelper.GetDecimal(query, ref counter);
						dbEntry.PDVPercent = SQLiteHelper.GetInt(query, ref counter);
						dbEntry.PDV = SQLiteHelper.GetDecimal(query, ref counter);
						dbEntry.AmountGross = SQLiteHelper.GetDecimal(query, ref counter);
						dbEntry.Currency = SQLiteHelper.GetInt(query, ref counter);
						dbEntry.DateOfPaymet = SQLiteHelper.GetDateTime(query, ref counter);
						dbEntry.Status = SQLiteHelper.GetString(query, ref counter);
						dbEntry.StatusDate = SQLiteHelper.GetDateTime(query, ref counter);
						dbEntry.Description = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Path = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
						dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
						dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
						dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
						InputInvoices.Add(dbEntry);
					}
				}
				catch (SqliteException error)
				{
					MainWindow.ErrorMessage = error.Message;
					response.Success = false;
					response.Message = error.Message;
					response.InputInvoices = new List<InputInvoiceViewModel>();
					return response;
				}
				db.Close();
			}
			response.Success = true;
			response.InputInvoices = InputInvoices;
			return response;
		}

		public InputInvoiceResponse GetInputInvoice(Guid identifier)
		{
			InputInvoiceResponse response = new InputInvoiceResponse();
			InputInvoiceViewModel InputInvoice = new InputInvoiceViewModel();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
				db.Open();
				try
				{
					SqliteCommand selectCommand = new SqliteCommand(
						SqlCommandSelectPart +
						"FROM InputInvoices " +
						"WHERE Identifier = @Identifier;", db);
					selectCommand.Parameters.AddWithValue("@Identifier", identifier);

					SqliteDataReader query = selectCommand.ExecuteReader();

					if (query.Read())
					{
						int counter = 0;
						InputInvoiceViewModel dbEntry = new InputInvoiceViewModel();
						dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
						dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
						dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
						dbEntry.BusinessPartner = SQLiteHelper.GetBusinessPartner(query, ref counter);
						dbEntry.Supplier = SQLiteHelper.GetString(query, ref counter);
						dbEntry.Address = SQLiteHelper.GetString(query, ref counter);
						dbEntry.InvoiceNumber = SQLiteHelper.GetString(query, ref counter);
						dbEntry.InvoiceDate = SQLiteHelper.GetDateTime(query, ref counter);
						dbEntry.AmountNet = SQLiteHelper.GetDecimal(query, ref counter);
						dbEntry.PDVPercent = SQLiteHelper.GetInt(query, ref counter);
						dbEntry.PDV = SQLiteHelper.GetDecimal(query, ref counter);
						dbEntry.AmountGross = SQLiteHelper.GetDecimal(query, ref counter);
						dbEntry.Currency = SQLiteHelper.GetInt(query, ref counter);
						dbEntry.DateOfPaymet = SQLiteHelper.GetDateTime(query, ref counter);
						dbEntry.Status = SQLiteHelper.GetString(query, ref counter);
						dbEntry.StatusDate = SQLiteHelper.GetDateTime(query, ref counter);
						dbEntry.Description = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.Path = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
						dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
						dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
						dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
						InputInvoice = dbEntry;
					}
				}
				catch (SqliteException error)
				{
					MainWindow.ErrorMessage = error.Message;
					response.Success = false;
					response.Message = error.Message;
					response.InputInvoice = new InputInvoiceViewModel();
					return response;
				}
				db.Close();
			}
			response.Success = true;
			response.InputInvoice = InputInvoice;
			return response;
		}

		public void Sync(IInputInvoiceService inputInvoiceService)
		{
			SyncInputInvoiceRequest request = new SyncInputInvoiceRequest();
			request.CompanyId = MainWindow.CurrentCompanyId;
			request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

			InputInvoiceListResponse response = inputInvoiceService.Sync(request);
			if (response.Success)
			{
				List<InputInvoiceViewModel> inputInvoicesFromDB = response.InputInvoices;
				foreach (var inputInvoice in inputInvoicesFromDB.OrderBy(x => x.Id))
				{
					Delete(inputInvoice.Identifier);
                    if (inputInvoice.IsActive)
                    {
                        inputInvoice.IsSynced = true;
                        Create(inputInvoice);
                    }
				}
			}
		}

		public DateTime? GetLastUpdatedAt(int companyId)
		{
			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
				db.Open();
				try
				{
					SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from InputInvoices WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
					selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
					SqliteDataReader query = selectCommand.ExecuteReader();
					int count = query.Read() ? query.GetInt32(0) : 0;

					if (count == 0)
						return null;
					else
					{
						selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from InputInvoices WHERE CompanyId = @CompanyId AND IsSynced = 1", db);
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

		public InputInvoiceResponse Create(InputInvoiceViewModel InputInvoice)
		{
			InputInvoiceResponse response = new InputInvoiceResponse();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
				db.Open();

				SqliteCommand insertCommand = new SqliteCommand();
				insertCommand.Connection = db;

				//Use parameterized query to prevent SQL injection attacks
				insertCommand.CommandText = SqlCommandInsertPart;

				insertCommand.Parameters.AddWithValue("@ServerId", InputInvoice.Id);
				insertCommand.Parameters.AddWithValue("@Identifier", InputInvoice.Identifier);
				insertCommand.Parameters.AddWithValue("@Code", ((object)InputInvoice.Code) ?? DBNull.Value);

				insertCommand.Parameters.AddWithValue("@BusinessPartnerId", ((object)InputInvoice.BusinessPartner?.Id) ?? DBNull.Value);
				insertCommand.Parameters.AddWithValue("@BusinessPartnerIdentifier", ((object)InputInvoice.BusinessPartner?.Identifier) ?? DBNull.Value);
				insertCommand.Parameters.AddWithValue("@BusinessPartnerCode", ((object)InputInvoice.BusinessPartner?.Code) ?? DBNull.Value);
				insertCommand.Parameters.AddWithValue("@BusinessPartnerName", ((object)InputInvoice.BusinessPartner?.Name) ?? DBNull.Value);
				insertCommand.Parameters.AddWithValue("@BusinessPartnerInternalCode", ((object)InputInvoice.BusinessPartner?.InternalCode) ?? DBNull.Value);
				insertCommand.Parameters.AddWithValue("@BusinessPartnerNameGer", ((object)InputInvoice.BusinessPartner?.NameGer) ?? DBNull.Value);
				insertCommand.Parameters.AddWithValue("@Supplier", ((object)InputInvoice.Supplier) ?? DBNull.Value);
				insertCommand.Parameters.AddWithValue("@Address", ((object)InputInvoice.Address) ?? DBNull.Value);
				insertCommand.Parameters.AddWithValue("@InvoiceNumber", ((object)InputInvoice.InvoiceNumber) ?? DBNull.Value);
				insertCommand.Parameters.AddWithValue("@InvoiceDate", ((object)InputInvoice.InvoiceDate) ?? DBNull.Value);
				insertCommand.Parameters.AddWithValue("@AmountNet", ((object)InputInvoice.AmountNet) ?? DBNull.Value);
				insertCommand.Parameters.AddWithValue("@PDVPercent", ((object)InputInvoice.PDVPercent) ?? DBNull.Value);
				insertCommand.Parameters.AddWithValue("@PDV", ((object)InputInvoice.PDV) ?? DBNull.Value);
				insertCommand.Parameters.AddWithValue("@AmountGross", ((object)InputInvoice.AmountGross) ?? DBNull.Value);
				insertCommand.Parameters.AddWithValue("@Currency", ((object)InputInvoice.Currency) ?? DBNull.Value);
				insertCommand.Parameters.AddWithValue("@DateOfPaymet", ((object)InputInvoice.DateOfPaymet) ?? DBNull.Value);
				insertCommand.Parameters.AddWithValue("@Status", ((object)InputInvoice.Status) ?? DBNull.Value);
				insertCommand.Parameters.AddWithValue("@StatusDate", ((object)InputInvoice.StatusDate) ?? DBNull.Value);
				insertCommand.Parameters.AddWithValue("@Description", ((object)InputInvoice.Description) ?? DBNull.Value);
				insertCommand.Parameters.AddWithValue("@Path", ((object)InputInvoice.Path) ?? DBNull.Value);
				insertCommand.Parameters.AddWithValue("@IsSynced", InputInvoice.IsSynced);
				insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)InputInvoice.UpdatedAt) ?? DBNull.Value);
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

		public InputInvoiceResponse UpdateSyncStatus(Guid identifier, string code, DateTime? updatedAt, int serverId, bool isSynced)
		{
			InputInvoiceResponse response = new InputInvoiceResponse();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
				db.Open();

				SqliteCommand insertCommand = new SqliteCommand();
				insertCommand.Connection = db;

				insertCommand.CommandText = "UPDATE InputInvoices SET " +
					"IsSynced = @IsSynced, " +
					"Code = @Code, " +
                    "UpdatedAt = @UpdatedAt, " +
                    "ServerId = @ServerId " +
                    "WHERE Identifier = @Identifier ";

				insertCommand.Parameters.AddWithValue("@IsSynced", isSynced);
                insertCommand.Parameters.AddWithValue("@Code", code);
                insertCommand.Parameters.AddWithValue("@UpdatedAt", updatedAt);
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

		public InputInvoiceResponse Delete(Guid identifier)
		{
			InputInvoiceResponse response = new InputInvoiceResponse();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
				db.Open();

				SqliteCommand insertCommand = new SqliteCommand();
				insertCommand.Connection = db;

				//Use parameterized query to prevent SQL injection attacks
				insertCommand.CommandText =
					"DELETE FROM InputInvoices WHERE Identifier = @Identifier";
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

		public InputInvoiceResponse DeleteAll()
		{
			InputInvoiceResponse response = new InputInvoiceResponse();

			try
			{
				using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
				{
					db.Open();
					db.EnableExtensions(true);

					SqliteCommand insertCommand = new SqliteCommand();
					insertCommand.Connection = db;

					//Use parameterized query to prevent SQL injection attacks
					insertCommand.CommandText = "DELETE FROM InputInvoices";
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
