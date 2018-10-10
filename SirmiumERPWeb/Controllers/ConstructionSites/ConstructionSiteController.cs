using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.ConstructionSites;
using ServiceInterfaces.Messages.ConstructionSites;
using ServiceInterfaces.ViewModels.ConstructionSites;

namespace SirmiumERPWeb.Controllers.ConstructionSites
{
    public class ConstructionSiteController : Controller
    {
        IConstructionSiteService constructionSiteService { get; set; }

        public ConstructionSiteController(IServiceProvider provider)
        {
            constructionSiteService = provider.GetRequiredService<IConstructionSiteService>();
        }

        [HttpGet]
        public JsonResult GetConstructionSites(int companyId)
        {
            ConstructionSiteListResponse response = new ConstructionSiteListResponse();
            try
            {
                response = constructionSiteService.GetConstructionSites(companyId);
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
        public JsonResult GetConstructionSitesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            ConstructionSiteListResponse response;
            try
            {
                response = constructionSiteService.GetConstructionSitesNewerThen(companyId, lastUpdateTime);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Create([FromBody] ConstructionSiteViewModel c)
        {
            ConstructionSiteResponse response = new ConstructionSiteResponse();
            try
            {
                response = this.constructionSiteService.Create(c);
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
        public JsonResult Delete([FromBody]ConstructionSiteViewModel constructionSite)
        {
            ConstructionSiteResponse response = new ConstructionSiteResponse();
            try
            {
                response = this.constructionSiteService.Delete(constructionSite.Identifier);
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
        public JsonResult Sync([FromBody] SyncConstructionSiteRequest request)
        {
            ConstructionSiteListResponse response = new ConstructionSiteListResponse();
            try
            {
                response = this.constructionSiteService.Sync(request);
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