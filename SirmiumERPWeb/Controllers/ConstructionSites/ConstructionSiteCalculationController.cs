using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.ConstructionSites;
using ServiceInterfaces.Messages.ConstructionSites;
using ServiceInterfaces.ViewModels.ConstructionSites;

namespace SirmiumERPWeb.Controllers.ConstructionSites
{
    public class ConstructionSiteCalculationController : Controller
    {
        IConstructionSiteCalculationService ConstructionSiteCalculationService { get; set; }

        public ConstructionSiteCalculationController(IServiceProvider provider)
        {
            ConstructionSiteCalculationService = provider.GetRequiredService<IConstructionSiteCalculationService>();

        }

        [HttpGet]
        public JsonResult GetConstructionSiteCalculations(int CompanyId)
        {
            ConstructionSiteCalculationListResponse response = new ConstructionSiteCalculationListResponse();
            try
            {
                response = ConstructionSiteCalculationService.GetConstructionSiteCalculations(CompanyId);
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
        public JsonResult GetConstructionSiteCalculationsNewerThen(int CompanyId, DateTime? lastUpdateTime)
        {
            ConstructionSiteCalculationListResponse response = new ConstructionSiteCalculationListResponse();
            try
            {
                response = ConstructionSiteCalculationService.GetConstructionSiteCalculationsNewerThen(CompanyId, lastUpdateTime);
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
        public JsonResult Create([FromBody] ConstructionSiteCalculationViewModel c)
        {
            ConstructionSiteCalculationResponse response = new ConstructionSiteCalculationResponse();
            try
            {
                response = this.ConstructionSiteCalculationService.Create(c);
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
        public JsonResult Sync([FromBody] SyncConstructionSiteCalculationRequest request)
        {
            ConstructionSiteCalculationListResponse response = new ConstructionSiteCalculationListResponse();
            try
            {
                response = this.ConstructionSiteCalculationService.Sync(request);
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