using DomainCore.Base;
using DomainCore.Common.Locations;
using DomainCore.Common.Professions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Employees
{
    public class EmployeeProfession : BaseEntity
    {
        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int? ProfessionId { get; set; }
        public Profession Profession { get; set; }

        public int? CountryId { get; set; }
        public Country Country { get; set; }

        public int ItemStatus { get; set; }
    }
}
