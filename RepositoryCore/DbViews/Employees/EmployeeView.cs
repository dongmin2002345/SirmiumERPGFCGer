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
                "SELECT employee.Id AS EmployeeId, employee.Identifier AS EmployeeIdentifier, employee.Code AS EmployeeCode, employee.EmployeeCode AS EmployeeEmployeeCode, employee.Name AS EmployeeName, employee.SurName AS EmployeeSurName, employee.ConstructionSiteCode AS EmployeeConstructionSiteCode, employee.ConstructionSiteName AS EmployeeonstructionSiteName, employee.DateOfBirth, employee.Gender " +
                "country.Id AS CountryeId, country.Identifier AS CountryIdentifier, country.Code AS CountryCode, country.Name AS CountryName, " +

                "region.Id AS RegionId, region.Identifier AS RegionIdentifier, region.Code AS RegionCode, region.Name AS RegionName, " +

                "municipality.Id AS MunicipalityId, municipality.Identifier AS MunicipalityIdentifier, municipality.Code AS MunicipalityCode, municipality.Name AS MunicipalityName, " +

                "city.Id AS CityId, city.Identifier AS CityIdentifier, city.Code AS CityCode, city.Name AS CityName, " +

                "employee.Address, " +

                 "country.Id AS PassportCountryId, country.Identifier AS PassportCountryIdentifier, country.Code AS PassportCountryCode, country.Name AS PassportCountryName, " +

                 "city.Id AS PassportCityId, city.Identifier AS PassportCityIdentifier, city.Code AS PassportCityCode, city.Name AS PassportCityName, " +

                 "employee.Passport, employee.VisaFrom, employee.Code, employee.VisaTo, employee.VisaTo, " +

                  "country.Id AS ResidenceCountryId, country.Identifier AS ResidenceCountryIdentifier, country.Code AS ResidenceCountryCode, country.Name AS ResidenceCountryName, " +

                  "city.Id AS ResidenceCityId, city.Identifier AS ResidenceCityIdentifier, city.Code AS ResidenceCityCode, city.Name AS ResidenceCityName, " +

                  "employee.ResidenceAddress, employee.EmbassyDate, employee.VisaDate, employee.VisaValidFrom, employee.VisaValidTo, employee.WorkPermitFrom, employee.WorkPermitTo, " +

                "(SELECT MAX(v) FROM (VALUES (employee.UpdatedAt), (country.UpdatedAt), (region.UpdatedAt), (municipality.UpdatedAt), (city.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM Employees employee " +
                "LEFT JOIN Countries country ON employee.CountryId = country.Id " +
                "LEFT JOIN Regions region ON employee.RegionId = region.Id " +
                "LEFT JOIN Municipalities municipality ON employee.MunicipalityId = municipality.Id " +
                "LEFT JOIN Cities city ON employee.CityId = city.Id " +
                "LEFT JOIN Countries country ON employee.PassportCountryId = country.Id " +
                "LEFT JOIN Cities city ON employee.PassportCityId = city.Id " +
                "LEFT JOIN Countries country ON employee.ResidenceCountry = country.Id " +
                "LEFT JOIN Cities city ON employee.ResidenceCityId = city.Id " +
                "LEFT JOIN Users createdBy ON employee.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON employee.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
