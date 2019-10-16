using Configurator;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Common.Prices;
using DomainCore.Common.Shipments;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Common.Shipments;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.Shipments
{
    public class ShipmentViewRepository : IShipmentRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        private string SelectString =
            "SELECT ShipmentId, ShipmentIdentifier, ShipmentCode, " +
            "ShipmentDate, Address, " +
            "ServiceDeliveryId, ServiceDeliveryIdentifier, ServiceDeliveryCode, ServiceDeliveryName, " +
            "ShipmentNumber, Acceptor, DeliveryDate, ReturnReceipt, DocumentName, Note, " +
            "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
            "FROM vShipments ";

        private Shipment Read(SqlDataReader reader)
        {
            Shipment shipment = new Shipment();
            shipment.Id = Int32.Parse(reader["ShipmentId"].ToString());
            shipment.Identifier = Guid.Parse(reader["ShipmentIdentifier"].ToString());

            if (reader["ShipmentCode"] != DBNull.Value)
                shipment.Code = reader["ShipmentCode"].ToString();
            if (reader["ShipmentDate"] != DBNull.Value)
                shipment.ShipmentDate = DateTime.Parse(reader["ShipmentDate"].ToString());
            if (reader["Address"] != DBNull.Value)
                shipment.Address = reader["Address"].ToString();

            if (reader["ServiceDeliveryId"] != DBNull.Value)
            {
                shipment.ServiceDelivery = new ServiceDelivery();
                if (reader["ServiceDeliveryId"] != DBNull.Value)
                    shipment.ServiceDeliveryId = Int32.Parse(reader["ServiceDeliveryId"].ToString());
                if (reader["ServiceDeliveryId"] != DBNull.Value)
                    shipment.ServiceDelivery.Id = Int32.Parse(reader["ServiceDeliveryId"].ToString());
                if (reader["ServiceDeliveryIdentifier"] != DBNull.Value)
                    shipment.ServiceDelivery.Identifier = Guid.Parse(reader["ServiceDeliveryIdentifier"].ToString());
                if (reader["ServiceDeliveryCode"] != DBNull.Value)
                    shipment.ServiceDelivery.Code = reader["ServiceDeliveryCode"].ToString();
                if (reader["ServiceDeliveryName"] != DBNull.Value)
                    shipment.ServiceDelivery.Name = reader["ServiceDeliveryName"].ToString();
            }

            if (reader["ShipmentNumber"] != DBNull.Value)
                shipment.ShipmentNumber = reader["ShipmentNumber"].ToString();
            if (reader["Acceptor"] != DBNull.Value)
                shipment.Acceptor = reader["Acceptor"].ToString();
            if (reader["DeliveryDate"] != DBNull.Value)
                shipment.DeliveryDate = DateTime.Parse(reader["DeliveryDate"].ToString());
            if (reader["ReturnReceipt"] != DBNull.Value)
                shipment.ReturnReceipt = reader["ReturnReceipt"].ToString();
            if (reader["DocumentName"] != DBNull.Value)
                shipment.DocumentName = reader["DocumentName"].ToString();
            if (reader["Note"] != DBNull.Value)
                shipment.Note = reader["Note"].ToString();
            shipment.Active = bool.Parse(reader["Active"].ToString());
            shipment.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

            if (reader["CreatedById"] != DBNull.Value)
            {
                shipment.CreatedBy = new User();
                shipment.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                shipment.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                shipment.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                shipment.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
            }

            if (reader["CompanyId"] != DBNull.Value)
            {
                shipment.Company = new Company();
                shipment.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                shipment.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                shipment.Company.Name = reader["CompanyName"].ToString();
            }

            return shipment;
        }

        public ShipmentViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<Shipment> GetShipments(int companyId)
        {
            List<Shipment> shipments = new List<Shipment>();

            string queryString =
                SelectString +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Shipment shipment;
                    while (reader.Read())
                    {
                        shipment = Read(reader);
                        shipments.Add(shipment);
                    }
                }
            }

            return shipments;
        }

        public List<Shipment> GetShipmentsNewerThan(int companyId, DateTime lastUpdateTime)
        {
            List<Shipment> shipments = new List<Shipment>();

            string queryString =
                SelectString +
                "WHERE CompanyId = @CompanyId AND " +
                "CONVERT(DATETIME, CONVERT(VARCHAR(20), UpdatedAt, 120)) > CONVERT(DATETIME, CONVERT(VARCHAR(20), @LastUpdateTime, 120));";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));
                command.Parameters.Add(new SqlParameter("@LastUpdateTime", lastUpdateTime));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Shipment shipment;
                    while (reader.Read())
                    {
                        shipment = Read(reader);
                        shipments.Add(shipment);
                    }
                }
            }

            return shipments;
        }

        //public Shipment GetShipment(int shipmentId)
        //{
        //    Shipment shipment = null;

        //    string queryString =
        //        SelectString +
        //        "WHERE ShipmentId = @ShipmentId;";

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        SqlCommand command = connection.CreateCommand();
        //        command.CommandText = queryString;
        //        command.Parameters.Add(new SqlParameter("@ShipmentId", shipmentId));

        //        connection.Open();
        //        using (SqlDataReader reader = command.ExecuteReader())
        //        {
        //            if (reader.Read())
        //            {
        //                shipment = Read(reader);
        //            }
        //        }
        //    }

        //    return shipment;
        //}

        private string GetNewCodeValue(int companyId)
        {
            int count = context.Shipments
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Shipment))
                    .Select(x => x.Entity as Shipment))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "1";
            else
            {
                string activeCode = context.Shipments
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Shipment))
                        .Select(x => x.Entity as Shipment))
                    .Where(x => x.CompanyId == companyId)
                    .OrderByDescending(x => x.Id).FirstOrDefault()
                    .Code;
                if (!String.IsNullOrEmpty(activeCode))
                {
                    int intValue = Int32.Parse(activeCode);
                    return (intValue + 1).ToString();
                }
                else
                    return count.ToString();
            }
        }

        public Shipment Create(Shipment shipment)
        {
            if (context.Shipments.Where(x => x.Identifier != null && x.Identifier == shipment.Identifier).Count() == 0)
            {
                shipment.Id = 0;

                shipment.Code = GetNewCodeValue(shipment.CompanyId ?? 0);
                shipment.Active = true;

                shipment.UpdatedAt = DateTime.Now;
                shipment.CreatedAt = DateTime.Now;

                context.Shipments.Add(shipment);
                return shipment;
            }
            else
            {
                // Load Shipment that will be updated
                Shipment dbEntry = context.Shipments
                    .FirstOrDefault(x => x.Identifier == shipment.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = shipment.CompanyId ?? null;
                    dbEntry.CreatedById = shipment.CreatedById ?? null;
                    dbEntry.ServiceDeliveryId = shipment.ServiceDeliveryId ?? null;
                    // Set properties
                    dbEntry.Code = shipment.Code;
                    dbEntry.ShipmentDate = shipment.ShipmentDate;
                    dbEntry.Address = shipment.Address;

                    dbEntry.ShipmentNumber = shipment.ShipmentNumber;
                    dbEntry.Acceptor = shipment.Acceptor;
                    dbEntry.DeliveryDate = shipment.DeliveryDate;

                    dbEntry.ReturnReceipt = shipment.ReturnReceipt;
                    dbEntry.DocumentName = shipment.DocumentName;
                    dbEntry.Note = shipment.Note;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public Shipment Delete(Guid identifier)
        {
            // Load item that will be deleted
            Shipment dbEntry = context.Shipments
                .FirstOrDefault(x => x.Identifier == identifier);

            if (dbEntry != null)
            {
                // Set activity
                dbEntry.Active = false;
                // Set timestamp
                dbEntry.UpdatedAt = DateTime.Now;
            }

            return dbEntry;
        }
    }
}
