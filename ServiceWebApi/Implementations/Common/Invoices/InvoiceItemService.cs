using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Common.Invoices;
using ServiceInterfaces.Messages.Common.Invoices;
using ServiceInterfaces.ViewModels.Common.Invoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Common.Invoices
{
    public class InvoiceItemService : IInvoiceItemService
    {
        public InvoiceItemListResponse Sync(SyncInvoiceItemRequest request)
        {
            InvoiceItemListResponse response = new InvoiceItemListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncInvoiceItemRequest, InvoiceItemViewModel, InvoiceItemListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.InvoiceItems = new List<InvoiceItemViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}

