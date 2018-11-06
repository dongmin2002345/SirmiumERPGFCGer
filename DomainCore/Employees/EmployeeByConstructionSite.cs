using DomainCore.Base;
using DomainCore.ConstructionSites;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Employees
{
    public class EmployeeByConstructionSite : BaseEntity
    {
        public string Code { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int? ConstructionSiteId { get; set; }
        public ConstructionSite ConstructionSite { get; set; }
    }
}
