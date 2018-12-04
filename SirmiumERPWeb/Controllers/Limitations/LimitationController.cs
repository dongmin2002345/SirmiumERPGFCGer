using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Limitations;
using ServiceInterfaces.Messages.Limitations;
using ServiceInterfaces.ViewModels.Limitations;

namespace SirmiumERPWeb.Controllers.Limitations
{
    public class LimitationController : Controller
    {
        ILimitationService limitationService { get; set; }

        public LimitationController(IServiceProvider provider)
        {
            limitationService = provider.GetRequiredService<ILimitationService>();
        }

        [HttpGet]
        public JsonResult GetLimitations(int companyId)
        {
            LimitationListResponse response = new LimitationListResponse();
            try
            {
                response = limitationService.GetLimitations(companyId);
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
        public JsonResult GetLimitationsNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            LimitationListResponse response;
            try
            {
                response = limitationService.GetLimitationsNewerThen(companyId, lastUpdateTime);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Create([FromBody] LimitationViewModel c)
        {
            LimitationResponse response = new LimitationResponse();
            try
            {
                response = this.limitationService.Create(c);
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
        public JsonResult Sync([FromBody] SyncLimitationRequest request)
        {
            LimitationListResponse response = new LimitationListResponse();
            try
            {
                response = this.limitationService.Sync(request);
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