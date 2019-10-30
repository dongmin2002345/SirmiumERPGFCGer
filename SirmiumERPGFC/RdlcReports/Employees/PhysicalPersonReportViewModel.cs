using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.RdlcReports.Employees
{
    public class PhysicalPersonReportViewModel
    {
        public string OrderNumberForPhysicalPerson { get; set; }
        public string PhysicalPersonCode { get; set; }
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
    }
}
