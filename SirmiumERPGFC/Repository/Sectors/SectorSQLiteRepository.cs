using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.Sectors;
using ServiceInterfaces.Messages.Common.Sectors;
using ServiceInterfaces.ViewModels.Common.Sectors;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;

namespace SirmiumERPGFC.Repository.Sectors
{
	public class SectorSQLiteRepository
	{
        #region SQL

        public static string SectorTableCreatePart =
		 "CREATE TABLE IF NOT EXISTS Sectors " +
		  "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
		  "ServerId INTEGER NULL, " +
		  "Identifier GUID, " +
		  "Code NAVCHAR(48) NULL, " +
		  "SecondCode NVARCHAR(48) NULL, " +
		  "Name NVARCHAR(48) NULL, " +
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
			"SELECT ServerId, Identifier, Code, SecondCode, Name, " +
			"CountryId, CountryIdentifier, CountryCode, CountryName, " +
			"IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

		public string SqlCommandInsertPart = "INSERT INTO Sectors " +
			"(Id, ServerId, Identifier, Code, SecondCode, Name, " +
			"CountryId, CountryIdentifier, CountryCode, CountryName, " +
			"IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

			"VALUES (NULL, @ServerId, @Identifier, @Code, @SecondCode, @Name, " +
			"@CountryId, @CountryIdentifier, @CountryCode, @CountryName, " +
			"@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods
        
        private SectorViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            SectorViewModel dbEntry = new SectorViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
            dbEntry.SecondCode = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Country = SQLiteHelper.GetCountry(query, ref counter);
            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);

            return dbEntry;
        }

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, SectorViewModel sector)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", sector.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", sector.Identifier);
            insertCommand.Parameters.AddWithValue("@Code", ((object)sector.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@SecondCode", ((object)sector.SecondCode) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Name", ((object)sector.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryId", ((object)sector.Country?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryIdentifier", ((object)sector.Country?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryCode", ((object)sector.Country?.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryName", ((object)sector.Country?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsSynced", sector.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)sector.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read

        public SectorListResponse GetSectorsByPage(int companyId, SectorViewModel sectorSearchObject, int currentPage = 1, int itemsPerPage = 50)
		{
			SectorListResponse response = new SectorListResponse();
			List<SectorViewModel> Sectors = new List<SectorViewModel>();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
				db.Open();
				try
				{
					SqliteCommand selectCommand = new SqliteCommand(
						SqlCommandSelectPart +
						"FROM Sectors " +
						"WHERE (@SecondCode IS NULL OR @SecondCode = '' OR SecondCode LIKE @SecondCode) " +
						"AND (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND (@CountryName IS NULL OR @CountryName = '' OR CountryName LIKE @CountryName) " +
                        "AND CompanyId = @CompanyId " +
						"ORDER BY IsSynced, Id DESC " +
						"LIMIT @ItemsPerPage OFFSET @Offset;", db);
					
                    selectCommand.Parameters.AddWithValue("@SecondCode", ((object)sectorSearchObject.Search_SecondCode) != null ? "%" + sectorSearchObject.Search_SecondCode + "%" : "");
					selectCommand.Parameters.AddWithValue("@Name", ((object)sectorSearchObject.Search_Name) != null ? "%" + sectorSearchObject.Search_Name + "%" : "");
					selectCommand.Parameters.AddWithValue("@CountryName", ((object)sectorSearchObject.Search_Country) != null ? "%" + sectorSearchObject.Search_Country + "%" : "");
					selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
					selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
					selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

					SqliteDataReader query = selectCommand.ExecuteReader();

					while (query.Read())
						Sectors.Add(Read(query));
					


					selectCommand = new SqliteCommand(
						"SELECT Count(*) " +
						"FROM Sectors " +
						"WHERE (@SecondCode IS NULL OR @SecondCode = '' OR SecondCode LIKE @SecondCode) " +
						"AND (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND (@CountryName IS NULL OR @CountryName = '' OR CountryName LIKE @CountryName) " +
						"AND CompanyId = @CompanyId;", db);
					
                    selectCommand.Parameters.AddWithValue("@SecondCode", ((object)sectorSearchObject.Search_SecondCode) != null ? "%" + sectorSearchObject.Search_SecondCode + "%" : "");
					selectCommand.Parameters.AddWithValue("@Name", ((object)sectorSearchObject.Search_Name) != null ? "%" + sectorSearchObject.Search_Name + "%" : "");
					selectCommand.Parameters.AddWithValue("@CountryName", ((object)sectorSearchObject.Search_Country) != null ? "%" + sectorSearchObject.Search_Country + "%" : "");
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
					response.Sectors = new List<SectorViewModel>();
					return response;
				}
				db.Close();
			}
			response.Success = true;
			response.Sectors = Sectors;
			return response;
		}

		public SectorListResponse GetSectorsForPopup(int companyId, Guid countryIdentifier, string filterString)
		{
			SectorListResponse response = new SectorListResponse();
			List<SectorViewModel> Sectors = new List<SectorViewModel>();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
				db.Open();
				try
				{
					SqliteCommand selectCommand = new SqliteCommand(
						SqlCommandSelectPart +
						"FROM Sectors " +
                        "WHERE (@Filter IS NULL OR @Filter = '' OR SecondCode LIKE @Filter OR Name LIKE @Filter) " +
                        "AND CountryIdentifier = @CountryIdentifier " +
                        "AND CompanyId = @CompanyId " +
						"ORDER BY IsSynced, Id DESC " +
						"LIMIT @ItemsPerPage;", db);
					
                    selectCommand.Parameters.AddWithValue("@Filter", ((object)filterString) != null ? "%" + filterString + "%" : "");
                    selectCommand.Parameters.AddWithValue("@CountryIdentifier", countryIdentifier);
                    selectCommand.Parameters.AddWithValue("@CompanyId", ((object)companyId) != null ? companyId : 0);
					selectCommand.Parameters.AddWithValue("@ItemsPerPage", 100);

					SqliteDataReader query = selectCommand.ExecuteReader();

					while (query.Read())
						Sectors.Add(Read(query));
					
				}
				catch (SqliteException error)
				{
					MainWindow.ErrorMessage = error.Message;
					response.Success = false;
					response.Message = error.Message;
					response.Sectors = new List<SectorViewModel>();
					return response;
				}
				db.Close();
			}
			response.Success = true;
			response.Sectors = Sectors;
			return response;
		}

		public SectorResponse GetSector(Guid identifier)
		{
			SectorResponse response = new SectorResponse();
			SectorViewModel sector = new SectorViewModel();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
				db.Open();
				try
				{
					SqliteCommand selectCommand = new SqliteCommand(
						SqlCommandSelectPart +
						"FROM Sectors " +
						"WHERE Identifier = @Identifier;", db);
					selectCommand.Parameters.AddWithValue("@Identifier", identifier);

					SqliteDataReader query = selectCommand.ExecuteReader();

					if (query.Read())
						sector = Read(query);
					
				}
				catch (SqliteException error)
				{
					MainWindow.ErrorMessage = error.Message;
					response.Success = false;
					response.Message = error.Message;
					response.Sector = new SectorViewModel();
					return response;
				}
				db.Close();
			}
			response.Success = true;
			response.Sector = sector;
			return response;
		}

        #endregion

        #region Sync

        public void Sync(ISectorService sectorService, Action<int, int> callback = null)
        {
            try
            {
                SyncSectorRequest request = new SyncSectorRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                SectorListResponse response = sectorService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.Sectors?.Count ?? 0;
                    List<SectorViewModel> sectorsFromDB = response.Sectors;

                    using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM Sectors WHERE Identifier = @Identifier";

                            SqliteCommand insertCommand = db.CreateCommand();
                            insertCommand.CommandText = SqlCommandInsertPart;

                            foreach (var sector in sectorsFromDB)
                            {
                                deleteCommand.Parameters.AddWithValue("@Identifier", sector.Identifier);
                                deleteCommand.ExecuteNonQuery();
                                deleteCommand.Parameters.Clear();

                                if (sector.IsActive)
                                {
                                    sector.IsSynced = true;

                                    insertCommand = AddCreateParameters(insertCommand, sector);
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
					SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from Sectors WHERE CompanyId = @CompanyId", db);
					selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
					SqliteDataReader query = selectCommand.ExecuteReader();
					int count = query.Read() ? query.GetInt32(0) : 0;

					if (count == 0)
						return null;
					else
					{
						selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from Sectors WHERE CompanyId = @CompanyId", db);
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

        public SectorResponse Create(SectorViewModel sector)
		{
			SectorResponse response = new SectorResponse();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
				db.Open();

				SqliteCommand insertCommand = db.CreateCommand();
				insertCommand.CommandText = SqlCommandInsertPart;

				try
				{
                    insertCommand = AddCreateParameters(insertCommand, sector);
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

        public SectorResponse Delete(Guid identifier)
		{
			SectorResponse response = new SectorResponse();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
				db.Open();

				SqliteCommand insertCommand = new SqliteCommand();
				insertCommand.Connection = db;

				//Use parameterized query to prevent SQL injection attacks
				insertCommand.CommandText = "DELETE FROM Sectors WHERE Identifier = @Identifier";
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

		public SectorResponse DeleteAll()
		{
			SectorResponse response = new SectorResponse();

			try
			{
				using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
				{
					db.Open();
					db.EnableExtensions(true);

					SqliteCommand insertCommand = new SqliteCommand();
					insertCommand.Connection = db;

					//Use parameterized query to prevent SQL injection attacks
					insertCommand.CommandText = "DELETE FROM Sectors";
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
