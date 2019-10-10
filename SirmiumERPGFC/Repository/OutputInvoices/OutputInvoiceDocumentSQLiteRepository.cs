using Microsoft.Data.Sqlite;
using ServiceInterfaces.Abstractions.Common.OutputInvoices;
using ServiceInterfaces.Gloabals;
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
	public class OutputInvoiceDocumentSQLiteRepository
	{
		public static string OutputInvoiceDocumentTableCreatePart =
				 "CREATE TABLE IF NOT EXISTS OutputInvoiceDocuments " +
				 "(Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
				 "ServerId INTEGER NULL, " +
				 "Identifier GUID, " +
				 "OutputInvoiceId INTEGER NULL, " +
				 "OutputInvoiceIdentifier GUID NULL, " +
				 "OutputInvoiceCode NVARCHAR(48) NULL, " +
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
			"SELECT ServerId, Identifier, OutputInvoiceId, OutputInvoiceIdentifier, " +
            "OutputInvoiceCode, Name, CreateDate, Path, ItemStatus,  " +
			"IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName ";

		public string SqlCommandInsertPart = "INSERT INTO OutputInvoiceDocuments " +
			"(Id, ServerId, Identifier, OutputInvoiceId, OutputInvoiceIdentifier, " +
            "OutputInvoiceCode, Name, CreateDate, Path, ItemStatus,  " +
			"IsSynced, UpdatedAt, CreatedById, CreatedByName, CompanyId, CompanyName) " +

			"VALUES (NULL, @ServerId, @Identifier, @OutputInvoiceId, @OutputInvoiceIdentifier, " +
            "@OutputInvoiceCode, @Name, @CreateDate, @Path, @ItemStatus,  " +
			"@IsSynced, @UpdatedAt, @CreatedById, @CreatedByName, @CompanyId, @CompanyName)";

        #region Helper methods
        private static OutputInvoiceDocumentViewModel Read(SqliteDataReader query)
        {
            int counter = 0;
            OutputInvoiceDocumentViewModel dbEntry = new OutputInvoiceDocumentViewModel();
            dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
            dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
            dbEntry.OutputInvoice = SQLiteHelper.GetOutputInvoice(query, ref counter);
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

        private SqliteCommand AddCreateParameters(SqliteCommand insertCommand, OutputInvoiceDocumentViewModel OutputInvoiceDocument)
        {
            insertCommand.Parameters.AddWithValue("@ServerId", OutputInvoiceDocument.Id);
            insertCommand.Parameters.AddWithValue("@Identifier", OutputInvoiceDocument.Identifier);
            insertCommand.Parameters.AddWithValue("@OutputInvoiceId", ((object)OutputInvoiceDocument.OutputInvoice.Id) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@OutputInvoiceIdentifier", ((object)OutputInvoiceDocument.OutputInvoice.Identifier) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@OutputInvoiceCode", ((object)OutputInvoiceDocument.OutputInvoice.Code) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Name", OutputInvoiceDocument.Name);
            insertCommand.Parameters.AddWithValue("@CreateDate", ((object)OutputInvoiceDocument.CreateDate) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@Path", ((object)OutputInvoiceDocument.Path) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@ItemStatus", ((object)OutputInvoiceDocument.ItemStatus) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@IsSynced", OutputInvoiceDocument.IsSynced);
            insertCommand.Parameters.AddWithValue("@UpdatedAt", ((object)OutputInvoiceDocument.UpdatedAt) ?? DBNull.Value);
            insertCommand.Parameters.AddWithValue("@CreatedById", MainWindow.CurrentUser.Id);
            insertCommand.Parameters.AddWithValue("@CreatedByName", MainWindow.CurrentUser.FirstName + " " + MainWindow.CurrentUser.LastName);
            insertCommand.Parameters.AddWithValue("@CompanyId", MainWindow.CurrentCompany.Id);
            insertCommand.Parameters.AddWithValue("@CompanyName", MainWindow.CurrentCompany.CompanyName);

            return insertCommand;
        }

        #endregion

        public OutputInvoiceDocumentListResponse GetOutputInvoiceDocumentsByOutputInvoice(int companyId, Guid OutputInvoiceIdentifier)
		{
			OutputInvoiceDocumentListResponse response = new OutputInvoiceDocumentListResponse();
			List<OutputInvoiceDocumentViewModel> OutputInvoiceDocuments = new List<OutputInvoiceDocumentViewModel>();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
				db.Open();
				try
				{
					SqliteCommand selectCommand = new SqliteCommand(
						SqlCommandSelectPart +
						"FROM OutputInvoiceDocuments " +
						"WHERE OutputInvoiceIdentifier = @OutputInvoiceIdentifier " +
						"AND CompanyId = @CompanyId " +
						"ORDER BY IsSynced, Id DESC;", db);
					selectCommand.Parameters.AddWithValue("@OutputInvoiceIdentifier", OutputInvoiceIdentifier);
					selectCommand.Parameters.AddWithValue("@CompanyId", companyId);

					SqliteDataReader query = selectCommand.ExecuteReader();

					while (query.Read())
					{
						int counter = 0;
						OutputInvoiceDocumentViewModel dbEntry = new OutputInvoiceDocumentViewModel();
						dbEntry.Id = SQLiteHelper.GetInt(query, ref counter);
						dbEntry.Identifier = SQLiteHelper.GetGuid(query, ref counter);
						dbEntry.OutputInvoice = SQLiteHelper.GetOutputInvoice(query, ref counter);
						dbEntry.Name = SQLiteHelper.GetString(query, ref counter);
						dbEntry.CreateDate = SQLiteHelper.GetDateTime(query, ref counter);
						dbEntry.Path = SQLiteHelper.GetString(query, ref counter);
                        dbEntry.ItemStatus = SQLiteHelper.GetInt(query, ref counter);
                        dbEntry.IsSynced = SQLiteHelper.GetBoolean(query, ref counter);
						dbEntry.UpdatedAt = SQLiteHelper.GetDateTime(query, ref counter);
						dbEntry.CreatedBy = SQLiteHelper.GetCreatedBy(query, ref counter);
						dbEntry.Company = SQLiteHelper.GetCompany(query, ref counter);
						OutputInvoiceDocuments.Add(dbEntry);
					}

				}
				catch (SqliteException error)
				{
					MainWindow.ErrorMessage = error.Message;
					response.Success = false;
					response.Message = error.Message;
					response.OutputInvoiceDocuments = new List<OutputInvoiceDocumentViewModel>();
					return response;
				}
				db.Close();
			}
			response.Success = true;
			response.OutputInvoiceDocuments = OutputInvoiceDocuments;
			return response;
		}

		public OutputInvoiceDocumentResponse GetOutputInvoiceDocument(Guid identifier)
		{
			OutputInvoiceDocumentResponse response = new OutputInvoiceDocumentResponse();
			OutputInvoiceDocumentViewModel OutputInvoiceDocument = new OutputInvoiceDocumentViewModel();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
				db.Open();
				try
				{
					SqliteCommand selectCommand = new SqliteCommand(
						SqlCommandSelectPart +
						"FROM OutputInvoiceDocuments " +
						"WHERE Identifier = @Identifier;", db);
					selectCommand.Parameters.AddWithValue("@Identifier", identifier);

					SqliteDataReader query = selectCommand.ExecuteReader();

                    if (query.Read())
                    {
                        OutputInvoiceDocumentViewModel dbEntry = Read(query);
                        OutputInvoiceDocument = dbEntry;
                    }
                }
				catch (SqliteException error)
				{
					MainWindow.ErrorMessage = error.Message;
					response.Success = false;
					response.Message = error.Message;
					response.OutputInvoiceDocument = new OutputInvoiceDocumentViewModel();
					return response;
				}
				db.Close();
			}
			response.Success = true;
			response.OutputInvoiceDocument = OutputInvoiceDocument;
			return response;
		}

        public void Sync(IOutputInvoiceDocumentService OutputInvoiceDocumentService, Action<int, int> callback = null)
        {
            try
            {
                SyncOutputInvoiceDocumentRequest request = new SyncOutputInvoiceDocumentRequest();
                request.CompanyId = MainWindow.CurrentCompanyId;
                request.LastUpdatedAt = GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                int toSync = 0;
                int syncedItems = 0;

                OutputInvoiceDocumentListResponse response = OutputInvoiceDocumentService.Sync(request);
                if (response.Success)
                {
                    toSync = response?.OutputInvoiceDocuments?.Count ?? 0;
                    List<OutputInvoiceDocumentViewModel> OutputInvoiceDocumentsFromDB = response.OutputInvoiceDocuments;
                    foreach (var OutputInvoiceDocument in OutputInvoiceDocumentsFromDB.OrderBy(x => x.Id))
                    {
                            Delete(OutputInvoiceDocument.Identifier);
                            if (OutputInvoiceDocument.IsActive)
                            {
                                OutputInvoiceDocument.IsSynced = true;
                                Create(OutputInvoiceDocument);
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
					SqliteCommand selectCommand = new SqliteCommand("SELECT COUNT(*) from OutputInvoiceDocuments WHERE CompanyId = @CompanyId", db);
					selectCommand.Parameters.AddWithValue("@CompanyId", companyId);
					SqliteDataReader query = selectCommand.ExecuteReader();
					int count = query.Read() ? query.GetInt32(0) : 0;

					if (count == 0)
						return null;
					else
					{
						selectCommand = new SqliteCommand("SELECT MAX(UpdatedAt) from OutputInvoiceDocuments WHERE CompanyId = @CompanyId", db);
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

		public OutputInvoiceDocumentResponse Create(OutputInvoiceDocumentViewModel OutputInvoiceDocument)
		{
			OutputInvoiceDocumentResponse response = new OutputInvoiceDocumentResponse();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
                db.Open();
                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = SqlCommandInsertPart;

                try
                {
                    insertCommand = AddCreateParameters(insertCommand, OutputInvoiceDocument);
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

        //public OutputInvoiceDocumentResponse UpdateSyncStatus(Guid identifier, string code, DateTime? updatedAt, int serverId, bool isSynced)
        //{
        //	OutputInvoiceDocumentResponse response = new OutputInvoiceDocumentResponse();

        //	using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
        //	{
        //		db.Open();

        //		SqliteCommand insertCommand = new SqliteCommand();
        //		insertCommand.Connection = db;

        //		insertCommand.CommandText = "UPDATE OutputInvoiceDocuments SET " +
        //			"IsSynced = @IsSynced, " +
        //			"Code = @Code, " +
        //			"UpdatedAt = @UpdatedAt, " +
        //			"ServerId = @ServerId " +
        //			"WHERE Identifier = @Identifier ";

        //		insertCommand.Parameters.AddWithValue("@IsSynced", isSynced);
        //		insertCommand.Parameters.AddWithValue("@Code", code);
        //		insertCommand.Parameters.AddWithValue("@UpdatedAt", updatedAt);
        //		insertCommand.Parameters.AddWithValue("@ServerId", serverId);
        //		insertCommand.Parameters.AddWithValue("@Identifier", identifier);

        //		try
        //		{
        //			insertCommand.ExecuteReader();
        //		}
        //		catch (SqliteException error)
        //		{
        //			MainWindow.ErrorMessage = error.Message;
        //			response.Success = false;
        //			response.Message = error.Message;
        //			return response;
        //		}
        //		db.Close();

        //		response.Success = true;
        //		return response;
        //	}
        //}

        public OutputInvoiceDocumentResponse Delete(Guid identifier)
		{
			OutputInvoiceDocumentResponse response = new OutputInvoiceDocumentResponse();

			using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
			{
				db.Open();

				SqliteCommand insertCommand = new SqliteCommand();
				insertCommand.Connection = db;

				//Use parameterized query to prevent SQL injection attacks
				insertCommand.CommandText =
					"DELETE FROM OutputInvoiceDocuments WHERE Identifier = @Identifier";
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

		//public OutputInvoiceDocumentResponse DeleteAll()
		//{
		//	OutputInvoiceDocumentResponse response = new OutputInvoiceDocumentResponse();

		//	try
		//	{
		//		using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
		//		{
		//			db.Open();
		//			db.EnableExtensions(true);

		//			SqliteCommand insertCommand = new SqliteCommand();
		//			insertCommand.Connection = db;

		//			//Use parameterized query to prevent SQL injection attacks
		//			insertCommand.CommandText = "DELETE FROM OutputInvoiceDocuments";
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
        public OutputInvoiceDocumentResponse SetStatusDeleted(Guid identifier)
        {
            OutputInvoiceDocumentResponse response = new OutputInvoiceDocumentResponse();

            using (SqliteConnection db = new SqliteConnection("Filename=SirmiumERPGFC.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText =
                    "UPDATE InputInvoiceDocuments SET ItemStatus = @ItemStatus WHERE Identifier = @Identifier";
                insertCommand.Parameters.AddWithValue("@ItemStatus", ItemStatus.Deleted);
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
    }
}

