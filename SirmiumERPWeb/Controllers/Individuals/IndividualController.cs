using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Common.Individuals;
using ServiceInterfaces.Messages.Common.Individuals;
using ServiceInterfaces.ViewModels.Common.Individuals;

namespace SirmiumERPWeb.Controllers.Individuals
{
    public class IndividualController : Controller
    {
        IIndividualService individualService { get; set; }

        public IndividualController(IServiceProvider provider)
        {
            individualService = provider.GetRequiredService<IIndividualService>();

        }
        // GET: api/Individual
        [HttpGet]
        public JsonResult GetIndividuals(string filterString)
        {
            IndividualListResponse response;
            try
            {
                response = individualService.GetIndividuals(filterString);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpGet]
        public JsonResult GetIndividual(int id)
        {
            IndividualResponse response;
            try
            {
                response = individualService.GetIndividual(id);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpGet]
        public JsonResult GetIndividualsByPage(int currentPage = 1, int itemsPerPage = 6, string IndividualName = "")
        {
            IndividualListResponse response;
            try
            {
                response = individualService.GetIndividualsByPage(currentPage, itemsPerPage, IndividualName);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpGet]
        public JsonResult GetIndividualsCount(string searchParameter = "")
        {
            IndividualListResponse response;
            try
            {
                response = individualService.GetIndividualsCount(searchParameter);
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
            IndividualCodeResponse response;
            try
            {
                response = individualService.GetNewCodeValue();
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpGet]
        public JsonResult GetIndividualsForPopup(string filterString)
        {
            IndividualListResponse response;
            try
            {
                response = individualService.GetIndividualsForPopup(filterString);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Create([FromBody] IndividualViewModel c)
        {
            IndividualResponse response;
            try
            {
                response = this.individualService.Create(c);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }

            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Update([FromBody] IndividualViewModel c)
        {
            IndividualResponse response;
            try
            {
                response = this.individualService.Update(c);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }

            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Delete([FromBody] IndividualViewModel c)
        {
            IndividualResponse response;
            try
            {
                response = this.individualService.Delete(c.Id);
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
        public ActionResult GetIndividualsForReport(int companyId)
        {
            IndividualListResponse response;
            try
            {
                response = individualService.GetIndividualsByPage(1, 50, "");

                if (response.Success)
                {
                    return View("~/Views/Reports/Individuals/IndividualReportView.cshtml", response.IndividualsByPage.ToList());
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