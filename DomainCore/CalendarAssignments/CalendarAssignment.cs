using DomainCore.Base;
using DomainCore.Common.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.CalendarAssignments
{
    public class CalendarAssignment : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int? AssignedToId { get; set; }
        public User AssignedTo { get; set; }

        public DateTime Date { get; set; }
    }
}
