using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceCore.Implementations.Common.Locations;
using ServiceInterfaces.Abstractions.Common.Locations;
using ServiceInterfaces.Messages.Common.Locations;
using ServiceInterfaces.ViewModels.Common.Locations;

namespace SirmiumERPWeb.Controllers.Locations
{
    public class CityController : Controller
    {
        ICityService cityService { get; set; }

        public CityController(IServiceProvider provider)
        {
            cityService = provider.GetRequiredService<ICityService>();
        }

        [HttpGet]
        public JsonResult GetCities(int companyId)
        {
            CityListResponse response = new CityListResponse();
            try
            {
                response = cityService.GetCities(companyId);
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
        public JsonResult GetCitiesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            CityListResponse response;
            try
            {
                response = cityService.GetCitiesNewerThen(companyId, lastUpdateTime);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Create([FromBody] CityViewModel c)
        {
            CityResponse response = new CityResponse();
            try
            {
                response = this.cityService.Create(c);
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
        public JsonResult Delete([FromBody]CityViewModel remedy)
        {
            CityResponse response = new CityResponse();
            try
            {
                response = this.cityService.Delete(remedy.Identifier);
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
        public JsonResult Sync([FromBody] SyncCityRequest request)
        {
            CityListResponse response = new CityListResponse();
            try
            {
                response = this.cityService.Sync(request);
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