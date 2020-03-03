using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.ConstructionSites
{
    public class ConstructionSiteDocumentView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vConstructionSiteDocuments";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vConstructionSiteDocuments AS " +
                "SELECT " +
                "constructionSiteDocument.Id AS ConstructionSiteDocumentId, " +
                "constructionSiteDocument.Identifier AS ConstructionSiteDocumentIdentifier, " +

                "constructionSite.Id AS ConstructionSiteId, " +
                "constructionSite.Identifier AS ConstructionSiteIdentifier, " +
                "constructionSite.Code AS ConstructionSiteCode, " +
                "constructionSite.Name AS ConstructionSiteName, " +
                "constructionSite.InternalCode AS ConstructionSiteInternalCode, " +

                "constructionSiteDocument.Name, " +
                "constructionSiteDocument.CreateDate, " +
                "constructionSiteDocument.Path, " +
                "constructionSiteDocument.ItemStatus," +
                "constructionSiteDocument.Active AS Active, " +

                "(SELECT MAX(v) FROM (VALUES " +
                "(constructionSiteDocument.UpdatedAt), " +
                "(constructionSite.UpdatedAt)) AS value(v)) AS UpdatedAt, " +

                "createdBy.Id AS CreatedById, " +
                "createdBy.FirstName AS CreatedByFirstName, " +
                "createdBy.LastName AS CreatedByLastName, " +

                "company.Id AS CompanyId, " +
                "company.Name AS CompanyName " +

                "FROM ConstructionSiteDocuments constructionSiteDocument " +
                "LEFT JOIN ConstructionSites constructionSite ON constructionSiteDocument.ConstructionSiteId = constructionSite.Id " +
                "LEFT JOIN Users createdBy ON constructionSiteDocument.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON constructionSiteDocument.CompanyId = company.Id;";


            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
