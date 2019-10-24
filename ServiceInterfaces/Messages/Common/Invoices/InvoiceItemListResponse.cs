using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.Invoices;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.Common.Invoices
{
    public class InvoiceItemListResponse : BaseResponse
    {
        public List<InvoiceItemViewModel> InvoiceItems { get; set; }
        public int TotalItems { get; set; }
    }
}
