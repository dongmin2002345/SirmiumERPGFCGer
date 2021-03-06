﻿using System;
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