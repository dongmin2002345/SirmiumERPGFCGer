using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.ConstructionSites
{
    public class ConstructionSiteView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vConstructionSites";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vConstructionSites AS " +
                "SELECT " +
                "constructionSite.Id AS ConstructionSiteId, " +
                "constructionSite.Identifier AS ConstructionSiteIdentifier, " +
                "constructionSite.Code AS ConstructionSiteCode, " +
                "constructionSite.InternalCode AS ConstructionSiteInternalCode, " +
                "constructionSite.Name AS ConstructionSiteName, " +

                "city.Id AS CityId, " +
                "city.Identifier AS CityIdentifier, " +
                "city.Code AS CityCode, " +
                "city.Name AS CityName, " +

                "country.Id AS CountryId, " +
                "country.Identifier AS CountryIdentifier, " +
                "country.Code AS CountryCode, " +
                "country.Name AS CountryName, " +

                "businessPartner.Id AS BusinessPartnerId, " +
                "businessPartner.Identifier AS BusinessPartnerIdentifier, " +
                "businessPartner.Code AS BusinessPartnerCode, " +
                "businessPartner.Name AS BusinessPartnerName, " +

                "status.Id AS StatusId, " +
                "status.Identifier AS StatusIdentifier, " +
                "status.Code AS StatusCode, " +
                "status.Name AS StatusName, " +

                "constructionSite.StatusDate, " +
                "constructionSite.Address, " +
                "constructionSite.MaxWorkers, " +
                "constructionSite.ProContractDate, " +
                "constructionSite.ContractStart, " +
                "constructionSite.ContractExpiration, " +

                "constructionSite.PaymentDate, " +
                "constructionSite.Path, " +
                "constructionSite.PaymentValue, " +

                "shipment.Id AS ShipmentId, " +
                "shipment.Identifier AS ShipmentIdentifier, " +
                "shipment.Code AS ShipmentCode, " +
                "shipment.ShipmentNumber AS ShipmentNumber, " +

                "constructionSite.Active AS Active, " +

                "(SELECT MAX(v) FROM (VALUES " +
                "(constructionSite.UpdatedAt), " +
                "(city.UpdatedAt), (country.UpdatedAt), (businessPartner.UpdatedAt), (status.UpdatedAt), (shipment.UpdatedAt)) AS value(v)) AS UpdatedAt, " +

                "createdBy.Id AS CreatedById, " +
                "createdBy.FirstName AS CreatedByFirstName, " +
                "createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +

                "FROM ConstructionSites constructionSite " +
                "LEFT JOIN Cities city ON constructionSite.CityId = city.Id " +
                "LEFT JOIN Countries country ON constructionSite.CountryId = country.Id " +
                "LEFT JOIN BusinessPartners businessPartner ON constructionSite.BusinessPartnerId = businessPartner.Id " +
                "LEFT JOIN Statuses status ON constructionSite.StatusId = status.Id " +
                "LEFT JOIN Shipments shipment ON constructionSite.ShipmentId = shipment.Id " +
                "LEFT JOIN Users createdBy ON constructionSite.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON constructionSite.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
