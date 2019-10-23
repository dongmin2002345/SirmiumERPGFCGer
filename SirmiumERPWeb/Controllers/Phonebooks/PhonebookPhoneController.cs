using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Common.Phonebook;
using ServiceInterfaces.Messages.Common.Phonebooks;

namespace SirmiumERPWeb.Controllers.Phonebooks
{
    public class PhonebookPhoneController : Controller
    {
        IPhonebookPhoneService PhonebookPhoneService { get; set; }

        public PhonebookPhoneController(IServiceProvider provider)
        {
            PhonebookPhoneService = provider.GetRequiredService<IPhonebookPhoneService>();

        }

        [HttpPost]
        public JsonResult Sync([FromBody] SyncPhonebookPhoneRequest request)
        {
            PhonebookPhoneListResponse response = new PhonebookPhoneListResponse();
            try
            {
                response = this.PhonebookPhoneService.Sync(request);
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