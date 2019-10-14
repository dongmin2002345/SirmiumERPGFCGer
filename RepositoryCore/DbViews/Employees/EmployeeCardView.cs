using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Employees
{
    public class EmployeeCardView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vEmployeeCards";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vEmployeeCards AS " +
                "SELECT employeeCard.Id AS EmployeeCardId, employeeCard.Identifier AS EmployeeCardIdentifier, " +
                "employee.Id AS EmployeeId, employee.Identifier AS EmployeeIdentifier, employee.Code AS EmployeeCode, employee.Name AS EmployeeName, " +
                "employeeCard.CardDate, employeeCard.Description, employeeCard.PlusMinus, employeeCard.ItemStatus, " +
                "employeeCard.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (employeeCard.UpdatedAt), (employee.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM EmployeeCards employeeCard " +
                "LEFT JOIN Employees employee ON employeeCard.EmployeeId = employee.Id " +
                "LEFT JOIN Users createdBy ON employeeCard.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON employeeCard.CompanyId = company.Id;";


            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
