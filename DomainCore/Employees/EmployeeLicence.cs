using DomainCore.Base;
using DomainCore.Common.Locations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Employees
{
    public class EmployeeLicence : BaseEntity
    {
        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int? LicenceId { get; set; }
        public LicenceType Licence { get; set; }

        public int? CountryId { get; set; }
        public Country Country { get; set; }

        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public int ItemStatus { get; set; }
    }
}
