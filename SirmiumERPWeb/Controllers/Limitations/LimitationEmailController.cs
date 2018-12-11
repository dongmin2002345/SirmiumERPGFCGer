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
    public class LimitationEmailController : Controller
    {
        ILimitationEmailService LimitationEmailService { get; set; }

        public LimitationEmailController(IServiceProvider provider)
        {
            LimitationEmailService = provider.GetRequiredService<ILimitationEmailService>();
        }

        [HttpGet]
        public JsonResult GetLimitationEmails(int companyId)
        {
            LimitationEmailListResponse response = new LimitationEmailListResponse();
            try
            {
                response = LimitationEmailService.GetLimitationEmails(companyId);
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
        public JsonResult GetLimitationEmailsNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            LimitationEmailListResponse response;
            try
            {
                response = LimitationEmailService.GetLimitationEmailsNewerThen(companyId, lastUpdateTime);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Create([FromBody] LimitationEmailViewModel c)
        {
            LimitationEmailResponse response = new LimitationEmailResponse();
            try
            {
                response = this.LimitationEmailService.Create(c);
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
        public JsonResult Delete([FromBody]LimitationEmailViewModel LimitationEmail)
        {
            LimitationEmailResponse response = new LimitationEmailResponse();
            try
            {
                response = this.LimitationEmailService.Delete(LimitationEmail.Identifier);
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
        public JsonResult Sync([FromBody] SyncLimitationEmailRequest request)
        {
            LimitationEmailListResponse response = new LimitationEmailListResponse();
            try
            {
                response = this.LimitationEmailService.Sync(request);
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