using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Common.Locations;
using ServiceInterfaces.Messages.Common.Locations;
using ServiceInterfaces.ViewModels.Common.Locations;

namespace SirmiumERPWeb.Controllers.Locations
{
    public class RegionController : Controller
    {
        IRegionService regionService { get; set; }

        public RegionController(IServiceProvider provider)
        {
            regionService = provider.GetRequiredService<IRegionService>();
        }

        [HttpGet]
        public JsonResult GetRegions(int companyId)
        {
            RegionListResponse response = new RegionListResponse();
            try
            {
                response = regionService.GetRegions(companyId);
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
        public JsonResult GetRegionsNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            RegionListResponse response;
            try
            {
                response = regionService.GetRegionsNewerThen(companyId, lastUpdateTime);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Create([FromBody] RegionViewModel c)
        {
            RegionResponse response = new RegionResponse();
            try
            {
                response = this.regionService.Create(c);
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
        public JsonResult Delete([FromBody]RegionViewModel region)
        {
            RegionResponse response = new RegionResponse();
            try
            {
                response = this.regionService.Delete(region.Identifier);
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
        public JsonResult Sync([FromBody] SyncRegionRequest request)
        {
            RegionListResponse response = new RegionListResponse();
            try
            {
                response = this.regionService.Sync(request);
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