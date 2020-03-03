using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.ConstructionSites
{
    public class ConstructionSiteNote : BaseEntity
    {
        public int? ConstructionSiteId { get; set; }
        public ConstructionSite ConstructionSite { get; set; }

        public string Note { get; set; }
        public DateTime NoteDate { get; set; }
        public int ItemStatus { get; set; }
        public string Path { get; set; }

    }
}
