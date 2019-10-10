using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.OutputInvoices
{
	public class OutputInvoiceDocument : BaseEntity
	{
		public int? OutputInvoiceId { get; set; }
		public OutputInvoice OutputInvoice { get; set; }

		public string Name { get; set; }
		public DateTime? CreateDate { get; set; }
		public string Path { get; set; }

        public int ItemStatus { get; set; }
    }
}
