using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.Identity
{
    public class UserView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vUsers";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vUsers AS " +
                "SELECT userx.Id AS UserId, userx.Identifier AS UserIdentifier, userx.Username AS UserUsername, userx.FirstName AS UserFirstName, userx.LastName AS UserLastName, userx.PasswordHash AS UserPasswordHash, userx.Email AS UserEmail, " +
                "userx.Active, userx.UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM Users userx " +
                "LEFT JOIN Users createdBy ON userx.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON userx.CompanyId = company.Id;";
                
                //Password, Roles??

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
