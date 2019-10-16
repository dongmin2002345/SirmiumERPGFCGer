using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.Common.Shipments
{
    public class ShipmentDocumentView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vShipmentDocuments";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vShipmentDocuments AS " +
               "SELECT shipmentDocument.Id AS ShipmentDocumentId, shipmentDocument.Identifier AS ShipmentDocumentIdentifier, " +
                "shipment.Id AS ShipmentId, shipment.Identifier AS ShipmentIdentifier, shipment.Code AS ShipmentCode, " +
                "shipmentDocument.Name, shipmentDocument.CreateDate, shipmentDocument.Path, shipmentDocument.ItemStatus, " +
                "shipmentDocument.Active AS Active, " +

                "(SELECT MAX(v) FROM (VALUES (shipmentDocument.UpdatedAt), (shipment.UpdatedAt)) AS value(v)) AS UpdatedAt, " +

                "createdBy.Id AS CreatedById, " +
                "createdBy.FirstName AS CreatedByFirstName, " +
                "createdBy.LastName AS CreatedByLastName, " +

                "company.Id AS CompanyId, " +
                "company.Name AS CompanyName " +

                "FROM ShipmentDocuments shipmentDocument " +
                "LEFT JOIN Shipments shipment ON shipmentDocument.ShipmentId = shipment.Id " +
                "LEFT JOIN Users createdBy ON shipmentDocument.CreatedById = createdBy.Id " +
                "LEFT JOIN Companies company ON shipmentDocument.CompanyId = company.Id;";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
