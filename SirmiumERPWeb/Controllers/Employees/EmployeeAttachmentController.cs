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
    public class EmployeeAttachmentController : Controller
    {
        IEmployeeAttachmentService employeeAttachmentService;

        public EmployeeAttachmentController(IServiceProvider serviceProvider)
        {
            employeeAttachmentService = serviceProvider.GetRequiredService<IEmployeeAttachmentService>();
        }

        [HttpGet]
        public JsonResult GetEmployeeAttachments(int companyId)
        {
            EmployeeAttachmentListResponse response;
            try
            {
                response = employeeAttachmentService.GetEmployeeAttachments(companyId);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpGet]
        public JsonResult GetEmployeeAttachmentsNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            EmployeeAttachmentListResponse response;
            try
            {
                response = employeeAttachmentService.GetEmployeeAttachmentsNewerThen(companyId, lastUpdateTime);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Create([FromBody] EmployeeAttachmentViewModel c)
        {
            EmployeeAttachmentResponse response;
            try
            {
                response = this.employeeAttachmentService.Create(c);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }

            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult CreateList([FromBody] List<EmployeeAttachmentViewModel> c)
        {
            EmployeeAttachmentListResponse response;
            try
            {
                response = this.employeeAttachmentService.CreateList(c);
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
            EmployeeAttachmentResponse response;
            try
            {
                response = this.employeeAttachmentService.Delete(identifier);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }

            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Sync([FromBody] SyncEmployeeAttachmentRequest request)
        {
            EmployeeAttachmentListResponse response = new EmployeeAttachmentListResponse();
            try
            {
                response = this.employeeAttachmentService.Sync(request);
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