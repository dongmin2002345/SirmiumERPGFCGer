using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Common.InputInvoices;
using ServiceInterfaces.Messages.Common.InputInvoices;

namespace SirmiumERPWeb.Controllers.InputInvoice
{
    public class InputInvoiceNoteController : Controller
    {
        IInputInvoiceNoteService InputInvoiceNoteService { get; set; }

        public InputInvoiceNoteController(IServiceProvider provider)
        {
            InputInvoiceNoteService = provider.GetRequiredService<IInputInvoiceNoteService>();

        }

        [HttpPost]
        public JsonResult Sync([FromBody] SyncInputInvoiceNoteRequest request)
        {
            InputInvoiceNoteListResponse response = new InputInvoiceNoteListResponse();
            try
            {
                response = this.InputInvoiceNoteService.Sync(request);
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