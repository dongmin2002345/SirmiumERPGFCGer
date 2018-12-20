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
    public class PhysicalPersonLicenceController : Controller
    {
        IPhysicalPersonLicenceService PhysicalPersonItemService { get; set; }

        public PhysicalPersonLicenceController(IServiceProvider provider)
        {
            PhysicalPersonItemService = provider.GetRequiredService<IPhysicalPersonLicenceService>();

        }

        [HttpGet]
        public JsonResult GetPhysicalPersonItems(int CompanyId)
        {
            PhysicalPersonLicenceListResponse response = new PhysicalPersonLicenceListResponse();
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
            PhysicalPersonLicenceListResponse response = new PhysicalPersonLicenceListResponse();
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
        public JsonResult Create([FromBody] PhysicalPersonLicenceViewModel c)
        {
            PhysicalPersonLicenceResponse response = new PhysicalPersonLicenceResponse();
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
        public JsonResult Sync([FromBody] SyncPhysicalPersonLicenceRequest request)
        {
            PhysicalPersonLicenceListResponse response = new PhysicalPersonLicenceListResponse();
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
