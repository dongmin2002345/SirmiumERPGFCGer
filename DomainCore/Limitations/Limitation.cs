using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Limitations
{
    public class Limitation : BaseEntity
    {
        public int ConstructionSiteLimit { get; set; }
        public int BusinessPartnerConstructionSiteLimit { get; set; }
        public int EmployeeConstructionSiteLimit { get; set; }
        public int EmployeeBusinessPartnerLimit { get; set; }
        public int EmployeeBirthdayLimit { get; set; }

        public int EmployeePassportLimit { get; set; }
        public int EmployeeEmbasyLimit { get; set; }
        public int EmployeeVisaTakeOffLimit { get; set; }
        public int EmployeeVisaLimit { get; set; }
        public int EmployeeWorkLicenceLimit { get; set; }
        public int EmployeeDriveLicenceLimit { get; set; }
        public int EmployeeEmbasyFamilyLimit { get; set; }

        public int PersonPassportLimit { get; set; }
        public int PersonEmbasyLimit { get; set; }
        public int PersonVisaTakeOffLimit { get; set; }
        public int PersonVisaLimit { get; set; }
        public int PersonWorkLicenceLimit { get; set; }
        public int PersonDriveLicenceLimit { get; set; }
        public int PersonEmbasyFamilyLimit { get; set; }
    }
}
