using DomainCore.Base;
using DomainCore.Common.BusinessPartners;
using DomainCore.ConstructionSites;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Employees
{
    public class EmployeeByBusinessPartner : BaseEntity
    {
        public string Code { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? RealEndDate { get; set; }

        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int? BusinessPartnerId { get; set; }
        public BusinessPartner BusinessPartner { get; set; }
    }
}
