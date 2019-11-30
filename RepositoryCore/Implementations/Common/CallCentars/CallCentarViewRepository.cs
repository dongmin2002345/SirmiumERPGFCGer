using Configurator;
using DomainCore.Common.CallCentars;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Common.CallCentars;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.CallCentars
{
    public class CallCentarViewRepository : ICallCentarRepository
    {
        private ApplicationDbContext context;
        private string connectionString;

        private string SelectString =
            "SELECT CallCentarId, CallCentarIdentifier, CallCentarCode, CallCentarReceivingDate, " +
            "UserId, UserIdentifier, UserCode, UserFirstName, UserLastName, " +
            "CallCentarComment, CallCentarEndingDate, CheckedDone, " +
            "Active, UpdatedAt, CreatedById, CreatedByFirstName, CreatedByLastName, CompanyId, CompanyName " +
            "FROM vCallCentars ";

        private CallCentar Read(SqlDataReader reader)
        {
            CallCentar CallCentar = new CallCentar();
            CallCentar.Id = Int32.Parse(reader["CallCentarId"].ToString());
            CallCentar.Identifier = Guid.Parse(reader["CallCentarIdentifier"].ToString());

            if (reader["CallCentarCode"] != DBNull.Value)
                CallCentar.Code = reader["CallCentarCode"].ToString();
            if (reader["CallCentarReceivingDate"] != DBNull.Value)
                CallCentar.ReceivingDate = DateTime.Parse(reader["CallCentarReceivingDate"].ToString());

            if (reader["UserId"] != DBNull.Value)
            {
                CallCentar.User = new User();
                CallCentar.UserId = Int32.Parse(reader["UserId"].ToString());
                CallCentar.User.Id = Int32.Parse(reader["UserId"].ToString());
                CallCentar.User.Identifier = Guid.Parse(reader["UserIdentifier"].ToString());
                CallCentar.User.Code = reader["UserCode"]?.ToString();
                CallCentar.User.FirstName = reader["UserFirstName"]?.ToString();
                CallCentar.User.LastName = reader["UserLastName"]?.ToString();
            }


            if (reader["CallCentarComment"] != DBNull.Value)
                CallCentar.Comment = reader["CallCentarComment"].ToString();
            if (reader["CallCentarEndingDate"] != DBNull.Value)
                CallCentar.EndingDate = DateTime.Parse(reader["CallCentarEndingDate"].ToString());

            CallCentar.CheckedDone = bool.Parse(reader["CheckedDone"].ToString());
            CallCentar.Active = bool.Parse(reader["Active"].ToString());
            CallCentar.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

            if (reader["CreatedById"] != DBNull.Value)
            {
                CallCentar.CreatedBy = new User();
                CallCentar.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                CallCentar.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                CallCentar.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                CallCentar.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
            }

            if (reader["CompanyId"] != DBNull.Value)
            {
                CallCentar.Company = new Company();
                CallCentar.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                CallCentar.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                CallCentar.Company.Name = reader["CompanyName"].ToString();
            }

            return CallCentar;
        }

        public CallCentarViewRepository(ApplicationDbContext context)
        {
            this.context = context;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        public List<CallCentar> GetCallCentars(int companyId)
        {
            List<CallCentar> CallCentars = new List<CallCentar>();

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
                    CallCentar CallCentar;
                    while (reader.Read())
                    {
                        CallCentar = Read(reader);
                        CallCentars.Add(CallCentar);
                    }
                }
            }

            return CallCentars;
        }

        public List<CallCentar> GetCallCentarsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<CallCentar> CallCentars = new List<CallCentar>();

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
                    CallCentar CallCentar;
                    while (reader.Read())
                    {
                        CallCentar = Read(reader);
                        CallCentars.Add(CallCentar);
                    }
                }
            }

            return CallCentars;
        }

        public CallCentar GetCallCentar(int CallCentarId)
        {
            CallCentar CallCentar = null;

            string queryString =
                SelectString +
                "WHERE CallCentarId = @CallCentarId;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CallCentarId", CallCentarId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        CallCentar = Read(reader);
                    }
                }
            }

            return CallCentar;
        }

        private string GetNewCodeValue(int companyId)
        {
            int count = context.CallCentars
                .Union(context.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(CallCentar))
                    .Select(x => x.Entity as CallCentar))
                .Where(x => x.CompanyId == companyId).Count();
            if (count == 0)
                return "1";
            else
            {
                string activeCode = context.CallCentars
                    .Union(context.ChangeTracker.Entries()
                        .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(CallCentar))
                        .Select(x => x.Entity as CallCentar))
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

        public CallCentar Create(CallCentar CallCentar)
        {
            if (context.CallCentars.Where(x => x.Identifier != null && x.Identifier == CallCentar.Identifier).Count() == 0)
            {
                CallCentar.Id = 0;

                CallCentar.Code = GetNewCodeValue(CallCentar.CompanyId ?? 0);
                CallCentar.Active = true;

                CallCentar.UpdatedAt = DateTime.Now;
                CallCentar.CreatedAt = DateTime.Now;

                context.CallCentars.Add(CallCentar);
                return CallCentar;
            }
            else
            {
                // Load CallCentar that will be updated
                CallCentar dbEntry = context.CallCentars
                    .FirstOrDefault(x => x.Identifier == CallCentar.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = CallCentar.CompanyId ?? null;
                    dbEntry.CreatedById = CallCentar.CreatedById ?? null;
                    dbEntry.UserId = CallCentar.UserId ?? null;
                    // Set properties
                    dbEntry.Code = CallCentar.Code;
                    dbEntry.ReceivingDate = CallCentar.ReceivingDate;
                    dbEntry.Comment = CallCentar.Comment;
                    dbEntry.EndingDate = CallCentar.EndingDate;
                    dbEntry.CheckedDone = CallCentar.CheckedDone;
                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public CallCentar Delete(Guid identifier)
        {
            // Load item that will be deleted
            CallCentar dbEntry = context.CallCentars
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
