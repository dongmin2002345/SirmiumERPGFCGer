using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.CalendarAssignments;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.CalendarAssignments
{
    public class CalendarAssignmentListResponse : BaseResponse
    {
        public List<CalendarAssignmentViewModel> CalendarAssignments { get; set; }
        public int TotalItems { get; set; }
    }
}
