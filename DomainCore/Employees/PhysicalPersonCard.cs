using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Employees
{
    public class PhysicalPersonCard : BaseEntity
    {
        public int? PhysicalPersonId { get; set; }
        public PhysicalPerson PhysicalPerson { get; set; }

        public DateTime CardDate { get; set; }
        public string Description { get; set; }

        public string PlusMinus { get; set; }

        public int ItemStatus { get; set; }
    }
}
