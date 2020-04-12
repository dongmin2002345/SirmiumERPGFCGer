using DataMapper.Mappers.Common.DocumentStores;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.DocumentStores;
using ServiceInterfaces.Messages.Common.Invoices;
using ServiceInterfaces.ViewModels.Common.DocumentStores;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Common.DocumentStores
{
    public class DocumentFileService : IDocumentFileService
    {
        IUnitOfWork unitOfWork;

        public DocumentFileService(IUnitOfWork uow)
        {
            unitOfWork = uow;
        }

        public DocumentFileResponse Clear(int companyId)
        {
            DocumentFileResponse response = new DocumentFileResponse();
            try
            {
                unitOfWork.GetDocumentFileRepository().Clear(companyId);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public DocumentFileResponse Create(DocumentFileViewModel viewModel)
        {
            DocumentFileResponse response = new DocumentFileResponse();
            try
            {
                response.DocumentFile = unitOfWork.GetDocumentFileRepository().Create(viewModel.ConvertToDocumentFile())
                    ?.ConvertToDocumentFileViewModel();
                response.Success = true;
            } catch (Exception ex)
            {
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
                response.DocumentFile = unitOfWork.GetDocumentFileRepository().Delete(toDelete?.Id ?? 0)
                    ?.ConvertToDocumentFileViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
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
                response.DocumentFiles = unitOfWork.GetDocumentFileRepository().SubmitList(toCreate
                    .ConvertToDocumentFileList())
                    ?.ConvertToDocumentFileViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
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
                response.DocumentFiles = unitOfWork.GetDocumentFileRepository().GetDocumentFiles(request.CompanyId, request?.LastUpdatedAt, request.CurrentPage, request.ItemsPerPage)
                    ?.ConvertToDocumentFileViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
