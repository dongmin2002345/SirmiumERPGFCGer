﻿using ServiceInterfaces.ViewModels.ConstructionSites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.RdlcReports.ConstructionSites
{
    public class ConstructionSiteReportViewModel
    {
        //public string OrderNumberForConstructionSite { get; set; }

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

        //Notes
        public int NoteOrderNumber { get; set; }
        public string NoteName { get; set; }
        public string NoteDate { get; set; }

        //Documents
        public int DocumentOrderNumber { get; set; }
        public string DocumentName { get; set; }
        public string DocumentDate { get; set; }
    }
}
