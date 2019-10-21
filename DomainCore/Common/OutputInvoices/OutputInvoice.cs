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
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }

        public decimal AmountNet { get; set; }//neto
        public int PdvPercent { get; set; }
        public decimal Pdv{ get; set; }
        public decimal AmountGross { get; set; } //bruto
        public int Currency { get; set; }

        public DateTime DateOfPayment { get; set; }
        public string Status { get; set; }
        public DateTime StatusDate { get; set; }

        public string Description { get; set; } //slobodan tekst


        public string Path { get; set; }
    }
}
