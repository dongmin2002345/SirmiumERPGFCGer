using DomainCore.Base;
using DomainCore.Common.Companies;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.Locations
{
    public class Country : BaseEntity
    {
        public string Code { get; set; }

        public string AlfaCode { get; set; }

        public string NumericCode { get; set; }

        public string Mark { get; set; }

        public string Name { get; set; }

        public List<Company> Companies { get; set; }
    }
}
