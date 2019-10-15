using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.Prices
{
    public class Discount : BaseEntity
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }
    }
}
