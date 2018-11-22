using DomainCore.Common.OutputInvoices;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.Invoices
{
    public interface IOutputInvoiceRepository
    {
        List<OutputInvoice> GetOutputInvoices(int companyId);
        List<OutputInvoice> GetOutputInvoicesNewerThan(int companyId, DateTime lastUpdateTime);

        OutputInvoice Create(OutputInvoice outputInvoice);
        OutputInvoice Delete(Guid identifier);

    }
}
