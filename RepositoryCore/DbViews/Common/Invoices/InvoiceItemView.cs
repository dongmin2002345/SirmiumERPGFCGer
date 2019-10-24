using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.Invoices
{
    public class InvoiceItemView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vInvoiceItems";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vInvoiceItems AS " +
                "SELECT " +
                "invoiceItem.Id AS InvoiceItemId, " +
                "invoiceItem.Identifier AS InvoiceItemIdentifier, " +
                "invoiceItem.Code AS InvoiceItemCode, " +
                "invoiceItem.Name AS InvoiceItemName, " +
                "invoice.Id AS InvoiceId, " +
                "invoice.Identifier AS InvoiceIdentifier, " +
                "invoice.Code AS InvoiceCode, " +

                "invoiceItem.UnitOfMeasure, " +
                "invoiceItem.Quantity, " +
                "invoiceItem.PriceWithPDV, " +
                "invoiceItem.PriceWithoutPDV, " +
                "invoiceItem.Discount, " +
                "invoiceItem.PDVPercent, " +
                "invoiceItem.PDV, " +
                "invoiceItem.Amount, " +
                "invoiceItem.ItemStatus, " +
                "invoiceItem.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (invoiceItem.UpdatedAt), (invoice.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM InvoiceItems invoiceItem " +
                "LEFT JOIN Invoices invoice ON invoiceItem.InvoiceId = invoice.Id " +
                "LEFT JOIN Users createdBy ON invoiceItem.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON invoiceItem.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
