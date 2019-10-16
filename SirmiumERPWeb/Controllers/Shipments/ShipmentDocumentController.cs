using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Common.Shipments;
using ServiceInterfaces.Messages.Common.Shipments;

namespace SirmiumERPWeb.Controllers.Shipments
{
    public class ShipmentDocumentController : Controller
    {
        IShipmentDocumentService ShipmentDocumentService { get; set; }

        public ShipmentDocumentController(IServiceProvider provider)
        {
            ShipmentDocumentService = provider.GetRequiredService<IShipmentDocumentService>();

        }

        [HttpPost]
        public JsonResult Sync([FromBody] SyncShipmentDocumentRequest request)
        {
            ShipmentDocumentListResponse response = new ShipmentDocumentListResponse();
            try
            {
                response = this.ShipmentDocumentService.Sync(request);
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