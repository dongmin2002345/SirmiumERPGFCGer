using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.PhysicalPersons
{
    public class PhysicalPersonView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vPhysicalPersons";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vPhysicalPersons AS " +
                "SELECT physicalPerson.Id AS PhysicalPersonId, physicalPerson.Identifier AS PhysicalPersonIdentifier, physicalPerson.Code AS PhysicalPersonCode, physicalPerson.PhysicalPersonCode AS PhysicalPersonPhysicalPersonCode, physicalPerson.Name AS PhysicalPersonName, physicalPerson.SurName AS PhysicalPersonSurName, physicalPerson.ConstructionSiteCode AS PhysicalPersonConstructionSiteCode, physicalPerson.ConstructionSiteName AS PhysicalPersonConstructionSiteName, physicalPerson.DateOfBirth, physicalPerson.Gender, " +
                "country.Id AS CountryId, country.Identifier AS CountryIdentifier, country.Mark AS CountryCode, country.Name AS CountryName, " +
                "region.Id AS RegionId, region.Identifier AS RegionIdentifier, region.RegionCode AS RegionCode, region.Name AS RegionName, " +
                "municipality.Id AS MunicipalityId, municipality.Identifier AS MunicipalityIdentifier, municipality.MunicipalityCode AS MunicipalityCode, municipality.Name AS MunicipalityName, " +
                "city.Id AS CityId, city.Identifier AS CityIdentifier, city.ZipCode AS CityZipCode, city.Name AS CityName, " +
                "physicalPerson.Address, " +
                "countryPassport.Id AS PassportCountryId, countryPassport.Identifier AS PassportCountryIdentifier, countryPassport.Mark AS PassportCountryCode, countryPassport.Name AS PassportCountryName, " +
                "cityPassport.Id AS PassportCityId, cityPassport.Identifier AS PassportCityIdentifier, cityPassport.ZipCode AS PassportCityZipCode, cityPassport.Name AS PassportCityName, " +
                "physicalPerson.Passport, physicalPerson.VisaFrom, physicalPerson.VisaTo, " +
                "countryResidence.Id AS ResidenceCountryId, countryResidence.Identifier AS ResidenceCountryIdentifier, countryResidence.Mark AS ResidenceCountryCode, countryResidence.Name AS ResidenceCountryName, " +
                "cityResidence.Id AS ResidenceCityId, cityResidence.Identifier AS ResidenceCityIdentifier, cityResidence.ZipCode AS ResidenceCityZipCode, cityResidence.Name AS ResidenceCityName, " +
                "physicalPerson.ResidenceAddress, physicalPerson.EmbassyDate, physicalPerson.VisaDate, physicalPerson.VisaValidFrom, physicalPerson.VisaValidTo, physicalPerson.WorkPermitFrom, physicalPerson.WorkPermitTo, physicalPerson.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (physicalPerson.UpdatedAt), (country.UpdatedAt), (region.UpdatedAt), (municipality.UpdatedAt), (city.UpdatedAt), (countryPassport.UpdatedAt), (cityPassport.UpdatedAt), (countryResidence.UpdatedAt), (cityResidence.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM PhysicalPersons physicalPerson " +
                "LEFT JOIN Countries country ON physicalPerson.CountryId = country.Id " +
                "LEFT JOIN Regions region ON physicalPerson.RegionId = region.Id " +
                "LEFT JOIN Municipalities municipality ON physicalPerson.MunicipalityId = municipality.Id " +
                "LEFT JOIN Cities city ON physicalPerson.CityId = city.Id " +
                "LEFT JOIN Countries countryPassport ON physicalPerson.PassportCountryId = countryPassport.Id " +
                "LEFT JOIN Cities cityPassport ON physicalPerson.PassportCityId = cityPassport.Id " +
                "LEFT JOIN Countries countryResidence ON physicalPerson.ResidenceCountryId = countryResidence.Id " +
                "LEFT JOIN Cities cityResidence ON physicalPerson.ResidenceCityId = cityResidence.Id " +
                "LEFT JOIN Users createdBy ON physicalPerson.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON physicalPerson.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
