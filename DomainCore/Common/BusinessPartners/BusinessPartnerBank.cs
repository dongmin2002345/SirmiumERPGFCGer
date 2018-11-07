using DomainCore.Banks;
using DomainCore.Base;
using DomainCore.Common.Locations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.BusinessPartners
{
    public class BusinessPartnerBank : BaseEntity
    {
        public int? BusinessPartnerId { get; set; }
        public BusinessPartner BusinessPartner { get; set; }

        public int? BankId { get; set; }
        public Bank Bank { get; set; }

        public int? CountryId { get; set; }
        public Country Country { get; set; }

        public string AccountNumber { get; set; }
    }
}
