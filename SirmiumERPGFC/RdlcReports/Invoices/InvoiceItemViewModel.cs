using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.RdlcReports.Invoices
{
    public class InvoiceItemViewModel
    {
        public string Id { get; set; }
        public string ProductCode{ get; set; }
        public string ProductName{ get; set; }
        public string UnitOfMeasurement{ get; set; }
        public string Quantity{ get; set; }
        public string PriceWithoutPDV{ get; set; }
        public string PriceWithPDV{ get; set; }
        public string Rebate { get; set; }
        public string PDVPercent{ get; set; }
        public string PDVValue{ get; set; }
        public string Amount { get; set; }
    }
}
