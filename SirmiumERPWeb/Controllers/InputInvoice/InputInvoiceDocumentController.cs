using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Common.InputInvoices;
using ServiceInterfaces.Messages.Common.InputInvoices;

namespace SirmiumERPWeb.Controllers.InputInvoice
{
    public class InputInvoiceDocumentController : Controller
    {
		IInputInvoiceDocumentService InputInvoiceDocumentService { get; set; }

		public InputInvoiceDocumentController(IServiceProvider provider)
		{
			InputInvoiceDocumentService = provider.GetRequiredService<IInputInvoiceDocumentService>();

		}

		[HttpPost]
		public JsonResult Sync([FromBody] SyncInputInvoiceDocumentRequest request)
		{
			InputInvoiceDocumentListResponse response = new InputInvoiceDocumentListResponse();
			try
			{
				response = this.InputInvoiceDocumentService.Sync(request);
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