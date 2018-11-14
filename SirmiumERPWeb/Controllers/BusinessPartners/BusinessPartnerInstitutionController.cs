using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Messages.Common.BusinessPartners;

namespace SirmiumERPWeb.Controllers.BusinessPartners
{
    public class BusinessPartnerInstitutionController : Controller
    {
        IBusinessPartnerInstitutionService businessPartnerInstitutionService { get; set; }

        public BusinessPartnerInstitutionController(IServiceProvider provider)
        {
            businessPartnerInstitutionService = provider.GetRequiredService<IBusinessPartnerInstitutionService>();
        }

        [HttpPost]
        public JsonResult Sync([FromBody] SyncBusinessPartnerInstitutionRequest request)
        {
            BusinessPartnerInstitutionListResponse response = new BusinessPartnerInstitutionListResponse();
            try
            {
                response = this.businessPartnerInstitutionService.Sync(request);
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