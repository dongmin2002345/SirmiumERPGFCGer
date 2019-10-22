using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.RdlcReports.Employees
{
    public class EmployeesReportViewModel
    {
        public int OrderNumbersForEmployees { get; set; }
        public string EmployeeCode { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string ConstructionSiteCode { get; set; }
        public string ConstructionSiteName { get; set; }
        public string DateOfBirth { get; set; }
        public string Passport { get; set; }
        public string ResidenceCountryName { get; set; }
        public string ResidenceCityName { get; set; }
        public string ResidenceAddress { get; set; }
        
    }
}
