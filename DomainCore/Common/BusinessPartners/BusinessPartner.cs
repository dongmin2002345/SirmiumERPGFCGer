using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.BusinessPartners
{
    public class BusinessPartner : BaseEntity
    {
        public int Code { get; set; }
        public string Name { get; set; }

        public string Director { get; set; }

        public string Address { get; set; }
        public string InoAddress { get; set; }

        public string PIB { get; set; }
        public string MatCode { get; set; }

        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public string ActivityCode { get; set; }

        public string BankAccountNumber { get; set; }

        public DateTime OpeningDate { get; set; }
        public DateTime? BranchOpeningDate { get; set; }

    }
}
