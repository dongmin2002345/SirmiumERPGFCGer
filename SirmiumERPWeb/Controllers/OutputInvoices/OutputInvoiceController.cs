using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Common.OutputInvoices;
using ServiceInterfaces.Messages.Common.OutputInvoices;
using ServiceInterfaces.ViewModels.Common.OutputInvoices;

namespace SirmiumERPWeb.Controllers.OutputInvoices
{
    //[Route("api/[controller]")]
    public class OutputInvoiceController : Controller
    {
        IOutputInvoiceService outputInvoiceService { get; set; }

        public OutputInvoiceController(IServiceProvider provider)
        {
            outputInvoiceService = provider.GetRequiredService<IOutputInvoiceService>();
        }

        [HttpGet]
        public JsonResult GetOutputInvoices(int companyId)
        {
            OutputInvoiceListResponse response = new OutputInvoiceListResponse();
            try
            {
                response = outputInvoiceService.GetOutputInvoices(companyId);
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
        public JsonResult GetOutputInvoicesNewerThan(int companyId, DateTime? lastUpdateTime)
        {
            OutputInvoiceListResponse response;
            try
            {
                response = outputInvoiceService.GetOutputInvoicesNewerThan(companyId, lastUpdateTime);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Create([FromBody] OutputInvoiceViewModel c)
        {
            OutputInvoiceResponse response = new OutputInvoiceResponse();
            try
            {
                response = this.outputInvoiceService.Create(c);
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
        public JsonResult Delete([FromBody]OutputInvoiceViewModel outputInvoice)
        {
            OutputInvoiceResponse response = new OutputInvoiceResponse();
            try
            {
                response = this.outputInvoiceService.Delete(outputInvoice.Identifier);
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
        public JsonResult Sync([FromBody] SyncOutputInvoiceRequest request)
        {
            OutputInvoiceListResponse response = new OutputInvoiceListResponse();
            try
            {
                response = this.outputInvoiceService.Sync(request);
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