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
    public class FamilyMemberController : Controller
    {
        IFamilyMemberService familyMemberService { get; set; }

        public FamilyMemberController(IServiceProvider provider)
        {
            familyMemberService = provider.GetRequiredService<IFamilyMemberService>();
        }

        [HttpGet]
        public JsonResult GetFamilyMembers(int companyId)
        {
            FamilyMemberListResponse response = new FamilyMemberListResponse();
            try
            {
                response = familyMemberService.GetFamilyMembers(companyId);
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
        public JsonResult GetFamilyMembersNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            FamilyMemberListResponse response;
            try
            {
                response = familyMemberService.GetFamilyMembersNewerThen(companyId, lastUpdateTime);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Create([FromBody] FamilyMemberViewModel c)
        {
            FamilyMemberResponse response = new FamilyMemberResponse();
            try
            {
                response = this.familyMemberService.Create(c);
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
        public JsonResult Delete([FromBody]FamilyMemberViewModel remedy)
        {
            FamilyMemberResponse response = new FamilyMemberResponse();
            try
            {
                response = this.familyMemberService.Delete(remedy.Identifier);
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
        public JsonResult Sync([FromBody] SyncFamilyMemberRequest request)
        {
            FamilyMemberListResponse response = new FamilyMemberListResponse();
            try
            {
                response = this.familyMemberService.Sync(request);
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