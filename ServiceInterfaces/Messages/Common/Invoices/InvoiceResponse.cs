using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.Invoices;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.Common.Invoices
{
    public class InvoiceResponse : BaseResponse
    {
        public InvoiceViewModel Invoice { get; set; }
    }
}
