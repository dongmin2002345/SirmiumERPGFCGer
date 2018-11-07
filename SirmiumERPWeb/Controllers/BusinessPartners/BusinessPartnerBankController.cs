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
    public class BusinessPartnerBankController : Controller
    {
        IBusinessPartnerBankService businessPartnerBankService { get; set; }

        public BusinessPartnerBankController(IServiceProvider provider)
        {
            businessPartnerBankService = provider.GetRequiredService<IBusinessPartnerBankService>();

        }

        [HttpGet]
        public JsonResult GetBusinessPartnerBanks(int CompanyId)
        {
            BusinessPartnerBankListResponse response = new BusinessPartnerBankListResponse();
            try
            {
                response = businessPartnerBankService.GetBusinessPartnerBanks(CompanyId);
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
        public JsonResult GetBusinessPartnerBanksNewerThen(int CompanyId, DateTime? lastUpdateTime)
        {
            BusinessPartnerBankListResponse response = new BusinessPartnerBankListResponse();
            try
            {
                response = businessPartnerBankService.GetBusinessPartnerBanksNewerThen(CompanyId, lastUpdateTime);
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
        public JsonResult Create([FromBody] BusinessPartnerBankViewModel c)
        {
            BusinessPartnerBankResponse response = new BusinessPartnerBankResponse();
            try
            {
                response = this.businessPartnerBankService.Create(c);
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
        public JsonResult Delete([FromBody] BusinessPartnerBankViewModel c)
        {
            BusinessPartnerBankResponse response = new BusinessPartnerBankResponse();
            try
            {
                response = this.businessPartnerBankService.Delete(c.Identifier);
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
        public JsonResult Sync([FromBody] SyncBusinessPartnerBankRequest request)
        {
            BusinessPartnerBankListResponse response = new BusinessPartnerBankListResponse();
            try
            {
                response = this.businessPartnerBankService.Sync(request);
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