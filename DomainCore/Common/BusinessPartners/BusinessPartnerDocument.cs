using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.BusinessPartners
{
    public class BusinessPartnerDocument : BaseEntity
    {
        public int? BusinessPartnerId { get; set; }
        public BusinessPartner BusinessPartner { get; set; }

        public string Name { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Path { get; set; }
        public int ItemStatus { get; set; }
    }
}
