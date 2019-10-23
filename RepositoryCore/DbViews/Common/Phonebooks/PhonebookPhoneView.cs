using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.Phonebooks
{
    public class PhonebookPhoneView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vPhonebookPhones";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vPhonebookPhones AS " +
                "SELECT phonebookPhone.Id AS PhonebookPhoneId, phonebookPhone.Identifier AS PhonebookPhoneIdentifier, " +
                "phonebook.Id AS PhonebookId, phonebook.Identifier AS PhonebookIdentifier, phonebook.Code AS PhonebookCode, phonebook.Name AS PhonebookName, " +
                "phonebookPhone.Name, phonebookPhone.SurName, phonebookPhone.PhoneNumber, phonebookPhone.Email, phonebookPhone.ItemStatus, phonebookPhone.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (phonebookPhone.UpdatedAt), (phonebook.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM PhonebookPhones phonebookPhone " +
                "LEFT JOIN Phonebooks phonebook ON phonebookPhone.PhonebookId = phonebook.Id " +
                "LEFT JOIN Users createdBy ON phonebookPhone.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON phonebookPhone.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
