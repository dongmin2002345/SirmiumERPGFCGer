using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Employees
{
    public class EmployeeAttachmentView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vEmployeeAttachments;";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                @"  CREATE VIEW vEmployeeAttachments AS 
                    SELECT attachment.Id as AttachmentId, attachment.Identifier as AttachmentIdentifier, attachment.OK as AttachmentOK, attachment.Active,
	                    employee.Id as EmployeeId, employee.Identifier as EmployeeIdentifier, employee.Code as EmployeeCode, employee.Name as EmployeeName,
	                    (SELECT MAX(v) FROM (VALUES (attachment.UpdatedAt), (employee.UpdatedAt)) AS value(v)) AS UpdatedAt,
	                    createdBy.Id as CreatedById, createdBy.FirstName as CreatedByFirstName, createdBy.LastName as CreatedBylastName, 
	                    company.Id as CompanyId, company.Name as CompanyName
                    FROM EmployeeAttachments attachment
                    LEFT JOIN Employees employee ON attachment.EmployeeId = employee.Id
                    LEFT JOIN Users createdBy ON attachment.CreatedById = createdBy.Id
                    LEFT JOIN Companies company ON attachment.CompanyId = company.Id";


            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();

        }
    }
}
