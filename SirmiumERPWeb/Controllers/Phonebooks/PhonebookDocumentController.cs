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
    public class PhonebookDocumentController : Controller
    {
        IPhonebookDocumentService PhonebookDocumentService { get; set; }

        public PhonebookDocumentController(IServiceProvider provider)
        {
            PhonebookDocumentService = provider.GetRequiredService<IPhonebookDocumentService>();

        }

        [HttpPost]
        public JsonResult Sync([FromBody] SyncPhonebookDocumentRequest request)
        {
            PhonebookDocumentListResponse response = new PhonebookDocumentListResponse();
            try
            {
                response = this.PhonebookDocumentService.Sync(request);
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