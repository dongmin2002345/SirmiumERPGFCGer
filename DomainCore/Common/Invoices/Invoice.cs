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
        public string InvoiceNumber { get; set; }
        public string Code { get; set; }

        public DateTime InvoiceDate { get; set; }
        public DateTime DateOfSupplyOfGoods { get; set; }

        public DateTime DueDate { get; set; }


        public int? BusinessPartnerId { get; set; }
        public BusinessPartner BusinessPartner { get; set; }
        //BusinessPartner fields
        public string Customer { get; set; }
        public string PIB { get; set; }
        public string BPName { get; set; }
        public string Address { get; set; }
        public int? CityId { get; set; }
        public City City { get; set; }
        public int? MunicipalityId { get; set; }
        public Municipality Municipality { get; set; }
        public DateTime Currency { get; set; }
        public int? DiscountId { get; set; }
        public Discount Discount { get; set; }
        public int? VatId { get; set; }
        public Vat Vat { get; set; }
        public bool IsInPDV { get; set; }

        public int? PdvType { get; set; }
    }
}
