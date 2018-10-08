using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Common.Sectors;
using ServiceInterfaces.Messages.Common.Sectors;
using ServiceInterfaces.ViewModels.Common.Sectors;

namespace SirmiumERPWeb.Controllers.Sectors
{
    public class SectorController : Controller
    {
		ISectorService sectorService { get; set; }

		public SectorController(IServiceProvider provider)
		{
			sectorService = provider.GetRequiredService<ISectorService>();
		}

		[HttpGet]
		public JsonResult GetSectors(int companyId)
		{
			SectorListResponse response = new SectorListResponse();
			try
			{
				response = sectorService.GetSectors(companyId);
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
		public JsonResult GetSectorsNewerThen(int companyId, DateTime? lastUpdateTime)
		{
			SectorListResponse response;
			try
			{
				response = sectorService.GetSectorsNewerThen(companyId, lastUpdateTime);
			}
			catch (Exception ex)
			{
				response = null;
				Console.WriteLine(ex.Message);
			}
			return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
		}

		[HttpPost]
		public JsonResult Create([FromBody] SectorViewModel c)
		{
			SectorResponse response = new SectorResponse();
			try
			{
				response = this.sectorService.Create(c);
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
		public JsonResult Delete([FromBody]SectorViewModel sector)
		{
			SectorResponse response = new SectorResponse();
			try
			{
				response = this.sectorService.Delete(sector.Identifier);
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
		public JsonResult Sync([FromBody] SyncSectorRequest request)
		{
			SectorListResponse response = new SectorListResponse();
			try
			{
				response = this.sectorService.Sync(request);
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