using ServiceInterfaces.ViewModels.Common.InputInvoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.Common.InputInvoices
{
	public class SyncInputInvoiceRequest
	{
		public int CompanyId { get; set; }
		public DateTime? LastUpdatedAt { get; set; }
		//public List<InputInvoiceViewModel> UnSyncedInputInvoices { get; set; }
	}
}
