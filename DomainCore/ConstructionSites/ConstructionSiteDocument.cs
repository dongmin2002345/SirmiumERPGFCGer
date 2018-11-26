using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.ConstructionSites
{
    public class ConstructionSiteDocument : BaseEntity
    {
        public int? ConstructionSiteId { get; set; }
        public ConstructionSite ConstructionSite { get; set; }

        public string Name { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Path { get; set; }
    }
}
