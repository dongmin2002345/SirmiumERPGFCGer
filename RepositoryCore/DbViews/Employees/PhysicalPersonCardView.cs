using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.PhysicalPersons
{
    public class PhysicalPersonCardView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vPhysicalPersonCards";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vPhysicalPersonCards AS " +
                "SELECT physicalPersonCard.Id AS PhysicalPersonCardId, physicalPersonCard.Identifier AS PhysicalPersonCardIdentifier, " +
                "physicalPerson.Id AS PhysicalPersonId, physicalPerson.Identifier AS PhysicalPersonIdentifier, physicalPerson.Code AS PhysicalPersonCode, physicalPerson.Name AS PhysicalPersonName, " +
                "physicalPersonCard.CardDate, physicalPersonCard.Description, physicalPersonCard.PlusMinus, " +
                "physicalPersonCard.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (physicalPersonCard.UpdatedAt), (physicalPerson.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM PhysicalPersonCards physicalPersonCard " +
                "LEFT JOIN PhysicalPersons physicalPerson ON physicalPersonCard.PhysicalPersonId = physicalPerson.Id " +
                "LEFT JOIN Users createdBy ON physicalPersonCard.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON physicalPersonCard.CompanyId = company.Id;";


            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
