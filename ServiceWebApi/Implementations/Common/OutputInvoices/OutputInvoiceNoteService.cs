using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Common.OutputInvoices;
using ServiceInterfaces.Messages.Common.OutputInvoices;
using ServiceInterfaces.ViewModels.Common.OutputInvoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Common.OutputInvoices
{
    public class OutputInvoiceNoteService : IOutputInvoiceNoteService
    {
        public OutputInvoiceNoteListResponse Sync(SyncOutputInvoiceNoteRequest request)
        {
            OutputInvoiceNoteListResponse response = new OutputInvoiceNoteListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncOutputInvoiceNoteRequest, OutputInvoiceNoteViewModel, OutputInvoiceNoteListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.OutputInvoiceNotes = new List<OutputInvoiceNoteViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
