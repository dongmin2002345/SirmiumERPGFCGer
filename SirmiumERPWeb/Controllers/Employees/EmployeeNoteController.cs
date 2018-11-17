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
    public class EmployeeNoteController : Controller
    {
        IEmployeeNoteService EmployeeNoteService { get; set; }

        public EmployeeNoteController(IServiceProvider provider)
        {
            EmployeeNoteService = provider.GetRequiredService<IEmployeeNoteService>();

        }

        [HttpPost]
        public JsonResult Sync([FromBody] SyncEmployeeNoteRequest request)
        {
            EmployeeNoteListResponse response = new EmployeeNoteListResponse();
            try
            {
                response = this.EmployeeNoteService.Sync(request);
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