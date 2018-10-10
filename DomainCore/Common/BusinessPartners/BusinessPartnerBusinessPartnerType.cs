using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.BusinessPartners
{
    public class BusinessPartnerBusinessPartnerType : BaseEntity
    {
        public int BusinessPartnerId { get; set; }
        public BusinessPartner BusinessPartner { get; set; }

        public int BusinessPartnerTypeId { get; set; }
        public BusinessPartnerType BusinessPartnerType { get; set; }
    }
}
