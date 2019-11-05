using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.CalendarAssignments;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.CalendarAssignments
{
    public class CalendarAssignmentResponse : BaseResponse
    {
        public CalendarAssignmentViewModel CalendarAssignment { get; set; }
    }
}
