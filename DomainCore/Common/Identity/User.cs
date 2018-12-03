using DomainCore.Base;
using DomainCore.Common.Companies;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.Identity
{
    public class User
    {
        public int Id { get; set; }
        public Guid Identifier { get; set; }

        public string Code { get; set; }
        public string Username { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string PasswordHash { get; set; }

        public string Email { get; set; }

        public bool Active { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<CompanyUser> CompanyUsers { get; set; }
    }
}
