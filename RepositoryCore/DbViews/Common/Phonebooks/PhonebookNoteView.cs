using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.Phonebooks
{
    public class PhonebookNoteView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vPhonebookNotes";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vPhonebookNotes AS " +
                "SELECT phonebookNote.Id AS PhonebookNoteId, phonebookNote.Identifier AS PhonebookNoteIdentifier, " +
                "phonebook.Id AS PhonebookId, phonebook.Identifier AS PhonebookIdentifier, phonebook.Code AS PhonebookCode, phonebook.Name AS PhonebookName, " +
                "phonebookNote.Note, phonebookNote.NoteDate, phonebookNote.ItemStatus, phonebookNote.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (phonebookNote.UpdatedAt), (phonebook.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM PhonebookNotes phonebookNote " +
                "LEFT JOIN Phonebooks phonebook ON phonebookNote.PhonebookId = phonebook.Id " +
                "LEFT JOIN Users createdBy ON phonebookNote.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON phonebookNote.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
