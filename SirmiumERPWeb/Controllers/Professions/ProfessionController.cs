using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Common.Professions;
using ServiceInterfaces.Messages.Common.Professions;
using ServiceInterfaces.ViewModels.Common.Professions;

namespace SirmiumERPWeb.Controllers.Professions
{
    public class ProfessionController : Controller
    {
        IProfessionService professionService { get; set; }

        public ProfessionController(IServiceProvider provider)
        {
            professionService = provider.GetRequiredService<IProfessionService>();
        }

        [HttpGet]
        public JsonResult GetProfessions(int companyId)
        {
            ProfessionListResponse response = new ProfessionListResponse();
            try
            {
                response = professionService.GetProfessions(companyId);
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
        public JsonResult GetProfessionsNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            ProfessionListResponse response;
            try
            {
                response = professionService.GetProfessionsNewerThen(companyId, lastUpdateTime);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Create([FromBody] ProfessionViewModel c)
        {
            ProfessionResponse response = new ProfessionResponse();
            try
            {
                response = this.professionService.Create(c);
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
        public JsonResult Delete([FromBody]ProfessionViewModel profession)
        {
            ProfessionResponse response = new ProfessionResponse();
            try
            {
                response = this.professionService.Delete(profession.Identifier);
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
        public JsonResult Sync([FromBody] SyncProfessionRequest request)
        {
            ProfessionListResponse response = new ProfessionListResponse();
            try
            {
                response = this.professionService.Sync(request);
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