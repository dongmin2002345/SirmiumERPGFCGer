using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Employees
{
    public class FamilyMember : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }


    }
}
