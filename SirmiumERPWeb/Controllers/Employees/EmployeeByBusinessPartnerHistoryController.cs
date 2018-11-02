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
    public class EmployeeByBusinessPartnerHistoryController : Controller
    {
        IEmployeeByBusinessPartnerHistoryService employeeByBusinessPartnerHistoryService { get; set; }

        public EmployeeByBusinessPartnerHistoryController(IServiceProvider provider)
        {
            employeeByBusinessPartnerHistoryService = provider.GetRequiredService<IEmployeeByBusinessPartnerHistoryService>();
        }

        [HttpGet]
        public JsonResult GetEmployeeByBusinessPartnerHistories(int companyId)
        {
            EmployeeByBusinessPartnerHistoryListResponse response = new EmployeeByBusinessPartnerHistoryListResponse();
            try
            {
                response = employeeByBusinessPartnerHistoryService.GetEmployeeByBusinessPartnerHistories(companyId);
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
        public JsonResult GetEmployeeByBusinessPartnerHistoriesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            EmployeeByBusinessPartnerHistoryListResponse response;
            try
            {
                response = employeeByBusinessPartnerHistoryService.GetEmployeeByBusinessPartnerHistoriesNewerThen(companyId, lastUpdateTime);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Create([FromBody] EmployeeByBusinessPartnerHistoryViewModel c)
        {
            EmployeeByBusinessPartnerHistoryResponse response = new EmployeeByBusinessPartnerHistoryResponse();
            try
            {
                response = this.employeeByBusinessPartnerHistoryService.Create(c);
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
        public JsonResult Delete([FromBody]EmployeeByBusinessPartnerHistoryViewModel employeeByBusinessPartnerHistory)
        {
            EmployeeByBusinessPartnerHistoryResponse response = new EmployeeByBusinessPartnerHistoryResponse();
            try
            {
                response = this.employeeByBusinessPartnerHistoryService.Delete(employeeByBusinessPartnerHistory.Identifier);
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
        public JsonResult Sync([FromBody] SyncEmployeeByBusinessPartnerHistoryRequest request)
        {
            EmployeeByBusinessPartnerHistoryListResponse response = new EmployeeByBusinessPartnerHistoryListResponse();
            try
            {
                response = this.employeeByBusinessPartnerHistoryService.Sync(request);
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