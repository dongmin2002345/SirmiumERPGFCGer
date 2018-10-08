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
    public class MunicipalityController : Controller
    {
        IMunicipalityService MunicipalityService { get; set; }

        public MunicipalityController(IServiceProvider provider)
        {
            MunicipalityService = provider.GetRequiredService<IMunicipalityService>();
        }

        [HttpGet]
        public JsonResult GetMunicipalities(int companyId)
        {
            MunicipalityListResponse response = new MunicipalityListResponse();
            try
            {
                response = MunicipalityService.GetMunicipalities(companyId);
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
        public JsonResult GetMunicipalitiesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            MunicipalityListResponse response;
            try
            {
                response = MunicipalityService.GetMunicipalitiesNewerThen(companyId, lastUpdateTime);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Create([FromBody] MunicipalityViewModel c)
        {
            MunicipalityResponse response = new MunicipalityResponse();
            try
            {
                response = this.MunicipalityService.Create(c);
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
        public JsonResult Delete([FromBody]MunicipalityViewModel Municipality)
        {
            MunicipalityResponse response = new MunicipalityResponse();
            try
            {
                response = this.MunicipalityService.Delete(Municipality.Identifier);
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
        public JsonResult Sync([FromBody] SyncMunicipalityRequest request)
        {
            MunicipalityListResponse response = new MunicipalityListResponse();
            try
            {
                response = this.MunicipalityService.Sync(request);
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
