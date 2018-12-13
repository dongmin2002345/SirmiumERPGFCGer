using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.BusinessPartners
{
    public class BusinessPartnerByConstructionSiteView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vBusinessPartnerByConstructionSites";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vBusinessPartnerByConstructionSites AS " +
                "SELECT businessPartnerByConstructionSite.Id AS BusinessPartnerByConstructionSiteId, businessPartnerByConstructionSite.Identifier AS BusinessPartnerByConstructionSiteIdentifier, businessPartnerByConstructionSite.Code AS BusinessPartnerByConstructionSiteCode, businessPartnerByConstructionSite.StartDate, businessPartnerByConstructionSite.EndDate, businessPartnerByConstructionSite.RealEndDate, businessPartnerByConstructionSite.MaxNumOfEmployees, " +
                "businessPartner.Id AS BusinessPartnerId, businessPartner.Identifier AS BusinessPartnerIdentifier, businessPartner.Code AS BusinessPartnerCode, businessPartner.NameGer AS BusinessPartnerName, " +
                "businessPartnerByConstructionSite.BusinessPartnerCount, " +
                "constructionSite.Id AS ConstructionSiteId, constructionSite.Identifier AS ConstructionSiteIdentifier, constructionSite.Code AS ConstructionSiteCode, constructionSite.Name AS ConstructionSiteName, " +
                "businessPartnerByConstructionSite.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (businessPartnerByConstructionSite.UpdatedAt), (businessPartner.UpdatedAt), (constructionSite.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM BusinessPartnerByConstructionSites businessPartnerByConstructionSite " +
                "JOIN BusinessPartners businessPartner ON businessPartnerByConstructionSite.BusinessPartnerId = businessPartner.Id " +
                "JOIN ConstructionSites constructionSite ON businessPartnerByConstructionSite.ConstructionSiteId = constructionSite.Id " +
                "LEFT JOIN Users createdBy ON businessPartnerByConstructionSite.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON businessPartnerByConstructionSite.CompanyId = company.Id " +
                "WHERE constructionSite.Active = 1 AND businessPartner.Active = 1 ";

        command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
