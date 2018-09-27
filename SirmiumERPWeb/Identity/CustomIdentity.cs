using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace SirmiumERPWeb.Identity
{
    public class CustomIdentity : IIdentity
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public int CompanyId { get; private set; }
        public string CompanyName { get; private set; }
        public List<String> Roles { get; private set; }

        public CustomIdentity(int id, string name, string email, int companyId, string companyName, List<String> roles)
        {
            Id = id;
            Name = name;
            Email = email;
            CompanyId = companyId;
            CompanyName = companyName;
            Roles = roles;
        }

        public string AuthenticationType { get { return "Custom authentication"; } }

        public bool IsAuthenticated { get { return !string.IsNullOrEmpty(Name); } }
    }
}
