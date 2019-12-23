using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.RdlcReports.Invoices
{
    public class InvoiceViewModel
    {
        public string InvoiceNumber { get; set; }
        public string CityAndInvoiceDate { get; set; }
        public string DeliveryDateOfGoodsAndServices { get; set; }
        public string DueDate { get; set; }

        public string BusinessPartnerCode { get; set; }
        public string BusinessPartnerPIB { get; set; }
        public string BusinessPartnerMB { get; set; }
        public string BusinessPartnerName { get; set; }
        public string BusinessPartnerAddress { get; set; }
        public string BusinessPartnerCity { get; set; }

        public string Amount { get; set; }

        public string TotalBase { get; set; }
        public string TotalPDV { get; set; }
        public string AmountInCurrency { get; set; }
        
        public string TotalAmount { get; set; }
        
        public string TotalAmountInCurrency { get; set; }

        public string TotalAmountInText { get; set; }
    }
}
