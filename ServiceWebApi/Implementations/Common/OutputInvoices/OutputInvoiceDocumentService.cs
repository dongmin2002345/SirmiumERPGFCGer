using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Common.OutputInvoices;
using ServiceInterfaces.Messages.Common.OutputInvoices;
using ServiceInterfaces.ViewModels.Common.OutputInvoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Common.OutputInvoices
{
	public class OutputInvoiceDocumentService : IOutputInvoiceDocumentService
	{
		public OutputInvoiceDocumentListResponse Sync(SyncOutputInvoiceDocumentRequest request)
		{
			OutputInvoiceDocumentListResponse response = new OutputInvoiceDocumentListResponse();
			try
			{
				response = WpfApiHandler.SendToApi<SyncOutputInvoiceDocumentRequest, OutputInvoiceDocumentViewModel, OutputInvoiceDocumentListResponse>(request, "Sync");
			}
			catch (Exception ex)
			{
				response.OutputInvoiceDocuments = new List<OutputInvoiceDocumentViewModel>();
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}
	}
}
