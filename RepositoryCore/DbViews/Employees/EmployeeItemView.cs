using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Employees
{
    public class EmployeeItemView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vEmployeeItems";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vEmployeeItems AS " +
                "SELECT employeeItem.Id AS EmployeeItemId, employeeItem.Identifier AS EmployeeItemIdentifier, " +
                "employee.Id AS EmployeeId, employee.Identifier AS EmployeeIdentifier, employee.Code AS EmployeeCode, employee.Name AS EmployeeName, " +
                "familyMember.Id AS FamilyMemberId, familyMember.Identifier AS FamilyMemberIdentifier, familyMember.Code AS FamilyMemberCode, familyMember.Name AS FamilyMemberName, " +
                "employeeItem.Name, employeeItem.DateOfBirth, employeeItem.EmbassyDate, " +
                "employeeItem.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (employeeItem.UpdatedAt), (employee.UpdatedAt), (familyMember.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM EmployeeItems employeeItem " +
                "LEFT JOIN Employees employee ON employeeItem.EmployeeId = employee.Id " +
                "LEFT JOIN FamilyMembers familyMember ON employeeItem.FamilyMemberId = familyMember.Id " +
                "LEFT JOIN Users createdBy ON employeeItem.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON employeeItem.CompanyId = company.Id;";


            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
