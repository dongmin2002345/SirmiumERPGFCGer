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
    public class PhonebookNoteController : Controller
    {
        IPhonebookNoteService PhonebookNoteService { get; set; }

        public PhonebookNoteController(IServiceProvider provider)
        {
            PhonebookNoteService = provider.GetRequiredService<IPhonebookNoteService>();

        }

        [HttpPost]
        public JsonResult Sync([FromBody] SyncPhonebookNoteRequest request)
        {
            PhonebookNoteListResponse response = new PhonebookNoteListResponse();
            try
            {
                response = this.PhonebookNoteService.Sync(request);
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