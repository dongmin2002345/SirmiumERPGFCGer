using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceInterfaces.Abstractions.Common.DocumentStores;
using ServiceInterfaces.Messages.Common.Invoices;
using ServiceInterfaces.ViewModels.Common.DocumentStores;

namespace SirmiumERPWeb.Controllers.DocumentStores
{
    public class DocumentFolderController : Controller
    {
        IDocumentFolderService documentFolderService;
        public DocumentFolderController(IServiceProvider provider)
        {
            documentFolderService = provider.GetRequiredService<IDocumentFolderService>();
        }

        [HttpGet]
        public JsonResult Clear(int CompanyId)
        {
            DocumentFolderResponse response = new DocumentFolderResponse();
            try
            {
                response = documentFolderService.Clear(CompanyId);
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
        public JsonResult Create([FromBody] DocumentFolderViewModel c)
        {
            DocumentFolderResponse response = new DocumentFolderResponse();
            try
            {
                response = documentFolderService.Create(c);
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
        public JsonResult Delete([FromBody] DocumentFolderViewModel c)
        {
            DocumentFolderResponse response = new DocumentFolderResponse();
            try
            {
                response = this.documentFolderService.Delete(c);
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
        public JsonResult Sync([FromBody] SyncDocumentFolderRequest request)
        {
            DocumentFolderListResponse response = new DocumentFolderListResponse();
            try
            {
                response = this.documentFolderService.Sync(request);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult SubmitList([FromBody] List<DocumentFolderViewModel> docFolders)
        {
            DocumentFolderListResponse response = new DocumentFolderListResponse();
            try
            {
                response = this.documentFolderService.SubmitList(docFolders);
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
