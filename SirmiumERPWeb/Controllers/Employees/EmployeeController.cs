using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SirmiumERPWeb.Controllers.Employees
{
    public class EmployeeController : Controller
    {
        IEmployeeService EmployeeService { get; set; }

        public EmployeeController(IServiceProvider provider)
        {
            EmployeeService = provider.GetRequiredService<IEmployeeService>();
        }

        [HttpGet]
        public JsonResult GetEmployees(int companyId)
        {
            EmployeeListResponse response;
            try
            {
                response = EmployeeService.GetEmployees(companyId);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpGet]
        public JsonResult GetEmployeesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            EmployeeListResponse response;
            try
            {
                response = EmployeeService.GetEmployeesNewerThen(companyId, lastUpdateTime);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Create([FromBody] EmployeeViewModel c)
        {
            EmployeeResponse response;
            try
            {
                response = this.EmployeeService.Create(c);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }

            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Delete([FromBody] Guid identifier)
        {
            EmployeeResponse response;
            try
            {
                response = this.EmployeeService.Delete(identifier);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }

            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Sync([FromBody] SyncEmployeeRequest request)
        {
            EmployeeListResponse response = new EmployeeListResponse();
            try
            {
                response = this.EmployeeService.Sync(request);
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
