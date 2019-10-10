using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.InputInvoices
{
    public class InputInvoiceNote : BaseEntity
    {
        public int? InputInvoiceId { get; set; }
        public InputInvoice InputInvoice { get; set; }

        public string Note { get; set; }
        public DateTime NoteDate { get; set; }

        public int ItemStatus { get; set; }
    }
}
