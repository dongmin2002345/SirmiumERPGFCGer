﻿using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Banks;
using ServiceInterfaces.Messages.Banks;
using ServiceInterfaces.ViewModels.Banks;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;

namespace SirmiumERPGFC.Repository.Banks
{
	public class BankSQLiteRepository
	{
        #region SQL

        public static string BankTableCreatePart =
		 "CREATE TABLE IF NOT EXISTS Banks " +
		  "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
		  "ServerId INTEGER NULL, " +
		  "Identifier GUID, " +
		  "Code NAVCHAR(48) NULL, " +
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
			"SELECT ServerId, Identifier, Code, Name, Swift, " +
			"CountryId, CountryIdentifier, CountryCode, CountryName, " +
			"IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

		public string SqlCommandInsertPart = "INSERT INTO Banks " +
            "(Id, ServerId, Identifier, Code, Name, Swift, " +
			"CountryId, CountryIdentifier, CountryCode, CountryName, " +
			"IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

            "VALUES (NULL, @ServerId, @Identifier, @Code, @Name, @Swift, " +
			"@CountryId, @CountryIdentifier, @CountryCode, @CountryName, " +
			"@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods

        private BankViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            BankViewModel dbEntry = new BankViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.Code = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Swift = SQLiteHelper.GetString(query, ref counter);
            dbEntry.Country = SQLiteHelper.GetCountry(query, ref counter);
            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);

            return dbEntry;
        }

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, BankViewModel bank)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", bank.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", bank.Identifier);
            insertCommand.Parameters.AddWithValue("@Code", ((object)bank.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Name", ((object)bank.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Swift", ((object)bank.Swift) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryId", ((object)bank.Country?.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryIdentifier", ((object)bank.Country?.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryCode", ((object)bank.Country?.Mark) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CountryName", ((object)bank.Country?.Name) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsSynced", bank.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)bank.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read

        public BankListResponse GetBanksByPage(int companyId, BankViewModel bankSearchObject, int currentPage = 1, int itemsPerPage = 50)
		{
			BankListResponse response = new BankListResponse();
			List<BankViewModel> Banks = new List<BankViewModel>();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
				db.Open();
				try
				{
					SqliteCommand selectCommand = new SqliteCommand(
						SqlCommandSelectPart +
						"FROM Banks " +
						"WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND (@Swift IS NULL OR @Swift = '' OR Swift LIKE @Swift) " +
                        "AND (@Country IS NULL OR @Country = '' OR CountryName LIKE @Country) " +
						"AND CompanyId = @CompanyId " +
						"ORDER BY IsSynced, Id DESC " +
						"LIMIT @ItemsPerPage OFFSET @Offset;", db);
					
                    selectCommand.Parameters.AddWithValue("@Name", ((object)bankSearchObject.Search_Name) != null ? "%" + bankSearchObject.Search_Name + "%" : "");
					selectCommand.Parameters.AddWithValue("@Country", ((object)bankSearchObject.Search_Country) != null ? "%" + bankSearchObject.Search_Country + "%" : "");
					selectCommand.Parameters.AddWithValue("@Swift", ((object)bankSearchObject.Search_Swift) != null ? "%" + bankSearchObject.Search_Swift + "%" : "");
					selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
					selectCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);
					selectCommand.Parameters.AddWithValue("@Offset", (currentPage - 1) * itemsPerPage);

					SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        BankViewModel dbEntry = Read(query);

                        Banks.Add(dbEntry);
                    }


					selectCommand = new SqliteCommand(
						"SELECT Count(*) " +
						"FROM Banks " +
						"WHERE (@Name IS NULL OR @Name = '' OR Name LIKE @Name) " +
                        "AND (@Swift IS NULL OR @Swift = '' OR Swift LIKE @Swift) " +
                        "AND (@Country IS NULL OR @Country = '' OR CountryName LIKE @Country) " +
						"AND CompanyId = @CompanyId;", db);
					
                    selectCommand.Parameters.AddWithValue("@Name", ((object)bankSearchObject.Search_Name) != null ? "%" + bankSearchObject.Search_Name + "%" : "");
					selectCommand.Parameters.AddWithValue("@Country", ((object)bankSearchObject.Search_Country) != null ? "%" + bankSearchObject.Search_Country + "%" : "");
                    selectCommand.Parameters.AddWithValue("@Swift", ((object)bankSearchObject.Search_Swift) != null ? "%" + bankSearchObject.Search_Swift + "%" : "");
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
					response.Banks = new List<BankViewModel>();
					return response;
				}
				db.Close();
			}
			response.Success = true;
			response.Banks = Banks;
			return response;
		}

		public BankListResponse GetBanksForPopup(int companyId, string filterString)
		{
			BankListResponse response = new BankListResponse();
			List<BankViewModel> Banks = new List<BankViewModel>();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
				db.Open();
				try
				{
					SqliteCommand selectCommand = new SqliteCommand(
						SqlCommandSelectPart +
						"FROM Banks " +
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
                        BankViewModel dbEntry = Read(query);
                        Banks.Add(dbEntry);
                    }
				}
				catch (SqliteException error)
				{
					MainWindow.ErrorMessage = error.Message;
					response.Success = false;
					response.Message = error.Message;
					response.Banks = new List<BankViewModel>();
					return response;
				}
				db.Close();
			}
			response.Success = true;
			response.Banks = Banks;
			return response;
		}

		public BankResponse GetBank(Guid identifier)
		{
			BankResponse response = new BankResponse();
			BankViewModel bank = new BankViewModel();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
				db.Open();
				try
				{
					SqliteCommand selectCommand = new SqliteCommand(
						SqlCommandSelectPart +
						"FROM Banks " +
						"WHERE Identifier = @Identifier;", db);
					selectCommand.Parameters.AddWithValue("@Identifier", identifier);

					SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        BankViewModel dbEntry = Read(query);

                        bank = dbEntry;
                    }
				}
				catch (SqliteException error)
				{
					MainWindow.ErrorMessage = error.Message;
					response.Success = false;
					response.Message = error.Message;
					response.Bank = new BankViewModel();
					return response;
				}
				db.Close();
			}
			response.Success = true;
			response.Bank = bank;
			return response;
		}

        #endregion

        #region Sync

        public void Sync(IBankService bankService, Action<int, int> callback = null)
        {
            try
            {
                SyncBankRequest request = new SyncBankRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                BankListResponse response = bankService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.Banks?.Count ?? 0;
                    List<BankViewModel> banksFromDB = response.Banks;

                    using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM Banks WHERE Identifier = @Identifier";

                            SqliteCommand insertCommand = db.CreateCommand();
                            insertCommand.CommandText = SqlCommandInsertPart;

                            foreach (var bank in banksFromDB)
                            {
                                deleteCommand.Parameters.AddWithValue("@Identifier", bank.Identifier);
                                deleteCommand.ExecuteNonQuery();
                                deleteCommand.Parameters.Clear();

                                if (bank.IsActive)
                                {
                                    bank.IsSynced = true;

                                    insertCommand = AddCreateParameters(insertCommand, bank);
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
			using (SqliteConnection db = new SqliteConnection(SQLiteHelper.SqLiteTableName))
			{
				db.Open();
				try
				{
					SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from Banks WHERE CompanyId = @CompanyId", db);
					selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
					SqliteDataReader query = selectCommand.ExecuteReader();
					int count = query.Read() ? query.GetInt32(0) : 0;

					if (count == 0)
						return null;
					else
					{
						selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from Banks WHERE CompanyId = @CompanyId", db);
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

        public BankResponse Create(BankViewModel bank)
		{
			BankResponse response = new BankResponse();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
				db.Open();

				SqliteCommand insertCommand = db.CreateCommand();
				insertCommand.CommandText = SqlCommandInsertPart;

				try
				{
                    insertCommand = AddCreateParameters(insertCommand, bank);
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

        public BankResponse Delete(Guid identifier)
		{
			BankResponse response = new BankResponse();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
				db.Open();

				SqliteCommand insertCommand = new SqliteCommand();
				insertCommand.Connection = db;

				//Use parameterized query to prevent SQL injection attacks
				insertCommand.CommandText = "DELETE FROM Banks WHERE Identifier = @Identifier";
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

		public BankResponse DeleteAll()
		{
			BankResponse response = new BankResponse();

			try
			{
				using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
				{
					db.Open();
					db.EnableExtensions(true);

					SqliteCommand insertCommand = new SqliteCommand();
					insertCommand.Connection = db;

					//Use parameterized query to prevent SQL injection attacks
					insertCommand.CommandText = "DELETE FROM Banks";
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
