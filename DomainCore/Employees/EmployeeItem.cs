using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Employees
{
    public class EmployeeItem : BaseEntity
    {
        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int? FamilyMemberId { get; set; }
        public FamilyMember FamilyMember { get; set; }

        public string Name { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime? EmbassyDate { get; set; }
        public int ItemStatus { get; set; }
    }
}
