using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Common.TaxAdministrations;
using ServiceInterfaces.Messages.Common.TaxAdministrations;
using ServiceInterfaces.ViewModels.Common.TaxAdministrations;

namespace SirmiumERPWeb.Controllers.TaxAdministrations
{
    public class TaxAdministrationController : Controller
    {
        ITaxAdministrationService taxAdministrationService { get; set; }

        public TaxAdministrationController(IServiceProvider provider)
        {
            taxAdministrationService = provider.GetRequiredService<ITaxAdministrationService>();
        }

        [HttpGet]
        public JsonResult GetTaxAdministrations(int companyId)
        {
            TaxAdministrationListResponse response = new TaxAdministrationListResponse();
            try
            {
                response = taxAdministrationService.GetTaxAdministrations(companyId);
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
        public JsonResult GetTaxAdministrationsNewerThan(int companyId, DateTime? lastUpdateTime)
        {
            TaxAdministrationListResponse response;
            try
            {
                response = taxAdministrationService.GetTaxAdministrationsNewerThan(companyId, lastUpdateTime);
            }
            catch (Exception ex)
            {
                response = null;
                Console.WriteLine(ex.Message);
            }
            return new JsonResult(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult Create([FromBody] TaxAdministrationViewModel c)
        {
            TaxAdministrationResponse response = new TaxAdministrationResponse();
            try
            {
                response = this.taxAdministrationService.Create(c);
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
        public JsonResult Delete([FromBody]TaxAdministrationViewModel taxAdministration)
        {
            TaxAdministrationResponse response = new TaxAdministrationResponse();
            try
            {
                response = this.taxAdministrationService.Delete(taxAdministration.Identifier);
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
        public JsonResult Sync([FromBody] SyncTaxAdministrationRequest request)
        {
            TaxAdministrationListResponse response = new TaxAdministrationListResponse();
            try
            {
                response = this.taxAdministrationService.Sync(request);
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