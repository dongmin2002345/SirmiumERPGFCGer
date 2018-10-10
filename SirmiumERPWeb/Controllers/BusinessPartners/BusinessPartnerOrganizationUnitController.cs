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
    public class BusinessPartnerOrganizationUnitController : Controller
    {
        IBusinessPartnerOrganizationUnitService businessPartnerOrganizationUnitService { get; set; }

        public BusinessPartnerOrganizationUnitController(IServiceProvider provider)
        {
            businessPartnerOrganizationUnitService = provider.GetRequiredService<IBusinessPartnerOrganizationUnitService>();

        }

        [HttpGet]
        public JsonResult GetBusinessPartnerOrganizationUnits(int CompanyId)
        {
            BusinessPartnerOrganizationUnitListResponse response = new BusinessPartnerOrganizationUnitListResponse();
            try
            {
                response = businessPartnerOrganizationUnitService.GetBusinessPartnerOrganizationUnits(CompanyId);
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
        public JsonResult GetBusinessPartnerOrganizationUnitsNewerThen(int CompanyId, DateTime? lastUpdateTime)
        {
            BusinessPartnerOrganizationUnitListResponse response = new BusinessPartnerOrganizationUnitListResponse();
            try
            {
                response = businessPartnerOrganizationUnitService.GetBusinessPartnerOrganizationUnitsNewerThen(CompanyId, lastUpdateTime);
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
        public JsonResult Create([FromBody] BusinessPartnerOrganizationUnitViewModel c)
        {
            BusinessPartnerOrganizationUnitResponse response = new BusinessPartnerOrganizationUnitResponse();
            try
            {
                response = this.businessPartnerOrganizationUnitService.Create(c);
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
        public JsonResult Delete([FromBody] BusinessPartnerOrganizationUnitViewModel c)
        {
            BusinessPartnerOrganizationUnitResponse response = new BusinessPartnerOrganizationUnitResponse();
            try
            {
                response = this.businessPartnerOrganizationUnitService.Delete(c.Identifier);
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
        public JsonResult Sync([FromBody] SyncBusinessPartnerOrganizationUnitRequest request)
        {
            BusinessPartnerOrganizationUnitListResponse response = new BusinessPartnerOrganizationUnitListResponse();
            try
            {
                response = this.businessPartnerOrganizationUnitService.Sync(request);
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