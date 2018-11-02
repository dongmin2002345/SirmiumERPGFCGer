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
    public class EmployeeByConstructionSiteHistoryController : Controller
    {
        IEmployeeByConstructionSiteHistoryService employeeByConstructionSiteHistoryService { get; set; }

        public EmployeeByConstructionSiteHistoryController(IServiceProvider provider)
        {
            employeeByConstructionSiteHistoryService = provider.GetRequiredService<IEmployeeByConstructionSiteHistoryService>();
        }

        [HttpGet]
        public JsonResult GetEmployeeByConstructionSiteHistories(int companyId)
        {
            EmployeeByConstructionSiteHistoryListResponse response = new EmployeeByConstructionSiteHistoryListResponse();
            try
            {
                response = employeeByConstructionSiteHistoryService.GetEmployeeByConstructionSiteHistories(companyId);
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
        public JsonResult GetEmployeeByConstructionSiteHistoriesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            EmployeeByConstructionSiteHistoryListResponse response;
            try
            {
                response = employeeByConstructionSiteHistoryService.GetEmployeeByConstructionSiteHistoriesNewerThen(companyId, lastUpdateTime);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Create([FromBody] EmployeeByConstructionSiteHistoryViewModel c)
        {
            EmployeeByConstructionSiteHistoryResponse response = new EmployeeByConstructionSiteHistoryResponse();
            try
            {
                response = this.employeeByConstructionSiteHistoryService.Create(c);
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
        public JsonResult Delete([FromBody]EmployeeByConstructionSiteHistoryViewModel employeeByConstructionSiteHistory)
        {
            EmployeeByConstructionSiteHistoryResponse response = new EmployeeByConstructionSiteHistoryResponse();
            try
            {
                response = this.employeeByConstructionSiteHistoryService.Delete(employeeByConstructionSiteHistory.Identifier);
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
        public JsonResult Sync([FromBody] SyncEmployeeByConstructionSiteHistoryRequest request)
        {
            EmployeeByConstructionSiteHistoryListResponse response = new EmployeeByConstructionSiteHistoryListResponse();
            try
            {
                response = this.employeeByConstructionSiteHistoryService.Sync(request);
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