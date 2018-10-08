using DomainCore.Base;
using DomainCore.Common.Companies;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.Locations
{
    public class City : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public string ZipCode { get; set; }

        public int? CountryId { get; set; }
        public Country Country { get; set; }

        public int? RegionId { get; set; }
        public Region Region { get; set; }

        public int? MunicipalityId { get; set; }
        public Municipality Municipality { get; set; }

        public List<Company> Companies { get; set; }
    }
}
