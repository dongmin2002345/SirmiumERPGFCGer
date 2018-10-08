using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.Locations
{
    public class Region : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }

    }
}

