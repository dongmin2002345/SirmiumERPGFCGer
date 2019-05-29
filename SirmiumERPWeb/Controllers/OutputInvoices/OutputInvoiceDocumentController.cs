using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Common.OutputInvoices;
using ServiceInterfaces.Messages.Common.OutputInvoices;

namespace SirmiumERPWeb.Controllers.OutputInvoices
{
    public class OutputInvoiceDocumentController : Controller
    {
		IOutputInvoiceDocumentService OutputInvoiceDocumentService { get; set; }

		public OutputInvoiceDocumentController(IServiceProvider provider)
		{
			OutputInvoiceDocumentService = provider.GetRequiredService<IOutputInvoiceDocumentService>();

		}

		[HttpPost]
		public JsonResult Sync([FromBody] SyncOutputInvoiceDocumentRequest request)
		{
			OutputInvoiceDocumentListResponse response = new OutputInvoiceDocumentListResponse();
			try
			{
				response = this.OutputInvoiceDocumentService.Sync(request);
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}

			return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
		}
	}
}