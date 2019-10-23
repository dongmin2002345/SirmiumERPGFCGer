using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.Phonebooks
{
    public class PhonebookView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vPhonebooks";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vPhonebooks AS " +
                "SELECT phonebook.Id AS PhonebookId, phonebook.Identifier AS PhonebookIdentifier, phonebook.Code AS PhonebookCode, phonebook.Name AS PhonebookName, " +

                "municipality.Id AS MunicipalityId, municipality.Identifier AS MunicipalityIdentifier, municipality.Code AS MunicipalityCode, municipality.Name AS MunicipalityName, " +
                "city.Id AS CityId, city.Identifier AS CityIdentifier, city.Code AS CityCode, city.Name AS CityName, " +

                "phonebook.Address,  phonebook.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (phonebook.UpdatedAt), (municipality.UpdatedAt), (city.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM Phonebooks phonebook " +
                "LEFT JOIN Municipalities municipality ON phonebook.MunicipalityId = municipality.Id " +
                "LEFT JOIN Cities city ON phonebook.CityId = city.Id " +
                "LEFT JOIN Users createdBy ON phonebook.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON phonebook.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
