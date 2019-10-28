using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.CallCentars
{
    public class CallCentarView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vCallCentars";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vCallCentars AS " +
                "SELECT " +
                "callCentar.Id AS CallCentarId, " +
                "callCentar.Identifier AS CallCentarIdentifier, " +
                "callCentar.Code AS CallCentarCode, " +
                "callCentar.ReceivingDate AS CallCentarReceivingDate, " +

                "userr.Id AS UserId, " +
                "userr.Identifier AS UserIdentifier, " +
                "userr.Code AS UserCode, " +
                "userr.FirstName AS UserFirstName, " +
                "userr.LastName AS UserLastName, " +

                "callCentar.Comment AS CallCentarComment, " +
                "callCentar.EndingDate AS CallCentarEndingDate, " +
                "callCentar.Active AS Active, " +

                "(SELECT MAX(v) FROM (VALUES (callCentar.UpdatedAt), (userr.UpdatedAt)) AS value(v)) AS UpdatedAt, " +

                "createdBy.Id AS CreatedById, " +
                "createdBy.FirstName AS CreatedByFirstName, " +
                "createdBy.LastName AS CreatedByLastName, " +

                "company.Id AS CompanyId, " +
                "company.Name AS CompanyName " +

                "FROM CallCentars callCentar " +
                "LEFT JOIN Users userr ON callCentar.UserId = userr.Id " +
                "LEFT JOIN Users createdBy ON callCentar.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON callCentar.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
