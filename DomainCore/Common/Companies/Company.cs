using DomainCore.Common.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.Companies
{
    public class Company
    {
        public int Id { get; set; }
        public Guid Identifier { get; set; }

        public int Code { get; set; }
        public string Name { get; set; }

        public string Address { get; set; }
        //public City City { get; set; }
        //public Municipality Municipality { get; set; }
        //public Country Country { get; set; }

        public string IdentificationNumber { get; set; }
        public string PIBNumber { get; set; }
        public string PIONumber { get; set; }
        public string PDVNumber { get; set; }

        /// <summary>
        /// Industry code (srb. Sifra delatnosti)
        /// </summary>
        public string IndustryCode { get; set; }

        /// <summary>
        /// Industry name (srb. Naziv delatnosti)
        /// </summary>
        public string IndustryName { get; set; }

        /// <summary>
        /// Bank account number
        /// </summary>
        public string BankAccountNo { get; set; }

        /// <summary>
        /// Bank account name
        /// </summary>
        public string BankAccountName { get; set; }

        /// <summary>
        /// Company email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Company website
        /// </summary>
        public string WebSite { get; set; }

        /// <summary>
        /// User that created or updated entity
        /// </summary>
        public User CreatedBy { get; set; }

        /// <summary>
        /// If we delete some object, we set its state to inactive
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Time when entity was created
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Time when entity was last time updated
        /// </summary>
        public DateTime UpdatedAt { get; set; }


        // Pomocna polja za migraciju baze podataka
        public long? tmpIdLong { get; set; }
        public string tmpIdString { get; set; }
        public long? tmpPropLong { get; set; }
        public string tmpPropString { get; set; }
        public decimal? tmpPropDecimal { get; set; }
    }
}
