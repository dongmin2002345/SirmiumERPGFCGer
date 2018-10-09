using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Common.Sectors;
using ServiceInterfaces.Messages.Common.Sectors;
using ServiceInterfaces.ViewModels.Common.Sectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SirmiumERPWeb.Controllers.Sectors
{
    public class AgencyController : Controller
    {
        IAgencyService AgencyService { get; set; }

        public AgencyController(IServiceProvider provider)
        {
            AgencyService = provider.GetRequiredService<IAgencyService>();
        }

        [HttpGet]
        public JsonResult GetAgencies(int companyId)
        {
            AgencyListResponse response = new AgencyListResponse();
            try
            {
                response = AgencyService.GetAgencies(companyId);
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
        public JsonResult GetAgenciesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            AgencyListResponse response;
            try
            {
                response = AgencyService.GetAgenciesNewerThen(companyId, lastUpdateTime);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Create([FromBody] AgencyViewModel c)
        {
            AgencyResponse response = new AgencyResponse();
            try
            {
                response = this.AgencyService.Create(c);
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
        public JsonResult Delete([FromBody]AgencyViewModel remedy)
        {
            AgencyResponse response = new AgencyResponse();
            try
            {
                response = this.AgencyService.Delete(remedy.Identifier);
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
        public JsonResult Sync([FromBody] SyncAgencyRequest request)
        {
            AgencyListResponse response = new AgencyListResponse();
            try
            {
                response = this.AgencyService.Sync(request);
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
