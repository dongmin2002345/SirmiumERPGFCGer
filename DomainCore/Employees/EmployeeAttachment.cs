using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Employees
{
    public class EmployeeAttachment : BaseEntity
    {
        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public bool OK { get; set; }
    }
}
