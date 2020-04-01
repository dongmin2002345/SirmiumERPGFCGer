using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.Locations
{
    public class MunicipalityView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vMunicipalities";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vMunicipalities AS " +
                "SELECT municipality.Id AS MunicipalityId, municipality.Identifier AS MunicipalityIdentifier, municipality.Code, municipality.MunicipalityCode, municipality.Name, " +
                "region.Id AS RegionId, region.Identifier AS RegionIdentifier, region.RegionCode AS RegionCode, region.Name AS RegionName, " +
                "country.Id AS CountryId, country.Identifier AS CountryIdentifier, country.Mark AS CountryCode, country.Name AS CountryName, " +
                "municipality.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (municipality.UpdatedAt), (region.UpdatedAt), (country.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM Municipalities municipality " +
                "LEFT JOIN Regions region ON municipality.RegionId = region.Id " +
                "LEFT JOIN Countries country ON municipality.CountryId = country.Id " +
                "LEFT JOIN Users createdBy ON municipality.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON municipality.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
