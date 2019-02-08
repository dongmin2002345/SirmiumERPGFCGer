using DomainCore.Common.InputInvoices;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.Invoices
{
    public interface IInputInvoiceNoteRepository
    {
        List<InputInvoiceNote> GetInputInvoiceNotes(int companyId);
        List<InputInvoiceNote> GetInputInvoiceNotesByInputInvoice(int InputInvoiceId);
        List<InputInvoiceNote> GetInputInvoiceNotesNewerThen(int companyId, DateTime lastUpdateTime);

        InputInvoiceNote Create(InputInvoiceNote InputInvoiceNote);
        InputInvoiceNote Delete(Guid identifier);
    }
}
