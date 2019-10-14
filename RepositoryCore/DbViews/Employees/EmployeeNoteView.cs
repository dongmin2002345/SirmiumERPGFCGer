using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Employees
{
    public class EmployeeNoteView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vEmployeeNotes";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vEmployeeNotes AS " +
                "SELECT employeeNote.Id AS EmployeeNoteId, employeeNote.Identifier AS EmployeeNoteIdentifier, " +
                "employee.Id AS EmployeeId, employee.Identifier AS EmployeeIdentifier, employee.Code AS EmployeeCode, employee.Name AS EmployeeName, " +
                "employeeNote.Note, employeeNote.NoteDate, employeeNote.ItemStatus, employeeNote.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (employeeNote.UpdatedAt), (employee.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM EmployeeNotes employeeNote " +
                "LEFT JOIN Employees employee ON employeeNote.EmployeeId = employee.Id " +
                "LEFT JOIN Users createdBy ON employeeNote.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON employeeNote.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
