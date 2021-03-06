﻿using Configurator;
using System;
using System.Data.SqlClient;


namespace RepositoryCore.DbViews.Common.Invoices
{
    public class InputInvoiceView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            try
            {
                string strSQLCommand = "DROP VIEW IF EXISTS vInputInvoices";
                SqlCommand command = new SqlCommand(strSQLCommand, conn);
                string returnvalue = (string)command.ExecuteScalar();
            }
            catch (Exception ex) { }

            try
            {
                string strSQLCommand =
                "CREATE VIEW vInputInvoices AS " +
                "SELECT inputInvoice.Id AS InputInvoiceId, inputInvoice.Identifier AS InputInvoiceIdentifier, inputInvoice.Code AS InputInvoiceCode, " +
                "businessPartner.Id AS BusinessPartnerId, businessPartner.Identifier AS BusinessPartnerIdentifier, businessPartner.Code AS BusinessPartnerCode, businessPartner.Name AS BusinessPartnerName, businessPartner.InternalCode AS BusinessPartnerInternalCode, businessPartner.NameGer AS BusinessPartnerNameGer, " +
                "inputInvoice.Supplier, inputInvoice.Address, inputInvoice.InvoiceNumber, inputInvoice.InvoiceDate, inputInvoice.AmountNet, inputInvoice.PDVPercent, inputInvoice.PDV, inputInvoice.AmountGross, inputInvoice.Currency, inputInvoice.DateOfPaymet, inputInvoice.Status, inputInvoice.StatusDate, inputInvoice.Description, inputInvoice.Path, inputInvoice.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (inputInvoice.UpdatedAt), (businessPartner.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM InputInvoices inputInvoice " +
                "LEFT JOIN BusinessPartners businessPartner ON inputInvoice.BusinessPartnerId = businessPartner.Id " +
                "LEFT JOIN Users createdBy ON inputInvoice.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON inputInvoice.CompanyId = company.Id;";


                SqlCommand command = new SqlCommand(strSQLCommand, conn);
                string returnvalue = (string)command.ExecuteScalar();
            }
            catch (Exception ex) { }

            conn.Close();
        }
    }
}
