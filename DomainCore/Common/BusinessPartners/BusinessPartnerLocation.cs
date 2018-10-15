using DomainCore.Base;
using DomainCore.Common.Locations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.BusinessPartners
{
    public class BusinessPartnerLocation : BaseEntity
    {
        public int? BusinessPartnerId { get; set; }
        public BusinessPartner BusinessPartner { get; set; }

        public string Address { get; set; }

        public int? CountryId { get; set; }
        public Country Country { get; set; }
        public int? CityId { get; set; }
        public City City { get; set; }
        public int? MunicipalityId { get; set; }
        public Municipality Municipality { get; set; }

    }
}
