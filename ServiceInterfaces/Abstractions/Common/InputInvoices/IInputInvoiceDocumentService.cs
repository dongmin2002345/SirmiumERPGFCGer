using ServiceInterfaces.Messages.Common.InputInvoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Common.InputInvoices
{
	public interface IInputInvoiceDocumentService
	{
		InputInvoiceDocumentListResponse Sync(SyncInputInvoiceDocumentRequest request);
	}
}
