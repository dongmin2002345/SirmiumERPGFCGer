using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.Sectors
{
    public class SectorView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vSectors";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vSectors AS " +
                "SELECT sector.Id AS SectorId, sector.Identifier AS SectorIdentifier, sector.Code AS SectorCode, sector.SecondCode AS SectorSecondCode, sector.Name AS SectorName, " +
                "country.Id AS CountryId, country.Identifier AS CountryIdentifier, country.Mark AS CountryCode, country.Name AS CountryName, " +
                "sector.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (sector.UpdatedAt), (country.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM Sectors sector " +
                "LEFT JOIN Countries country ON sector.CountryId = country.Id " +
                "LEFT JOIN Users createdBy ON sector.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON sector.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
