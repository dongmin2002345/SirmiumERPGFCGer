using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.Invoices
{
    public class OutputInvoiceView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vOutputInvoices";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            //string returnvalue = (string)command.ExecuteScalar();
            command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vOutputInvoices AS " +
                "SELECT outputInvoice.Id AS OutputInvoiceId, outputInvoice.Identifier AS OutputInvoiceIdentifier, outputInvoice.Code AS OutputInvoiceCode, " +
                "businessPartner.Id AS BusinessPartnerId, businessPartner.Identifier AS BusinessPartnerIdentifier, businessPartner.Code AS BusinessPartnerCode, businessPartner.Name AS BusinessPartnerName, " +
                "outputInvoice.Supplier, outputInvoice.Address, outputInvoice.InvoiceNumber, outputInvoice.InvoiceDate, outputInvoice.AmountNet, outputInvoice.PdvPercent, outputInvoice.Pdv, outputInvoice.AmountGross, outputInvoice.Currency, outputInvoice.DateOfPayment, outputInvoice.Status, outputInvoice.StatusDate, outputInvoice.Description, outputInvoice.Path, outputInvoice.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (outputInvoice.UpdatedAt), (businessPartner.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM OutputInvoices outputInvoice " +
                "LEFT JOIN BusinessPartners businessPartner ON outputInvoice.BusinessPartnerId = businessPartner.Id " +
                "LEFT JOIN Users createdBy ON outputInvoice.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON outputInvoice.CompanyId = company.Id;";


            command = new SqlCommand(strSQLCommand, conn);
            //returnvalue = (string)command.ExecuteScalar();
            command.ExecuteScalar();

            conn.Close();
        }
    }
}
