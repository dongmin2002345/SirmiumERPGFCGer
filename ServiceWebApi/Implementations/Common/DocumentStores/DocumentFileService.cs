using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Common.DocumentStores;
using ServiceInterfaces.Messages.Common.Invoices;
using ServiceInterfaces.ViewModels.Common.DocumentStores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Common.DocumentStores
{
    public class DocumentFileService : IDocumentFileService
    {
        public DocumentFileResponse Clear(int companyId)
        {
            DocumentFileResponse response = new DocumentFileResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<DocumentFileViewModel, DocumentFileResponse>("Clear", new Dictionary<string, string>() {
                    { "CompanyId", companyId.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.DocumentFile = new DocumentFileViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public DocumentFileResponse Create(DocumentFileViewModel toCreate)
        {
            DocumentFileResponse response = new DocumentFileResponse();
            try
            {
                response = WpfApiHandler.SendToApi<DocumentFileViewModel, DocumentFileResponse>(toCreate, "Create");
            }
            catch (Exception ex)
            {
                response.DocumentFile = new DocumentFileViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public DocumentFileResponse Delete(DocumentFileViewModel toDelete)
        {
            DocumentFileResponse response = new DocumentFileResponse();
            try
            {
                response = WpfApiHandler.SendToApi<DocumentFileViewModel, DocumentFileResponse>(toDelete, "Delete");
            }
            catch (Exception ex)
            {
                response.DocumentFile = new DocumentFileViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public DocumentFileListResponse SubmitList(List<DocumentFileViewModel> toCreate)
        {
            DocumentFileListResponse response = new DocumentFileListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<List<DocumentFileViewModel>, DocumentFileListResponse>(toCreate, "SubmitList");
            }
            catch (Exception ex)
            {
                response.DocumentFiles = new List<DocumentFileViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public DocumentFileListResponse Sync(SyncDocumentFileRequest request)
        {
            DocumentFileListResponse response = new DocumentFileListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncDocumentFileRequest, List<DocumentFileViewModel>, DocumentFileListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.DocumentFiles = new List<DocumentFileViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
