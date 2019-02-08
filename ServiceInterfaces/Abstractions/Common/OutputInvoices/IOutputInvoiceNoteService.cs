using ServiceInterfaces.Messages.Common.OutputInvoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Common.OutputInvoices
{
    public interface IOutputInvoiceNoteService
    {
        OutputInvoiceNoteListResponse Sync(SyncOutputInvoiceNoteRequest request);
    }
}
