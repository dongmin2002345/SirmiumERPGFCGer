using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Statuses
{
    public class Status : BaseEntity
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

    }
}
