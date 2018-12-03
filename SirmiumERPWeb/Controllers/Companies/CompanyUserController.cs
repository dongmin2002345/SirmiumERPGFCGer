using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Common.Companies;
using ServiceInterfaces.Messages.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Companies;

namespace SirmiumERPWeb.Controllers.Companies
{
    public class CompanyUserController : Controller
    {
        ICompanyUserService userService { get; set; }

        public CompanyUserController(IServiceProvider provider)
        {
            userService = provider.GetRequiredService<ICompanyUserService>();

        }

        [HttpGet]
        public JsonResult GetCompanyUsers()
        {
            CompanyUserListResponse response = new CompanyUserListResponse();
            try
            {
                response = userService.GetCompanyUsers();
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
        public JsonResult GetCompanyUsersNewerThan(DateTime? dateFrom)
        {
            CompanyUserListResponse response = new CompanyUserListResponse();
            try
            {
                response = userService.GetCompanyUsersNewerThan(dateFrom);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Create([FromBody] CompanyUserViewModel c)
        {
            CompanyUserResponse response = new CompanyUserResponse();
            try
            {
                response = this.userService.Create(c);
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
        public JsonResult Delete(Guid identifier)
        {
            CompanyUserResponse response = new CompanyUserResponse();
            try
            {
                response = this.userService.Delete(identifier);
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
        public JsonResult Sync([FromBody] SyncCompanyUserRequest request)
        {
            CompanyUserListResponse response = new CompanyUserListResponse();
            try
            {
                response = this.userService.Sync(request);
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