using Configurator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryCore.DbViews.CalendarAssignments
{
    public class CalendarAssignmentView
    {
        public static void CreateView()
        {
            string connectionString = new Config().GetConfiguration()["ConnectionString"] as string;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string strSQLCommand = "DROP VIEW IF EXISTS vCalendarAssignments";
            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();

            strSQLCommand =
                "CREATE VIEW vCalendarAssignments AS " +
                "SELECT calendarAssignment.Id AS CalendarAssignmentId, calendarAssignment.Identifier AS CalendarAssignmentIdentifier, " +
                "calendarAssignment.Active, calendarAssignment.Name AS CalendarAssignmentName, calendarAssignment.Description AS CalendarAssignmentDescription, " +
                "calendarAssignment.Date AS CalendarAssignmentDate, " +
                "company.Id AS CompanyId, company.Identifier AS CompanyIdentifier, company.Code AS CompanyCode, company.Name AS CompanyName, " +
                "createdBy.Id AS CreatedById, createdBy.Identifier AS CreatedByIdentifier, createdBy.FirstName AS CreatedByCode, createdBy.LastName AS CreatedByName, " +
                "(SELECT MAX(v) FROM (VALUES (calendarAssignment.UpdatedAt)) AS value(v)) AS UpdatedAt, " +
                "assignedTo.Id AS AssignedToId, assignedTo.Identifier AS AsignedToIdentifier, assignedTo.FirstName AS AssignedToFirstName, assignedTo.LastName AS AssignedToLastName " +
                "FROM CalendarAssignments AS calendarAssignment " +
                "INNER JOIN Companies AS company ON calendarAssignment.CompanyId = company.Id " +
                "INNER JOIN Users AS createdBy ON calendarAssignment.CreatedById = createdBy.Id " +
                "INNER JOIN Users AS assignedTo ON calendarAssignment.AssignedToId = assignedTo.Id ";

            command = new SqlCommand(strSQLCommand, conn);
            returnvalue = (string)command.ExecuteScalar();

            conn.Close();
        }
    }
}
