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
    public class LicenceTypeController : Controller
    {
		ILicenceTypeService licenceTypeService { get; set; }

		public LicenceTypeController(IServiceProvider provider)
		{
			licenceTypeService = provider.GetRequiredService<ILicenceTypeService>();
		}

		[HttpGet]
		public JsonResult GetLicenceTypes(int companyId)
		{
			LicenceTypeListResponse response = new LicenceTypeListResponse();
			try
			{
				response = licenceTypeService.GetLicenceTypes(companyId);
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
		public JsonResult GetLicenceTypesNewerThen(int companyId, DateTime? lastUpdateTime)
		{
			LicenceTypeListResponse response;
			try
			{
				response = licenceTypeService.GetLicenceTypesNewerThen(companyId, lastUpdateTime);
			}
			catch (Exception ex)
			{
				response = null;
				Console.WriteLine(ex.Message);
			}
			return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
		}

		[HttpPost]
		public JsonResult Create([FromBody] LicenceTypeViewModel c)
		{
			LicenceTypeResponse response = new LicenceTypeResponse();
			try
			{
				response = this.licenceTypeService.Create(c);
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
		public JsonResult Delete([FromBody]LicenceTypeViewModel licenceType)
		{
			LicenceTypeResponse response = new LicenceTypeResponse();
			try
			{
				response = this.licenceTypeService.Delete(licenceType.Identifier);
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
		public JsonResult Sync([FromBody] SyncLicenceTypeRequest request)
		{
			LicenceTypeListResponse response = new LicenceTypeListResponse();
			try
			{
				response = this.licenceTypeService.Sync(request);
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