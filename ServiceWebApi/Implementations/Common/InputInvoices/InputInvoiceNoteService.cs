using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Common.InputInvoices;
using ServiceInterfaces.Messages.Common.InputInvoices;
using ServiceInterfaces.ViewModels.Common.InputInvoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Common.InputInvoices
{
    public class InputInvoiceNoteService : IInputInvoiceNoteService
    {
        public InputInvoiceNoteListResponse Sync(SyncInputInvoiceNoteRequest request)
        {
            InputInvoiceNoteListResponse response = new InputInvoiceNoteListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncInputInvoiceNoteRequest, InputInvoiceNoteViewModel, InputInvoiceNoteListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.InputInvoiceNotes = new List<InputInvoiceNoteViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
