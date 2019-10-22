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
        public string BusinessPartnerName { get; set; }
        public string Supplier { get; set; }
        public string Address { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string AmountNet { get; set; }
        public string PDVPercent { get; set; }
        public string PDV { get; set; }
        public string AmountGros { get; set; }
        public string Currency { get; set; }
        public string DateOfPaymetse { get; set; }
        public string Statuse { get; set; }
        public string StatusDate { get; set; }
    }
}
