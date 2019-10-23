using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.RdlcReports.ConstructionSites
{
    public class ConstructionSitesReportViewModel
    {
        public int OrderNumbersForConstructionSites { get; set; }
        public string ConstructionSiteCode { get; set; }
        public string InternalCode { get; set; }

        public string Name { get; set; }

        public string CityName { get; set; }

        public string CountryName { get; set; }

        public string BusinessPartnerName { get; set; }

        public string StatusName { get; set; }

        public string Address { get; set; }

        public string MaxWorkers { get; set; }

        public string ProContractDate { get; set; }
        public string ContractStart { get; set; }
        public string ContractExpiration { get; set; }
    }
}
