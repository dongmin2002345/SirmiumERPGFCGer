using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.RdlcReports.BusinessPartners
{
    public class BusinessPartnersReportViewModel
    {
        public int OrderNumbersForBusinessPartners { get; set; }
        public string InternalCode { get; set; }
        public string Name { get; set; }
        public string NameGer { get; set; }
        public string TaxNr { get; set; }
        public string Valuta { get; set; }
        public string PDV { get; set; }
        public string PIO { get; set; }
        public string Customer { get; set; }
        public string VatDescription { get; set; }
        public string DiscountName { get; set; }
        public string IsInPDV { get; set; }

    }
}
