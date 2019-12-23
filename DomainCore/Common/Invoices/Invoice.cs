using DomainCore.Base;
using DomainCore.Common.BusinessPartners;
using DomainCore.Common.Locations;
using DomainCore.Common.Prices;
using DomainCore.Vats;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.Invoices
{
    public class Invoice : BaseEntity
    {
        public string Code { get; set; }
        public string InvoiceNumber { get; set; }

        public int? BuyerId { get; set; }
        public BusinessPartner Buyer { get; set; }

        public string BuyerName { get; set; }
        public string Address { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? DateOfPayment { get; set; }
        public int Status { get; set; }
        public DateTime StatusDate { get; set; }
        public string Description { get; set; }
        public string CurrencyCode { get; set; }
        public double? CurrencyExchangeRate { get; set; }
        
        public int? CityId { get; set; }
        public City City { get; set; }

        public int? MunicipalityId { get; set; }
        public Municipality Municipality { get; set; }
        
        public int? VatId { get; set; }
        public Vat Vat { get; set; }

        public int? DiscountId { get; set; }
        public Discount Discount { get; set; }

        public int? PdvType { get; set; }

        public double TotalPrice { get; set; }
        public double TotalPDV { get; set; }
        public double TotalRebate { get; set; }

        public List<InvoiceItem> InvoiceItems { get; set; }
    }
}
