using Configurator;
using DomainCore.CalendarAssignments;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.CalendarAssignments;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.CalendarAssignments
{
    public class CalendarAssignmentRepository : ICalendarAssignmentRepository
    {
        private ApplicationDbContext context { get; set; }
        private string connectionString;

        string selectPart = "SELECT CalendarAssignmentId, CalendarAssignmentIdentifier, Active, CalendarAssignmentName, CalendarAssignmentDescription, " +
                "CalendarAssignmentDate, CompanyId, CompanyIdentifier, CompanyCode, CompanyName, " +
                "CreatedById, CreatedByIdentifier, CreatedByFirstName, CreatedByLastName, " +
                "UpdatedAt, AssignedToId, AsignedToIdentifier, AssignedToFirstName, AssignedToLastName " +
                "FROM vCalendarAssignments ";

        public CalendarAssignmentRepository(ApplicationDbContext ctx)
        {
            context = ctx;
            connectionString = new Config().GetConfiguration()["ConnectionString"] as string;
        }

        #region GET
        public List<CalendarAssignment> GetCalendarAssignments(int companyId)
        {
            List<CalendarAssignment> calendarAssignments = new List<CalendarAssignment>();
            string queryString = selectPart + 
                "WHERE CompanyId = @CompanyId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CalendarAssignment calendarAssignment = Read(reader);
                        calendarAssignments.Add(calendarAssignment);
                    }
                }
            }

            return calendarAssignments;
        }

        private CalendarAssignment Read(SqlDataReader reader)
        {
            CalendarAssignment assignment = new CalendarAssignment();
            assignment.Id = Int32.Parse(reader["CalendarAssignmentId"].ToString());
            assignment.Identifier = Guid.Parse(reader["CalendarAssignmentIdentifier"].ToString());
            assignment.Name = reader["CalendarAssignmentName"]?.ToString();
            assignment.Description = reader["CalendarAssignmentDescription"]?.ToString();
            assignment.Date = DateTime.Parse(reader["CalendarAssignmentDate"]?.ToString());

            assignment.Active = bool.Parse(reader["Active"].ToString());
            assignment.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());

            if (reader["CreatedById"] != DBNull.Value)
            {
                assignment.CreatedBy = new User();
                assignment.CreatedById = Int32.Parse(reader["CreatedById"].ToString());
                assignment.CreatedBy.Id = Int32.Parse(reader["CreatedById"].ToString());
                assignment.CreatedBy.FirstName = reader["CreatedByFirstName"]?.ToString();
                assignment.CreatedBy.LastName = reader["CreatedByLastName"]?.ToString();
            }

            if (reader["AssignedToId"] != DBNull.Value)
            {
                assignment.AssignedTo = new User();
                assignment.AssignedToId = Int32.Parse(reader["AssignedToId"].ToString());
                assignment.AssignedTo.Id = Int32.Parse(reader["AssignedToId"].ToString());
                assignment.AssignedTo.FirstName = reader["AssignedToFirstName"]?.ToString();
                assignment.AssignedTo.LastName = reader["AssignedToLastName"]?.ToString();
            }

            if (reader["CompanyId"] != DBNull.Value)
            {
                assignment.Company = new Company();
                assignment.CompanyId = Int32.Parse(reader["CompanyId"].ToString());
                assignment.Company.Id = Int32.Parse(reader["CompanyId"].ToString());
                assignment.Company.Name = reader["CompanyName"].ToString();
            }

            return assignment;
        }

        public List<CalendarAssignment> GetCalendarAssignmentsByEmployee(int EmployeeId)
        {
            List<CalendarAssignment> calendarAssignments = new List<CalendarAssignment>();
            string queryString = selectPart +
                "WHERE AssignedToId = @AssignedToId AND Active = 1;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@AssignedToId", EmployeeId));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CalendarAssignment calendarAssignment = Read(reader);
                        calendarAssignments.Add(calendarAssignment);
                    }
                }
            }

            return calendarAssignments;
        }

        public List<CalendarAssignment> GetCalendarAssignmentsNewerThen(int companyId, DateTime lastUpdateTime)
        {
            List<CalendarAssignment> calendarAssignments = new List<CalendarAssignment>();
            string queryString = selectPart +
                "WHERE CompanyId = @CompanyId " +
                "AND CONVERT(DATETIME, CONVERT(VARCHAR(20), UpdatedAt, 120)) > CONVERT(DATETIME, CONVERT(VARCHAR(20), @LastUpdateTime, 120)) ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                command.Parameters.Add(new SqlParameter("@CompanyId", companyId));
                command.Parameters.Add(new SqlParameter("@LastUpdateTime", lastUpdateTime));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CalendarAssignment calendarAssignment = Read(reader);
                        calendarAssignments.Add(calendarAssignment);
                    }
                }
            }

            return calendarAssignments;
        }
        #endregion

        #region CREATE
        public CalendarAssignment Create(CalendarAssignment calendarAssignment)
        {
            if (context.CalendarAssignments.Where(x => x.Identifier != null && x.Identifier == calendarAssignment.Identifier).Count() == 0)
            {
                calendarAssignment.Id = 0;

                calendarAssignment.Active = true;
                calendarAssignment.UpdatedAt = DateTime.Now;
                calendarAssignment.CreatedAt = DateTime.Now;
                context.CalendarAssignments.Add(calendarAssignment);
                return calendarAssignment;
            }
            else
            {
                // Load item that will be updated
                CalendarAssignment dbEntry = context.CalendarAssignments
                    .FirstOrDefault(x => x.Identifier == calendarAssignment.Identifier && x.Active == true);

                if (dbEntry != null)
                {
                    dbEntry.CompanyId = calendarAssignment.CompanyId ?? null;
                    dbEntry.CreatedById = calendarAssignment.CreatedById ?? null;
                    dbEntry.AssignedToId = calendarAssignment.AssignedToId ?? null;

                    dbEntry.Date = calendarAssignment.Date;
                    dbEntry.Name = calendarAssignment.Name;
                    dbEntry.Description = calendarAssignment.Description;

                    // Set timestamp
                    dbEntry.UpdatedAt = DateTime.Now;
                }

                return dbEntry;
            }
        }

        public CalendarAssignment Delete(Guid identifier)
        {
            // Load item that will be updated
            CalendarAssignment dbEntry = context.CalendarAssignments
                .Union(context.ChangeTracker.Entries()
                   .Where(x => x.State == EntityState.Added && x.Entity.GetType() == typeof(CalendarAssignment))
                   .Select(x => x.Entity as CalendarAssignment))
                .FirstOrDefault(x => x.Identifier == identifier && x.Active == true);

            if (dbEntry != null)
            {
                dbEntry.Active = false;

                // Set timestamp
                dbEntry.UpdatedAt = DateTime.Now;
            }

            return dbEntry;
        }
        #endregion
    }
}
