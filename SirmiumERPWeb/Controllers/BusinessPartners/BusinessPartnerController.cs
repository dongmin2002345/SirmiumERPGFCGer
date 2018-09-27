using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;

namespace SirmiumERPWeb.Controllers.BusinessPartners
{
    public class BusinessPartnerController : Controller
    {
        IBusinessPartnerService businessPartnerService { get; set; }

        public BusinessPartnerController(IServiceProvider provider)
        {
            businessPartnerService = provider.GetRequiredService<IBusinessPartnerService>();

        }
        // GET: api/BusinessPartner
        [HttpGet]
        public JsonResult GetBusinessPartners(string filterString)
        {
            BusinessPartnerListResponse response;
            try
            {
                response = businessPartnerService.GetBusinessPartners(filterString);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpGet]
        public JsonResult GetBusinessPartner(int id)
        {
            BusinessPartnerResponse response;
            try
            {
                response = businessPartnerService.GetBusinessPartner(id);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpGet]
        public JsonResult GetBusinessPartnersByPage(int currentPage = 1, int itemsPerPage = 6, string businessPartnerName = "")
        {
            BusinessPartnerListResponse response;
            try
            {
                response = businessPartnerService.GetBusinessPartnersByPage(currentPage, itemsPerPage, businessPartnerName);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpGet]
        public JsonResult GetBusinessPartnersCount(string searchParameter = "")
        {
            BusinessPartnerListResponse response;
            try
            {
                response = businessPartnerService.GetBusinessPartnersCount(searchParameter);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpGet]
        public JsonResult GetNewCodeValue()
        {
            BusinessPartnerCodeResponse response;
            try
            {
                response = businessPartnerService.GetNewCodeValue();
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }        

        [HttpGet]
        public JsonResult GetBusinessPartnersForPopup(string filterString)
        {
            BusinessPartnerListResponse response;
            try
            {
                response = businessPartnerService.GetBusinessPartnersForPopup(filterString);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Create([FromBody] BusinessPartnerViewModel c)
        {
            BusinessPartnerResponse response;
            try
            {
                response = this.businessPartnerService.Create(c);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }

            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Update([FromBody] BusinessPartnerViewModel c)
        {
            BusinessPartnerResponse response;
            try
            {
                response = this.businessPartnerService.Update(c);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }

            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Delete([FromBody] BusinessPartnerViewModel c)
        {
            BusinessPartnerResponse response;
            try
            {
                response = this.businessPartnerService.Delete(c.Id);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }

            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        #region Report test
        [HttpGet]
        public ActionResult GetBusinessPartnersForReport(int companyId)
        {
            BusinessPartnerListResponse response;
            try
            {
                response = businessPartnerService.GetBusinessPartnersByPage(1, 50, "");

                if (response.Success)
                {
                    return View("~/Views/Reports/BusinessPartners/BusinessPartnerReportView.cshtml", response.BusinessPartnersByPage.ToList());
                }
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return new NotFoundResult();
        }
        #endregion
    }
}