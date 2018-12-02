using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.Professions
{
    public class ProfessionView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vProfessions";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vProfessions AS " +
                "SELECT profession.Id AS ProfessionId, profession.Identifier AS ProfessionIdentifier, profession.Code AS ProfessionCode, profession.SecondCode AS ProfessionSecondCode, profession.Name AS ProfessionName, " +
                "country.Id AS CountryId, country.Identifier AS CountryIdentifier, country.Code AS CountryCode, country.Name AS CountryName, " +
                "profession.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (profession.UpdatedAt), (country.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM Professions profession " +
                "LEFT JOIN Countries country ON profession.CountryId = country.Id " +
                "LEFT JOIN Users createdBy ON profession.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON profession.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
