using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Repository.Employees
{
	public class LicenceTypeSQLiteRepository
	{
		public static string LicenceTypeTableCreatePart =
	 "CREATE TABLE IF NOT EXISTS LicenceTypes " +
	  "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
	  "ServerId INTEGER NULL, " +
	  "Identifier GUID, " +
	  "Code NAVCHAR(48) NULL, " +
	  "Name NVARCHAR(48) NULL, " +
	  "IsSynced BOOL NULL, " +
	  "UpdatedAt DATETIME NULL, " +
	  "CreatedById INTEGER NULL, " +
	  "CreatedByName NVARCHAR(2048) NULL, " +
	  "CompanyId INTEGER NULL, " +
	  "CompanyName NVARCHAR(2048) NULL)";

		public string SqlCommandSelectPart =
			"SELECT ServerId, Identifier, Code, Name, " +

			"IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

		public string SqlCommandInsertPart = "INSERT INTO LicenceTypes " +
			"(Id, ServerId, Identifier, Code, Name, " +
			"IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

			"VALUES (NULL, @ServerId, @Identifier, @Code, @Name, " +
			"@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

		public LicenceTypeListResponse GetLicenceTypesByPage(int companyId, LicenceTypeViewModel licenceTypeSearchObject, int currentPage = 1, int itemsPerPage = 50)
		{
			LicenceTypeListResponse response = new LicenceTypeListResponse();
			List<LicenceTypeViewModel> LicenceTypes = new List<LicenceTypeViewModel>();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
				db.Open();
				try
				{
					SqliteCommand selectCommand = new SqliteCommand(
						SqlCommandSelectPart +
						"FROM LicenceTypes " +
						"WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
						"AND CompanyId = @CompanyId " +
						"ORDER BY IsSynced, Id DESC " +
						"LIMIT @ItemsPerPage OFFSET @Offset;", db);
					selectCommand.Parameters.AddWithValue("@Name", ((object)licenceTypeSearchObject.Search_Name) != null ? "%" + licenceTypeSearchObject.Search_Name + "%" : "");
					selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
					selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
					selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

					SqliteDataReader query = selectCommand.ExecuteReader();

					while (query.Read())
					{
						int counter = 0;
						LicenceTypeViewModel dbEntry = new LicenceTypeViewModel();
						dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
						dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
						dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
						dbEntry.Name = SQLiteHelper.GetString(query, ref counter);

						dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
						dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
						dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
						dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
						LicenceTypes.Add(dbEntry);
					}


					selectCommand = new SqliteCommand(
						"SELECT Count(*) " +
						"FROM LicenceTypes " +
						"WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
						"AND CompanyId = @CompanyId ;", db);
					selectCommand.Parameters.AddWithValue("@Name", ((object)licenceTypeSearchObject.Search_Name) != null ? "%" + licenceTypeSearchObject.Search_Name + "%" : "");
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
					response.LicenceTypes = new List<LicenceTypeViewModel>();
					return response;
				}
				db.Close();
			}
			response.Success = true;
			response.LicenceTypes = LicenceTypes;
			return response;
		}

		public LicenceTypeListResponse GetLicenceTypesForPopup(int companyId, string filterString)
		{
			LicenceTypeListResponse response = new LicenceTypeListResponse();
			List<LicenceTypeViewModel> LicenceTypes = new List<LicenceTypeViewModel>();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
				db.Open();
				try
				{
					SqliteCommand selectCommand = new SqliteCommand(
						SqlCommandSelectPart +
						"FROM LicenceTypes " +
						"WHERE (@Code IS NULL OR @Code = '' OR Code LIKE @Code) " +
						"AND (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
						"AND CompanyId = @CompanyId " +
						"ORDER BY IsSynced, Id DESC " +
						"LIMIT @ItemsPerPage;", db);
					selectCommand.Parameters.AddWithValue("@Code", ((object)filterString) != null ? "%" + filterString + "%" : "");
					selectCommand.Parameters.AddWithValue("@Name", ((object)filterString) != null ? "%" + filterString + "%" : "");
					selectCommand.Parameters.AddWithValue("@CompanyId", ((object)filterString) != null ? companyId : 0);
					selectCommand.Parameters.AddWithValue("@ItemsPerPage", 100);

					SqliteDataReader query = selectCommand.ExecuteReader();

					while (query.Read())
					{
						int counter = 0;
						LicenceTypeViewModel dbEntry = new LicenceTypeViewModel();
						dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
						dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
						dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
						dbEntry.Name = SQLiteHelper.GetString(query, ref counter);

						dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
						dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
						dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
						dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
						LicenceTypes.Add(dbEntry);
					}
				}
				catch (SqliteException error)
				{
					MainWindow.ErrorMessage = error.Message;
					response.Success = false;
					response.Message = error.Message;
					response.LicenceTypes = new List<LicenceTypeViewModel>();
					return response;
				}
				db.Close();
			}
			response.Success = true;
			response.LicenceTypes = LicenceTypes;
			return response;
		}

		public LicenceTypeResponse GetLicenceType(Guid identifier)
		{
			LicenceTypeResponse response = new LicenceTypeResponse();
			LicenceTypeViewModel licenceType = new LicenceTypeViewModel();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
				db.Open();
				try
				{
					SqliteCommand selectCommand = new SqliteCommand(
						SqlCommandSelectPart +
						"FROM LicenceTypes " +
						"WHERE Identifier = @Identifier;", db);
					selectCommand.Parameters.AddWithValue("@Identifier", identifier);

					SqliteDataReader query = selectCommand.ExecuteReader();

					if (query.Read())
					{
						int counter = 0;
						LicenceTypeViewModel dbEntry = new LicenceTypeViewModel();
						dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
						dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
						dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
						dbEntry.Name = SQLiteHelper.GetString(query, ref counter);

						dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
						dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
						dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
						dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
						licenceType = dbEntry;
					}
				}
				catch (SqliteException error)
				{
					MainWindow.ErrorMessage = error.Message;
					response.Success = false;
					response.Message = error.Message;
					response.LicenceType = new LicenceTypeViewModel();
					return response;
				}
				db.Close();
			}
			response.Success = true;
			response.LicenceType = licenceType;
			return response;
		}

		public void Sync(ILicenceTypeService licenceTypeService)
		{
			SyncLicenceTypeRequest request = new SyncLicenceTypeRequest();
			request.CompanyId = MainWindow.CurrentCompanyId;
			request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

			LicenceTypeListResponse response = licenceTypeService.Sync(request);
			if (response.Success)
			{
				List<LicenceTypeViewModel> licenceTypesFromDB = response.LicenceTypes;
				foreach (var licenceType in licenceTypesFromDB.OrderBy(x => x.Id))
				{
					Delete(licenceType.Identifier);
					licenceType.IsSynced = true;
					Create(licenceType);
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
					SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from LicenceTypes WHERE CompanyId = @CompanyId", db);
					selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
					SqliteDataReader query = selectCommand.ExecuteReader();
					int count = query.Read() ? query.GetInt32(0) : 0;

					if (count == 0)
						return null;
					else
					{
						selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from LicenceTypes WHERE CompanyId = @CompanyId", db);
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

		public LicenceTypeResponse Create(LicenceTypeViewModel licenceType)
		{
			LicenceTypeResponse response = new LicenceTypeResponse();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
				db.Open();

				SqliteCommand insertCommand = new SqliteCommand();
				insertCommand.Connection = db;

				//Use parameterized query to prevent SQL injection attacks
				insertCommand.CommandText = SqlCommandInsertPart;

				insertCommand.Parameters.AddWithValue("@ServerId", licenceType.Id);
				insertCommand.Parameters.AddWithValue("@Identifier", licenceType.Identifier);
				insertCommand.Parameters.AddWithValue("@Code", ((object)licenceType.Code) ?? DBNull.Value);
				insertCommand.Parameters.AddWithValue("@Name", ((object)licenceType.Name) ?? DBNull.Value);
				insertCommand.Parameters.AddWithValue("@IsSynced", licenceType.IsSynced);
				insertCommand.Parameters.AddWithValue("@UpdatedAt", licenceType.UpdatedAt);
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

		public LicenceTypeResponse UpdateSyncStatus(Guid identifier, int serverId, bool isSynced)
		{
			LicenceTypeResponse response = new LicenceTypeResponse();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
				db.Open();

				SqliteCommand insertCommand = new SqliteCommand();
				insertCommand.Connection = db;

				insertCommand.CommandText = "UPDATE LicenceTypes SET " +
					"IsSynced = @IsSynced, " +
					"ServerId = @ServerId " +
					"WHERE Identifier = @Identifier ";

				insertCommand.Parameters.AddWithValue("@IsSynced", isSynced);
				insertCommand.Parameters.AddWithValue("@ServerId", serverId);
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

		public LicenceTypeResponse Delete(Guid identifier)
		{
			LicenceTypeResponse response = new LicenceTypeResponse();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
				db.Open();

				SqliteCommand insertCommand = new SqliteCommand();
				insertCommand.Connection = db;

				//Use parameterized query to prevent SQL injection attacks
				insertCommand.CommandText =
					"DELETE FROM LicenceTypes WHERE Identifier = @Identifier";
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

		public LicenceTypeResponse DeleteAll()
		{
			LicenceTypeResponse response = new LicenceTypeResponse();

			try
			{
				using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
				{
					db.Open();
					db.EnableExtensions(true);

					SqliteCommand insertCommand = new SqliteCommand();
					insertCommand.Connection = db;

					//Use parameterized query to prevent SQL injection attacks
					insertCommand.CommandText = "DELETE FROM LicenceTypes";
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
