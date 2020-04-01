using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.PhysicalPersons
{
    public class PhysicalPersonProfessionView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vPhysicalPersonProfessions";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vPhysicalPersonProfessions AS " +
                "SELECT physicalPersonProfession.Id AS PhysicalPersonProfessionId, physicalPersonProfession.Identifier AS PhysicalPersonProfessionIdentifier, " +
                "physicalPerson.Id AS PhysicalPersonId, physicalPerson.Identifier AS PhysicalPersonIdentifier, physicalPerson.Code AS PhysicalPersonCode, physicalPerson.Name AS PhysicalPersonName, " +
                "profession.Id AS ProfessionId, profession.Identifier AS ProfessionIdentifier, profession.Code AS ProfessionCode, profession.Name AS ProfessionName, " +
                "country.Id AS CountryId, country.Identifier AS CountryIdentifier, country.Mark AS CountryCode, country.Name AS CountryName, physicalPersonProfession.ItemStatus, " +
                "physicalPersonProfession.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (physicalPersonProfession.UpdatedAt), (physicalPerson.UpdatedAt), (profession.UpdatedAt), (country.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM PhysicalPersonProfessions physicalPersonProfession " +
                "LEFT JOIN PhysicalPersons physicalPerson ON physicalPersonProfession.PhysicalPersonId = physicalPerson.Id " +
                "LEFT JOIN Professions profession ON physicalPersonProfession.ProfessionId = profession.Id " +
                "LEFT JOIN Countries country ON physicalPersonProfession.CountryId = country.Id " +
                "LEFT JOIN Users createdBy ON physicalPersonProfession.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON physicalPersonProfession.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}

