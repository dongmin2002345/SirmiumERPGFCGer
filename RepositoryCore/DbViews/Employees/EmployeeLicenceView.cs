using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Employees
{
    public class EmployeeLicenceView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vEmployeeLicences";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vEmployeeLicences AS " +
                "SELECT employeeLicence.Id AS EmployeeLicenceId, employeeLicence.Identifier AS EmployeeLicenceIdentifier, " +
                "employee.Id AS EmployeeId, employee.Identifier AS EmployeeIdentifier, employee.Code AS EmployeeCode, employee.Name AS EmployeeName, " +
                "licence.Id AS LicenceId, licence.Identifier AS LicenceIdentifier, licence.Code AS LicenceCode, licence.Category AS LicenceCategory, licence.Description AS LicenceDescription, " +
                "country.Id AS CountryId, country.Identifier AS CountryIdentifier, country.Code AS CountryCode, country.Name AS CountryName, " +
                "employeeLicence.ValidFrom, employeeLicence.ValidTo, employeeLicence.ItemStatus, " +
                "employeeLicence.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (employeeLicence.UpdatedAt), (employee.UpdatedAt), (licence.UpdatedAt), (country.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM EmployeeLicences employeeLicence " +
                "LEFT JOIN Employees employee ON employeeLicence.EmployeeId = employee.Id " +
                "LEFT JOIN LicenceTypes licence ON employeeLicence.LicenceId = licence.Id " +
                "LEFT JOIN Countries country ON employeeLicence.CountryId = country.Id " +
                "LEFT JOIN Users createdBy ON employeeLicence.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON employeeLicence.CompanyId = company.Id;";


            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
