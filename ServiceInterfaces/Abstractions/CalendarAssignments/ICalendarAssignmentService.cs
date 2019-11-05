using ServiceInterfaces.Messages.CalendarAssignments;
using ServiceInterfaces.ViewModels.CalendarAssignments;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Abstractions.CalendarAssignments
{
    public interface ICalendarAssignmentService
    {
        CalendarAssignmentListResponse GetCalendarAssignments(int companyId);
        CalendarAssignmentListResponse GetCalendarAssignmentsNewerThen(int companyId, DateTime? lastUpdateTime);

        CalendarAssignmentResponse Create(CalendarAssignmentViewModel assignment);
        CalendarAssignmentResponse Delete(Guid identifier);

        CalendarAssignmentListResponse Sync(SyncCalendarAssignmentRequest request);
    }
}
