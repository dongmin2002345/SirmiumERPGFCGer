using DomainCore.Base;
using DomainCore.Common.Locations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.Sectors
{
    public class Agency : BaseEntity
    {
        public string Code { get; set; }

        public string InternalCode { get; set; }
        public string Name { get; set; }

        public int? CountryId { get; set; }
        public Country Country { get; set; }

        public int? SectorId { get; set; }
        public Sector Sector { get; set; }
    }
}
