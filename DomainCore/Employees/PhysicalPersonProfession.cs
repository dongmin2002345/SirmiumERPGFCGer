using DomainCore.Base;
using DomainCore.Common.Locations;
using DomainCore.Common.Professions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Employees
{
    public class PhysicalPersonProfession : BaseEntity
    {
        public int? PhysicalPersonId { get; set; }
        public PhysicalPerson PhysicalPerson { get; set; }

        public int? ProfessionId { get; set; }
        public Profession Profession { get; set; }

        public int? CountryId { get; set; }
        public Country Country { get; set; }
    }
}
