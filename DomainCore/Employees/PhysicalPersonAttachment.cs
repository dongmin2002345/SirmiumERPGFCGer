using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Employees
{
    public class PhysicalPersonAttachment : BaseEntity
    {
        public string Code { get; set; }

        public int? PhysicalPersonId { get; set; }
        public PhysicalPerson PhysicalPerson { get; set; }
        public bool OK { get; set; }
    }
}
