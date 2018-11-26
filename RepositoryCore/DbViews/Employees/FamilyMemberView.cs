using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Employees
{
    public class FamilyMemberView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vFamilyMembers";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vFamilyMembers AS " +
                "SELECT familyMember.Id AS FamilyMemberId, familyMember.Identifier AS FamilyMemberIdentifier, familyMember.Code AS FamilyMemberCode, familyMember.Name AS FamilyMemberName, " +
                "familyMember.Active AS Active, familyMember.UpdatedAt AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM FamilyMembers familyMember " +
                "LEFT JOIN Users createdBy ON familyMember.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON familyMember.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
