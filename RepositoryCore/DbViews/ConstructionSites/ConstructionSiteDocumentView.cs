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
                "SELECT employeeDocument.Id AS ConstructionSiteDocumentId, employeeDocument.Identifier AS ConstructionSiteDocumentIdentifier, " +
                "employee.Id AS ConstructionSiteId, employee.Identifier AS ConstructionSiteIdentifier, employee.Code AS ConstructionSiteCode, employee.Name AS ConstructionSiteName, " +
                "employeeDocument.Name, employeeDocument.CreateDate, employeeDocument.Path, " +
                "employeeDocument.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (employeeDocument.UpdatedAt), (employee.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM ConstructionSiteDocuments employeeDocument " +
                "LEFT JOIN ConstructionSites employee ON employeeDocument.ConstructionSiteId = employee.Id " +
                "LEFT JOIN Users createdBy ON employeeDocument.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON employeeDocument.CompanyId = company.Id;";


            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
