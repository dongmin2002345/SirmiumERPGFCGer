using DomainCore.Common.Invoices;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.Invoices
{
    public interface IInvoiceRepository
    {
        List<Invoice> GetInvoices(int companyId);
        List<Invoice> GetInvoicesNewerThan(int companyId, DateTime lastUpdateTime);

        Invoice Create(Invoice invoice);
        Invoice Delete(Guid identifier);
    }
}
