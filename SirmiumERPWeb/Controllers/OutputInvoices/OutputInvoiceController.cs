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
        //IOutputInvoiceItemService outputInvoiceItemService { get; set; }

        public OutputInvoiceController(IServiceProvider provider)
        {
            outputInvoiceService = provider.GetRequiredService<IOutputInvoiceService>();
            //outputInvoiceItemService = provider.GetRequiredService<IOutputInvoiceItemService>();
        }

        [HttpGet]
        public JsonResult GetOutputInvoicesByPage(int currentPage = 1, int itemsPerPage = 50, string filterString = "")
        {
            OutputInvoiceListResponse response = new OutputInvoiceListResponse();
            try
            {
                response = outputInvoiceService.GetOutputInvoicesByPage(currentPage, itemsPerPage, filterString);
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
        public JsonResult GetOutputInvoicesForPopup(string filterString)
        {
            OutputInvoiceListResponse response = new OutputInvoiceListResponse();
            try
            {
                response = outputInvoiceService.GetOutputInvoicesForPopup(filterString);
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
        public JsonResult GetOutputInvoice(int id)
        {
            OutputInvoiceResponse response = new OutputInvoiceResponse();
            try
            {
                response = outputInvoiceService.GetOutputInvoice(id);
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
        public JsonResult GetNewCodeValue()
        {
            OutputInvoiceResponse response = new OutputInvoiceResponse();
            try
            {
                response = outputInvoiceService.GetNewCodeValue();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                Console.WriteLine(ex.Message);
            }
            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpGet]
        public JsonResult SetInvoiceLock(int id, bool locked)
        {
            OutputInvoiceResponse response = new OutputInvoiceResponse();
            try
            {
                response = outputInvoiceService.SetInvoiceLock(id, locked);
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
        public JsonResult Update([FromBody] OutputInvoiceViewModel c)
        {
            OutputInvoiceResponse response = new OutputInvoiceResponse();
            try
            {
                response = this.outputInvoiceService.Update(c);
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
        public JsonResult Delete([FromBody]OutputInvoiceViewModel inOI)
        {
            OutputInvoiceResponse response = new OutputInvoiceResponse();
            try
            {
                if (inOI != null && inOI.Id > 0)
                {
                    response = this.outputInvoiceService.Delete(inOI.Id);
                }
                else
                {
                    response.Success = false;
                    string errorMessage = "Prilikom brisanja izlaznog racuna id nije prosledjen.";
                    response.Message = errorMessage;
                    Console.WriteLine(errorMessage);
                }
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
        public JsonResult CancelOutputInvoice([FromBody]OutputInvoiceViewModel inOI)
        {
            OutputInvoiceResponse response = new OutputInvoiceResponse();
            try
            {
                if (inOI != null && inOI.Id > 0)
                {
                    response = this.outputInvoiceService.CancelOutputInvoice(inOI.Id);
                }
                else
                {
                    response.Success = false;
                    string errorMessage = "Prilikom storniranja izlaznog racuna id nije prosledjen.";
                    response.Message = errorMessage;
                    Console.WriteLine(errorMessage);
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                Console.WriteLine(ex.Message);
            }

            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

    }
}