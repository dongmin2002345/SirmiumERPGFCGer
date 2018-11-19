using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Employees
{
    public class EmployeeCard : BaseEntity
    {
        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public DateTime CardDate { get; set; }
        public string Description { get; set; }

        public string PlusMinus { get; set; }
    }
}
