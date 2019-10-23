using DataMapper.Mappers.Common.Phonebooks;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.Phonebook;
using ServiceInterfaces.Messages.Common.Phonebooks;
using ServiceInterfaces.ViewModels.Common.Phonebooks;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Common.Phonebooks
{
    public class PhonebookDocumentService : IPhonebookDocumentService
    {
        IUnitOfWork unitOfWork;

        public PhonebookDocumentService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public PhonebookDocumentListResponse Sync(SyncPhonebookDocumentRequest request)
        {
            PhonebookDocumentListResponse response = new PhonebookDocumentListResponse();
            try
            {
                response.PhonebookDocuments = new List<PhonebookDocumentViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.PhonebookDocuments.AddRange(unitOfWork.GetPhonebookDocumentRepository()
                        .GetPhonebookDocumentsNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToPhonebookDocumentViewModelList() ?? new List<PhonebookDocumentViewModel>());
                }
                else
                {
                    response.PhonebookDocuments.AddRange(unitOfWork.GetPhonebookDocumentRepository()
                        .GetPhonebookDocuments(request.CompanyId)
                        ?.ConvertToPhonebookDocumentViewModelList() ?? new List<PhonebookDocumentViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.PhonebookDocuments = new List<PhonebookDocumentViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
