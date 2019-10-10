using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.Invoices
{
	public class InputInvoiceDocumentView
	{
		public static void CreateView()
		{
			string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

			SqlConnection conn = new SqlConnection(connectionString);
			conn.Open();

			string strSQLCommand = "DROP VIEW IF EXISTS vInputInvoiceDocuments";
			SqlCommand command = new SqlCommand(strSQLCommand, conn);
			string returnvalue = (string)command.ExecuteScalar();

			strSQLCommand =
				"CREATE VIEW vInputInvoiceDocuments AS " +
				"SELECT inputInvoiceDocument.Id AS InputInvoiceDocumentId, inputInvoiceDocument.Identifier AS InputInvoiceDocumentIdentifier, " +
				"inputInvoice.Id AS InputInvoiceId, inputInvoice.Identifier AS InputInvoiceIdentifier, inputInvoice.Code AS InputInvoiceCode, " +
                "inputInvoiceDocument.Name, inputInvoiceDocument.CreateDate, inputInvoiceDocument.Path, inputInvoiceDocument.ItemStatus, " +
				"inputInvoiceDocument.Active AS Active, " +
				"(SELECT MAX(v) FROM (VALUES (inputInvoiceDocument.UpdatedAt), (inputInvoice.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
				"createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
				"company.Id AS CompanyId, company.Name AS CompanyName " +
				"FROM InputInvoiceDocuments inputInvoiceDocument " +
				"LEFT JOIN InputInvoices inputInvoice ON inputInvoiceDocument.InputInvoiceId = inputInvoice.Id " +
				"LEFT JOIN Users createdBy ON inputInvoiceDocument.CreatedById = createdBy.Id " +
				"LEFT JOIN Companies company ON inputInvoiceDocument.CompanyId = company.Id;";


			command = new SqlCommand(strSQLCommand, conn);
			returnvalue = (string)command.ExecuteScalar();

			conn.Close();
		}
	}
}

