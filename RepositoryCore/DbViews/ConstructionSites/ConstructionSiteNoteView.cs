using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.ConstructionSites
{
    public class ConstructionSiteNoteView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vConstructionSiteNotes";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vConstructionSiteNotes AS " +
                "SELECT employeeNote.Id AS ConstructionSiteNoteId, employeeNote.Identifier AS ConstructionSiteNoteIdentifier, " +
                "employee.Id AS ConstructionSiteId, employee.Identifier AS ConstructionSiteIdentifier, employee.Code AS ConstructionSiteCode, employee.Name AS ConstructionSiteName, " +
                "employeeNote.Note, employeeNote.NoteDate, employeeNote.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (employeeNote.UpdatedAt), (employee.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM ConstructionSiteNotes employeeNote " +
                "LEFT JOIN ConstructionSites employee ON employeeNote.ConstructionSiteId = employee.Id " +
                "LEFT JOIN Users createdBy ON employeeNote.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON employeeNote.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
