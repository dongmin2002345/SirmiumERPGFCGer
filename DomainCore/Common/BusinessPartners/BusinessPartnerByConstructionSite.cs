using DomainCore.Base;
using DomainCore.ConstructionSites;
using DomainCore.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.BusinessPartners
{
    public class BusinessPartnerByConstructionSite : BaseEntity
    {
        public string Code { get; set; }

        public DateTime StartDate { get; set; }

        public int? BusinessPartnerId { get; set; }
        public BusinessPartner BusinessPartner { get; set; }

        public int? ConstructionSiteId { get; set; }
        public ConstructionSite ConstructionSite { get; set; }
    }
}
