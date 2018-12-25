﻿using DomainCore.Base;
using DomainCore.Common.Locations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Employees
{
    public class PhysicalPersonLicence : BaseEntity
    {
        public int? PhysicalPersonId { get; set; }
        public PhysicalPerson PhysicalPerson { get; set; }

        public int? LicenceId { get; set; }
        public LicenceType Licence { get; set; }

        public int? CountryId { get; set; }
        public Country Country { get; set; }

        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
    }
}