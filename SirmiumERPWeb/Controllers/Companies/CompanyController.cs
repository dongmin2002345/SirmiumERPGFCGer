using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ServiceInterfaces.Abstractions.Common.Companies;
using ServiceInterfaces.Messages.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Companies;

namespace SirmiumERPWeb.Controllers.Companies
{
    //[Route("api/[controller]")]
    public class CompanyController : Controller
    {
        ICompanyService companyService { get; set; }

        public CompanyController(IServiceProvider provider)
        {
            companyService = provider.GetRequiredService<ICompanyService>();

        }
        // GET: api/Company
        [HttpGet]
        public JsonResult GetCompanies()
        {
            CompanyListResponse response = new CompanyListResponse();
            try
            {
                response = companyService.GetCompanies();
                Console.WriteLine(response.Companies);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                response.Message = ex.Message;
                response.Success = false;
            }

            return Json(response, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        [HttpGet]
        public JsonResult GetCompany(int id)
        {
            CompanyResponse response = new CompanyResponse();
            try
            {
                response = companyService.GetCompany(id);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }

            return Json(response, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        [HttpGet]
        public JsonResult GetNewCodeValue()
        {
            CompanyResponse response = new CompanyResponse();
            try
            {
                response = companyService.GetNewCodeValue();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                Console.WriteLine(ex.Message);
            }
            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Create([FromBody] CompanyViewModel c)
        {
            CompanyResponse response;
            try
            {
                response = this.companyService.Create(c);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }

            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });


            //CompanyResponse response = new CompanyResponse();

            //try
            //{
            //    string obj = data["Object"];

            //    CompanyViewModel c = JsonConvert.DeserializeObject<CompanyViewModel>(obj,
            //            new JsonSerializerSettings()
            //            {
            //                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //            });

            //    response = companyService.Create(c);
            //    response.Success = true;
            //    response.Message = "Uspesno izmenjena kompanija!";
            //    return Json(response,
            //            new JsonSerializerSettings()
            //            {
            //                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //            });
            //}
            //catch (Exception ex)
            //{
            //    response.Message = ex.Message;
            //    response.Success = false;
            //}

            //return Json(response,
            //            new JsonSerializerSettings()
            //            {
            //                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //            });
        }

        [HttpPost]
        public JsonResult Update([FromBody] CompanyViewModel c)
        {

            CompanyResponse response;
            try
            {
                response = this.companyService.Update(c);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }

            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });


            //CompanyResponse response = new CompanyResponse();

            //try
            //{
            //    string obj = data["Object"];

            //    CompanyViewModel c = JsonConvert.DeserializeObject<CompanyViewModel>(obj,
            //            new JsonSerializerSettings()
            //            {
            //                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //            });

            //    response = companyService.Update(c);
            //    response.Success = true;
            //    response.Message = "Uspesno izmenjena kompanija!";
            //    return Json(response,
            //            new JsonSerializerSettings()
            //            {
            //                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //            });
            //}
            //catch (Exception ex)
            //{
            //    response.Message = ex.Message;
            //    response.Success = false;
            //}

            //return Json(response,
            //            new JsonSerializerSettings()
            //            {
            //                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //            });
        }

        [HttpPost]
        public JsonResult Delete([FromBody] CompanyViewModel c)
        {

            CompanyResponse response;
            try
            {
                response = this.companyService.Delete(c.Id);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }

            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }
    }
}