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
    public class EmployeeProfessionController : Controller
    {
        IEmployeeProfessionService EmployeeItemService { get; set; }

        public EmployeeProfessionController(IServiceProvider provider)
        {
            EmployeeItemService = provider.GetRequiredService<IEmployeeProfessionService>();

        }

        [HttpGet]
        public JsonResult GetEmployeeItems(int CompanyId)
        {
            EmployeeProfessionItemListResponse response = new EmployeeProfessionItemListResponse();
            try
            {
                response = EmployeeItemService.GetEmployeeItems(CompanyId);
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
        public JsonResult GetEmployeeItemsNewerThen(int CompanyId, DateTime? lastUpdateTime)
        {
            EmployeeProfessionItemListResponse response = new EmployeeProfessionItemListResponse();
            try
            {
                response = EmployeeItemService.GetEmployeeItemsNewerThen(CompanyId, lastUpdateTime);
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
        public JsonResult Create([FromBody] EmployeeProfessionItemViewModel c)
        {
            EmployeeProfessionItemResponse response = new EmployeeProfessionItemResponse();
            try
            {
                response = this.EmployeeItemService.Create(c);
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
        public JsonResult Sync([FromBody] SyncEmployeeProfessionItemRequest request)
        {
            EmployeeProfessionItemListResponse response = new EmployeeProfessionItemListResponse();
            try
            {
                response = this.EmployeeItemService.Sync(request);
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
