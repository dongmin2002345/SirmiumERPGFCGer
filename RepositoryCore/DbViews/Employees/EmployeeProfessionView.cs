using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Employees
{
    public class EmployeeProfessionView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vEmployeeProfessions";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vEmployeeProfessions AS " +
                "SELECT employeeProfession.Id AS EmployeeProfessionId, employeeProfession.Identifier AS EmployeeProfessionIdentifier, " +
                "employee.Id AS EmployeeId, employee.Identifier AS EmployeeIdentifier, employee.Code AS EmployeeCode, employee.Name AS EmployeeName, " +
                "profession.Id AS ProfessionId, profession.Identifier AS ProfessionIdentifier, profession.Code AS ProfessionCode, profession.Name AS ProfessionName, " +
                "country.Id AS CountryId, country.Identifier AS CountryIdentifier, country.Code AS CountryCode, country.Name AS CountryName, " +
                "employeeProfession.ItemStatus, employeeProfession.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (employeeProfession.UpdatedAt), (employee.UpdatedAt), (profession.UpdatedAt), (country.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM EmployeeProfessions employeeProfession " +
                "LEFT JOIN Employees employee ON employeeProfession.EmployeeId = employee.Id " +
                "LEFT JOIN Professions profession ON employeeProfession.ProfessionId = profession.Id " +
                "LEFT JOIN Countries country ON employeeProfession.CountryId = country.Id " +
                "LEFT JOIN Users createdBy ON employeeProfession.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON employeeProfession.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
