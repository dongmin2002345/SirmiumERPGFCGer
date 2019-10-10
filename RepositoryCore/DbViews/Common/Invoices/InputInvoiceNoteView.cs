using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.Invoices
{
    public class InputInvoiceNoteView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vInputInvoiceNotes";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vInputInvoiceNotes AS " +
                "SELECT " +
                "inputInvoiceNote.Id AS InputInvoiceNoteId, " +
                "inputInvoiceNote.Identifier AS InputInvoiceNoteIdentifier, " +
                "inputInvoice.Id AS InputInvoiceId, " +
                "inputInvoice.Identifier AS InputInvoiceIdentifier, " +
                "inputInvoice.Code AS InputInvoiceCode, " +
                "inputInvoiceNote.Note, " +
                "inputInvoiceNote.NoteDate, " +
                "inputInvoiceNote.ItemStatus, " +
                "inputInvoiceNote.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (inputInvoiceNote.UpdatedAt), (inputInvoice.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM InputInvoiceNotes inputInvoiceNote " +
                "LEFT JOIN InputInvoices inputInvoice ON inputInvoiceNote.InputInvoiceId = inputInvoice.Id " +
                "LEFT JOIN Users createdBy ON inputInvoiceNote.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON inputInvoiceNote.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
