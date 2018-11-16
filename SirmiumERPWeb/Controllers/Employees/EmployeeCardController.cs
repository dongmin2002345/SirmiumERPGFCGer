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
    public class EmployeeCardController : Controller
    {
        IEmployeeCardService EmployeeCardService { get; set; }

        public EmployeeCardController(IServiceProvider provider)
        {
            EmployeeCardService = provider.GetRequiredService<IEmployeeCardService>();

        }

        [HttpPost]
        public JsonResult Sync([FromBody] SyncEmployeeCardRequest request)
        {
            EmployeeCardListResponse response = new EmployeeCardListResponse();
            try
            {
                response = this.EmployeeCardService.Sync(request);
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