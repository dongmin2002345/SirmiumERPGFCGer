using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Common.InputInvoices;
using ServiceInterfaces.Messages.Common.InputInvoices;
using ServiceInterfaces.ViewModels.Common.InputInvoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Common.InputInvoices
{
	public class InputInvoiceDocumentService : IInputInvoiceDocumentService
	{
		public InputInvoiceDocumentListResponse Sync(SyncInputInvoiceDocumentRequest request)
		{
			InputInvoiceDocumentListResponse response = new InputInvoiceDocumentListResponse();
			try
			{
				response = WpfApiHandler.SendToApi<SyncInputInvoiceDocumentRequest, InputInvoiceDocumentViewModel, InputInvoiceDocumentListResponse>(request, "Sync");
			}
			catch (Exception ex)
			{
				response.InputInvoiceDocuments = new List<InputInvoiceDocumentViewModel>();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}
	}
}
