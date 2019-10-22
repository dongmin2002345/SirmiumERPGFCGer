using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.RdlcReports.InputInvoices
{
    public class InputInvoicesReportsViewModel
    {
        public string OrderNumbersForInputInvoices { get; set; }
        public string BusinessPartnerNames { get; set; }
        public string Suppliers { get; set; }
        public string Addresss { get; set; }
        public string InvoiceNumbers { get; set; }
        public string InvoiceDates { get; set; }
        public string AmountNets { get; set; }
        public string PDVPercents { get; set; }
        public string PDVs { get; set; }
        public string AmountGross { get; set; }
        public string Currencys { get; set; }
        public string DateOfPaymetses { get; set; }
        public string Statuses { get; set; }
        public string StatusDates { get; set; }
    }
}
