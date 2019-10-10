using Configurator;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Common.InputInvoices;
using RepositoryCore.Abstractions.Common.Invoices;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.Invoices
{
	public class InputInvoiceDocumentViewRepository : IInputInvoiceDocumentRepository
	{
		ApplicationDbContext context;
		private string connectionString;

		public InputInvoiceDocumentViewRepository(ApplicationDbContext context)
		{
			this.context = context;
			connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
		}

		public List<InputInvoiceDocument> GetInputInvoiceDocuments(int companyId)
		{
			List<InputInvoiceDocument> InputInvoiceDocuments = new List<InputInvoiceDocument>();

			string queryString =
				"SELECT InputInvoiceDocumentId, InputInvoiceDocumentIdentifier, " +
				"InputInvoiceId, InputInvoiceIdentifier, InputInvoiceCode, " +
                "Name, CreateDate, Path, ItemStatus, " +
				"Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
				"CompanyId, CompanyName " +
				"FROM vInputInvoiceDocuments " +
				"WHERE CompanyId = @CompanyId AND Active = 1;";

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				SqlCommand command = connection.CreateCommand();
				command.CommandText = queryString;
				command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

				connection.Open();
				using (SqlDataReader reader = command.ExecuteReader())
				{
					InputInvoiceDocument inputInvoiceDocument;
					while (reader.Read())
					{
						inputInvoiceDocument = new InputInvoiceDocument();
						inputInvoiceDocument.Id = Int32.Parse(reader["InputInvoiceDocumentId"].ToString());
						inputInvoiceDocument.Identifier = Guid.Parse(reader["InputInvoiceDocumentIdentifier"].ToString());

						if (reader["InputInvoiceId"] != DBNull.Value)
						{
							inputInvoiceDocument.InputInvoice = new InputInvoice();
							inputInvoiceDocument.InputInvoiceId = Int32.Parse(reader["InputInvoiceId"].ToString());
							inputInvoiceDocument.InputInvoice.Id = Int32.Parse(reader["InputInvoiceId"].ToString());
							inputInvoiceDocument.InputInvoice.Identifier = Guid.Parse(reader["InputInvoiceIdentifier"].ToString());
							inputInvoiceDocument.InputInvoice.Code = reader["InputInvoiceCode"].ToString();
							
						}

						if (reader["Name"] != DBNull.Value)
							inputInvoiceDocument.Name = reader["Name"].ToString();
						if (reader["CreateDate"] != DBNull.Value)
							inputInvoiceDocument.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
						if (reader["Path"] != DBNull.Value)
							inputInvoiceDocument.Path = reader["Path"].ToString();
                        if (reader["ItemStatus"] != DBNull.Value)
                            inputInvoiceDocument.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());
                        inputInvoiceDocument.Active = bool.Parse(reader["Active"].ToString());
						inputInvoiceDocument.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

						if (reader["CreatedById"] != DBNull.Value)
						{
							inputInvoiceDocument.CreatedBy = new User();
							inputInvoiceDocument.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
							inputInvoiceDocument.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
							inputInvoiceDocument.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
							inputInvoiceDocument.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
						}

						if (reader["CompanyId"] != DBNull.Value)
						{
							inputInvoiceDocument.Company = new Company();
							inputInvoiceDocument.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
							inputInvoiceDocument.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
							inputInvoiceDocument.Company.Name = reader["CompanyName"].ToString();
						}

						InputInvoiceDocuments.Add(inputInvoiceDocument);
					}
				}
			}
			return InputInvoiceDocuments;

		}

		public List<InputInvoiceDocument> GetInputInvoiceDocumentsByInputInvoice(int InputInvoiceId)
		{
			List<InputInvoiceDocument> InputInvoiceDocuments = new List<InputInvoiceDocument>();

			string queryString =
				"SELECT InputInvoiceDocumentId, InputInvoiceDocumentIdentifier, " +
				"InputInvoiceId, InputInvoiceIdentifier, InputInvoiceCode, " +
                "Name, CreateDate, Path, ItemStatus, " +
				"Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
				"CompanyId, CompanyName " +
				"FROM vInputInvoiceDocuments " +
				"WHERE InputInvoiceId = @InputInvoiceId AND Active = 1;";

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				SqlCommand command = connection.CreateCommand();
				command.CommandText = queryString;
				command.Parameters.Add(new SqlParameter("@InputInvoiceId", InputInvoiceId));

				connection.Open();
				using (SqlDataReader reader = command.ExecuteReader())
				{
					InputInvoiceDocument inputInvoiceDocument;
					while (reader.Read())
					{
						inputInvoiceDocument = new InputInvoiceDocument();
						inputInvoiceDocument.Id = Int32.Parse(reader["InputInvoiceDocumentId"].ToString());
						inputInvoiceDocument.Identifier = Guid.Parse(reader["InputInvoiceDocumentIdentifier"].ToString());

						if (reader["InputInvoiceId"] != DBNull.Value)
						{
							inputInvoiceDocument.InputInvoice = new InputInvoice();
							inputInvoiceDocument.InputInvoiceId = Int32.Parse(reader["InputInvoiceId"].ToString());
							inputInvoiceDocument.InputInvoice.Id = Int32.Parse(reader["InputInvoiceId"].ToString());
							inputInvoiceDocument.InputInvoice.Identifier = Guid.Parse(reader["InputInvoiceIdentifier"].ToString());
							inputInvoiceDocument.InputInvoice.Code = reader["InputInvoiceCode"].ToString();

						}

						if (reader["Name"] != DBNull.Value)
							inputInvoiceDocument.Name = reader["Name"].ToString();
						if (reader["CreateDate"] != DBNull.Value)
							inputInvoiceDocument.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
						if (reader["Path"] != DBNull.Value)
							inputInvoiceDocument.Path = reader["Path"].ToString();
                        if (reader["ItemStatus"] != DBNull.Value)
                            inputInvoiceDocument.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());
                        inputInvoiceDocument.Active = bool.Parse(reader["Active"].ToString());
						inputInvoiceDocument.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

						if (reader["CreatedById"] != DBNull.Value)
						{
							inputInvoiceDocument.CreatedBy = new User();
							inputInvoiceDocument.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
							inputInvoiceDocument.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
							inputInvoiceDocument.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
							inputInvoiceDocument.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
						}

						if (reader["CompanyId"] != DBNull.Value)
						{
							inputInvoiceDocument.Company = new Company();
							inputInvoiceDocument.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
							inputInvoiceDocument.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
							inputInvoiceDocument.Company.Name = reader["CompanyName"].ToString();
						}

						InputInvoiceDocuments.Add(inputInvoiceDocument);
					}
				}
			}
			return InputInvoiceDocuments;

			
		}

		public List<InputInvoiceDocument> GetInputInvoiceDocumentsNewerThen(int companyId, DateTime lastUpdateTime)
		{
			List<InputInvoiceDocument> InputInvoiceDocuments = new List<InputInvoiceDocument>();

			string queryString =
				"SELECT InputInvoiceDocumentId, InputInvoiceDocumentIdentifier, " +
				"InputInvoiceId, InputInvoiceIdentifier, InputInvoiceCode, " +
                "Name, CreateDate, Path, ItemStatus, " +
				"Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
				"CompanyId, CompanyName " +
				"FROM vInputInvoiceDocuments " +
				"WHERE CompanyId = @CompanyId " +
				"AND CONVERT(DATETIME, CONVERT(VARCHAR(20), UpdatedAt, 120)) > CONVERT(DATETIME, CONVERT(VARCHAR(20), @LastUpdateTime, 120));";

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				SqlCommand command = connection.CreateCommand();
				command.CommandText = queryString;
				command.Parameters.Add(new SqlParameter("@CompanyId", companyId));
				command.Parameters.Add(new SqlParameter("@LastUpdateTime", lastUpdateTime));

				connection.Open();
				using (SqlDataReader reader = command.ExecuteReader())
				{
					InputInvoiceDocument inputInvoiceDocument;
					while (reader.Read())
					{
						inputInvoiceDocument = new InputInvoiceDocument();
						inputInvoiceDocument.Id = Int32.Parse(reader["InputInvoiceDocumentId"].ToString());
						inputInvoiceDocument.Identifier = Guid.Parse(reader["InputInvoiceDocumentIdentifier"].ToString());

						if (reader["InputInvoiceId"] != DBNull.Value)
						{
							inputInvoiceDocument.InputInvoice = new InputInvoice();
							inputInvoiceDocument.InputInvoiceId = Int32.Parse(reader["InputInvoiceId"].ToString());
							inputInvoiceDocument.InputInvoice.Id = Int32.Parse(reader["InputInvoiceId"].ToString());
							inputInvoiceDocument.InputInvoice.Identifier = Guid.Parse(reader["InputInvoiceIdentifier"].ToString());
							inputInvoiceDocument.InputInvoice.Code = reader["InputInvoiceCode"].ToString();

						}

						if (reader["Name"] != DBNull.Value)
							inputInvoiceDocument.Name = reader["Name"].ToString();
						if (reader["CreateDate"] != DBNull.Value)
							inputInvoiceDocument.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
						if (reader["Path"] != DBNull.Value)
							inputInvoiceDocument.Path = reader["Path"].ToString();
                        if (reader["ItemStatus"] != DBNull.Value)
                            inputInvoiceDocument.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());
                        inputInvoiceDocument.Active = bool.Parse(reader["Active"].ToString());
						inputInvoiceDocument.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

						if (reader["CreatedById"] != DBNull.Value)
						{
							inputInvoiceDocument.CreatedBy = new User();
							inputInvoiceDocument.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
							inputInvoiceDocument.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
							inputInvoiceDocument.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
							inputInvoiceDocument.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
						}

						if (reader["CompanyId"] != DBNull.Value)
						{
							inputInvoiceDocument.Company = new Company();
							inputInvoiceDocument.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
							inputInvoiceDocument.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
							inputInvoiceDocument.Company.Name = reader["CompanyName"].ToString();
						}

						InputInvoiceDocuments.Add(inputInvoiceDocument);
					}
				}
			}
			return InputInvoiceDocuments;
		}

		public InputInvoiceDocument Create(InputInvoiceDocument InputInvoiceDocument)
		{
			if (context.InputInvoiceDocuments.Where(x => x.Identifier != null && x.Identifier == InputInvoiceDocument.Identifier).Count() == 0)
			{
				InputInvoiceDocument.Id = 0;

				InputInvoiceDocument.Active = true;
                InputInvoiceDocument.UpdatedAt = DateTime.Now;
                InputInvoiceDocument.CreatedAt = DateTime.Now;
                context.InputInvoiceDocuments.Add(InputInvoiceDocument);
				return InputInvoiceDocument;
			}
			else
			{
				// Load item that will be updated
				InputInvoiceDocument dbEntry = context.InputInvoiceDocuments
					.FirstOrDefault(x => x.Identifier == InputInvoiceDocument.Identifier && x.Active == true);

				if (dbEntry != null)
				{
					dbEntry.CompanyId = InputInvoiceDocument.CompanyId ?? null;
					dbEntry.CreatedById = InputInvoiceDocument.CreatedById ?? null;
                    dbEntry.InputInvoiceId = InputInvoiceDocument.InputInvoiceId ?? null;
                    // Set properties
                    dbEntry.Name = InputInvoiceDocument.Name;
					dbEntry.CreateDate = InputInvoiceDocument.CreateDate;
					dbEntry.Path = InputInvoiceDocument.Path;
                    dbEntry.ItemStatus = InputInvoiceDocument.ItemStatus;
                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
				}

				return dbEntry;
			}
		}

		public InputInvoiceDocument Delete(Guid identifier)
		{
			InputInvoiceDocument dbEntry = context.InputInvoiceDocuments
				.FirstOrDefault(x => x.Identifier == identifier && x.Active == true);

			if (dbEntry != null)
			{
				dbEntry.Active = false;
				dbEntry.UpdatedAt = DateTime.Now;
			}
			return dbEntry;
		}
	}
}

