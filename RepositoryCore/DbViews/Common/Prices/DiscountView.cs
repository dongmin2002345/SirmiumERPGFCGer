using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.Prices
{
    public class DiscountView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vDiscounts";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vDiscounts AS " +
                "SELECT " +
                "discount.Id AS DiscountId, " +
                "discount.Identifier AS DiscountIdentifier, " +
                "discount.Code AS DiscountCode, " +
                "discount.Name AS DiscountName, " +
                "discount.Amount AS DiscountAmount, " +

                "discount.Active AS Active, " +

                "(SELECT MAX(v) FROM (VALUES (discount.UpdatedAt)) AS value(v)) AS UpdatedAt, " +

                "createdBy.Id AS CreatedById, " +
                "createdBy.FirstName AS CreatedByFirstName, " +
                "createdBy.LastName AS CreatedByLastName, " +

                "company.Id AS CompanyId, " +
                "company.Name AS CompanyName " +

                "FROM Discounts discount " +
                "LEFT JOIN Users createdBy ON discount.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON discount.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
