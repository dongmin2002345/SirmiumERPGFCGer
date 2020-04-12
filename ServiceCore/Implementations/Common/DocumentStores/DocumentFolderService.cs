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
    public class DocumentFolderService : IDocumentFolderService
    {
        IUnitOfWork unitOfWork;

        public DocumentFolderService(IUnitOfWork uow)
        {
            unitOfWork = uow;
        }

        public DocumentFolderResponse Clear(int companyId)
        {
            DocumentFolderResponse response = new DocumentFolderResponse();
            try
            {
                unitOfWork.GetDocumentFolderRepository().Clear(companyId);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public DocumentFolderResponse Create(DocumentFolderViewModel viewModel)
        {
            DocumentFolderResponse response = new DocumentFolderResponse();
            try
            {
                response.DocumentFolder = unitOfWork.GetDocumentFolderRepository().Create(viewModel.ConvertToDocumentFolder())
                    ?.ConvertToDocumentFolderViewModel();
                response.Success = true;
            } catch (Exception ex)
            {
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
                response.DocumentFolder = unitOfWork.GetDocumentFolderRepository().Delete(toDelete?.Id ?? 0)
                    ?.ConvertToDocumentFolderViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
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
                response.DocumentFolders = unitOfWork.GetDocumentFolderRepository().SubmitList(toCreate
                    .ConvertToDocumentFolderList())
                    ?.ConvertToDocumentFolderViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
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
                response.DocumentFolders = unitOfWork.GetDocumentFolderRepository().GetDocumentFolders(request.CompanyId, request?.LastUpdatedAt, request.CurrentPage, request.ItemsPerPage)
                    ?.ConvertToDocumentFolderViewModelList();
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
