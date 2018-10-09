using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Banks;
using ServiceInterfaces.Messages.Banks;
using ServiceInterfaces.ViewModels.Banks;

namespace SirmiumERPWeb.Controllers.Banks
{
    public class BankController : Controller
    {
		IBankService bankService { get; set; }

		public BankController(IServiceProvider provider)
		{
			bankService = provider.GetRequiredService<IBankService>();
		}

		[HttpGet]
		public JsonResult GetBanks(int companyId)
		{
			BankListResponse response = new BankListResponse();
			try
			{
				response = bankService.GetBanks(companyId);
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
		public JsonResult GetBanksNewerThen(int companyId, DateTime? lastUpdateTime)
		{
			BankListResponse response;
			try
			{
				response = bankService.GetBanksNewerThen(companyId, lastUpdateTime);
			}
			catch (Exception ex)
			{
				response = null;
				Console.WriteLine(ex.Message);
			}
			return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
		}

		[HttpPost]
		public JsonResult Create([FromBody] BankViewModel c)
		{
			BankResponse response = new BankResponse();
			try
			{
				response = this.bankService.Create(c);
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
		public JsonResult Delete([FromBody]BankViewModel bank)
		{
			BankResponse response = new BankResponse();
			try
			{
				response = this.bankService.Delete(bank.Identifier);
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
		public JsonResult Sync([FromBody] SyncBankRequest request)
		{
			BankListResponse response = new BankListResponse();
			try
			{
				response = this.bankService.Sync(request);
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