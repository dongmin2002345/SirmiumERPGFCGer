using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.ConstructionSites
{
    public class ConstructionSiteCalculationView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vConstructionSiteCalculations";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vConstructionSiteCalculations AS " +
                "SELECT constructionSiteCalculation.Id AS ConstructionSiteCalculationId, constructionSiteCalculation.Identifier AS ConstructionSiteCalculationIdentifier, " +
                "constructionSite.Id AS ConstructionSiteId, constructionSite.Identifier AS ConstructionSiteIdentifier, constructionSite.Code AS ConstructionSiteCode, constructionSite.Name AS ConstructionSiteName, " +
                "constructionSiteCalculation.NumOfEmployees, constructionSiteCalculation.EmployeePrice, constructionSiteCalculation.NumOfMonths, constructionSiteCalculation.OldValue, constructionSiteCalculation.NewValue," +
                "constructionSiteCalculation.ValueDifference, constructionSiteCalculation.PlusMinus," +
                "constructionSiteCalculation.IsPaid, constructionSiteCalculation.IsRefunded," +
                "constructionSiteCalculation.ItemStatus, constructionSiteCalculation.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (constructionSiteCalculation.UpdatedAt), (constructionSite.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM ConstructionSiteCalculations constructionSiteCalculation " +
                "LEFT JOIN ConstructionSites constructionSite ON constructionSiteCalculation.ConstructionSiteId = constructionSite.Id " +
                "LEFT JOIN Users createdBy ON constructionSiteCalculation.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON constructionSiteCalculation.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
