using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.ConstructionSites;
using ServiceInterfaces.Messages.ConstructionSites;

namespace SirmiumERPWeb.Controllers.ConstructionSites
{
    public class ConstructionSiteNoteController : Controller
    {
        IConstructionSiteNoteService ConstructionSiteNoteService { get; set; }

        public ConstructionSiteNoteController(IServiceProvider provider)
        {
            ConstructionSiteNoteService = provider.GetRequiredService<IConstructionSiteNoteService>();

        }

        [HttpPost]
        public JsonResult Sync([FromBody] SyncConstructionSiteNoteRequest request)
        {
            ConstructionSiteNoteListResponse response = new ConstructionSiteNoteListResponse();
            try
            {
                response = this.ConstructionSiteNoteService.Sync(request);
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