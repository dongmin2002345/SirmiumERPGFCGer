﻿using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.Locations
{
    public class Municipality : BaseEntity
    {
        public string Code { get; set; }

        public string MunicipalityCode { get; set; }
        public string Name { get; set; }

        public int? RegionId { get; set; }
        public Region Region { get; set; }

        public int? CountryId { get; set; }
        public Country Country { get; set; }
    }
}

