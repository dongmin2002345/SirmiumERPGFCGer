using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.PhysicalPersons
{
    public class PhysicalPersonItemView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vPhysicalPersonItems";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vPhysicalPersonItems AS " +
                "SELECT physicalPersonItem.Id AS PhysicalPersonItemId, physicalPersonItem.Identifier AS PhysicalPersonItemIdentifier, " +
                "physicalPerson.Id AS PhysicalPersonId, physicalPerson.Identifier AS PhysicalPersonIdentifier, physicalPerson.Code AS PhysicalPersonCode, physicalPerson.Name AS PhysicalPersonName, " +
                "familyMember.Id AS FamilyMemberId, familyMember.Identifier AS FamilyMemberIdentifier, familyMember.Code AS FamilyMemberCode, familyMember.Name AS FamilyMemberName, " +
                "physicalPersonItem.Name, physicalPersonItem.DateOfBirth, physicalPersonItem.EmbassyDate, physicalPersonItem.ItemStatus, " +
                "physicalPersonItem.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (physicalPersonItem.UpdatedAt), (physicalPerson.UpdatedAt), (familyMember.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM PhysicalPersonItems physicalPersonItem " +
                "LEFT JOIN PhysicalPersons physicalPerson ON physicalPersonItem.PhysicalPersonId = physicalPerson.Id " +
                "LEFT JOIN FamilyMembers familyMember ON physicalPersonItem.FamilyMemberId = familyMember.Id " +
                "LEFT JOIN Users createdBy ON physicalPersonItem.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON physicalPersonItem.CompanyId = company.Id;";


            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
