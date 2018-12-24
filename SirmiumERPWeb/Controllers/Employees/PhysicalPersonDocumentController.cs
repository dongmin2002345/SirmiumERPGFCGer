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
    public class PhysicalPersonDocumentController : Controller
    {
        IPhysicalPersonDocumentService PhysicalPersonDocumentService { get; set; }

        public PhysicalPersonDocumentController(IServiceProvider provider)
        {
            PhysicalPersonDocumentService = provider.GetRequiredService<IPhysicalPersonDocumentService>();

        }

        [HttpPost]
        public JsonResult Sync([FromBody] SyncPhysicalPersonDocumentRequest request)
        {
            PhysicalPersonDocumentListResponse response = new PhysicalPersonDocumentListResponse();
            try
            {
                response = this.PhysicalPersonDocumentService.Sync(request);
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