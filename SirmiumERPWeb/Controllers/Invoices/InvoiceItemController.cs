using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Common.Invoices;
using ServiceInterfaces.Messages.Common.Invoices;

namespace SirmiumERPWeb.Controllers.Invoices
{
    public class InvoiceItemController : Controller
    {
        IInvoiceItemService InvoiceItemService { get; set; }

        public InvoiceItemController(IServiceProvider provider)
        {
            InvoiceItemService = provider.GetRequiredService<IInvoiceItemService>();

        }

        [HttpPost]
        public JsonResult Sync([FromBody] SyncInvoiceItemRequest request)
        {
            InvoiceItemListResponse response = new InvoiceItemListResponse();
            try
            {
                response = this.InvoiceItemService.Sync(request);
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