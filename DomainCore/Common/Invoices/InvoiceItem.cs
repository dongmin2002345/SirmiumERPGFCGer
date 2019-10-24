using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.Invoices
{
    public class InvoiceItem : BaseEntity
    {
        public int? InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal Quantity { get; set; }
        public decimal PriceWithPDV { get; set; }
        public decimal PriceWithoutPDV { get; set; }
        public decimal Discount { get; set; }
        public decimal PDVPercent { get; set; }
        public decimal PDV { get; set; }
        public decimal Amount { get; set; }
        public int ItemStatus { get; set; }
    }
}
