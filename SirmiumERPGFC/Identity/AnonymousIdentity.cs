using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Identity
{
    public class AnonymousIdentity : CustomIdentity
    {
        public AnonymousIdentity()
            : base(0, string.Empty, string.Empty, Guid.Empty, string.Empty,
                  0, Guid.Empty, string.Empty, new UserViewModel(), new CompanyUserViewModel())
        { }
    }
}
