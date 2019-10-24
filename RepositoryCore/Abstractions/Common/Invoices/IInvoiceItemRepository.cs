using DomainCore.Common.Invoices;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.Invoices
{
    public interface IInvoiceItemRepository
    {
        List<InvoiceItem> GetInvoiceItems(int companyId);
        List<InvoiceItem> GetInvoiceItemsNewerThen(int companyId, DateTime lastUpdateTime);

        InvoiceItem Create(InvoiceItem invoiceItem);
        InvoiceItem Delete(Guid identifier);
    }
}
