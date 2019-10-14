using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Employees
{
    public class EmployeeDocument : BaseEntity
    {
        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public string Name { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Path { get; set; }
        public int ItemStatus { get; set; }
    }
}
