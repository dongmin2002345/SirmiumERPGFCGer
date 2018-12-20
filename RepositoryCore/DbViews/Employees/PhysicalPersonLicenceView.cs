using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.PhysicalPersons
{
    public class PhysicalPersonLicenceView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vPhysicalPersonLicences";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vPhysicalPersonLicences AS " +
                "SELECT physicalPersonLicence.Id AS PhysicalPersonLicenceId, physicalPersonLicence.Identifier AS PhysicalPersonLicenceIdentifier, " +
                "physicalPerson.Id AS PhysicalPersonId, physicalPerson.Identifier AS PhysicalPersonIdentifier, physicalPerson.Code AS PhysicalPersonCode, physicalPerson.Name AS PhysicalPersonName, " +
                "licence.Id AS LicenceId, licence.Identifier AS LicenceIdentifier, licence.Code AS LicenceCode, licence.Category AS LicenceCategory, licence.Description AS LicenceDescription, " +
                "country.Id AS CountryId, country.Identifier AS CountryIdentifier, country.Code AS CountryCode, country.Name AS CountryName, " +
                "physicalPersonLicence.ValidFrom, physicalPersonLicence.ValidTo, " +
                "physicalPersonLicence.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (physicalPersonLicence.UpdatedAt), (physicalPerson.UpdatedAt), (licence.UpdatedAt), (country.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM PhysicalPersonLicences physicalPersonLicence " +
                "LEFT JOIN PhysicalPersons physicalPerson ON physicalPersonLicence.PhysicalPersonId = physicalPerson.Id " +
                "LEFT JOIN LicenceTypes licence ON physicalPersonLicence.LicenceId = licence.Id " +
                "LEFT JOIN Countries country ON physicalPersonLicence.CountryId = country.Id " +
                "LEFT JOIN Users createdBy ON physicalPersonLicence.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON physicalPersonLicence.CompanyId = company.Id;";


            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
