using Configurator;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Common.Prices;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Common.Prices;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.Prices
{
    public class ServiceDeliveryViewRepository : IServiceDeliveryRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        private string SelectString =
            "SELECT ServiceDeliveryId, ServiceDeliveryIdentifier, ServiceDeliveryCode, ServiceDeliveryName, ServiceDeliveryURL, " +
            "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
            "FROM vServiceDeliverys ";

        private ServiceDelivery Read(SqlDataReader reader)
        {
            ServiceDelivery serviceDelivery = new ServiceDelivery();
            serviceDelivery.Id = Int32.Parse(reader["ServiceDeliveryId"].ToString());
            serviceDelivery.Identifier = Guid.Parse(reader["ServiceDeliveryIdentifier"].ToString());

            if (reader["ServiceDeliveryCode"] != DBNull.Value)
                serviceDelivery.Code = reader["ServiceDeliveryCode"].ToString();
            if (reader["ServiceDeliveryName"] != DBNull.Value)
                serviceDelivery.Name = reader["ServiceDeliveryName"].ToString();
            if (reader["ServiceDeliveryURL"] != DBNull.Value)
                serviceDelivery.URL = reader["ServiceDeliveryURL"].ToString();

            serviceDelivery.Active = bool.Parse(reader["Active"].ToString());
            serviceDelivery.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

            if (reader["CreatedById"] != DBNull.Value)
            {
                serviceDelivery.CreatedBy = new User();
                serviceDelivery.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                serviceDelivery.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                serviceDelivery.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                serviceDelivery.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
            }

            if (reader["CompanyId"] != DBNull.Value)
            {
                serviceDelivery.Company = new Company();
                serviceDelivery.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                serviceDelivery.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                serviceDelivery.Company.Name = reader["CompanyName"].ToString();
            }

            return serviceDelivery;
        }

        public ServiceDeliveryViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<ServiceDelivery> GetServiceDeliverys(int companyId)
        {
            List<ServiceDelivery> serviceDeliverys = new List<ServiceDelivery>();

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
                    ServiceDelivery serviceDelivery;
                    while (reader.Read())
                    {
                        serviceDelivery = Read(reader);
                        serviceDeliverys.Add(serviceDelivery);
                    }
                }
            }

            return serviceDeliverys;
        }

        public List<ServiceDelivery> GetServiceDeliverysNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<ServiceDelivery> serviceDeliverys = new List<ServiceDelivery>();

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
                    ServiceDelivery serviceDelivery;
                    while (reader.Read())
                    {
                        serviceDelivery = Read(reader);
                        serviceDeliverys.Add(serviceDelivery);
                    }
                }
            }

            return serviceDeliverys;
        }

        public ServiceDelivery GetServiceDelivery(int serviceDeliveryId)
        {
            ServiceDelivery serviceDelivery = null;

            string queryString =
                SelectString +
                "WHERE ServiceDeliveryId = @ServiceDeliveryId;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@ServiceDeliveryId", serviceDeliveryId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        serviceDelivery = Read(reader);
                    }
                }
            }

            return serviceDelivery;
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.ServiceDeliverys
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(ServiceDelivery))
                    .Select(x => x.Entity as ServiceDelivery))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "1";
            else
            {
                string activeCode = context.ServiceDeliverys
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(ServiceDelivery))
                        .Select(x => x.Entity as ServiceDelivery))
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

        public ServiceDelivery Create(ServiceDelivery serviceDelivery)
        {
            if (context.ServiceDeliverys.Where(x => x.Identifier != null && x.Identifier == serviceDelivery.Identifier).Count() == 0)
            {
                serviceDelivery.Id = 0;

                serviceDelivery.Code = GetNewCodeValue(serviceDelivery.CompanyId ?? 0);
                serviceDelivery.Active = true;

                serviceDelivery.UpdatedAt = DateTime.Now;
                serviceDelivery.CreatedAt = DateTime.Now;

                context.ServiceDeliverys.Add(serviceDelivery);
                return serviceDelivery;
            }
            else
            {
                // Load ServiceDelivery that will be updated
                ServiceDelivery dbEntry = context.ServiceDeliverys
                    .FirstOrDefault(x => x.Identifier == serviceDelivery.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = serviceDelivery.CompanyId ?? null;
                    dbEntry.CreatedById = serviceDelivery.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = serviceDelivery.Code;
                    dbEntry.Name = serviceDelivery.Name;
                    dbEntry.URL = serviceDelivery.URL;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public ServiceDelivery Delete(Guid identifier)
        {
            // Load item that will be deleted
            ServiceDelivery dbEntry = context.ServiceDeliverys
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
