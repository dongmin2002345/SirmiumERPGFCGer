using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.Invoices
{
    public class OutputInvoiceNoteView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vOutputInvoiceNotes";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vOutputInvoiceNotes AS " +
                "SELECT outputInvoiceNote.Id AS OutputInvoiceNoteId, outputInvoiceNote.Identifier AS OutputInvoiceNoteIdentifier, " +
                "outputInvoice.Id AS OutputInvoiceId, outputInvoice.Identifier AS OutputInvoiceIdentifier, outputInvoice.Code AS OutputInvoiceCode, " +
                "outputInvoiceNote.Note, outputInvoiceNote.NoteDate, outputInvoiceNote.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (outputInvoiceNote.UpdatedAt), (outputInvoice.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM OutputInvoiceNotes outputInvoiceNote " +
                "LEFT JOIN OutputInvoices outputInvoice ON outputInvoiceNote.OutputInvoiceId = outputInvoice.Id " +
                "LEFT JOIN Users createdBy ON outputInvoiceNote.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON outputInvoiceNote.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
