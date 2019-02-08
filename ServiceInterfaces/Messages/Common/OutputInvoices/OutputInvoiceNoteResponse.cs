using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.OutputInvoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.Common.OutputInvoices
{
    public class OutputInvoiceNoteResponse : BaseResponse
    {
        public OutputInvoiceNoteViewModel OutputInvoiceNote { get; set; }
    }
}
