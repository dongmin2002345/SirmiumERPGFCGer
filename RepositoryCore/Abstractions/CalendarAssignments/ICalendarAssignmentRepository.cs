using DomainCore.CalendarAssignments;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.CalendarAssignments
{
    public interface ICalendarAssignmentRepository
    {
        List<CalendarAssignment> GetCalendarAssignments(int companyId);
        List<CalendarAssignment> GetCalendarAssignmentsByEmployee(int EmployeeId);
        List<CalendarAssignment> GetCalendarAssignmentsNewerThen(int companyId, DateTime lastUpdateTime);

        CalendarAssignment Create(CalendarAssignment calendarAssignment);
        CalendarAssignment Delete(Guid identifier);
    }
}
