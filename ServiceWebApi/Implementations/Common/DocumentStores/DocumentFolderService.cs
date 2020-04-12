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
    public class DocumentFolderService : IDocumentFolderService
    {
        public DocumentFolderResponse Clear(int companyId)
        {
            DocumentFolderResponse response = new DocumentFolderResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<DocumentFolderViewModel, DocumentFolderResponse>("Clear", new Dictionary<string, string>() {
                    { "CompanyId", companyId.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.DocumentFolder = new DocumentFolderViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public DocumentFolderResponse Create(DocumentFolderViewModel toCreate)
        {
            DocumentFolderResponse response = new DocumentFolderResponse();
            try
            {
                response = WpfApiHandler.SendToApi<DocumentFolderViewModel, DocumentFolderResponse>(toCreate, "Create");
            }
            catch (Exception ex)
            {
                response.DocumentFolder = new DocumentFolderViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public DocumentFolderResponse Delete(DocumentFolderViewModel toDelete)
        {
            DocumentFolderResponse response = new DocumentFolderResponse();
            try
            {
                response = WpfApiHandler.SendToApi<DocumentFolderViewModel, DocumentFolderResponse>(toDelete, "Delete");
            }
            catch (Exception ex)
            {
                response.DocumentFolder = new DocumentFolderViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public DocumentFolderListResponse SubmitList(List<DocumentFolderViewModel> toCreate)
        {
            DocumentFolderListResponse response = new DocumentFolderListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<List<DocumentFolderViewModel>, DocumentFolderListResponse>(toCreate, "SubmitList");
            }
            catch (Exception ex)
            {
                response.DocumentFolders = new List<DocumentFolderViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public DocumentFolderListResponse Sync(SyncDocumentFolderRequest request)
        {
            DocumentFolderListResponse response = new DocumentFolderListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncDocumentFolderRequest, List<DocumentFolderViewModel>, DocumentFolderListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.DocumentFolders = new List<DocumentFolderViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
