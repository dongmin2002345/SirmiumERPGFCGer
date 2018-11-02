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
    public class BusinessPartnerByConstructionSiteHistoryController : Controller
    {
        IBusinessPartnerByConstructionSiteHistoryService businessPartnerByConstructionSiteHistoryService { get; set; }

        public BusinessPartnerByConstructionSiteHistoryController(IServiceProvider provider)
        {
            businessPartnerByConstructionSiteHistoryService = provider.GetRequiredService<IBusinessPartnerByConstructionSiteHistoryService>();
        }

        [HttpGet]
        public JsonResult GetBusinessPartnerByConstructionSiteHistories(int companyId)
        {
            BusinessPartnerByConstructionSiteHistoryListResponse response = new BusinessPartnerByConstructionSiteHistoryListResponse();
            try
            {
                response = businessPartnerByConstructionSiteHistoryService.GetBusinessPartnerByConstructionSiteHistories(companyId);
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
        public JsonResult GetBusinessPartnerByConstructionSiteHistoriesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            BusinessPartnerByConstructionSiteHistoryListResponse response;
            try
            {
                response = businessPartnerByConstructionSiteHistoryService.GetBusinessPartnerByConstructionSiteHistoriesNewerThen(companyId, lastUpdateTime);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Create([FromBody] BusinessPartnerByConstructionSiteHistoryViewModel c)
        {
            BusinessPartnerByConstructionSiteHistoryResponse response = new BusinessPartnerByConstructionSiteHistoryResponse();
            try
            {
                response = this.businessPartnerByConstructionSiteHistoryService.Create(c);
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
        public JsonResult Delete([FromBody]BusinessPartnerByConstructionSiteHistoryViewModel businessPartnerByConstructionSiteHistory)
        {
            BusinessPartnerByConstructionSiteHistoryResponse response = new BusinessPartnerByConstructionSiteHistoryResponse();
            try
            {
                response = this.businessPartnerByConstructionSiteHistoryService.Delete(businessPartnerByConstructionSiteHistory.Identifier);
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
        public JsonResult Sync([FromBody] SyncBusinessPartnerByConstructionSiteHistoryRequest request)
        {
            BusinessPartnerByConstructionSiteHistoryListResponse response = new BusinessPartnerByConstructionSiteHistoryListResponse();
            try
            {
                response = this.businessPartnerByConstructionSiteHistoryService.Sync(request);
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