using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.PhysicalPersons
{
    public class PhysicalPersonDocumentView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vPhysicalPersonDocuments";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vPhysicalPersonDocuments AS " +
                "SELECT physicalPersonDocument.Id AS PhysicalPersonDocumentId, physicalPersonDocument.Identifier AS PhysicalPersonDocumentIdentifier, " +
                "physicalPerson.Id AS PhysicalPersonId, physicalPerson.Identifier AS PhysicalPersonIdentifier, physicalPerson.Code AS PhysicalPersonCode, physicalPerson.Name AS PhysicalPersonName, " +
                "physicalPersonDocument.Name, physicalPersonDocument.CreateDate, physicalPersonDocument.Path, physicalPersonDocument.ItemStatus," +
                "physicalPersonDocument.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (physicalPersonDocument.UpdatedAt), (physicalPerson.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM PhysicalPersonDocuments physicalPersonDocument " +
                "LEFT JOIN PhysicalPersons physicalPerson ON physicalPersonDocument.PhysicalPersonId = physicalPerson.Id " +
                "LEFT JOIN Users createdBy ON physicalPersonDocument.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON physicalPersonDocument.CompanyId = company.Id;";


            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
