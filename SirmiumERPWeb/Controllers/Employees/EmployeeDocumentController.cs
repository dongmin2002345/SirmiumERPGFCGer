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
    public class EmployeeDocumentController : Controller
    {
        IEmployeeDocumentService EmployeeDocumentService { get; set; }

        public EmployeeDocumentController(IServiceProvider provider)
        {
            EmployeeDocumentService = provider.GetRequiredService<IEmployeeDocumentService>();

        }

        [HttpPost]
        public JsonResult Sync([FromBody] SyncEmployeeDocumentRequest request)
        {
            EmployeeDocumentListResponse response = new EmployeeDocumentListResponse();
            try
            {
                response = this.EmployeeDocumentService.Sync(request);
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