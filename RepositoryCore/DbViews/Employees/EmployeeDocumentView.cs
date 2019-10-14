using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Employees
{
    public class EmployeeDocumentView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vEmployeeDocuments";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vEmployeeDocuments AS " +
                "SELECT employeeDocument.Id AS EmployeeDocumentId, employeeDocument.Identifier AS EmployeeDocumentIdentifier, " +
                "employee.Id AS EmployeeId, employee.Identifier AS EmployeeIdentifier, employee.Code AS EmployeeCode, employee.Name AS EmployeeName, " +
                "employeeDocument.Name, employeeDocument.CreateDate, employeeDocument.Path, employeeDocument.ItemStatus, " +
                "employeeDocument.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (employeeDocument.UpdatedAt), (employee.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM EmployeeDocuments employeeDocument " +
                "LEFT JOIN Employees employee ON employeeDocument.EmployeeId = employee.Id " +
                "LEFT JOIN Users createdBy ON employeeDocument.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON employeeDocument.CompanyId = company.Id;";


            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
