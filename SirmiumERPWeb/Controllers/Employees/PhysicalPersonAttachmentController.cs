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
    public class PhysicalPersonAttachmentController : Controller
    {
        IPhysicalPersonAttachmentService PhysicalPersonAttachmentService;

        public PhysicalPersonAttachmentController(IServiceProvider serviceProvider)
        {
            PhysicalPersonAttachmentService = serviceProvider.GetRequiredService<IPhysicalPersonAttachmentService>();
        }

        [HttpGet]
        public JsonResult GetPhysicalPersonAttachments(int companyId)
        {
            PhysicalPersonAttachmentListResponse response;
            try
            {
                response = PhysicalPersonAttachmentService.GetPhysicalPersonAttachments(companyId);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpGet]
        public JsonResult GetPhysicalPersonAttachmentsNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            PhysicalPersonAttachmentListResponse response;
            try
            {
                response = PhysicalPersonAttachmentService.GetPhysicalPersonAttachmentsNewerThen(companyId, lastUpdateTime);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Create([FromBody] PhysicalPersonAttachmentViewModel c)
        {
            PhysicalPersonAttachmentResponse response;
            try
            {
                response = this.PhysicalPersonAttachmentService.Create(c);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }

            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult CreateList([FromBody] List<PhysicalPersonAttachmentViewModel> c)
        {
            PhysicalPersonAttachmentListResponse response;
            try
            {
                response = this.PhysicalPersonAttachmentService.CreateList(c);
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
            PhysicalPersonAttachmentResponse response;
            try
            {
                response = this.PhysicalPersonAttachmentService.Delete(identifier);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }

            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Sync([FromBody] SyncPhysicalPersonAttachmentRequest request)
        {
            PhysicalPersonAttachmentListResponse response = new PhysicalPersonAttachmentListResponse();
            try
            {
                response = this.PhysicalPersonAttachmentService.Sync(request);
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