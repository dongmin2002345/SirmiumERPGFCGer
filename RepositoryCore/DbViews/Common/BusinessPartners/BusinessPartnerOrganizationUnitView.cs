using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.BusinessPartners
{
    public class BusinessPartnerOrganizationUnitView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vBusinessPartnerOrganizationUnits";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vBusinessPartnerOrganizationUnits AS " +
                "SELECT businessPartnerOrganizationUnit.Id AS BusinessPartnerOrganizationUnitId, businessPartnerOrganizationUnit.Identifier AS BusinessPartnerOrganizationUnitIdentifier, " +
                "businessPartner.Id AS BusinessPartnerId, businessPartner.Identifier AS BusinessPartnerIdentifier, businessPartner.Code AS BusinessPartnerCode, businessPartner.Name AS BusinessPartnerName, " +
                "businessPartnerOrganizationUnit.Code, businessPartnerOrganizationUnit.Name, businessPartnerOrganizationUnit.Address, " +
                "country.Id AS CountryId, country.Identifier AS CountryIdentifier, country.Code AS CountryCode, country.Name AS CountryName, " +
                "city.Id AS CityId, city.Identifier AS CityIdentifier, city.Code AS CityCode, city.Name AS CityName, " +
                "municipality.Id AS MunicipalityId, municipality.Identifier AS MunicipalityIdentifier, municipality.Code AS MunicipalityCode, municipality.Name AS MunicipalityName, " +
                "businessPartnerOrganizationUnit.ContactPerson, businessPartnerOrganizationUnit.Phone, businessPartnerOrganizationUnit.Mobile, businessPartnerOrganizationUnit.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (businessPartnerOrganizationUnit.UpdatedAt), (businessPartner.UpdatedAt), (country.UpdatedAt), (city.UpdatedAt), (municipality.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM BusinessPartnerOrganizationUnits businessPartnerOrganizationUnit " +
                "LEFT JOIN BusinessPartners businessPartner ON businessPartnerOrganizationUnit.BusinessPartnerId = businessPartner.Id " +
                "LEFT JOIN Countries country ON businessPartnerOrganizationUnit.CountryId = country.Id " +
                "LEFT JOIN Cities city ON businessPartnerOrganizationUnit.CityId = city.Id " +
                "LEFT JOIN Municipalities municipality ON businessPartnerOrganizationUnit.MunicipalityId = municipality.Id " +
                "LEFT JOIN Users createdBy ON businessPartnerOrganizationUnit.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON businessPartnerOrganizationUnit.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
