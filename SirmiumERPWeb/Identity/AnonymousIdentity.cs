using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SirmiumERPWeb.Identity
{
    public class AnonymousIdentity : CustomIdentity
    {
        public AnonymousIdentity()
            : base(0, string.Empty, string.Empty, 0, string.Empty, new List<String>())
        { }
    }
}
