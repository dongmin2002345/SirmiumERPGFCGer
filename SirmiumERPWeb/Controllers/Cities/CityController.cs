using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceCore.Implementations.Common.Cities;
using ServiceInterfaces.Abstractions.Common.Cities;
using ServiceInterfaces.Messages.Common.Cities;
using ServiceInterfaces.ViewModels.Common.Cities;

namespace SirmiumERPWeb.Controllers.Cities
{
    public class CityController : Controller
    {
        ICityService cityService;

        public CityController(IServiceProvider provider)
        {
            cityService = provider.GetRequiredService<ICityService>();
        }

        [HttpGet]
        public JsonResult GetCities()
        {
            CityListResponse response = new CityListResponse();
            try
            {
                response = cityService.GetCities();
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
        public JsonResult GetCitysNewerThen(DateTime? lastUpdateTime)
        {
            CityListResponse response = new CityListResponse();
            try
            {
                response = cityService.GetCitiesNewerThen(lastUpdateTime);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
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
        public JsonResult Delete([FromBody] CityViewModel c)
        {
            CityResponse response = new CityResponse();
            try
            {
                response = this.cityService.Delete(c.Identifier);
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