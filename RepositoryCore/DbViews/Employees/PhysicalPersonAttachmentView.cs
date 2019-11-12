using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Employees
{
    public class PhysicalPersonAttachmentView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vEmployeeAttachments;";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                @"  CREATE VIEW vEmployeeAttachments AS
                    SELECT attachment.Id as AttachmentId, attachment.Identifier as AttachmentIdentifier, attachment.OK as AttachmentOK, attachment.Active,
	                    physicalPerson.Id as PhysicalPersonId, physicalPerson.Identifier as PhysicalPersonIdentifier, physicalPerson.Code as PhysicalPersonCode, physicalPerson.Name as PhysicalPersonName,
	                    (SELECT MAX(v) FROM (VALUES (attachment.UpdatedAt), (physicalPerson.UpdatedAt)) AS value(v)) AS UpdatedAt,
	                    createdBy.Id as CreatedById, createdBy.FirstName as CreatedByFirstName, createdBy.LastName as CreatedBylastName, 
	                    company.Id as CompanyId, company.Name as CompanyName
                    FROM PhysicalPersonAttachments attachment
                    LEFT JOIN PhysicalPersons physicalPerson ON attachment.PhysicalPersonId = physicalPerson.Id
                    LEFT JOIN Users createdBy ON attachment.CreatedById = createdBy.Id
                    LEFT JOIN Companies company ON attachment.CompanyId = company.Id";


            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();

        }
    }
}
