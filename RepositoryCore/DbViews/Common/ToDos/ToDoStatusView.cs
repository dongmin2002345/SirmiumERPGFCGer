using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.ToDos
{
    public class ToDoStatusView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vToDoStatuses";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vToDoStatuses AS " +
                "SELECT " +
                "toDoStatus.Id AS ToDoStatusId, " +
                "toDoStatus.Identifier AS ToDoStatusIdentifier, " +
                "toDoStatus.Code AS ToDoStatusCode, " +
                "toDoStatus.Name AS ToDoStatusName, " +

                "toDoStatus.Active AS Active, " +

                "(SELECT MAX(v) FROM (VALUES (toDoStatus.UpdatedAt)) AS value(v)) AS UpdatedAt, " +

                "createdBy.Id AS CreatedById, " +
                "createdBy.FirstName AS CreatedByFirstName, " +
                "createdBy.LastName AS CreatedByLastName, " +

                "company.Id AS CompanyId, " +
                "company.Name AS CompanyName " +

                "FROM ToDoStatuses toDoStatus " +
                "LEFT JOIN Users createdBy ON toDoStatus.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON toDoStatus.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
