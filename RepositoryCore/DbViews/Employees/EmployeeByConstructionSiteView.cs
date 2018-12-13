using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Employees
{
    public class EmployeeByConstructionSiteView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vEmployeeByConstructionSites";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vEmployeeByConstructionSites AS " +
                "SELECT employeeByConstructionSite.Id AS EmployeeByConstructionSiteId, employeeByConstructionSite.Identifier AS EmployeeByConstructionSiteIdentifier, employeeByConstructionSite.Code AS EmployeeByConstructionSiteCode, employeeByConstructionSite.StartDate, employeeByConstructionSite.EndDate, employeeByConstructionSite.RealEndDate, " +
                "employee.Id AS EmployeeId, employee.Identifier AS EmployeeIdentifier, employee.Code AS EmployeeCode, employee.Name AS EmployeeName, " +
                "employeeByConstructionSite.EmployeeCount, " +
                "businessPartner.Id AS BusinessPartnerId, businessPartner.Identifier AS BusinessPartnerIdentifier, businessPartner.Code AS BusinessPartnerCode, businessPartner.NameGer AS BusinessPartnerName, " +
                "employeeByConstructionSite.BusinessPartnerCount, " +
                "constructionSite.Id AS ConstructionSiteId, constructionSite.Identifier AS ConstructionSiteIdentifier, constructionSite.Code AS ConstructionSiteCode, constructionSite.Name AS ConstructionSiteName, " +
                "employeeByConstructionSite.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (employeeByConstructionSite.UpdatedAt), (employee.UpdatedAt), (businessPartner.UpdatedAt), (constructionSite.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM EmployeeByConstructionSites employeeByConstructionSite " +
                "JOIN Employees employee ON employeeByConstructionSite.EmployeeId = employee.Id " +
                "JOIN BusinessPartners businessPartner ON employeeByConstructionSite.BusinessPartnerId = businessPartner.Id " +
                "JOIN ConstructionSites constructionSite ON employeeByConstructionSite.ConstructionSiteId = constructionSite.Id " +
                "LEFT JOIN Users createdBy ON employeeByConstructionSite.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON employeeByConstructionSite.CompanyId = company.Id " +
                "WHERE constructionSite.Active = 1 AND businessPartner.Active = 1 AND employee.Active = 1;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
