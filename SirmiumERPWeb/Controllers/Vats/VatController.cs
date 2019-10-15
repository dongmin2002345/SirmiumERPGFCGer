using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Vats;
using ServiceInterfaces.Messages.Vats;
using ServiceInterfaces.ViewModels.Vats;

namespace SirmiumERPWeb.Controllers.Vats
{
    public class VatController : Controller
    {
        IVatService VatService { get; set; }

        public VatController(IServiceProvider provider)
        {
            VatService = provider.GetRequiredService<IVatService>();
        }

        [HttpGet]
        public JsonResult GetVats(int companyId)
        {
            VatListResponse response = new VatListResponse();
            try
            {
                response = VatService.GetVats(companyId);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                Console.WriteLine(ex.Message);
            }

            JsonResult result = Json(response, new Newtonsoft.Json.JsonSerializerSettings()
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize
            });
            return result;
        }

        [HttpPost]
        public JsonResult Create([FromBody] VatViewModel c)
        {
            VatResponse response = new VatResponse();
            try
            {
                response = this.VatService.Create(c);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                Console.WriteLine(ex.Message);
            }

            JsonResult result = Json(response, new Newtonsoft.Json.JsonSerializerSettings()
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize
            });
            return result;
        }

        [HttpPost]
        public JsonResult Delete([FromBody]VatViewModel Vat)
        {
            VatResponse response = new VatResponse();
            try
            {
                response = this.VatService.Delete(Vat.Identifier);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                Console.WriteLine(ex.Message);
            }

            JsonResult result = Json(response, new Newtonsoft.Json.JsonSerializerSettings()
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize
            });
            return result;
        }

        [HttpPost]
        public JsonResult Sync([FromBody] SyncVatRequest request)
        {
            VatListResponse response = new VatListResponse();
            try
            {
                response = this.VatService.Sync(request);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            JsonResult result = Json(response, new Newtonsoft.Json.JsonSerializerSettings()
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize
            });
            return result;
        }
    }
}
