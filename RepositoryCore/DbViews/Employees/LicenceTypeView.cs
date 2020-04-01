using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Employees
{
    public class LicenceTypeView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vLicenceTypes";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vLicenceTypes AS " +
                "SELECT licenceType.Id AS LicenceTypeId, licenceType.Identifier AS LicenceTypeIdentifier, licenceType.Code AS LicenceTypeCode, licenceType.Category, licenceType.Description, " +
                "country.Id AS CountryId, country.Identifier AS CountryIdentifier, country.Mark AS CountryCode, country.Name AS CountryName, " +
                "licenceType.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (licenceType.UpdatedAt), (country.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM LicenceTypes licenceType " +
                "LEFT JOIN Countries country ON licenceType.CountryId = country.Id " +
                "LEFT JOIN Users createdBy ON licenceType.CountryId = createdBy.Id " +
                "LEFT JOIN Companies company ON licenceType.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
