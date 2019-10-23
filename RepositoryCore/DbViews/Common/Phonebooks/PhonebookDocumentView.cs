using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.Phonebooks
{
    public class PhonebookDocumentView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vPhonebookDocuments";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vPhonebookDocuments AS " +
                "SELECT phonebookDocument.Id AS PhonebookDocumentId, phonebookDocument.Identifier AS PhonebookDocumentIdentifier, " +
                "phonebook.Id AS PhonebookId, phonebook.Identifier AS PhonebookIdentifier, phonebook.Code AS PhonebookCode, phonebook.Name AS PhonebookName, " +
                "phonebookDocument.Name, phonebookDocument.CreateDate, phonebookDocument.Path, phonebookDocument.ItemStatus, " +
                "phonebookDocument.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (phonebookDocument.UpdatedAt), (phonebook.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM PhonebookDocuments phonebookDocument " +
                "LEFT JOIN Phonebooks phonebook ON phonebookDocument.PhonebookId = phonebook.Id " +
                "LEFT JOIN Users createdBy ON phonebookDocument.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON phonebookDocument.CompanyId = company.Id;";


            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
