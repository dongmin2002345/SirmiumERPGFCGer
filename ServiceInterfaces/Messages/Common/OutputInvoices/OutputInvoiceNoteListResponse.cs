using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.OutputInvoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.Common.OutputInvoices
{
    public class OutputInvoiceNoteListResponse : BaseResponse
    {
        public List<OutputInvoiceNoteViewModel> OutputInvoiceNotes { get; set; }
        public int TotalItems { get; set; }
    }
}
