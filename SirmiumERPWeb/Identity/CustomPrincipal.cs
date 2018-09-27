using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace SirmiumERPWeb.Identity
{
    public class CustomPrincipal : IPrincipal
    {
        private CustomIdentity identity;

        public CustomIdentity Identity
        {
            get { return identity ?? new AnonymousIdentity(); }
            set { identity = value; }
        }

        IIdentity IPrincipal.Identity
        {
            get { return this.Identity; }
        }

        public bool IsInRole(string role)
        {
            return Identity.Roles.Contains(role);
        }
    }
}

