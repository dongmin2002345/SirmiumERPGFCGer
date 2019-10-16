using Configurator;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
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
    public class ShipmentDocumentViewRepository : IShipmentDocumentRepository
    {
        ApplicationDbContext context;
        private string connectionString;

        public ShipmentDocumentViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<ShipmentDocument> GetShipmentDocuments(int companyId)
        {
            List<ShipmentDocument> ShipmentDocuments = new List<ShipmentDocument>();

            string queryString =
                "SELECT ShipmentDocumentId, ShipmentDocumentIdentifier, " +
                "ShipmentId, ShipmentIdentifier, ShipmentCode, " +
                "Name, CreateDate, Path, ItemStatus, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
                "CompanyId, CompanyName " +
                "FROM vShipmentDocuments " +
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    ShipmentDocument shipmentDocument;
                    while (reader.Read())
                    {
                        shipmentDocument = new ShipmentDocument();
                        shipmentDocument.Id = Int32.Parse(reader["ShipmentDocumentId"].ToString());
                        shipmentDocument.Identifier = Guid.Parse(reader["ShipmentDocumentIdentifier"].ToString());

                        if (reader["ShipmentId"] != DBNull.Value)
                        {
                            shipmentDocument.Shipment = new Shipment();
                            shipmentDocument.ShipmentId = Int32.Parse(reader["ShipmentId"].ToString());
                            shipmentDocument.Shipment.Id = Int32.Parse(reader["ShipmentId"].ToString());
                            shipmentDocument.Shipment.Identifier = Guid.Parse(reader["ShipmentIdentifier"].ToString());
                            shipmentDocument.Shipment.Code = reader["ShipmentCode"].ToString();

                        }

                        if (reader["Name"] != DBNull.Value)
                            shipmentDocument.Name = reader["Name"].ToString();
                        if (reader["CreateDate"] != DBNull.Value)
                            shipmentDocument.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                        if (reader["Path"] != DBNull.Value)
                            shipmentDocument.Path = reader["Path"].ToString();
                        if (reader["ItemStatus"] != DBNull.Value)
                            shipmentDocument.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());
                        shipmentDocument.Active = bool.Parse(reader["Active"].ToString());
                        shipmentDocument.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            shipmentDocument.CreatedBy = new User();
                            shipmentDocument.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            shipmentDocument.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            shipmentDocument.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            shipmentDocument.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            shipmentDocument.Company = new Company();
                            shipmentDocument.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            shipmentDocument.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            shipmentDocument.Company.Name = reader["CompanyName"].ToString();
                        }

                        ShipmentDocuments.Add(shipmentDocument);
                    }
                }
            }
            return ShipmentDocuments;

        }

        public List<ShipmentDocument> GetShipmentDocumentsByShipment(int ShipmentId)
        {
            List<ShipmentDocument> ShipmentDocuments = new List<ShipmentDocument>();

            string queryString =
                "SELECT ShipmentDocumentId, ShipmentDocumentIdentifier, " +
                "ShipmentId, ShipmentIdentifier, ShipmentCode, " +
                "Name, CreateDate, Path, ItemStatus, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
                "CompanyId, CompanyName " +
                "FROM vShipmentDocuments " +
                "WHERE ShipmentId = @ShipmentId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@ShipmentId", ShipmentId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    ShipmentDocument shipmentDocument;
                    while (reader.Read())
                    {
                        shipmentDocument = new ShipmentDocument();
                        shipmentDocument.Id = Int32.Parse(reader["ShipmentDocumentId"].ToString());
                        shipmentDocument.Identifier = Guid.Parse(reader["ShipmentDocumentIdentifier"].ToString());

                        if (reader["ShipmentId"] != DBNull.Value)
                        {
                            shipmentDocument.Shipment = new Shipment();
                            shipmentDocument.ShipmentId = Int32.Parse(reader["ShipmentId"].ToString());
                            shipmentDocument.Shipment.Id = Int32.Parse(reader["ShipmentId"].ToString());
                            shipmentDocument.Shipment.Identifier = Guid.Parse(reader["ShipmentIdentifier"].ToString());
                            shipmentDocument.Shipment.Code = reader["ShipmentCode"].ToString();

                        }

                        if (reader["Name"] != DBNull.Value)
                            shipmentDocument.Name = reader["Name"].ToString();
                        if (reader["CreateDate"] != DBNull.Value)
                            shipmentDocument.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                        if (reader["Path"] != DBNull.Value)
                            shipmentDocument.Path = reader["Path"].ToString();
                        if (reader["ItemStatus"] != DBNull.Value)
                            shipmentDocument.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());
                        shipmentDocument.Active = bool.Parse(reader["Active"].ToString());
                        shipmentDocument.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            shipmentDocument.CreatedBy = new User();
                            shipmentDocument.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            shipmentDocument.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            shipmentDocument.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            shipmentDocument.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            shipmentDocument.Company = new Company();
                            shipmentDocument.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            shipmentDocument.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            shipmentDocument.Company.Name = reader["CompanyName"].ToString();
                        }

                        ShipmentDocuments.Add(shipmentDocument);
                    }
                }
            }
            return ShipmentDocuments;


        }

        public List<ShipmentDocument> GetShipmentDocumentsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<ShipmentDocument> ShipmentDocuments = new List<ShipmentDocument>();

            string queryString =
                "SELECT ShipmentDocumentId, ShipmentDocumentIdentifier, " +
                "ShipmentId, ShipmentIdentifier, ShipmentCode, " +
                "Name, CreateDate, Path, ItemStatus, " +
                "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, " +
                "CompanyId, CompanyName " +
                "FROM vShipmentDocuments " +
                "WHERE CompanyId = @CompanyId " +
                "AND CONVERT(DATETIME, CONVERT(VARCHAR(20), UpdatedAt, 120)) > CONVERT(DATETIME, CONVERT(VARCHAR(20), @LastUpdateTime, 120));";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));
                command.Parameters.Add(new SqlParameter("@LastUpdateTime", lastUpdateTime));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    ShipmentDocument shipmentDocument;
                    while (reader.Read())
                    {
                        shipmentDocument = new ShipmentDocument();
                        shipmentDocument.Id = Int32.Parse(reader["ShipmentDocumentId"].ToString());
                        shipmentDocument.Identifier = Guid.Parse(reader["ShipmentDocumentIdentifier"].ToString());

                        if (reader["ShipmentId"] != DBNull.Value)
                        {
                            shipmentDocument.Shipment = new Shipment();
                            shipmentDocument.ShipmentId = Int32.Parse(reader["ShipmentId"].ToString());
                            shipmentDocument.Shipment.Id = Int32.Parse(reader["ShipmentId"].ToString());
                            shipmentDocument.Shipment.Identifier = Guid.Parse(reader["ShipmentIdentifier"].ToString());
                            shipmentDocument.Shipment.Code = reader["ShipmentCode"].ToString();

                        }

                        if (reader["Name"] != DBNull.Value)
                            shipmentDocument.Name = reader["Name"].ToString();
                        if (reader["CreateDate"] != DBNull.Value)
                            shipmentDocument.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                        if (reader["Path"] != DBNull.Value)
                            shipmentDocument.Path = reader["Path"].ToString();
                        if (reader["ItemStatus"] != DBNull.Value)
                            shipmentDocument.ItemStatus = Int32.Parse(reader["ItemStatus"].ToString());
                        shipmentDocument.Active = bool.Parse(reader["Active"].ToString());
                        shipmentDocument.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

                        if (reader["CreatedById"] != DBNull.Value)
                        {
                            shipmentDocument.CreatedBy = new User();
                            shipmentDocument.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                            shipmentDocument.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                            shipmentDocument.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                            shipmentDocument.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
                        }

                        if (reader["CompanyId"] != DBNull.Value)
                        {
                            shipmentDocument.Company = new Company();
                            shipmentDocument.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                            shipmentDocument.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                            shipmentDocument.Company.Name = reader["CompanyName"].ToString();
                        }

                        ShipmentDocuments.Add(shipmentDocument);
                    }
                }
            }
            return ShipmentDocuments;
        }

        public ShipmentDocument Create(ShipmentDocument ShipmentDocument)
        {
            if (context.ShipmentDocuments.Where(x => x.Identifier != null && x.Identifier == ShipmentDocument.Identifier).Count() == 0)
            {
                ShipmentDocument.Id = 0;

                ShipmentDocument.Active = true;
                ShipmentDocument.UpdatedAt = DateTime.Now;
                ShipmentDocument.CreatedAt = DateTime.Now;
                context.ShipmentDocuments.Add(ShipmentDocument);
                return ShipmentDocument;
            }
            else
            {
                // Load item that will be updated
                ShipmentDocument dbEntry = context.ShipmentDocuments
                    .FirstOrDefault(x => x.Identifier == ShipmentDocument.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = ShipmentDocument.CompanyId ?? null;
                    dbEntry.CreatedById = ShipmentDocument.CreatedById ?? null;
                    dbEntry.ShipmentId = ShipmentDocument.ShipmentId ?? null;
                    // Set properties
                    dbEntry.Name = ShipmentDocument.Name;
                    dbEntry.CreateDate = ShipmentDocument.CreateDate;
                    dbEntry.Path = ShipmentDocument.Path;
                    dbEntry.ItemStatus = ShipmentDocument.ItemStatus;
                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public ShipmentDocument Delete(Guid identifier)
        {
            ShipmentDocument dbEntry = context.ShipmentDocuments
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(ShipmentDocument))
                    .Select(x => x.Entity as ShipmentDocument))
                .FirstOrDefault(x => x.Identifier == identifier && x.Active == true);

            if (dbEntry != null)
            {
                dbEntry.Active = false;
                dbEntry.UpdatedAt = DateTime.Now;
            }
            return dbEntry;
        }
    }
}
