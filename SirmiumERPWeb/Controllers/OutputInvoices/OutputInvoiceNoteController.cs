using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Common.OutputInvoices;
using ServiceInterfaces.Messages.Common.OutputInvoices;

namespace SirmiumERPWeb.Controllers.OutputInvoices
{
    public class OutputInvoiceNoteController : Controller
    {
        IOutputInvoiceNoteService OutputInvoiceNoteService { get; set; }

        public OutputInvoiceNoteController(IServiceProvider provider)
        {
            OutputInvoiceNoteService = provider.GetRequiredService<IOutputInvoiceNoteService>();

        }

        [HttpPost]
        public JsonResult Sync([FromBody] SyncOutputInvoiceNoteRequest request)
        {
            OutputInvoiceNoteListResponse response = new OutputInvoiceNoteListResponse();
            try
            {
                response = this.OutputInvoiceNoteService.Sync(request);
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