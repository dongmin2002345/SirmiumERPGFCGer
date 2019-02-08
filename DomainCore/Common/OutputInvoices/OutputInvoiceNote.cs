using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.OutputInvoices
{
    public class OutputInvoiceNote : BaseEntity
    {
        public int? OutputInvoiceId { get; set; }
        public OutputInvoice OutputInvoice { get; set; }

        public string Note { get; set; }
        public DateTime NoteDate { get;set; }
    }
}
