﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Messages.Common.BusinessPartners;

namespace SirmiumERPWeb.Controllers.BusinessPartners
{
    public class BusinessPartnerNoteController : Controller
    {
        IBusinessPartnerNoteService BusinessPartnerNoteService { get; set; }

        public BusinessPartnerNoteController(IServiceProvider provider)
        {
            BusinessPartnerNoteService = provider.GetRequiredService<IBusinessPartnerNoteService>();

        }

        [HttpPost]
        public JsonResult Sync([FromBody] SyncBusinessPartnerNoteRequest request)
        {
            BusinessPartnerNoteListResponse response = new BusinessPartnerNoteListResponse();
            try
            {
                response = this.BusinessPartnerNoteService.Sync(request);
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