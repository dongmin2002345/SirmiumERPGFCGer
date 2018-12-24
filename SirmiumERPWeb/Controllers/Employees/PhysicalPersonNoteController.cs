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
    public class PhysicalPersonNoteController : Controller
    {
        IPhysicalPersonNoteService PhysicalPersonNoteService { get; set; }

        public PhysicalPersonNoteController(IServiceProvider provider)
        {
            PhysicalPersonNoteService = provider.GetRequiredService<IPhysicalPersonNoteService>();

        }

        [HttpPost]
        public JsonResult Sync([FromBody] SyncPhysicalPersonNoteRequest request)
        {
            PhysicalPersonNoteListResponse response = new PhysicalPersonNoteListResponse();
            try
            {
                response = this.PhysicalPersonNoteService.Sync(request);
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