using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.Identity
{
    public class User : BaseEntity
    {
        public string Username { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string PasswordHash { get; set; }

        public string Email { get; set; }
    }
}
