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
    public class PhysicalPersonCardController : Controller
    {
        IPhysicalPersonCardService PhysicalPersonCardService { get; set; }

        public PhysicalPersonCardController(IServiceProvider provider)
        {
            PhysicalPersonCardService = provider.GetRequiredService<IPhysicalPersonCardService>();

        }

        [HttpPost]
        public JsonResult Sync([FromBody] SyncPhysicalPersonCardRequest request)
        {
            PhysicalPersonCardListResponse response = new PhysicalPersonCardListResponse();
            try
            {
                response = this.PhysicalPersonCardService.Sync(request);
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