using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.InputInvoices;
using ServiceInterfaces.Gloabals;
using ServiceInterfaces.Messages.Common.InputInvoices;
using ServiceInterfaces.ViewModels.Common.InputInvoices;
using SirmiumERPGFC.Repository.Common;
using System;
using System.Collections.Generic;

namespace SirmiumERPGFC.Repository.InputInvoices
{
	public class InputInvoiceDocumentSQLiteRepository
	{
        #region SQL

        public static string InputInvoiceDocumentTableCreatePart =
				 "CREATE TABLE IF NOT EXISTS InputInvoiceDocuments " +
				 "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
				 "ServerId INTEGER NULL, " +
				 "Identifier GUID, " +
				 "InputInvoiceId INTEGER NULL, " +
				 "InputInvoiceIdentifier GUID NULL, " +
				 "InputInvoiceCode NVARCHAR(48) NULL, " +
				 "Name NVARCHAR(2048), " +
				 "CreateDate DATETIME NULL, " +
				 "Path NVARCHAR(2048) NULL, " +
                 "ItemStatus INTEGER NOT NULL, " +
                 "IsSynced BOOL NULL, " +
				 "UpdatedAt DATETIME NULL, " +
				 "CreatedById INTEGER NULL, " +
				 "CreatedByName NVARCHAR(2048) NULL, " +
				 "CompanyId INTEGER NULL, " +
				 "CompanyName NVARCHAR(2048) NULL)";

		public string SqlCommandSelectPart =
			"SELECT ServerId, Identifier, InputInvoiceId, InputInvoiceIdentifier, " +
            "InputInvoiceCode, Name, CreateDate, Path, ItemStatus, " +
			"IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

		public string SqlCommandInsertPart = "INSERT INTO InputInvoiceDocuments " +
			"(Id, ServerId, Identifier, InputInvoiceId, InputInvoiceIdentifier, " +
            "InputInvoiceCode, Name, CreateDate, Path, ItemStatus, " +
			"IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

			"VALUES (NULL, @ServerId, @Identifier, @InputInvoiceId, @InputInvoiceIdentifier, " +
            "@InputInvoiceCode, @Name, @CreateDate, @Path, @ItemStatus, " +
			"@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #endregion

        #region Helper methods
        private static InputInvoiceDocumentViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            InputInvoiceDocumentViewModel dbEntry = new InputInvoiceDocumentViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.InputInvoice = SQLiteHelper.GetInputInvoice(query, ref counter);
            dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
            dbEntry.CreateDate = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.Path = SQLiteHelper.GetString(query, ref counter);
            dbEntry.ItemStatus = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
            dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
            dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
            dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
            return dbEntry;
        }

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, InputInvoiceDocumentViewModel InputInvoiceDocument)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", InputInvoiceDocument.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", InputInvoiceDocument.Identifier);
            insertCommand.Parameters.AddWithValue("@InputInvoiceId", ((object)InputInvoiceDocument.InputInvoice.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@InputInvoiceIdentifier", ((object)InputInvoiceDocument.InputInvoice.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@InputInvoiceCode", ((object)InputInvoiceDocument.InputInvoice.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Name", InputInvoiceDocument.Name);
            insertCommand.Parameters.AddWithValue("@CreateDate", ((object)InputInvoiceDocument.CreateDate) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Path", ((object)InputInvoiceDocument.Path) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ItemStatus", ((object)InputInvoiceDocument.ItemStatus) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsSynced", InputInvoiceDocument.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)InputInvoiceDocument.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        #region Read

        public InputInvoiceDocumentListResponse GetInputInvoiceDocumentsByInputInvoice(int companyId, Guid InputInvoiceIdentifier)
		{
			InputInvoiceDocumentListResponse response = new InputInvoiceDocumentListResponse();
			List<InputInvoiceDocumentViewModel> InputInvoiceDocuments = new List<InputInvoiceDocumentViewModel>();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
				db.Open();
				try
				{
					SqliteCommand selectCommand = new SqliteCommand(
						SqlCommandSelectPart +
						"FROM InputInvoiceDocuments " +
						"WHERE InputInvoiceIdentifier = @InputInvoiceIdentifier " +
						"AND CompanyId = @CompanyId " +
						"ORDER BY IsSynced, Id DESC;", db);
					
                    selectCommand.Parameters.AddWithValue("@InputInvoiceIdentifier", InputInvoiceIdentifier);
					selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

					SqliteDataReader query = selectCommand.ExecuteReader();

					while (query.Read())
					{

                        InputInvoiceDocumentViewModel dbEntry = Read(query);
                        InputInvoiceDocuments.Add(dbEntry);
					}

				}
				catch (SqliteException error)
				{
					MainWindow.ErrorMessage = error.Message;
					response.Success = false;
					response.Message = error.Message;
					response.InputInvoiceDocuments = new List<InputInvoiceDocumentViewModel>();
					return response;
				}
				db.Close();
			}
			response.Success = true;
			response.InputInvoiceDocuments = InputInvoiceDocuments;
			return response;
		}

		public InputInvoiceDocumentResponse GetInputInvoiceDocument(Guid identifier)
		{
			InputInvoiceDocumentResponse response = new InputInvoiceDocumentResponse();
			InputInvoiceDocumentViewModel InputInvoiceDocument = new InputInvoiceDocumentViewModel();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
				db.Open();
				try
				{
					SqliteCommand selectCommand = new SqliteCommand(
						SqlCommandSelectPart +
						"FROM InputInvoiceDocuments " +
						"WHERE Identifier = @Identifier;", db);
					selectCommand.Parameters.AddWithValue("@Identifier", identifier);

					SqliteDataReader query = selectCommand.ExecuteReader();

					if (query.Read())
					{
                        InputInvoiceDocumentViewModel dbEntry = Read(query);
                        InputInvoiceDocument = dbEntry;
					}
				}
				catch (SqliteException error)
				{
					MainWindow.ErrorMessage = error.Message;
					response.Success = false;
					response.Message = error.Message;
					response.InputInvoiceDocument = new InputInvoiceDocumentViewModel();
					return response;
				}
				db.Close();
			}
			response.Success = true;
			response.InputInvoiceDocument = InputInvoiceDocument;
			return response;
		}

        public void Sync(IInputInvoiceDocumentService InputInvoiceDocumentService, Action<int, int> callback = null)
        {
            try
            {
                SyncInputInvoiceDocumentRequest request = new SyncInputInvoiceDocumentRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                InputInvoiceDocumentListResponse response = InputInvoiceDocumentService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.InputInvoiceDocuments?.Count ?? 0;
                    List<InputInvoiceDocumentViewModel> items = response.InputInvoiceDocuments;

                    using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
                    {
                        db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            SqliteCommand deleteCommand = db.CreateCommand();
                            deleteCommand.CommandText = "DELETE FROM InputInvoiceDocuments WHERE Identifier = @Identifier";

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
					SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from InputInvoiceDocuments WHERE CompanyId = @CompanyId", db);
					selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
					SqliteDataReader query = selectCommand.ExecuteReader();
					int count = query.Read() ? query.GetInt32(0) : 0;

					if (count == 0)
						return null;
					else
					{
						selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from InputInvoiceDocuments WHERE CompanyId = @CompanyId", db);
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

        public InputInvoiceDocumentResponse Create(InputInvoiceDocumentViewModel InputInvoiceDocument)
		{
			InputInvoiceDocumentResponse response = new InputInvoiceDocumentResponse();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
                db.Open();
                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, InputInvoiceDocument);
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

        public InputInvoiceDocumentResponse Delete(Guid identifier)
		{
			InputInvoiceDocumentResponse response = new InputInvoiceDocumentResponse();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
				db.Open();

				SqliteCommand insertCommand = new SqliteCommand();
				insertCommand.Connection = db;

				//Use parameterized query to prevent SQL injection attacks
				insertCommand.CommandText = "DELETE FROM InputInvoiceDocuments WHERE Identifier = @Identifier";
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

        public InputInvoiceDocumentResponse SetStatusDeleted(Guid identifier)
        {
            InputInvoiceDocumentResponse response = new InputInvoiceDocumentResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "UPDATE InputInvoiceDocuments SET ItemStatus = @ItemStatus WHERE Identifier = @Identifier";
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

        //public InputInvoiceDocumentResponse DeleteAll()
        //{
        //	InputInvoiceDocumentResponse response = new InputInvoiceDocumentResponse();

        //	try
        //	{
        //		using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
        //		{
        //			db.Open();
        //			db.EnableExtensions(true);

        //			SqliteCommand insertCommand = new SqliteCommand();
        //			insertCommand.Connection = db;

        //			//Use parameterized query to prevent SQL injection attacks
        //			insertCommand.CommandText = "DELETE FROM InputInvoiceDocuments";
        //			try
        //			{
        //				insertCommand.ExecuteReader();
        //			}
        //			catch (SqliteException error)
        //			{
        //				response.Success = false;
        //				response.Message = error.Message;

        //				MainWindow.ErrorMessage = error.Message;
        //				return response;
        //			}
        //			db.Close();
        //		}
        //	}
        //	catch (SqliteException error)
        //	{
        //		response.Success = false;
        //		response.Message = error.Message;
        //		return response;
        //	}

        //	response.Success = true;
        //	return response;
        //}

        #endregion
    }
}
