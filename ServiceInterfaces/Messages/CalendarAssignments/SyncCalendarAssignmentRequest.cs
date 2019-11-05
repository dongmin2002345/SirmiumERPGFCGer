using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.CalendarAssignments
{
    public class SyncCalendarAssignmentRequest
    {
        public int CompanyId { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
    }
}
