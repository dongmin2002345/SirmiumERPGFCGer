using DomainCore.Common.OutputInvoices;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.Invoices
{
    public interface IOutputInvoiceRepository
    {
        List<OutputInvoice> GetOutputInvoicesByPages(int currentPage = 1, int itemsPerPage = 50, string filterString = "");
        int GetOutputInvoicesCount(string filterString = "");

        List<OutputInvoice> GetOutputInvoicesForPopup(string filterString);

        OutputInvoice GetOutputInvoice(int id);

        OutputInvoice CancelOutputInvoice(int id);

        int GetNewCodeValue();

        OutputInvoice SetInvoiceStatus(int id, int status);
        OutputInvoice SetInvoiceLock(int id, bool locked);

        OutputInvoice Create(OutputInvoice outputinvoice);
        OutputInvoice Update(OutputInvoice outputinvoice);
        OutputInvoice Delete(int id);
    }
}
