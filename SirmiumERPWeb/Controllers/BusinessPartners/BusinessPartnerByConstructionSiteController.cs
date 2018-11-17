using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;

namespace SirmiumERPWeb.Controllers.BusinessPartners
{
    public class BusinessPartnerByConstructionSiteController : Controller
    {
        IBusinessPartnerByConstructionSiteService employeeByConstructionSiteService { get; set; }

        public BusinessPartnerByConstructionSiteController(IServiceProvider provider)
        {
            employeeByConstructionSiteService = provider.GetRequiredService<IBusinessPartnerByConstructionSiteService>();
        }

        [HttpGet]
        public JsonResult GetBusinessPartnerByConstructionSites(int companyId)
        {
            BusinessPartnerByConstructionSiteListResponse response = new BusinessPartnerByConstructionSiteListResponse();
            try
            {
                response = employeeByConstructionSiteService.GetBusinessPartnerByConstructionSites(companyId);
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
        public JsonResult GetBusinessPartnerByConstructionSitesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            BusinessPartnerByConstructionSiteListResponse response;
            try
            {
                response = employeeByConstructionSiteService.GetBusinessPartnerByConstructionSitesNewerThen(companyId, lastUpdateTime);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Create([FromBody] BusinessPartnerByConstructionSiteViewModel c)
        {
            BusinessPartnerByConstructionSiteResponse response = new BusinessPartnerByConstructionSiteResponse();
            try
            {
                response = this.employeeByConstructionSiteService.Create(c);
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
        public JsonResult Delete([FromBody]BusinessPartnerByConstructionSiteViewModel employeeByConstructionSite)
        {
            BusinessPartnerByConstructionSiteResponse response = new BusinessPartnerByConstructionSiteResponse();
            try
            {
                response = this.employeeByConstructionSiteService.Delete(employeeByConstructionSite);
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
        public JsonResult Sync([FromBody] SyncBusinessPartnerByConstructionSiteRequest request)
        {
            BusinessPartnerByConstructionSiteListResponse response = new BusinessPartnerByConstructionSiteListResponse();
            try
            {
                response = this.employeeByConstructionSiteService.Sync(request);
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