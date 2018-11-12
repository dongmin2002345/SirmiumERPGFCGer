﻿using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.ConstructionSites
{
    public class ConstructionSiteCalculation : BaseEntity
    {
        public int? ConstructionSiteId { get; set; }
        public ConstructionSite ConstructionSite { get; set; }

        public int NumOfEmployees { get; set; }
        public decimal EmployeePrice { get; set; }
        public int NumOfMonths { get; set; }

        public decimal OldValue { get; set; }
        public decimal NewValue { get; set; }
        public decimal ValueDifference { get; set; }

        public string PlusMinus { get; set; }
    }
}
