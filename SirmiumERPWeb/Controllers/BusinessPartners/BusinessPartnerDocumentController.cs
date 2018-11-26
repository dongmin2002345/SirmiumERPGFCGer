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
    public class BusinessPartnerDocumentController : Controller
    {
        IBusinessPartnerDocumentService BusinessPartnerDocumentService { get; set; }

        public BusinessPartnerDocumentController(IServiceProvider provider)
        {
            BusinessPartnerDocumentService = provider.GetRequiredService<IBusinessPartnerDocumentService>();

        }

        [HttpPost]
        public JsonResult Sync([FromBody] SyncBusinessPartnerDocumentRequest request)
        {
            BusinessPartnerDocumentListResponse response = new BusinessPartnerDocumentListResponse();
            try
            {
                response = this.BusinessPartnerDocumentService.Sync(request);
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