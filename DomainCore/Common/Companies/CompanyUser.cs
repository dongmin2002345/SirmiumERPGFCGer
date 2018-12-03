using DomainCore.Common.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.Companies
{
    public class CompanyUser
    {
        public int Id { get; set; }
        public Guid Identifier { get; set; }

        public int? CompanyId { get; set; }
        public Company Company { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }

        public string RolesCSV { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool Active { get; set; }
    }
}
