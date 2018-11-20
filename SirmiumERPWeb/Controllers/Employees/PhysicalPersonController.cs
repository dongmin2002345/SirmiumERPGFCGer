using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;

namespace SirmiumERPWeb.Controllers.Employees
{
    public class PhysicalPersonController : Controller
    {
		IPhysicalPersonService PhysicalPersonService { get; set; }

		public PhysicalPersonController(IServiceProvider provider)
		{
			PhysicalPersonService = provider.GetRequiredService<IPhysicalPersonService>();
		}

		[HttpGet]
		public JsonResult GetPhysicalPersons(int companyId)
		{
			PhysicalPersonListResponse response;
			try
			{
				response = PhysicalPersonService.GetPhysicalPersons(companyId);
			}
			catch (Exception ex)
			{
				response = null;
				Console.WriteLine(ex.Message);
			}
			return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
		}

		[HttpGet]
		public JsonResult GetPhysicalPersonsNewerThen(int companyId, DateTime? lastUpdateTime)
		{
			PhysicalPersonListResponse response;
			try
			{
				response = PhysicalPersonService.GetPhysicalPersonsNewerThen(companyId, lastUpdateTime);
			}
			catch (Exception ex)
			{
				response = null;
				Console.WriteLine(ex.Message);
			}
			return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
		}

		[HttpPost]
		public JsonResult Create([FromBody] PhysicalPersonViewModel c)
		{
			PhysicalPersonResponse response;
			try
			{
				response = this.PhysicalPersonService.Create(c);
			}
			catch (Exception ex)
			{
				response = null;
				Console.WriteLine(ex.Message);
			}

			return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
		}

		[HttpPost]
		public JsonResult Delete([FromBody] Guid identifier)
		{
			PhysicalPersonResponse response;
			try
			{
				response = this.PhysicalPersonService.Delete(identifier);
			}
			catch (Exception ex)
			{
				response = null;
				Console.WriteLine(ex.Message);
			}

			return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
		}

		[HttpPost]
		public JsonResult Sync([FromBody] SyncPhysicalPersonRequest request)
		{
			PhysicalPersonListResponse response = new PhysicalPersonListResponse();
			try
			{
				response = this.PhysicalPersonService.Sync(request);
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