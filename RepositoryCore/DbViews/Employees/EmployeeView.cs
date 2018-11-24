using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Employees
{
    public class EmployeeView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vEmployees";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vEmployees AS " +
                "SELECT employee.Id AS EmployeeId, employee.Identifier AS EmployeeIdentifier, employee.Code AS EmployeeCode, employee.EmployeeCode AS EmployeeEmployeeCode, employee.Name AS EmployeeName, employee.SurName AS EmployeeSurName, employee.ConstructionSiteCode AS EmployeeConstructionSiteCode, employee.ConstructionSiteName AS EmployeeConstructionSiteName, employee.DateOfBirth, employee.Gender, " +
                "country.Id AS CountryId, country.Identifier AS CountryIdentifier, country.Code AS CountryCode, country.Name AS CountryName, " +
                "region.Id AS RegionId, region.Identifier AS RegionIdentifier, region.Code AS RegionCode, region.Name AS RegionName, " +
                "municipality.Id AS MunicipalityId, municipality.Identifier AS MunicipalityIdentifier, municipality.Code AS MunicipalityCode, municipality.Name AS MunicipalityName, " +
                "city.Id AS CityId, city.Identifier AS CityIdentifier, city.Code AS CityCode, city.Name AS CityName, " +
                "employee.Address, " +
                "countryPassport.Id AS PassportCountryId, countryPassport.Identifier AS PassportCountryIdentifier, countryPassport.Code AS PassportCountryCode, countryPassport.Name AS PassportCountryName, " +
                "cityPassport.Id AS PassportCityId, cityPassport.Identifier AS PassportCityIdentifier, cityPassport.Code AS PassportCityCode, cityPassport.Name AS PassportCityName, " +
                "employee.Passport, employee.VisaFrom, employee.VisaTo, " +
                "countryResidence.Id AS ResidenceCountryId, countryResidence.Identifier AS ResidenceCountryIdentifier, countryResidence.Code AS ResidenceCountryCode, countryResidence.Name AS ResidenceCountryName, " +
                "cityResidence.Id AS ResidenceCityId, cityResidence.Identifier AS ResidenceCityIdentifier, cityResidence.Code AS ResidenceCityCode, cityResidence.Name AS ResidenceCityName, " +
                "employee.ResidenceAddress, employee.EmbassyDate, employee.VisaDate, employee.VisaValidFrom, employee.VisaValidTo, employee.WorkPermitFrom, employee.WorkPermitTo, employee.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (employee.UpdatedAt), (country.UpdatedAt), (region.UpdatedAt), (municipality.UpdatedAt), (city.UpdatedAt), (countryPassport.UpdatedAt), (cityPassport.UpdatedAt), (countryResidence.UpdatedAt), (cityResidence.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM Employees employee " +
                "LEFT JOIN Countries country ON employee.CountryId = country.Id " +
                "LEFT JOIN Regions region ON employee.RegionId = region.Id " +
                "LEFT JOIN Municipalities municipality ON employee.MunicipalityId = municipality.Id " +
                "LEFT JOIN Cities city ON employee.CityId = city.Id " +
                "LEFT JOIN Countries countryPassport ON employee.PassportCountryId = countryPassport.Id " +
                "LEFT JOIN Cities cityPassport ON employee.PassportCityId = cityPassport.Id " +
                "LEFT JOIN Countries countryResidence ON employee.ResidenceCountryId = countryResidence.Id " +
                "LEFT JOIN Cities cityResidence ON employee.ResidenceCityId = cityResidence.Id " +
                "LEFT JOIN Users createdBy ON employee.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON employee.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
