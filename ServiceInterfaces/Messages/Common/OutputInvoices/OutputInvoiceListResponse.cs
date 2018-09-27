using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.OutputInvoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.Common.OutputInvoices
{
    public class OutputInvoiceListResponse : BaseResponse
    {
        public List<OutputInvoiceViewModel> OutputInvoices { get; set; }
        public List<OutputInvoiceViewModel> OutputInvoicesByPage { get; set; }
        public int TotalItems { get; set; }
    }
}
