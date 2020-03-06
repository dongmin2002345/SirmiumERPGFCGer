using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Common.ToDos;
using ServiceInterfaces.Messages.Common.ToDos;
using ServiceInterfaces.ViewModels.Common.ToDos;

namespace SirmiumERPWeb.Controllers.ToDos
{
    public class ToDoStatusController : Controller
    {
        IToDoStatusService ToDoStatusService { get; set; }

        public ToDoStatusController(IServiceProvider provider)
        {
            ToDoStatusService = provider.GetRequiredService<IToDoStatusService>();
        }

        [HttpGet]
        public JsonResult GetStatuses(int companyId)
        {
            ToDoStatusListResponse response = new ToDoStatusListResponse();
            try
            {
                response = ToDoStatusService.GetToDoStatuses(companyId);
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
        public JsonResult Create([FromBody] ToDoStatusViewModel c)
        {
            ToDoStatusResponse response = new ToDoStatusResponse();
            try
            {
                response = this.ToDoStatusService.Create(c);
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
        public JsonResult Delete([FromBody]ToDoStatusViewModel Status)
        {
            ToDoStatusResponse response = new ToDoStatusResponse();
            try
            {
                response = this.ToDoStatusService.Delete(Status.Identifier);
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
        public JsonResult Sync([FromBody] SyncToDoStatusRequest request)
        {
            ToDoStatusListResponse response = new ToDoStatusListResponse();
            try
            {
                response = this.ToDoStatusService.Sync(request);
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