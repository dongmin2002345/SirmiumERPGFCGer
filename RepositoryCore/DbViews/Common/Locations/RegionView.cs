using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.Locations
{
    public class RegionView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vRegions";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vRegions AS " +
                "SELECT region.Id AS RegionId, region.Identifier AS RegionIdentifier, region.Code, region.RegionCode, region.Name AS RegionName, " +
                "country.Id AS CountryId, country.Identifier AS CountryIdentifier, Country.Code AS CountryCode, Country.Name AS CountryName, " +
                "region.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (region.UpdatedAt), (country.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM Regions region " +
                "LEFT JOIN Countries country ON region.CountryId = country.Id " +
                "LEFT JOIN Users createdBy ON region.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON region.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
