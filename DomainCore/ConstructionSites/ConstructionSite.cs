using DomainCore.Base;
using DomainCore.Common.Locations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.ConstructionSites
{
    public class ConstructionSite : BaseEntity
    {
        public string Code { get; set; }
        public string InternalCode { get; set; }

        public string Name { get; set; }

        public int? CityId { get; set; }
        public City City { get; set; }

        public string Address { get; set; }

        public int MaxWorkers { get; set; }

        public DateTime ContractStart { get; set; }
        public DateTime ContractExpiration { get; set; }
    }
}
