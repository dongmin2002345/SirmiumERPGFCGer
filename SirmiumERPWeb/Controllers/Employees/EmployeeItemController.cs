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
    public class EmployeeItemController : Controller
    {
        IEmployeeItemService EmployeeItemService { get; set; }

        public EmployeeItemController(IServiceProvider provider)
        {
            EmployeeItemService = provider.GetRequiredService<IEmployeeItemService>();

        }

        [HttpGet]
        public JsonResult GetEmployeeItems(int CompanyId)
        {
            EmployeeItemListResponse response = new EmployeeItemListResponse();
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
            EmployeeItemListResponse response = new EmployeeItemListResponse();
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
        public JsonResult Create([FromBody] EmployeeItemViewModel c)
        {
            EmployeeItemResponse response = new EmployeeItemResponse();
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
        public JsonResult Sync([FromBody] SyncEmployeeItemRequest request)
        {
            EmployeeItemListResponse response = new EmployeeItemListResponse();
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
