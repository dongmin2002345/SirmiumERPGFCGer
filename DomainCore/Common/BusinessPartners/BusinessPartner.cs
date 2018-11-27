using DomainCore.Base;
using DomainCore.Common.Locations;
using DomainCore.Common.Sectors;
using DomainCore.Common.TaxAdministrations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.BusinessPartners
{
    public class BusinessPartner : BaseEntity
    {
        public string Code { get; set; }
        public string InternalCode { get; set; }
        public string Name { get; set; }

        public string PIB { get; set; }
        public string PIO { get; set; }
        public string PDV { get; set; }
        public string IdentificationNumber { get; set; } // Business partner identification number (srb. Maticni broj)

        public decimal Rebate { get; set; }
        public int DueDate { get; set; }

        public string WebSite { get; set; }
        public string ContactPerson { get; set; }

        public bool IsInPDV { get; set; }

        public string JBKJS { get; set; }

        #region GER

        public string NameGer { get; set; }

        public bool IsInPDVGer { get; set; }

        public int? TaxAdministrationId { get; set; }
        public TaxAdministration TaxAdministration { get; set; }

        public string IBAN { get; set; }
        public string BetriebsNumber { get; set; }

        public int? CountryId { get; set; }
        public Country Country { get; set; }
        public int? SectorId { get; set; }
        public Sector Sector { get; set; }
        public int? AgencyId { get; set; }
        public Agency Agency { get; set; }

        public string TaxNr { get; set; }
        public string CommercialNr { get; set; }

        public string ContactPersonGer { get; set; }

        #endregion

        public List<BusinessPartnerLocation> Locations { get; set; }
        public List<BusinessPartnerPhone> Phones { get; set; }
        public List<BusinessPartnerBank> Banks { get; set; }
        public List<BusinessPartnerBusinessPartnerType> BusinessPartnerTypes { get; set; }
    }
}
