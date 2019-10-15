using Configurator;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Statuses;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Statuses;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Statuses
{
    public class StatusViewRepository : IStatusRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        private string SelectString =
            "SELECT StatusId, StatusIdentifier, StatusCode, StatusName, StatusShortName, " +
            "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
            "FROM vStatuses ";

        private Status Read(SqlDataReader reader)
        {
            Status status = new Status();
            status.Id = Int32.Parse(reader["StatusId"].ToString());
            status.Identifier = Guid.Parse(reader["StatusIdentifier"].ToString());

            if (reader["StatusCode"] != DBNull.Value)
                status.Code = reader["StatusCode"].ToString();
            if (reader["StatusName"] != DBNull.Value)
                status.Name = reader["StatusName"].ToString();
            if (reader["StatusShortName"] != DBNull.Value)
                status.ShortName = reader["StatusShortName"].ToString();

            status.Active = bool.Parse(reader["Active"].ToString());
            status.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

            if (reader["CreatedById"] != DBNull.Value)
            {
                status.CreatedBy = new User();
                status.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                status.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                status.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                status.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
            }

            if (reader["CompanyId"] != DBNull.Value)
            {
                status.Company = new Company();
                status.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                status.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                status.Company.Name = reader["CompanyName"].ToString();
            }

            return status;
        }

        public StatusViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<Status> GetStatuses(int companyId)
        {
            List<Status> statuses = new List<Status>();

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
                    Status status;
                    while (reader.Read())
                    {
                        status = Read(reader);
                        statuses.Add(status);
                    }
                }
            }

            return statuses;
        }

        public List<Status> GetStatusesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<Status> statuses = new List<Status>();

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
                    Status status;
                    while (reader.Read())
                    {
                        status = Read(reader);
                        statuses.Add(status);
                    }
                }
            }

            return statuses;
        }

        public Status GetStatus(int statusId)
        {
            Status status = null;

            string queryString =
                SelectString +
                "WHERE StatusId = @StatusId;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@StatusId", statusId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        status = Read(reader);
                    }
                }
            }

            return status;
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.Statuses
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Status))
                    .Select(x => x.Entity as Status))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "1";
            else
            {
                string activeCode = context.Statuses
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(Status))
                        .Select(x => x.Entity as Status))
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

        public Status Create(Status status)
        {
            if (context.Statuses.Where(x => x.Identifier != null && x.Identifier == status.Identifier).Count() == 0)
            {
                status.Id = 0;

                status.Code = GetNewCodeValue(status.CompanyId ?? 0);
                status.Active = true;

                status.UpdatedAt = DateTime.Now;
                status.CreatedAt = DateTime.Now;

                context.Statuses.Add(status);
                return status;
            }
            else
            {
                // Load Status that will be updated
                Status dbEntry = context.Statuses
                    .FirstOrDefault(x => x.Identifier == status.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = status.CompanyId ?? null;
                    dbEntry.CreatedById = status.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = status.Code;
                    dbEntry.Name = status.Name;
                    dbEntry.ShortName = status.ShortName;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public Status Delete(Guid identifier)
        {
            // Load item that will be deleted
            Status dbEntry = context.Statuses
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
