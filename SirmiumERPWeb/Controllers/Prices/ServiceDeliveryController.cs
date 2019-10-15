using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Common.Prices;
using ServiceInterfaces.Messages.Common.Prices;
using ServiceInterfaces.ViewModels.Common.Prices;

namespace SirmiumERPWeb.Controllers.Prices
{
    public class ServiceDeliveryController : Controller
    {
        IServiceDeliveryService serviceDeliveryService { get; set; }

        public ServiceDeliveryController(IServiceProvider provider)
        {
            serviceDeliveryService = provider.GetRequiredService<IServiceDeliveryService>();
        }

        [HttpGet]
        public JsonResult GetServiceDeliverys(int companyId)
        {
            ServiceDeliveryListResponse response = new ServiceDeliveryListResponse();
            try
            {
                response = serviceDeliveryService.GetServiceDeliverys(companyId);
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
        public JsonResult Create([FromBody] ServiceDeliveryViewModel c)
        {
            ServiceDeliveryResponse response = new ServiceDeliveryResponse();
            try
            {
                response = this.serviceDeliveryService.Create(c);
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
        public JsonResult Delete([FromBody]ServiceDeliveryViewModel serviceDelivery)
        {
            ServiceDeliveryResponse response = new ServiceDeliveryResponse();
            try
            {
                response = this.serviceDeliveryService.Delete(serviceDelivery.Identifier);
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
        public JsonResult Sync([FromBody] SyncServiceDeliveryRequest request)
        {
            ServiceDeliveryListResponse response = new ServiceDeliveryListResponse();
            try
            {
                response = this.serviceDeliveryService.Sync(request);
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