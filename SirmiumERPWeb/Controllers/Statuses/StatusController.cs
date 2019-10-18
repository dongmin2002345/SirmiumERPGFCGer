using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions;
using ServiceInterfaces.Abstractions.Statuses;
using ServiceInterfaces.Messages.Statuses;
using ServiceInterfaces.ViewModels.Statuses;

namespace SirmiumERPWeb.Controllers.Statuses
{
    public class StatusController : Controller
    {
        IStatusService StatusService { get; set; }

        public StatusController(IServiceProvider provider)
        {
            StatusService = provider.GetRequiredService<IStatusService>();
        }

        [HttpGet]
        public JsonResult GetStatuses(int companyId)
        {
            StatusListResponse response = new StatusListResponse();
            try
            {
                response = StatusService.GetStatuses(companyId);
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
        public JsonResult Create([FromBody] StatusViewModel c)
        {
            StatusResponse response = new StatusResponse();
            try
            {
                response = this.StatusService.Create(c);
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
        public JsonResult Delete([FromBody]StatusViewModel Status)
        {
            StatusResponse response = new StatusResponse();
            try
            {
                response = this.StatusService.Delete(Status.Identifier);
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
        public JsonResult Sync([FromBody] SyncStatusRequest request)
        {
            StatusListResponse response = new StatusListResponse();
            try
            {
                response = this.StatusService.Sync(request);
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