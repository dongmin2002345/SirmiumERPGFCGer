using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.BusinessPartners
{
    public class BusinessPartnerNoteView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vBusinessPartnerNotes";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vBusinessPartnerNotes AS " +
                "SELECT employeeNote.Id AS BusinessPartnerNoteId, employeeNote.Identifier AS BusinessPartnerNoteIdentifier, " +
                "employee.Id AS BusinessPartnerId, employee.Identifier AS BusinessPartnerIdentifier, employee.Code AS BusinessPartnerCode, employee.Name AS BusinessPartnerName, " +
                "employeeNote.Note, employeeNote.NoteDate, employeeNote.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (employeeNote.UpdatedAt), (employee.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM BusinessPartnerNotes employeeNote " +
                "LEFT JOIN BusinessPartners employee ON employeeNote.BusinessPartnerId = employee.Id " +
                "LEFT JOIN Users createdBy ON employeeNote.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON employeeNote.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
