using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;

namespace SirmiumERPGFC.Repository.Employees
{
	public class LicenceTypeSQLiteRepository
	{
        #region SQL

        public static string LicenceTypeTableCreatePart =
	         "CREATE TABLE IF NOT EXISTS LicenceTypes " +
	          "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
	          "ServerId INTEGER NULL, " +
	          "Identifier GUID, " +
	          "Code NAVCHAR(48) NULL, " +
	          "Category NVARCHAR(48) NULL, " +
	          "Description NVARCHAR(48) NULL, " +
	          "CountryId INTEGER NULL, " +
	          "CountryIdentifier GUID NULL, " +
	          "CountryCode NVARCHAR(2048) NULL, " +
	          "CountryName NVARCHAR(2048) NULL, " +
	          "IsSynced BOOL NULL, " +
	          "UpdatedAt DATETIME NULL, " +
	          "CreatedById INTEGER NULL, " +
	          "CreatedByName NVARCHAR(2048) NULL, " +
	          "CompanyId INTEGER NULL, " +
	          "CompanyName NVARCHAR(2048) NULL)";

		public string SqlCommandSelectPart =
			"SELECT ServerId, Identifier, Code, Category, Description, " +
			"CountryId, CountryIdentifier, CountryCode, CountryName, " +
			"IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

		public string SqlCommandInsertPart = "INSERT INTO LicenceTypes " +
			"(Id, ServerId, Identifier, Code, Category, Description,  " +
			"CountryId, CountryIdentifier, CountryCode, CountryName, " +
			"IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

			"VALUES (NULL, @ServerId, @Identifier, @Code, @Category, @Description,  " +
			"@CountryId, @CountryIdentifier, @CountryCode, @CountryName, " +
			"@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods

        private LicenceTypeViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            LicenceTypeViewModel dbEntry = new LicenceTypeViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Category = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Description = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Country = SQLiteHelper.GetCountry(query, ref counter);
            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);

            return dbEntry;
        }

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, LicenceTypeViewModel licenceType)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", licenceType.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", licenceType.Identifier);
            insertCommand.Parameters.AddWithValue("@Code", ((object)licenceType.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Category", ((object)licenceType.Category) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Description", ((object)licenceType.Description) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryId", ((object)licenceType.Country?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryIdentifier", ((object)licenceType.Country?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryCode", ((object)licenceType.Country?.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryName", ((object)licenceType.Country?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsSynced", licenceType.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)licenceType.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read

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
						"WHERE (@Category IS NULL OR @Category = '' OR Category LIKE @Category) " +
						  "AND (@Description IS NULL OR @Description = '' OR Description LIKE @Description) " +
						"AND CompanyId = @CompanyId " +
						"ORDER BY IsSynced, Id DESC " +
						"LIMIT @ItemsPerPage OFFSET @Offset;", db);
					
                    selectCommand.Parameters.AddWithValue("@Category", ((object)licenceTypeSearchObject.Search_Category) != null ? "%" + licenceTypeSearchObject.Search_Category + "%" : "");
					selectCommand.Parameters.AddWithValue("@Description", ((object)licenceTypeSearchObject.Search_Description) != null ? "%" + licenceTypeSearchObject.Search_Description + "%" : "");
					selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
					selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
					selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

					SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        LicenceTypeViewModel dbEntry = Read(query);
                        LicenceTypes.Add(dbEntry);
                    }

					selectCommand = new SqliteCommand(
						"SELECT Count(*) " +
						"FROM LicenceTypes " +
						"WHERE (@Category IS NULL OR @Category = '' OR Category LIKE @Category) " +
						  "AND (@Description IS NULL OR @Description = '' OR Description LIKE @Description) " +
						"AND CompanyId = @CompanyId ;", db);
					
                    selectCommand.Parameters.AddWithValue("@Category", ((object)licenceTypeSearchObject.Search_Category) != null ? "%" + licenceTypeSearchObject.Search_Category + "%" : "");
					selectCommand.Parameters.AddWithValue("@Description", ((object)licenceTypeSearchObject.Search_Description) != null ? "%" + licenceTypeSearchObject.Search_Description + "%" : "");
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
                        "WHERE ((@Code IS NULL OR @Code = '' OR Code LIKE @Code) " +
                        "OR (@Category IS NULL OR @Category = '' OR Category LIKE @Category)) " +
						"AND CompanyId = @CompanyId " +
						"ORDER BY IsSynced, Id DESC " +
						"LIMIT @ItemsPerPage;", db);
					
                    selectCommand.Parameters.AddWithValue("@Code", ((object)filterString) != null ? "%" + filterString + "%" : "");
					selectCommand.Parameters.AddWithValue("@Category", ((object)filterString) != null ? "%" + filterString + "%" : "");
					selectCommand.Parameters.AddWithValue("@CompanyId", ((object)filterString) != null ? companyId : 0);
					selectCommand.Parameters.AddWithValue("@ItemsPerPage", 100);

					SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        LicenceTypeViewModel dbEntry = Read(query);
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
                        LicenceTypeViewModel dbEntry = Read(query);
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

        #endregion

        #region Sync

        public void Sync(ILicenceTypeService licenceTypeService, Action<int, int> callback = null)
        {
            try
            {
                SyncLicenceTypeRequest request = new SyncLicenceTypeRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                LicenceTypeListResponse response = licenceTypeService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.LicenceTypes?.Count ?? 0;
                    List<LicenceTypeViewModel> licenceTypesFromDB = response.LicenceTypes;

                    using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM LicenceTypes WHERE Identifier = @Identifier";

                            SqliteCommand insertCommand = db.CreateCommand();
                            insertCommand.CommandText = SqlCommandInsertPart;

                            foreach (var licenceType in licenceTypesFromDB)
                            {
                                deleteCommand.Parameters.AddWithValue("@Identifier", licenceType.Identifier);
                                deleteCommand.ExecuteNonQuery();
                                deleteCommand.Parameters.Clear();

                                if (licenceType.IsActive)
                                {
                                    licenceType.IsSynced = true;

                                    insertCommand = AddCreateParameters(insertCommand, licenceType);
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

        public LicenceTypeResponse Create(LicenceTypeViewModel licenceType)
		{
			LicenceTypeResponse response = new LicenceTypeResponse();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
				db.Open();

				SqliteCommand insertCommand = new SqliteCommand();
				insertCommand.CommandText = SqlCommandInsertPart;
                insertCommand.Connection = db;

                try
				{
                    insertCommand = AddCreateParameters(insertCommand, licenceType);
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

        public LicenceTypeResponse Delete(Guid identifier)
		{
			LicenceTypeResponse response = new LicenceTypeResponse();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
				db.Open();

				SqliteCommand insertCommand = new SqliteCommand();
				insertCommand.Connection = db;

				//Use parameterized query to prevent SQL injection attacks
				insertCommand.CommandText = "DELETE FROM LicenceTypes WHERE Identifier = @Identifier";
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
