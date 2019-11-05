using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.CalendarAssignments;
using ServiceInterfaces.Messages.CalendarAssignments;
using ServiceInterfaces.ViewModels.CalendarAssignments;

namespace SirmiumERPWeb.Controllers.CalendarAssignments
{
    public class CalendarAssignmentController : Controller
    {
        ICalendarAssignmentService calendarAssignmentService { get; set; }

        public CalendarAssignmentController(IServiceProvider provider)
        {
            calendarAssignmentService = provider.GetRequiredService<ICalendarAssignmentService>();
        }

        [HttpGet]
        public JsonResult GetCalendarAssignments(int companyId)
        {
            CalendarAssignmentListResponse response;
            try
            {
                response = calendarAssignmentService.GetCalendarAssignments(companyId);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpGet]
        public JsonResult GetCalendarAssignmentsNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            CalendarAssignmentListResponse response;
            try
            {
                response = calendarAssignmentService.GetCalendarAssignmentsNewerThen(companyId, lastUpdateTime);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Create([FromBody] CalendarAssignmentViewModel c)
        {
            CalendarAssignmentResponse response;
            try
            {
                response = this.calendarAssignmentService.Create(c);
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
            CalendarAssignmentResponse response;
            try
            {
                response = this.calendarAssignmentService.Delete(identifier);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }

            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Sync([FromBody] SyncCalendarAssignmentRequest request)
        {
            CalendarAssignmentListResponse response = new CalendarAssignmentListResponse();
            try
            {
                response = this.calendarAssignmentService.Sync(request);
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