using Configurator;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Common.OutputInvoices;
using RepositoryCore.Abstractions.Common.Invoices;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.Invoices
{
	class OutputInvoiceDocumentViewRepository : IOutputInvoiceDocumentRepository
	{
		ApplicationDbContext context;
		private string connectionString;

		public OutputInvoiceDocumentViewRepository(ApplicationDbContext context)
		{
			this.context = context;
			connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
		}

		public List<OutputInvoiceDocument> GetOutputInvoiceDocuments(int companyId)
		{
			List<OutputInvoiceDocument> OutputInvoiceDocuments = new List<OutputInvoiceDocument>();

			string queryString =
				"SELECT OutputInvoiceDocumentId, OutputInvoiceDocumentIdentifier, " +
				"OutputInvoiceId, OutputInvoiceIdentifier, OutputInvoiceCode, " +
				"Name, CreateDate, Path, " +
				"Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
				"CompanyId, CompanyName " +
				"FROM vOutputInvoiceDocuments " +
				"WHERE CompanyId = @CompanyId AND Active = 1;";

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				SqlCommand command = connection.CreateCommand();
				command.CommandText = queryString;
				command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

				connection.Open();
				using (SqlDataReader reader = command.ExecuteReader())
				{
					OutputInvoiceDocument outputInvoiceDocument;
					while (reader.Read())
					{
						outputInvoiceDocument = new OutputInvoiceDocument();
						outputInvoiceDocument.Id = Int32.Parse(reader["OutputInvoiceDocumentId"].ToString());
						outputInvoiceDocument.Identifier = Guid.Parse(reader["OutputInvoiceDocumentIdentifier"].ToString());

						if (reader["OutputInvoiceId"] != DBNull.Value)
						{
							outputInvoiceDocument.OutputInvoice = new OutputInvoice();
							outputInvoiceDocument.OutputInvoiceId = Int32.Parse(reader["OutputInvoiceId"].ToString());
							outputInvoiceDocument.OutputInvoice.Id = Int32.Parse(reader["OutputInvoiceId"].ToString());
							outputInvoiceDocument.OutputInvoice.Identifier = Guid.Parse(reader["OutputInvoiceIdentifier"].ToString());
							outputInvoiceDocument.OutputInvoice.Code = reader["OutputInvoiceCode"].ToString();

						}

						if (reader["Name"] != DBNull.Value)
							outputInvoiceDocument.Name = reader["Name"].ToString();
						if (reader["CreateDate"] != DBNull.Value)
							outputInvoiceDocument.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
						if (reader["Path"] != DBNull.Value)
							outputInvoiceDocument.Path = reader["Path"].ToString();

						outputInvoiceDocument.Active = bool.Parse(reader["Active"].ToString());
						outputInvoiceDocument.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

						if (reader["CreatedById"] != DBNull.Value)
						{
							outputInvoiceDocument.CreatedBy = new User();
							outputInvoiceDocument.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
							outputInvoiceDocument.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
							outputInvoiceDocument.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
							outputInvoiceDocument.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
						}

						if (reader["CompanyId"] != DBNull.Value)
						{
							outputInvoiceDocument.Company = new Company();
							outputInvoiceDocument.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
							outputInvoiceDocument.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
							outputInvoiceDocument.Company.Name = reader["CompanyName"].ToString();
						}

						OutputInvoiceDocuments.Add(outputInvoiceDocument);
					}
				}
			}
			return OutputInvoiceDocuments;

		}

		public List<OutputInvoiceDocument> GetOutputInvoiceDocumentsByOutputInvoice(int OutputInvoiceId)
		{
			List<OutputInvoiceDocument> OutputInvoiceDocuments = new List<OutputInvoiceDocument>();

			string queryString =
				"SELECT OutputInvoiceDocumentId, OutputInvoiceDocumentIdentifier, " +
				"OutputInvoiceId, OutputInvoiceIdentifier, OutputInvoiceCode, " +
				"Name, CreateDate, Path, " +
				"Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
				"CompanyId, CompanyName " +
				"FROM vOutputInvoiceDocuments " +
				"WHEREOutputInvoiceId = @OutputInvoiceId AND Active = 1;";

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				SqlCommand command = connection.CreateCommand();
				command.CommandText = queryString;
				command.Parameters.Add(new SqlParameter("@OutputInvoiceId", OutputInvoiceId));

				connection.Open();
				using (SqlDataReader reader = command.ExecuteReader())
				{
					OutputInvoiceDocument outputInvoiceDocument;
					while (reader.Read())
					{
						outputInvoiceDocument = new OutputInvoiceDocument();
						outputInvoiceDocument.Id = Int32.Parse(reader["OutputInvoiceDocumentId"].ToString());
						outputInvoiceDocument.Identifier = Guid.Parse(reader["OutputInvoiceDocumentIdentifier"].ToString());

						if (reader["OutputInvoiceId"] != DBNull.Value)
						{
							outputInvoiceDocument.OutputInvoice = new OutputInvoice();
							outputInvoiceDocument.OutputInvoiceId = Int32.Parse(reader["OutputInvoiceId"].ToString());
							outputInvoiceDocument.OutputInvoice.Id = Int32.Parse(reader["OutputInvoiceId"].ToString());
							outputInvoiceDocument.OutputInvoice.Identifier = Guid.Parse(reader["OutputInvoiceIdentifier"].ToString());
							outputInvoiceDocument.OutputInvoice.Code = reader["OutputInvoiceCode"].ToString();

						}

						if (reader["Name"] != DBNull.Value)
							outputInvoiceDocument.Name = reader["Name"].ToString();
						if (reader["CreateDate"] != DBNull.Value)
							outputInvoiceDocument.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
						if (reader["Path"] != DBNull.Value)
							outputInvoiceDocument.Path = reader["Path"].ToString();

						outputInvoiceDocument.Active = bool.Parse(reader["Active"].ToString());
						outputInvoiceDocument.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

						if (reader["CreatedById"] != DBNull.Value)
						{
							outputInvoiceDocument.CreatedBy = new User();
							outputInvoiceDocument.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
							outputInvoiceDocument.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
							outputInvoiceDocument.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
							outputInvoiceDocument.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
						}

						if (reader["CompanyId"] != DBNull.Value)
						{
							outputInvoiceDocument.Company = new Company();
							outputInvoiceDocument.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
							outputInvoiceDocument.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
							outputInvoiceDocument.Company.Name = reader["CompanyName"].ToString();
						}

						OutputInvoiceDocuments.Add(outputInvoiceDocument);
					}
				}
			}
			return OutputInvoiceDocuments;


		}

		public List<OutputInvoiceDocument> GetOutputInvoiceDocumentsNewerThen(int companyId, DateTime lastUpdateTime)
		{
			List<OutputInvoiceDocument> OutputInvoiceDocuments = new List<OutputInvoiceDocument>();

			string queryString =
				"SELECT OutputInvoiceDocumentId, OutputInvoiceDocumentIdentifier, " +
				"OutputInvoiceId, OutputInvoiceIdentifier, OutputInvoiceCode, " +
				"Name, CreateDate, Path, " +
				"Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
				"CompanyId, CompanyName " +
				"FROM vOutputInvoiceDocuments " +
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
					OutputInvoiceDocument outputInvoiceDocument;
					while (reader.Read())
					{
						outputInvoiceDocument = new OutputInvoiceDocument();
						outputInvoiceDocument.Id = Int32.Parse(reader["OutputInvoiceDocumentId"].ToString());
						outputInvoiceDocument.Identifier = Guid.Parse(reader["OutputInvoiceDocumentIdentifier"].ToString());

						if (reader["OutputInvoiceId"] != DBNull.Value)
						{
							outputInvoiceDocument.OutputInvoice = new OutputInvoice();
							outputInvoiceDocument.OutputInvoiceId = Int32.Parse(reader["OutputInvoiceId"].ToString());
							outputInvoiceDocument.OutputInvoice.Id = Int32.Parse(reader["OutputInvoiceId"].ToString());
							outputInvoiceDocument.OutputInvoice.Identifier = Guid.Parse(reader["OutputInvoiceIdentifier"].ToString());
							outputInvoiceDocument.OutputInvoice.Code = reader["OutputInvoiceCode"].ToString();

						}

						if (reader["Name"] != DBNull.Value)
							outputInvoiceDocument.Name = reader["Name"].ToString();
						if (reader["CreateDate"] != DBNull.Value)
							outputInvoiceDocument.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
						if (reader["Path"] != DBNull.Value)
							outputInvoiceDocument.Path = reader["Path"].ToString();

						outputInvoiceDocument.Active = bool.Parse(reader["Active"].ToString());
						outputInvoiceDocument.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

						if (reader["CreatedById"] != DBNull.Value)
						{
							outputInvoiceDocument.CreatedBy = new User();
							outputInvoiceDocument.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
							outputInvoiceDocument.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
							outputInvoiceDocument.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
							outputInvoiceDocument.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
						}

						if (reader["CompanyId"] != DBNull.Value)
						{
							outputInvoiceDocument.Company = new Company();
							outputInvoiceDocument.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
							outputInvoiceDocument.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
							outputInvoiceDocument.Company.Name = reader["CompanyName"].ToString();
						}

						OutputInvoiceDocuments.Add(outputInvoiceDocument);
					}
				}
			}
			return OutputInvoiceDocuments;
		}

		public OutputInvoiceDocument Create(OutputInvoiceDocument OutputInvoiceDocument)
		{
			if (context.OutputInvoiceDocuments.Where(x => x.Identifier != null && x.Identifier == OutputInvoiceDocument.Identifier).Count() == 0)
			{
				OutputInvoiceDocument.Id = 0;

				OutputInvoiceDocument.Active = true;

				context.OutputInvoiceDocuments.Add(OutputInvoiceDocument);
				return OutputInvoiceDocument;
			}
			else
			{
				// Load item that will be updated
				OutputInvoiceDocument dbEntry = context.OutputInvoiceDocuments
					.FirstOrDefault(x => x.Identifier == OutputInvoiceDocument.Identifier && x.Active == true);

				if (dbEntry != null)
				{
					dbEntry.CompanyId = OutputInvoiceDocument.CompanyId ?? null;
					dbEntry.CreatedById = OutputInvoiceDocument.CreatedById ?? null;

					// Set properties
					dbEntry.Name = OutputInvoiceDocument.Name;
					dbEntry.CreateDate = OutputInvoiceDocument.CreateDate;
					dbEntry.Path = OutputInvoiceDocument.Path;

					// Set timestamp
					dbEntry.UpdatedAt = DateTime.Now;
				}

				return dbEntry;
			}
		}

		public OutputInvoiceDocument Delete(Guid identifier)
		{
			OutputInvoiceDocument dbEntry = context.OutputInvoiceDocuments
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

