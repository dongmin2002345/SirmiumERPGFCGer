using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Common.Shipments;
using ServiceInterfaces.Messages.Common.Shipments;
using ServiceInterfaces.ViewModels.Common.Shipments;

namespace SirmiumERPWeb.Controllers.Shipments
{
    public class ShipmentController : Controller
    {
        IShipmentService shipmentService { get; set; }

        public ShipmentController(IServiceProvider provider)
        {
            shipmentService = provider.GetRequiredService<IShipmentService>();
        }

        [HttpGet]
        public JsonResult GetShipments(int companyId)
        {
            ShipmentListResponse response = new ShipmentListResponse();
            try
            {
                response = shipmentService.GetShipments(companyId);
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
        public JsonResult GetShipmentsNewerThan(int companyId, DateTime? lastUpdateTime)
        {
            ShipmentListResponse response;
            try
            {
                response = shipmentService.GetShipmentsNewerThan(companyId, lastUpdateTime);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Create([FromBody] ShipmentViewModel c)
        {
            ShipmentResponse response = new ShipmentResponse();
            try
            {
                response = this.shipmentService.Create(c);
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
        public JsonResult Delete([FromBody]ShipmentViewModel shipment)
        {
            ShipmentResponse response = new ShipmentResponse();
            try
            {
                response = this.shipmentService.Delete(shipment.Identifier);
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
        public JsonResult Sync([FromBody] SyncShipmentRequest request)
        {
            ShipmentListResponse response = new ShipmentListResponse();
            try
            {
                response = this.shipmentService.Sync(request);
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