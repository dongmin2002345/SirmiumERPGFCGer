using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.InputInvoices
{
	public class InputInvoiceDocument : BaseEntity
	{
		public int? InputInvoiceId { get; set; }
		public InputInvoice InputInvoice { get; set; }

		public string Name { get; set; }
		public DateTime? CreateDate { get; set; }
		public string Path { get; set; }
	}
}
