using DomainCore.Base;
using DomainCore.Common.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.Companies
{
    public class Company : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public string Address { get; set; }

        public string IdentificationNumber { get; set; }
        public string PIBNumber { get; set; }
        public string PIONumber { get; set; }
        public string PDVNumber { get; set; }

        public string IndustryCode { get; set; }

        public string IndustryName { get; set; }

        public string Email { get; set; }

        public string WebSite { get; set; }

        public List<CompanyUser> CompanyUsers { get; set; }
    }
}
