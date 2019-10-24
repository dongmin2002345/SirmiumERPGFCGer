using DomainCore.Base;
using DomainCore.Common.Locations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.Phonebooks
{
    public class Phonebook : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public int? CountryId { get; set; }
        public Country Country { get; set; }

        public int? RegionId { get; set; }
        public Region Region { get; set; }
        public int? MunicipalityId { get; set; }
        public Municipality Municipality { get; set; }

        public int? CityId { get; set; }
        public City City { get; set; }
        public string Address { get; set; }
    }
}
