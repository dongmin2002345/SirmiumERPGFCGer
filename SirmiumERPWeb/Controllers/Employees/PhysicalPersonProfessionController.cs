using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;

namespace SirmiumERPWeb.Controllers.Employees
{
    public class PhysicalPersonProfessionController : Controller
    {
        IPhysicalPersonProfessionService PhysicalPersonItemService { get; set; }

        public PhysicalPersonProfessionController(IServiceProvider provider)
        {
            PhysicalPersonItemService = provider.GetRequiredService<IPhysicalPersonProfessionService>();

        }

        [HttpGet]
        public JsonResult GetPhysicalPersonItems(int CompanyId)
        {
            PhysicalPersonProfessionListResponse response = new PhysicalPersonProfessionListResponse();
            try
            {
                response = PhysicalPersonItemService.GetPhysicalPersonItems(CompanyId);
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
        public JsonResult GetPhysicalPersonItemsNewerThen(int CompanyId, DateTime? lastUpdateTime)
        {
            PhysicalPersonProfessionListResponse response = new PhysicalPersonProfessionListResponse();
            try
            {
                response = PhysicalPersonItemService.GetPhysicalPersonItemsNewerThen(CompanyId, lastUpdateTime);
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
        public JsonResult Create([FromBody] PhysicalPersonProfessionViewModel c)
        {
            PhysicalPersonProfessionResponse response = new PhysicalPersonProfessionResponse();
            try
            {
                response = this.PhysicalPersonItemService.Create(c);
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
        public JsonResult Sync([FromBody] SyncPhysicalPersonProfessionRequest request)
        {
            PhysicalPersonProfessionListResponse response = new PhysicalPersonProfessionListResponse();
            try
            {
                response = this.PhysicalPersonItemService.Sync(request);
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
