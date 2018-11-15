using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Common.InputInvoices;
using ServiceInterfaces.Messages.Common.InputInvoices;
using ServiceInterfaces.ViewModels.Common.InputInvoices;

namespace SirmiumERPWeb.Controllers.InputInvoice
{
    public class InputInvoiceController : Controller
    {
		IInputInvoiceService inputInvoiceService { get; set; }

		public InputInvoiceController(IServiceProvider provider)
		{
			inputInvoiceService = provider.GetRequiredService<IInputInvoiceService>();
		}

		[HttpGet]
		public JsonResult GetInputInvoices(int companyId)
		{
			InputInvoiceListResponse response = new InputInvoiceListResponse();
			try
			{
				response = inputInvoiceService.GetInputInvoices(companyId);
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
				Console.WriteLine(ex.Message);
			}
			return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
		}

		[HttpGet]
		public JsonResult GetInputInvoicesNewerThan(int companyId, DateTime? lastUpdateTime)
		{
			InputInvoiceListResponse response;
			try
			{
				response = inputInvoiceService.GetInputInvoicesNewerThan(companyId, lastUpdateTime);
			}
			catch (Exception ex)
			{
				response = null;
				Console.WriteLine(ex.Message);
			}
			return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
		}

		[HttpPost]
		public JsonResult Create([FromBody] InputInvoiceViewModel c)
		{
			InputInvoiceResponse response = new InputInvoiceResponse();
			try
			{
				response = this.inputInvoiceService.Create(c);
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
				Console.WriteLine(ex.Message);
			}

			return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
		}

		[HttpPost]
		public JsonResult Delete([FromBody]InputInvoiceViewModel inputInvoice)
		{
			InputInvoiceResponse response = new InputInvoiceResponse();
			try
			{
				response = this.inputInvoiceService.Delete(inputInvoice.Identifier);
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
				Console.WriteLine(ex.Message);
			}

			return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
		}

		[HttpPost]
		public JsonResult Sync([FromBody] SyncInputInvoiceRequest request)
		{
			InputInvoiceListResponse response = new InputInvoiceListResponse();
			try
			{
				response = this.inputInvoiceService.Sync(request);
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