using DomainCore.Base;
using DomainCore.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.OutputInvoices
{
    public class OutputInvoice : BaseEntity
    {
        public string Code { get; set; }

        public int? BusinessPartnerId { get; set; }
        public BusinessPartner BusinessPartner { get; set; }

        public string Supplier { get; set; }
        public string Address { get; set; }


        // public string Construction { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }

        public decimal AmountNet { get; set; }//neto
        public int PdvPercent { get; set; }
        public decimal Pdv{ get; set; }
        public decimal AmountGross { get; set; } //bruto
        public decimal Currency { get; set; }

        public DateTime DateOfPayment { get; set; }
        public string Status { get; set; }
        public DateTime StatusDate { get; set; }

        public string Description { get; set; } //slobodan tekst



        // public string BusinessPartner { get; set; }

        //public string InvoiceType { get; set; }

        //public decimal Quantity { get; set; }

        //public DateTime TrafficDate { get; set; }

        //public decimal Price { get; set; }
        //public decimal Rebate { get; set; }
        //public decimal RebateValue { get; set; }
        //public decimal Base { get; set; }
        //public decimal PDV { get; set; }
        //public decimal Total { get; set; }


    }
}
