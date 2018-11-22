using DomainCore.Banks;
using DomainCore.Base;
using DomainCore.Common.Locations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.TaxAdministrations
{
    public class TaxAdministration : BaseEntity
    {
        public string Code { get; set; }
        public string SecondCode { get; set; }
        public string Name { get; set; }

        public int? CityId { get; set; }
        public City City { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }

        public int? BankId1 { get; set; }
        public Bank Bank1 { get; set; }

        public int? BankId2 { get; set; }
        public Bank Bank2 { get; set; }

        public int IBAN1 { get; set; }
        public int SWIFT { get; set; } //BIC/SWIFT

    }
}
