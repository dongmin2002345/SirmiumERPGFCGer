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
    public class EmployeeByConstructionSiteController : Controller
    {
        IEmployeeByConstructionSiteService employeeByConstructionSiteService { get; set; }

        public EmployeeByConstructionSiteController(IServiceProvider provider)
        {
            employeeByConstructionSiteService = provider.GetRequiredService<IEmployeeByConstructionSiteService>();
        }

        [HttpGet]
        public JsonResult GetEmployeeByConstructionSites(int companyId)
        {
            EmployeeByConstructionSiteListResponse response = new EmployeeByConstructionSiteListResponse();
            try
            {
                response = employeeByConstructionSiteService.GetEmployeeByConstructionSites(companyId);
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
        public JsonResult GetEmployeeByConstructionSitesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            EmployeeByConstructionSiteListResponse response;
            try
            {
                response = employeeByConstructionSiteService.GetEmployeeByConstructionSitesNewerThen(companyId, lastUpdateTime);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Create([FromBody] EmployeeByConstructionSiteViewModel c)
        {
            EmployeeByConstructionSiteResponse response = new EmployeeByConstructionSiteResponse();
            try
            {
                response = this.employeeByConstructionSiteService.Create(c);
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
        public JsonResult Delete([FromBody]EmployeeByConstructionSiteViewModel employeeByConstructionSite)
        {
            EmployeeByConstructionSiteResponse response = new EmployeeByConstructionSiteResponse();
            try
            {
                response = this.employeeByConstructionSiteService.Delete(employeeByConstructionSite);
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
        public JsonResult Sync([FromBody] SyncEmployeeByConstructionSiteRequest request)
        {
            EmployeeByConstructionSiteListResponse response = new EmployeeByConstructionSiteListResponse();
            try
            {
                response = this.employeeByConstructionSiteService.Sync(request);
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