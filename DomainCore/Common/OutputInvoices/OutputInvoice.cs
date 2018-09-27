using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.OutputInvoices
{
    public class OutputInvoice : BaseEntity
    {
        public int Code { get; set; }

        public string Construction { get; set; }

        public DateTime InvoiceDate { get; set; }

        public string BusinessPartner { get; set; }

        public string InvoiceType { get; set; }

        public decimal Quantity { get; set; }

        public DateTime TrafficDate { get; set; }

        public decimal Price { get; set; }
        public decimal Rebate { get; set; }
        public decimal RebateValue { get; set; }
        public decimal Base { get; set; }
        public decimal PDV { get; set; }
        public decimal Total { get; set; }


    }
}
