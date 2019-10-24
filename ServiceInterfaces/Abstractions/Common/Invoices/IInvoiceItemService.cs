using ServiceInterfaces.Messages.Common.Invoices;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Abstractions.Common.Invoices
{
    public interface IInvoiceItemService
    {
        InvoiceItemListResponse Sync(SyncInvoiceItemRequest request);
    }
}
