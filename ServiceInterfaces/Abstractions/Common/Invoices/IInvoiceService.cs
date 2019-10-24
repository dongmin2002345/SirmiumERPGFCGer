using ServiceInterfaces.Messages.Common.Invoices;
using ServiceInterfaces.ViewModels.Common.Invoices;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Abstractions.Common.Invoices
{
    public interface IInvoiceService
    {
        InvoiceListResponse GetInvoices(int companyId);
        InvoiceListResponse GetInvoicesNewerThan(int companyId, DateTime? lastUpdateTime);

        InvoiceResponse Create(InvoiceViewModel outputInvoice);
        InvoiceResponse Delete(Guid identifier);

        InvoiceListResponse Sync(SyncInvoiceRequest request);
    }
}
