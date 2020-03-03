using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.BusinessPartners
{
    public class BusinessPartnerView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vBusinessPartners";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vBusinessPartners AS " +
                "SELECT businessPartner.Id AS BusinessPartnerId, businessPartner.Identifier AS BusinessPartnerIdentifier, businessPartner.Code AS BusinessPartnerCode, businessPartner.InternalCode AS BusinessPartnerInternalCode, businessPartner.Name AS BusinessPartnerName, " +
                "businessPartner.PIB, businessPartner.PIO, businessPartner.IdentificationNumber, businessPartner.DueDate, businessPartner.WebSite, businessPartner.ContactPerson, businessPartner.IsInPDV, businessPartner.JBKJS, businessPartner.NameGer, businessPartner.IsInPDVGer, " +
                "taxAdministration.Id AS TaxAdministrationId, taxAdministration.Identifier AS TaxAdministrationIdentifier, taxAdministration.Code AS TaxAdministrationCode, taxAdministration.Name AS TaxAdministrationName, " +
                "businessPartner.IBAN, businessPartner.BetriebsNumber, businessPartner.Customer, " +
                "country.Id AS CountryId, country.Identifier AS CountryIdentifier, country.Code AS CountryCode, country.Name AS CountryName, " +
                "sector.Id AS SectorId, sector.Identifier AS SectorIdentifier, sector.Code AS SectorCode, sector.Name AS SectorName, " +
                "agency.Id AS AgencyId, agency.Identifier AS AgencyIdentifier, agency.Code AS AgencyCode, agency.Name AS AgencyName, " +
                "vat.Id AS VatId, vat.Identifier AS VatIdentifier, vat.Code AS VatCode, vat.Description AS VatDescription, vat.Amount AS VatAmount, " +
                "discount.Id AS DiscountId, discount.Identifier AS DiscountIdentifier, discount.Code AS DiscountCode, discount.Name AS DiscountName, discount.Amount AS DiscountAmount, " +
                "businessPartner.TaxNr, businessPartner.CommercialNr, businessPartner.ContactPersonGer, businessPartner.VatDeductionFrom, businessPartner.Path, businessPartner.VatDeductionTo,  businessPartner.PdvType as PdvType, businessPartner.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (businessPartner.UpdatedAt), (taxAdministration.UpdatedAt), (country.UpdatedAt), (sector.UpdatedAt), (agency.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM BusinessPartners businessPartner " +
                "LEFT JOIN TaxAdministrations taxAdministration ON businessPartner.TaxAdministrationId = TaxAdministration.Id " +
                "LEFT JOIN Countries country ON businessPartner.CountryId = country.Id " +
                "LEFT JOIN Sectors sector ON businessPartner.SectorId = sector.Id " +
                "LEFT JOIN Agencies agency ON businessPartner.AgencyId = agency.Id " +
                "LEFT JOIN Vats vat ON businessPartner.VatId = vat.Id " +
                "LEFT JOIN Discounts discount ON businessPartner.DiscountId = discount.Id " +
                "LEFT JOIN Users createdBy ON businessPartner.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON businessPartner.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
