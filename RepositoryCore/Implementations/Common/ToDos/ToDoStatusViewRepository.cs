using Configurator;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using DomainCore.Common.ToDos;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Common.ToDos;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.ToDos
{
    public class ToDoStatusViewRepository : IToDoStatusRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        private string SelectString =
            "SELECT ToDoStatusId, ToDoStatusIdentifier, ToDoStatusCode, ToDoStatusName, " +
            "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
            "FROM vToDoStatuses ";

        private ToDoStatus Read(SqlDataReader reader)
        {
            ToDoStatus toDoStatus = new ToDoStatus();
            toDoStatus.Id = Int32.Parse(reader["ToDoStatusId"].ToString());
            toDoStatus.Identifier = Guid.Parse(reader["ToDoStatusIdentifier"].ToString());

            if (reader["ToDoStatusCode"] != DBNull.Value)
                toDoStatus.Code = reader["ToDoStatusCode"].ToString();
            if (reader["ToDoStatusName"] != DBNull.Value)
                toDoStatus.Name = reader["ToDoStatusName"].ToString();

            toDoStatus.Active = bool.Parse(reader["Active"].ToString());
            toDoStatus.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

            if (reader["CreatedById"] != DBNull.Value)
            {
                toDoStatus.CreatedBy = new User();
                toDoStatus.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                toDoStatus.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                toDoStatus.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                toDoStatus.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
            }

            if (reader["CompanyId"] != DBNull.Value)
            {
                toDoStatus.Company = new Company();
                toDoStatus.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                toDoStatus.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                toDoStatus.Company.Name = reader["CompanyName"].ToString();
            }

            return toDoStatus;
        }

        public ToDoStatusViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<ToDoStatus> GetToDoStatuses(int companyId)
        {
            List<ToDoStatus> countries = new List<ToDoStatus>();

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
                    ToDoStatus toDoStatus;
                    while (reader.Read())
                    {
                        toDoStatus = Read(reader);
                        countries.Add(toDoStatus);
                    }
                }
            }

            return countries;
        }

        public List<ToDoStatus> GetToDoStatusesNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<ToDoStatus> countries = new List<ToDoStatus>();

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
                    ToDoStatus toDoStatus;
                    while (reader.Read())
                    {
                        toDoStatus = Read(reader);
                        countries.Add(toDoStatus);
                    }
                }
            }

            return countries;
        }

        public ToDoStatus GetToDoStatus(int toDoStatusId)
        {
            ToDoStatus toDoStatus = null;

            string queryString =
                SelectString +
                "WHERE ToDoStatusId = @ToDoStatusId;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@ToDoStatusId", toDoStatusId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        toDoStatus = Read(reader);
                    }
                }
            }

            return toDoStatus;
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.ToDoStatuses
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(ToDoStatus))
                    .Select(x => x.Entity as ToDoStatus))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "1";
            else
            {
                string activeCode = context.ToDoStatuses
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(ToDoStatus))
                        .Select(x => x.Entity as ToDoStatus))
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

        public ToDoStatus Create(ToDoStatus toDoStatus)
        {
            if (context.ToDoStatuses.Where(x => x.Identifier != null && x.Identifier == toDoStatus.Identifier).Count() == 0)
            {
                toDoStatus.Id = 0;

                toDoStatus.Code = GetNewCodeValue(toDoStatus.CompanyId ?? 0);
                toDoStatus.Active = true;

                toDoStatus.UpdatedAt = DateTime.Now;
                toDoStatus.CreatedAt = DateTime.Now;

                context.ToDoStatuses.Add(toDoStatus);
                return toDoStatus;
            }
            else
            {
                // Load ToDoStatus that will be updated
                ToDoStatus dbEntry = context.ToDoStatuses
                    .FirstOrDefault(x => x.Identifier == toDoStatus.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = toDoStatus.CompanyId ?? null;
                    dbEntry.CreatedById = toDoStatus.CreatedById ?? null;

                    // Set properties
                    dbEntry.Code = toDoStatus.Code;
                    dbEntry.Name = toDoStatus.Name;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public ToDoStatus Delete(Guid identifier)
        {
            // Load item that will be deleted
            ToDoStatus dbEntry = context.ToDoStatuses
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
