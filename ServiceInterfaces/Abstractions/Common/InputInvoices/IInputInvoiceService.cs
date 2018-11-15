using ServiceInterfaces.Messages.Common.InputInvoices;
using ServiceInterfaces.ViewModels.Common.InputInvoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Common.InputInvoices
{
	public interface IInputInvoiceService
	{
		InputInvoiceListResponse GetInputInvoices(int companyId);
		InputInvoiceListResponse GetInputInvoicesNewerThan(int companyId, DateTime? lastUpdateTime);

		InputInvoiceResponse Create(InputInvoiceViewModel inputInvoice);
		InputInvoiceResponse Delete(Guid identifier);

		InputInvoiceListResponse Sync(SyncInputInvoiceRequest request);
	}
}
