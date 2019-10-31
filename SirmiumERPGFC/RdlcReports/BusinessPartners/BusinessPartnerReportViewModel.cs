using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.RdlcReports.BusinessPartners
{
    public class BusinessPartnerReportViewModel
    {
        //Poslovni partneri srbije
        public string Code { get; set; }
        public string InternalCode { get; set; }
        public string Name { get; set; }
        public string IdentificationNumber { get; set; }
        public string PIB { get; set; }
        public string PIO { get; set; }
        public string VatDescription { get; set; }
        public string DiscountName { get; set; }
        public string DuoDate { get; set; }
        public string IsInPDV { get; set; }
        public string WebSite { get; set; }

        //Poslovni partneri Nemacke
        public string InternalCodeGer { get; set; }
        public string GerName { get; set; }
        public string CountryName { get; set; }
        public string SectorName { get; set; }
        public string AgencyName { get; set; }
        public string TaxNr { get; set; }
        public string IsInPDVGer { get; set; }
        public string BetriebsNumber { get; set; }
        public string CommercialNr { get; set; }
        public string ContactPersonGer { get; set; }
        public string VatDeductionFrom { get; set; }
        public string VatDeductionTo{ get; set; }

        //Telefoni
        public int OrderNumbersForPhones { get; set; }
        public string ContactPersonFirstName { get; set; }
        public string ContactPersonLastName { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Birthday { get; set; }
        public string Description { get; set; }

        //Lokacije
        public int OrderNumbersForLocations { get; set; }
        public string LocationCountryName { get; set; }
        public string LocationRegionName { get; set; }
        public string LocationMunicipalityName { get; set; }
        public string LocationCityName { get; set; }
        public string LocationAddress { get; set; }

        //Banke
        public int OrderNumbersForBanks { get; set; }
        public string BankCountryName { get; set; }
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }

        //Institucije
        public int OrderNumbersForInstitutions { get; set; }
        public string Institution { get; set; }
        public string InstitutionUsername { get; set; }
        public string InstitutionPassword { get; set; }
        public string InstitutionContactPerson { get; set; }
        public string InstitutionPhone { get; set; }
        public string InstitutionFax { get; set; }
        public string InstitutionEmail { get; set; }

        //Dokumenti
        public int OrderNumbersForDocuments { get; set; }
        public string DocumentName { get; set; }
        public string DocumentCreateDate { get; set; }
        public string DocumentPath { get; set; }

        //Napomene
        public int OrderNumbersForNotes { get; set; }
        public string Note { get; set; }
        public string NoteDate { get; set; }

        //Tip poslovnog partner
        public int OrderNumbersForTypes { get; set; }
        public string TypeCode { get; set; }
        public string TypeName { get; set; }
    }
}
