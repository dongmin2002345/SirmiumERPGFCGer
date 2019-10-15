using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Vats
{
    public class VatView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vVats";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vVats AS " +
                "SELECT " +
                "vat.Id AS VatId, " +
                "vat.Identifier AS VatIdentifier, " +
                "vat.Code AS VatCode, " +
                "vat.Amount AS VatAmount, " +
                "vat.Description AS VatDescription, " +

                "vat.Active AS Active, " +

                "(SELECT MAX(v) FROM (VALUES (vat.UpdatedAt)) AS value(v)) AS UpdatedAt, " +

                "createdBy.Id AS CreatedById, " +
                "createdBy.FirstName AS CreatedByFirstName, " +
                "createdBy.LastName AS CreatedByLastName, " +

                "company.Id AS CompanyId, " +
                "company.Name AS CompanyName " +

                "FROM Vats vat " +
                "LEFT JOIN Users createdBy ON vat.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON vat.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
