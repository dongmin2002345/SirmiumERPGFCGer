using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.PhysicalPersons
{
    public class PhysicalPersonNoteView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vPhysicalPersonNotes";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vPhysicalPersonNotes AS " +
                "SELECT physicalPersonNote.Id AS PhysicalPersonNoteId, physicalPersonNote.Identifier AS PhysicalPersonNoteIdentifier, " +
                "physicalPerson.Id AS PhysicalPersonId, physicalPerson.Identifier AS PhysicalPersonIdentifier, physicalPerson.Code AS PhysicalPersonCode, physicalPerson.Name AS PhysicalPersonName, " +
                "physicalPersonNote.Note, physicalPersonNote.NoteDate, physicalPersonNote.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (physicalPersonNote.UpdatedAt), (physicalPerson.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM PhysicalPersonNotes physicalPersonNote " +
                "LEFT JOIN PhysicalPersons physicalPerson ON physicalPersonNote.PhysicalPersonId = physicalPerson.Id " +
                "LEFT JOIN Users createdBy ON physicalPersonNote.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON physicalPersonNote.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
