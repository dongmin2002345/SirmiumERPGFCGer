using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.Locations
{
    public class CityView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vCountries";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vCountries AS " +
                "SELECT city.Id AS CityId, city.Identifier AS CityIdentifier, city.Code AS CityCode, country.Name AS CityName, city.ZipCode, " +
                "country.Id AS CountryId, country.Identifier AS CountryIdentifier, country.Code AS CountryCode, country.Name AS CountryName, " +
                "region.Id AS RegionId, region.Identifier AS RegionIdentifier, region.Code AS RegionCode, region.Name AS RegionName, " +
                "municipality.Id AS MunicipalityId, municipality.Identifier AS MunicipalityIdentifier, municipality.Code AS MunicipalityCode, municipality.Name AS MunicipalityName, " +
                "municipality.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (city.UpdatedAt), (country.UpdatedAt), (region.UpdatedAt), (municipality.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM Cities city " +
                "LEFT JOIN Countries country ON city.CountryId = country.Id " +
                "LEFT JOIN Regions region ON city.RegionId = region.Id " +
                "LEFT JOIN Municipalities municipality ON city.MunicipalityId = municipality.Id " +
                "LEFT JOIN Users createdBy ON city.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON city.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
