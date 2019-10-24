using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.Invoices;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.Common.Invoices
{
    public class InvoiceListResponse : BaseResponse
    {
        public List<InvoiceViewModel> Invoices { get; set; }
        public int TotalItems { get; set; }
    }
}
