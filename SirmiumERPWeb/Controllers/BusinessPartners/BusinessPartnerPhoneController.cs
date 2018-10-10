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
    public class BusinessPartnerPhoneController : Controller
    {
        IBusinessPartnerPhoneService businessPartnerPhoneService { get; set; }

        public BusinessPartnerPhoneController(IServiceProvider provider)
        {
            businessPartnerPhoneService = provider.GetRequiredService<IBusinessPartnerPhoneService>();

        }

        [HttpGet]
        public JsonResult GetBusinessPartnerPhones(int CompanyId)
        {
            BusinessPartnerPhoneListResponse response = new BusinessPartnerPhoneListResponse();
            try
            {
                response = businessPartnerPhoneService.GetBusinessPartnerPhones(CompanyId);
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
        public JsonResult GetBusinessPartnerPhonesNewerThen(int CompanyId, DateTime? lastUpdateTime)
        {
            BusinessPartnerPhoneListResponse response = new BusinessPartnerPhoneListResponse();
            try
            {
                response = businessPartnerPhoneService.GetBusinessPartnerPhonesNewerThen(CompanyId, lastUpdateTime);
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
        public JsonResult Create([FromBody] BusinessPartnerPhoneViewModel c)
        {
            BusinessPartnerPhoneResponse response = new BusinessPartnerPhoneResponse();
            try
            {
                response = this.businessPartnerPhoneService.Create(c);
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
        public JsonResult Delete([FromBody] BusinessPartnerPhoneViewModel c)
        {
            BusinessPartnerPhoneResponse response = new BusinessPartnerPhoneResponse();
            try
            {
                response = this.businessPartnerPhoneService.Delete(c.Identifier);
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
        public JsonResult Sync([FromBody] SyncBusinessPartnerPhoneRequest request)
        {
            BusinessPartnerPhoneListResponse response = new BusinessPartnerPhoneListResponse();
            try
            {
                response = this.businessPartnerPhoneService.Sync(request);
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