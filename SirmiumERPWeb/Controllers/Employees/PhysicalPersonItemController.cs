using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;

namespace SirmiumERPWeb.Controllers.Employees
{
    public class PhysicalPersonItemController : Controller
    {
        IPhysicalPersonItemService PhysicalPersonItemService { get; set; }

        public PhysicalPersonItemController(IServiceProvider provider)
        {
            PhysicalPersonItemService = provider.GetRequiredService<IPhysicalPersonItemService>();

        }

        [HttpPost]
        public JsonResult Sync([FromBody] SyncPhysicalPersonItemRequest request)
        {
            PhysicalPersonItemListResponse response = new PhysicalPersonItemListResponse();
            try
            {
                response = this.PhysicalPersonItemService.Sync(request);
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
