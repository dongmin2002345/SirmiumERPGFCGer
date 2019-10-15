using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Vats
{
    public class Vat : BaseEntity
    {
        public string Code { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

    }
}
