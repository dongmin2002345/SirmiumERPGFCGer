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
    public class BusinessPartnerLocationController : Controller
    {
        IBusinessPartnerLocationService businessPartnerLocationService { get; set; }

        public BusinessPartnerLocationController(IServiceProvider provider)
        {
            businessPartnerLocationService = provider.GetRequiredService<IBusinessPartnerLocationService>();

        }

        [HttpGet]
        public JsonResult GetBusinessPartnerLocations(int CompanyId)
        {
            BusinessPartnerLocationListResponse response = new BusinessPartnerLocationListResponse();
            try
            {
                response = businessPartnerLocationService.GetBusinessPartnerLocations(CompanyId);
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
        public JsonResult GetBusinessPartnerLocationsNewerThen(int CompanyId, DateTime? lastUpdateTime)
        {
            BusinessPartnerLocationListResponse response = new BusinessPartnerLocationListResponse();
            try
            {
                response = businessPartnerLocationService.GetBusinessPartnerLocationsNewerThen(CompanyId, lastUpdateTime);
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
        public JsonResult Create([FromBody] BusinessPartnerLocationViewModel c)
        {
            BusinessPartnerLocationResponse response = new BusinessPartnerLocationResponse();
            try
            {
                response = this.businessPartnerLocationService.Create(c);
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
        public JsonResult Delete([FromBody] BusinessPartnerLocationViewModel c)
        {
            BusinessPartnerLocationResponse response = new BusinessPartnerLocationResponse();
            try
            {
                response = this.businessPartnerLocationService.Delete(c.Identifier);
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
        public JsonResult Sync([FromBody] SyncBusinessPartnerLocationRequest request)
        {
            BusinessPartnerLocationListResponse response = new BusinessPartnerLocationListResponse();
            try
            {
                response = this.businessPartnerLocationService.Sync(request);
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