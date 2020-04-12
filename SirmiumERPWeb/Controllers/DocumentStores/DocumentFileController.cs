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
    public class DocumentFileController : Controller
    {
        IDocumentFileService documentFileService;
        public DocumentFileController(IServiceProvider provider)
        {
            documentFileService = provider.GetRequiredService<IDocumentFileService>();
        }

        [HttpGet]
        public JsonResult Clear(int CompanyId)
        {
            DocumentFileResponse response = new DocumentFileResponse();
            try
            {
                response = documentFileService.Clear(CompanyId);
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
        public JsonResult Create([FromBody] DocumentFileViewModel c)
        {
            DocumentFileResponse response = new DocumentFileResponse();
            try
            {
                response = documentFileService.Create(c);
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
        public JsonResult Delete([FromBody] DocumentFileViewModel c)
        {
            DocumentFileResponse response = new DocumentFileResponse();
            try
            {
                response = this.documentFileService.Delete(c);
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
        public JsonResult Sync([FromBody] SyncDocumentFileRequest request)
        {
            DocumentFileListResponse response = new DocumentFileListResponse();
            try
            {
                response = this.documentFileService.Sync(request);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return Json(response, new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented });
        }

        [HttpPost]
        public JsonResult SubmitList([FromBody] List<DocumentFileViewModel> docFiles)
        {
            DocumentFileListResponse response = new DocumentFileListResponse();
            try
            {
                response = this.documentFileService.SubmitList(docFiles);
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
