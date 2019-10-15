using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Statuses
{
    public class StatusView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vStatuses";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vStatuses AS " +
                "SELECT " +
                "status.Id AS StatusId, " +
                "status.Identifier AS StatusIdentifier, " +
                "status.Code AS StatusCode, " +
                "status.Name AS StatusName, " +
                "status.ShortName AS StatusShortName, " +

                "status.Active AS Active, " +

                "(SELECT MAX(v) FROM (VALUES (status.UpdatedAt)) AS value(v)) AS UpdatedAt, " +

                "createdBy.Id AS CreatedById, " +
                "createdBy.FirstName AS CreatedByFirstName, " +
                "createdBy.LastName AS CreatedByLastName, " +

                "company.Id AS CompanyId, " +
                "company.Name AS CompanyName " +

                "FROM Statuses status " +
                "LEFT JOIN Users createdBy ON status.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON status.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
