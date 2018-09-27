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
    public class BusinessPartnerController : Controller
    {
        IBusinessPartnerService businessPartnerService { get; set; }

        public BusinessPartnerController(IServiceProvider provider)
        {
            businessPartnerService = provider.GetRequiredService<IBusinessPartnerService>();

        }

        [HttpGet]
        public JsonResult GetBusinessPartners()
        {
            BusinessPartnerListResponse response = new BusinessPartnerListResponse();
            try
            {
                response = businessPartnerService.GetBusinessPartners();
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
        public JsonResult GetBusinessPartnersNewerThen(DateTime? lastUpdateTime)
        {
            BusinessPartnerListResponse response;
            try
            {
                response = businessPartnerService.GetBusinessPartnersNewerThen(lastUpdateTime);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }


        [HttpPost]
        public JsonResult Create([FromBody] BusinessPartnerViewModel c)
        {
            BusinessPartnerResponse response = new BusinessPartnerResponse();
            try
            {
                response = this.businessPartnerService.Create(c);
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
        public JsonResult Delete([FromBody] BusinessPartnerViewModel c)
        {
            BusinessPartnerResponse response = new BusinessPartnerResponse();
            try
            {
                response = this.businessPartnerService.Delete(c.Identifier);
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
        public JsonResult Sync([FromBody] SyncBusinessPartnerRequest request)
        {
            BusinessPartnerListResponse response = new BusinessPartnerListResponse();
            try
            {
                response = this.businessPartnerService.Sync(request);
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