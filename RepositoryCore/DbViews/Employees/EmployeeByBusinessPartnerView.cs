using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Employees
{
    public class EmployeeByBusinessPartnerView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vEmployeeByBusinessPartners";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vEmployeeByBusinessPartners AS " +
                "SELECT employeeByBusinessPartner.Id AS EmployeeByBusinessPartnerId, employeeByBusinessPartner.Identifier AS EmployeeByBusinessPartnerIdentifier, employeeByBusinessPartner.Code AS EmployeeByBusinessPartnerCode, employeeByBusinessPartner.StartDate, employeeByBusinessPartner.EndDate, employeeByBusinessPartner.RealEndDate, " +
                "employee.Id AS EmployeeId, employee.Identifier AS EmployeeIdentifier, employee.Code AS EmployeeCode, employee.Name AS EmployeeName, " +
                "employeeByBusinessPartner.EmployeeCount, " +
                "businessPartner.Id AS BusinessPartnerId, businessPartner.Identifier AS BusinessPartnerIdentifier, businessPartner.Code AS BusinessPartnerCode, businessPartner.NameGer AS BusinessPartnerName, " +
                "employeeByBusinessPartner.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (employeeByBusinessPartner.UpdatedAt), (employee.UpdatedAt), (businessPartner.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM EmployeeByBusinessPartners employeeByBusinessPartner " +
                "LEFT JOIN Employees employee ON employeeByBusinessPartner.EmployeeId = employee.Id " +
                "LEFT JOIN BusinessPartners businessPartner ON employeeByBusinessPartner.BusinessPartnerId = businessPartner.Id " +
                "LEFT JOIN Users createdBy ON employeeByBusinessPartner.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON employeeByBusinessPartner.CompanyId = company.Id;";


            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
