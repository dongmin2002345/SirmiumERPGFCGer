using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.RdlcReports.Employees
{
    public class EmployeeReportViewModel
    {
        public string OrderNumberForEmployee { get; set; }
        public string EmployeeCode { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string CountryName { get; set; }
        
        public string RegionName { get; set; }
        public string MunicipalityName { get; set; }
        public string CityName { get; set; }
        public string Address { get; set; }
        public string ConstructionSiteCode { get; set; }

        //Podaci o pasosu
        public string PassportCountryName { get; set; }
        public string PassportCityName { get; set; }
        public string Passport { get; set; }
        public string VisaFrom { get; set; }
        public string VisaTo { get; set; }

        //podaci o prebivalistu
        public string ResidenceCountryName { get; set; }
        public string ResidenceCityName { get; set; }
        public string ResidenceAddress { get; set; }

        //Podaci o radnoj dozvoli
        public string EmbassyDate { get; set; }
        public string VisaDate { get; set; }
        public string VisaValidFrom { get; set; }
        public string VisaValidTo { get; set; }
        public string WorkPermitFrom { get; set; }
        public string WorkPermitTo { get; set; }

        //Zanimanja
        public int OrderNumberProfession { get; set; }
        public string ProfessionCountryName { get; set; }
        public string ProfessionName { get; set; }

        //Stavke dozvole
        public int OrderNumberLicence { get; set; }
        public string LicenceCountryName { get; set; }
        public string LicenceCode { get; set; }
        public string LicenceValidFrom { get; set; }
        public string LicenceValidTo { get; set; }

        //Napomena
        public int OrderNumberNote { get; set; }
        public string Note { get; set; }
        public string NoteDAte { get; set; }

        //Spajanje porodica
        public int OrderNumberFamilyMember { get; set; }
        public string FamilyMemberName { get; set; }
        public string ItemName { get; set; }
        public string ItemsDateOfBirth { get; set; }
        public string ItemPassport { get; set; }
        public string ItemEmbassyDate { get; set; }

        //Istorija radnika
        public int OrderNumberCard { get; set; }
        public string CardDescription { get; set; }
        public string CardDate { get; set; }
    }
}
