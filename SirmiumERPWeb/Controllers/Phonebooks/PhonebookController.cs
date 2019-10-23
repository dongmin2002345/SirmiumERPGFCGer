using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Common.Phonebook;
using ServiceInterfaces.Messages.Common.Phonebooks;
using ServiceInterfaces.ViewModels.Common.Phonebooks;

namespace SirmiumERPWeb.Controllers.Phonebooks
{
    public class PhonebookController : Controller
    {
        IPhonebookService phonebookService { get; set; }

        public PhonebookController(IServiceProvider provider)
        {
            phonebookService = provider.GetRequiredService<IPhonebookService>();
        }

        [HttpGet]
        public JsonResult GetPhonebooks(int companyId)
        {
            PhonebookListResponse response = new PhonebookListResponse();
            try
            {
                response = phonebookService.GetPhonebooks(companyId);
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
        public JsonResult GetPhonebooksNewerThan(int companyId, DateTime? lastUpdateTime)
        {
            PhonebookListResponse response;
            try
            {
                response = phonebookService.GetPhonebooksNewerThen(companyId, lastUpdateTime);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Create([FromBody] PhonebookViewModel c)
        {
            PhonebookResponse response = new PhonebookResponse();
            try
            {
                response = this.phonebookService.Create(c);
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
        public JsonResult Delete([FromBody]PhonebookViewModel phonebook)
        {
            PhonebookResponse response = new PhonebookResponse();
            try
            {
                response = this.phonebookService.Delete(phonebook.Identifier);
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
        public JsonResult Sync([FromBody] SyncPhonebookRequest request)
        {
            PhonebookListResponse response = new PhonebookListResponse();
            try
            {
                response = this.phonebookService.Sync(request);
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