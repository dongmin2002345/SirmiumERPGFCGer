using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.Individuals
{
    public class Individual : BaseEntity
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Address { get; set; }
        public string Passport { get; set; }
        public string Interest { get; set; }
        public string License { get; set; }

        public DateTime EmbassyDate { get; set; }
        public DateTime VisaFrom { get; set; }
        public DateTime VisaTo { get; set; }
        public DateTime WorkPermitFrom { get; set; }
        public DateTime WorkPermitTo { get; set; }

        public bool Family { get; set; }
    }
}
