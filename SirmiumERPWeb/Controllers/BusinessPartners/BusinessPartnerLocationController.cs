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