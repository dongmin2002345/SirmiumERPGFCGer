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
            : base(0, string.Empty, string.Empty, Guid.Empty, string.Empty, new List<String>())
        { }
    }
}
