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
    public class BusinessPartnerTypeController : Controller
    {
        IBusinessPartnerTypeService businessPartnerTypeService { get; set; }

        public BusinessPartnerTypeController(IServiceProvider provider)
        {
            businessPartnerTypeService = provider.GetRequiredService<IBusinessPartnerTypeService>();
        }

        [HttpGet]
        public JsonResult GetBusinessPartnerTypes(int companyId)
        {
            BusinessPartnerTypeListResponse response = new BusinessPartnerTypeListResponse();
            try
            {
                response = businessPartnerTypeService.GetBusinessPartnerTypes(companyId);
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
        public JsonResult GetBusinessPartnerTypesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            BusinessPartnerTypeListResponse response;
            try
            {
                response = businessPartnerTypeService.GetBusinessPartnerTypesNewerThen(companyId, lastUpdateTime);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Create([FromBody] BusinessPartnerTypeViewModel c)
        {
            BusinessPartnerTypeResponse response = new BusinessPartnerTypeResponse();
            try
            {
                response = this.businessPartnerTypeService.Create(c);
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
        public JsonResult Delete([FromBody]BusinessPartnerTypeViewModel businessPartnerType)
        {
            BusinessPartnerTypeResponse response = new BusinessPartnerTypeResponse();
            try
            {
                response = this.businessPartnerTypeService.Delete(businessPartnerType.Identifier);
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
        public JsonResult Sync([FromBody] SyncBusinessPartnerTypeRequest request)
        {
            BusinessPartnerTypeListResponse response = new BusinessPartnerTypeListResponse();
            try
            {
                response = this.businessPartnerTypeService.Sync(request);
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