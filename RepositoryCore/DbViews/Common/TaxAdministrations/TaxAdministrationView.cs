using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.TaxAdministrations
{
    public class TaxAdministrationView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vTaxAdministrations";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vTaxAdministrations AS " +
                "SELECT taxAdministration.Id AS TaxAdministrationId, taxAdministration.Identifier AS TaxAdministrationIdentifier, taxAdministration.Code AS TaxAdministrationCode, taxAdministration.SecondCode AS TaxAdministrationSecondCode, taxAdministration.Name AS TaxAdministrationName, " +
                "city.Id AS CityId, city.Identifier AS CityIdentifier, city.Code AS CityCode, city.Name AS CityName, " +
                "taxAdministration.Address1, taxAdministration.Address2, taxAdministration.Address3, " +
                "bank1.Id AS BankId1, bank1.Identifier AS BankIdentifier1, bank1.Code AS BankCode1, bank1.Name AS BankName1, " +
                "bank2.Id AS BankId2, bank2.Identifier AS BankIdentifier2, bank2.Code AS BankCode2, bank2.Name AS BankName2, " +
                "taxAdministration.IBAN1, taxAdministration.SWIFT, taxAdministration.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (taxAdministration.UpdatedAt), (city.UpdatedAt), (bank1.UpdatedAt), (bank2.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM TaxAdministrations taxAdministration " +
                "LEFT JOIN Cities city ON taxAdministration.CityId = city.Id " +
                "LEFT JOIN Banks bank1 ON taxAdministration.BankId1 = bank1.Id " +
                "LEFT JOIN Banks bank2 ON taxAdministration.BankId2 = bank2.Id " +
                "LEFT JOIN Users createdBy ON taxAdministration.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON taxAdministration.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
