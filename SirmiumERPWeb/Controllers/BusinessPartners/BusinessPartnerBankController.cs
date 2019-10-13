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