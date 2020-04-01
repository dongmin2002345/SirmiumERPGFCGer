using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.BusinessPartners
{
    public class BusinessPartnerLocationView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vBusinessPartnerLocations";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vBusinessPartnerLocations AS " +
                "SELECT businessPartnerLocation.Id AS BusinessPartnerLocationId, businessPartnerLocation.Identifier AS BusinessPartnerLocationIdentifier, " +
                "businessPartner.Id AS BusinessPartnerId, businessPartner.Identifier AS BusinessPartnerIdentifier, businessPartner.Code AS BusinessPartnerCode, businessPartner.Name AS BusinessPartnerName, " +
                "businessPartnerLocation.Address, " +
                "country.Id AS CountryId, country.Identifier AS CountryIdentifier, country.Code AS CountryCode, country.Name AS CountryName, " +
                "city.Id AS CityId, city.Identifier AS CityIdentifier, city.ZipCode AS CityZipCode, city.Name AS CityName, " +
                "municipality.Id AS MunicipalityId, municipality.Identifier AS MunicipalityIdentifier, municipality.MunicipalityCode AS MunicipalityCode, municipality.Name AS MunicipalityName, " +
                "region.Id AS RegionId, region.Identifier AS RegionIdentifier, region.RegionCode AS RegionCode, region.Name AS RegionName, " +
                "businessPartnerLocation.ItemStatus, businessPartnerLocation.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (businessPartnerLocation.UpdatedAt), (businessPartner.UpdatedAt), (country.UpdatedAt), (city.UpdatedAt), (municipality.UpdatedAt), (region.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM BusinessPartnerLocations businessPartnerLocation " +
                "LEFT JOIN BusinessPartners businessPartner ON businessPartnerLocation.BusinessPartnerId = businessPartner.Id " +
                "LEFT JOIN Countries country ON businessPartnerLocation.CountryId = country.Id " +
                "LEFT JOIN Cities city ON businessPartnerLocation.CityId = city.Id " +
                "LEFT JOIN Municipalities municipality ON businessPartnerLocation.MunicipalityId = municipality.Id " +
                "LEFT JOIN Regions region ON businessPartnerLocation.RegionId = region.Id " +
                "LEFT JOIN Users createdBy ON businessPartnerLocation.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON businessPartnerLocation.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
