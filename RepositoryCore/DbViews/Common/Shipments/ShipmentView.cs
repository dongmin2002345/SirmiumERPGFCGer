using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.Shipments
{
    public class ShipmentView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            try
            {
                string strSQLCommand = "DROP VIEW IF EXISTS vShipments";
                SqlCommand command = new SqlCommand(strSQLCommand, conn);
                string returnvalue = (string)command.ExecuteScalar();
            }
            catch (Exception ex) { }

            try
            {
                string strSQLCommand =
                "CREATE VIEW vShipments AS " +
                "SELECT shipment.Id AS ShipmentId, shipment.Identifier AS ShipmentIdentifier, shipment.Code AS ShipmentCode, " +
                "shipment.ShipmentDate, shipment.Address, " +
                "serviceDelivery.Id AS ServiceDeliveryId, serviceDelivery.Identifier AS ServiceDeliveryIdentifier, serviceDelivery.Code AS ServiceDeliveryCode, serviceDelivery.Name AS ServiceDeliveryName, " +
                "shipment.ShipmentNumber, shipment.Acceptor, shipment.DeliveryDate, shipment.ReturnReceipt, shipment.DocumentName, shipment.Note, shipment.Active AS Active, " +
                "(SELECT MAX(v) FROM (VALUES (shipment.UpdatedAt), (serviceDelivery.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "createdBy.Id AS CreatedById, createdBy.FirstName AS CreatedByFirstName, createdBy.LastName AS CreatedByLastName, " +
                "company.Id AS CompanyId, company.Name AS CompanyName " +
                "FROM Shipments shipment " +
                "LEFT JOIN ServiceDeliverys serviceDelivery ON shipment.ServiceDeliveryId = serviceDelivery.Id " +
                "LEFT JOIN Users createdBy ON shipment.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON shipment.CompanyId = company.Id;";


                SqlCommand command = new SqlCommand(strSQLCommand, conn);
                string returnvalue = (string)command.ExecuteScalar();
            }
            catch (Exception ex) { }

            conn.Close();
        }
    }
}
