using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Employees
{
    public class PhysicalPersonNote : BaseEntity
    {
        public int? PhysicalPersonId { get; set; }
        public PhysicalPerson PhysicalPerson { get; set; }

        public string Note { get; set; }
        public DateTime NoteDate { get; set; }

        public int ItemStatus { get; set; }
    }
}
