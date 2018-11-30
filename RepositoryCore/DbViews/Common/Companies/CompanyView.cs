using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.Companies
{
    public class CompanyView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vCompanies";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vCompanies AS " +
                "SELECT Companies.Id, Companies.Identifier, Companies.Code, Companies.Name, Companies.Address, " +
                "Companies.Active, Companies.UpdatedAt " +
                "FROM Companies";

            //strSQLCommand =
            //    "CREATE VIEW vCompanies AS " +
            //    "SELECT company.Id AS CompanyId, company.Identifier AS CompanyIdentifier, company.Code AS CompanyCode, company.Name AS CompanyName, " +
            //    "company.Address AS CompanyAddress, " +
            //    "company.IdentificationNumber, company.PIBNumber, company.PIONumber, company.PDVNumber, company.IndustryCode, company.IndustryName, company.BankAccountNo, company.BankAccountName, company.Email, company.WebSite, company.CreatedAt, " +
            //    "company.Active, company.UpdatedAt, " +
            //    "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName " +
            //    "FROM Companies company " +
            //    "LEFT JOIN Users createdBy ON company.CreatedBy.Id = createdBy.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
