using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Common.Invoices;
using ServiceInterfaces.Messages.Common.Invoices;
using ServiceInterfaces.ViewModels.Common.Invoices;

namespace SirmiumERPWeb.Controllers.Invoices
{
    public class InvoiceController : Controller
    {
        IInvoiceService invoiceService { get; set; }

        public InvoiceController(IServiceProvider provider)
        {
            invoiceService = provider.GetRequiredService<IInvoiceService>();
        }

        [HttpGet]
        public JsonResult GetInvoices(int companyId)
        {
            InvoiceListResponse response = new InvoiceListResponse();
            try
            {
                response = invoiceService.GetInvoices(companyId);
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
        public JsonResult GetInvoicesNewerThan(int companyId, DateTime? lastUpdateTime)
        {
            InvoiceListResponse response;
            try
            {
                response = invoiceService.GetInvoicesNewerThan(companyId, lastUpdateTime);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Create([FromBody] InvoiceViewModel c)
        {
            InvoiceResponse response = new InvoiceResponse();
            try
            {
                response = this.invoiceService.Create(c);
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
        public JsonResult Delete([FromBody]InvoiceViewModel invoice)
        {
            InvoiceResponse response = new InvoiceResponse();
            try
            {
                response = this.invoiceService.Delete(invoice.Identifier);
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
        public JsonResult Sync([FromBody] SyncInvoiceRequest request)
        {
            InvoiceListResponse response = new InvoiceListResponse();
            try
            {
                response = this.invoiceService.Sync(request);
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