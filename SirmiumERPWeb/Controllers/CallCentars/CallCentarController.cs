using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Common.CallCentars;
using ServiceInterfaces.Messages.Common.CallCentars;
using ServiceInterfaces.ViewModels.Common.CallCentars;
using SirmiumERPWeb.Hubs;

namespace SirmiumERPWeb.Controllers.CallCentars
{
    public class CallCentarController : Controller
    {
        ICallCentarService callCentarService { get; set; }

        IHubContext<NotificationHub> notificationHub;

        public CallCentarController(IServiceProvider provider, IHubContext<NotificationHub> hubContext)
        {
            callCentarService = provider.GetRequiredService<ICallCentarService>();

            notificationHub = hubContext;
        }

        [HttpGet]
        public JsonResult GetCallCentars(int companyId)
        {
            CallCentarListResponse response = new CallCentarListResponse();
            try
            {
                response = callCentarService.GetCallCentars(companyId);
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
        public JsonResult Create([FromBody] CallCentarViewModel c)
        {
            CallCentarResponse response = new CallCentarResponse();
            try
            {
                response = this.callCentarService.Create(c);

                if(response.Success)
                {
                    if(c.User != null && !c.CheckedDone)
                    {
                        string target = "Korisnik_" + c.User.Id;
                        notificationHub.Clients.Group(target).SendAsync("SendMessage", c);
                    }
                }
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
        public JsonResult NotifyUser([FromBody] CallCentarViewModel c)
        {
            CallCentarResponse response = new CallCentarResponse();
            try
            {
                if (c.User != null)
                {
                    string target = "Korisnik_" + c.User.Id;
                    notificationHub.Clients.Group(target).SendAsync("SendMessage", c);
                }
                response.CallCentar = c;
                response.Success = true;
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
        public JsonResult Delete([FromBody]CallCentarViewModel callCentar)
        {
            CallCentarResponse response = new CallCentarResponse();
            try
            {
                response = this.callCentarService.Delete(callCentar.Identifier);
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
        public JsonResult Sync([FromBody] SyncCallCentarRequest request)
        {
            CallCentarListResponse response = new CallCentarListResponse();
            try
            {
                response = this.callCentarService.Sync(request);
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