using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.BusinessPartners
{
    public class BusinessPartnerType : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public bool IsBuyer { get; set; }
        public bool IsSupplier { get; set; }

        public List<BusinessPartnerBusinessPartnerType> BusinessPartnerTypes { get; set; }
    }
}
