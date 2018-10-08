using DomainCore.Base;
using DomainCore.Common.Locations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.Professions
{
    public class Profession : BaseEntity
    {
        public string Code { get; set; }
        public string SecondCode { get; set; }
        public string Name { get; set; }

        public int? CountryId { get; set; }
        public Country Country { get; set; }
    }
}
