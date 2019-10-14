using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Employees
{
    public class EmployeeNote : BaseEntity
    {
        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public string Note { get; set; }
        public DateTime NoteDate { get; set; }
        public int ItemStatus { get; set; }
    }
}
