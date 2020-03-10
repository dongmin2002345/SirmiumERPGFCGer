using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Common.ToDos;
using ServiceInterfaces.Messages.Common.ToDos;
using ServiceInterfaces.ViewModels.Common.ToDos;
using SirmiumERPWeb.Hubs;

namespace SirmiumERPWeb.Controllers.ToDos
{
    public class ToDoController : Controller
    {
        IToDoService toDoService { get; set; }

        IHubContext<NotificationHub> notificationHub;

        public ToDoController(IServiceProvider provider, IHubContext<NotificationHub> hubContext)
        {
            toDoService = provider.GetRequiredService<IToDoService>();
            notificationHub = hubContext;
        }

        [HttpGet]
        public JsonResult GetToDos(int companyId)
        {
            ToDoListResponse response = new ToDoListResponse();
            try
            {
                response = toDoService.GetToDos(companyId);
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
        public JsonResult GetToDosNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            ToDoListResponse response;
            try
            {
                response = toDoService.GetToDosNewerThen(companyId, lastUpdateTime);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Create([FromBody] ToDoViewModel c)
        {
            ToDoResponse response = new ToDoResponse();
            try
            {
                response = this.toDoService.Create(c);
                if (response.Success)
                {
                    if (c.User != null)
                    {
                        string target = "Korisnik_" + c.User.Id;
                        notificationHub.Clients.Group(target).SendAsync("SendToDo", c);
                    }
                }
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
        public JsonResult Delete([FromBody]ToDoViewModel toDo)
        {
            ToDoResponse response = new ToDoResponse();
            try
            {
                response = this.toDoService.Delete(toDo.Identifier);
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
        public JsonResult Sync([FromBody] SyncToDoRequest request)
        {
            ToDoListResponse response = new ToDoListResponse();
            try
            {
                response = this.toDoService.Sync(request);
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