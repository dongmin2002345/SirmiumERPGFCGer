using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Employees
{
    public class PhysicalPersonDocument : BaseEntity
    {
        public int? PhysicalPersonId { get; set; }
        public PhysicalPerson PhysicalPerson { get; set; }

        public string Name { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Path { get; set; }

        public int ItemStatus { get; set; }
    }
}
