using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.Invoices
{
    public class InvoiceView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vInvoices";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            //string returnvalue = (string)command.ExecuteScalar();
            command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vInvoices AS " +
                "SELECT invoice.Id AS InvoiceId, invoice.Identifier AS InvoiceIdentifier, invoice.Code AS InvoiceCode, invoice.InvoiceNumber AS InvoiceNumber, " +
                "businessPartner.Id AS BuyerId, businessPartner.Identifier AS BuyerIdentifier, businessPartner.Code AS BuyerCode, businessPartner.Name AS BuyerName, businessPartner.InternalCode AS BuyerInternalCode, businessPartner.NameGer AS BuyerNameGer, " +
                "invoice.BuyerName AS EnteredBuyerName, invoice.Address AS Address, invoice.InvoiceDate AS InvoiceDate, invoice.DueDate AS DueDate, invoice.DateOfPayment AS DateOfPayment, " +
                "invoice.Status AS Status, invoice.StatusDate AS StatusDate, invoice.Description AS Description, invoice.CurrencyCode AS CurrencyCode, invoice.CurrencyExchangeRate AS CurrencyExchangeRate, " +
                
                "city.Id AS CityId, city.Identifier AS CityIdentifier, city.ZipCode AS CityZipCode, city.Name AS CityName, " +
                "municipality.Id AS MunicipalityId, municipality.Identifier AS MunicipalityIdentifier, municipality.Code AS MunicipalityCode, municipality.Name AS MunicipalityName, " +
                "vat.Id AS VatId, vat.Identifier AS VatIdentifier, vat.Code AS VatCode, vat.Description AS VatDescription, vat.Amount AS VatAmount, " +
                "discount.Id AS DiscountId, discount.Identifier AS DiscountIdentifier, discount.Code AS DiscountCode, discount.Name AS DiscountName, discount.Amount AS DiscountAmount, " +
                
                "invoice.PdvType, invoice.TotalPrice, invoice.TotalPDV, invoice.TotalRebate, invoice.Active AS Active," +
                "(SELECT MAX(v) FROM (VALUES (invoice.UpdatedAt), (businessPartner.UpdatedAt), (discount.UpdatedAt), (vat.UpdatedAt), (city.UpdatedAt), (municipality.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +

                "FROM Invoices invoice " +
                "LEFT JOIN BusinessPartners businessPartner ON invoice.BuyerId = businessPartner.Id " +
                "LEFT JOIN Discounts discount ON invoice.DiscountId = discount.Id " +
                "LEFT JOIN Vats vat ON invoice.VatId = vat.Id " +
                "LEFT JOIN Cities city ON invoice.CityId = city.Id " +
                "LEFT JOIN Municipalities municipality ON invoice.MunicipalityId = municipality.Id " +
                "LEFT JOIN Users createdBy ON invoice.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON invoice.CompanyId = company.Id;";


            command = new SqlCommand(strSQLCommand, conn);
            //returnvalue = (string)command.ExecuteScalar();
            command.ExecuteScalar();

            conn.Close();
        }
    }
}
