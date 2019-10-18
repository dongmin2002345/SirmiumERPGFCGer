using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.Prices
{
    public class ServiceDeliveryView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vServiceDeliverys";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vServiceDeliverys AS " +
                "SELECT " +
                "serviceDelivery.Id AS ServiceDeliveryId, " +
                "serviceDelivery.Identifier AS ServiceDeliveryIdentifier, " +
                "serviceDelivery.Code AS ServiceDeliveryCode, " +
                "serviceDelivery.InternalCode AS ServiceDeliveryInternalCode, " +
                "serviceDelivery.Name AS ServiceDeliveryName, " +
                "serviceDelivery.URL AS ServiceDeliveryURL, " +

                "serviceDelivery.Active AS Active, " +

                "(SELECT MAX(v) FROM (VALUES (serviceDelivery.UpdatedAt)) AS value(v)) AS UpdatedAt, " +

                "createdBy.Id AS CreatedById, " +
                "createdBy.FirstName AS CreatedByFirstName, " +
                "createdBy.LastName AS CreatedByLastName, " +

                "company.Id AS CompanyId, " +
                "company.Name AS CompanyName " +

                "FROM ServiceDeliverys serviceDelivery " +
                "LEFT JOIN Users createdBy ON serviceDelivery.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON serviceDelivery.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
