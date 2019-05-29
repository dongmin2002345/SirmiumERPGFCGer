using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.Invoices
{
	public class OutputInvoiceDocumentView
	{
		public static void CreateView()
		{
			string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

			SqlConnection conn = new SqlConnection(connectionString);
			conn.Open();

			string strSQLCommand = "DROP VIEW IF EXISTS vOutputInvoiceDocuments";
			SqlCommand command = new SqlCommand(strSQLCommand, conn);
			string returnvalue = (string)command.ExecuteScalar();

			strSQLCommand =
				"CREATE VIEW vOutputInvoiceDocuments AS " +
				"SELECT outputInvoiceDocument.Id AS OutputInvoiceDocumentId, outputInvoiceDocument.Identifier AS OutputInvoiceDocumentIdentifier, " +
				"outputInvoice.Id AS OutputInvoiceId, outputInvoice.Identifier AS OutputInvoiceIdentifier, outputInvoice.Code AS OutputInvoiceCode, " +
				"outputInvoiceDocument.Name, outputInvoiceDocument.CreateDate, outputInvoiceDocument.Path, " +
				"outputInvoiceDocument.Active AS Active, " +
				"(SELECT MAX(v) FROM (VALUES (outputInvoiceDocument.UpdatedAt), (outputInvoice.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
				"createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
				"company.Id AS CompanyId, company.Name AS CompanyName " +
				"FROM OutputInvoiceDocuments outputInvoiceDocument " +
				"LEFT JOIN OutputInvoices outputInvoice ON outputInvoiceDocument.OutputInvoiceId = outputInvoice.Id " +
				"LEFT JOIN Users createdBy ON outputInvoiceDocument.CreatedById = createdBy.Id " +
				"LEFT JOIN Companies company ON outputInvoiceDocument.CompanyId = company.Id;";


			command = new SqlCommand(strSQLCommand, conn);
			returnvalue = (string)command.ExecuteScalar();

			conn.Close();
		}
	}
}
