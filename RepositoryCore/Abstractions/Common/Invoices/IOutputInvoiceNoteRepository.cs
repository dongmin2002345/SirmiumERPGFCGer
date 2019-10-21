using DomainCore.Common.OutputInvoices;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.Invoices
{
    public interface IOutputInvoiceNoteRepository
    {
        List<OutputInvoiceNote> GetOutputInvoiceNotes(int companyId);
        List<OutputInvoiceNote> GetOutputInvoiceNotesNewerThen(int companyId, DateTime lastUpdateTime);

        OutputInvoiceNote Create(OutputInvoiceNote OutputInvoiceNote);
        OutputInvoiceNote Delete(Guid identifier);
    }
}
