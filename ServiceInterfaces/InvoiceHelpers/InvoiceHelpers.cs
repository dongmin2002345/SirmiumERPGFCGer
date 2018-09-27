using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.InvoiceHelpers
{
    public enum InvoiceHelpers : int
    {
        Active = 0,
        Locked = 1
    }

    public enum InvoiceTypes : int
    {
        All = 0,
        OutputInvoice = 1
    }
}

