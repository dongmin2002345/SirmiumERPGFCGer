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
    public class EmployeeByBusinessPartnerController : Controller
    {
        IEmployeeByBusinessPartnerService employeeByBusinessPartnerService { get; set; }

        public EmployeeByBusinessPartnerController(IServiceProvider provider)
        {
            employeeByBusinessPartnerService = provider.GetRequiredService<IEmployeeByBusinessPartnerService>();
        }

        [HttpGet]
        public JsonResult GetEmployeeByBusinessPartners(int companyId)
        {
            EmployeeByBusinessPartnerListResponse response = new EmployeeByBusinessPartnerListResponse();
            try
            {
                response = employeeByBusinessPartnerService.GetEmployeeByBusinessPartners(companyId);
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
        public JsonResult GetEmployeeByBusinessPartnersNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            EmployeeByBusinessPartnerListResponse response;
            try
            {
                response = employeeByBusinessPartnerService.GetEmployeeByBusinessPartnersNewerThen(companyId, lastUpdateTime);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Create([FromBody] EmployeeByBusinessPartnerViewModel c)
        {
            EmployeeByBusinessPartnerResponse response = new EmployeeByBusinessPartnerResponse();
            try
            {
                response = this.employeeByBusinessPartnerService.Create(c);
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
        public JsonResult Delete([FromBody]EmployeeByBusinessPartnerViewModel employeeByBusinessPartner)
        {
            EmployeeByBusinessPartnerResponse response = new EmployeeByBusinessPartnerResponse();
            try
            {
                response = this.employeeByBusinessPartnerService.Delete(employeeByBusinessPartner);
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
        public JsonResult Sync([FromBody] SyncEmployeeByBusinessPartnerRequest request)
        {
            EmployeeByBusinessPartnerListResponse response = new EmployeeByBusinessPartnerListResponse();
            try
            {
                response = this.employeeByBusinessPartnerService.Sync(request);
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