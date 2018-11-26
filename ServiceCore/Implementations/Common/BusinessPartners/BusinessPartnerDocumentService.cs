using DataMapper.Mappers.Common.BusinessPartners;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Common.BusinessPartners
{
    public class BusinessPartnerDocumentService : IBusinessPartnerDocumentService
    {
        IUnitOfWork unitOfWork;

        public BusinessPartnerDocumentService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public BusinessPartnerDocumentListResponse Sync(SyncBusinessPartnerDocumentRequest request)
        {
            BusinessPartnerDocumentListResponse response = new BusinessPartnerDocumentListResponse();
            try
            {
                response.BusinessPartnerDocuments = new List<BusinessPartnerDocumentViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.BusinessPartnerDocuments.AddRange(unitOfWork.GetBusinessPartnerDocumentRepository()
                        .GetBusinessPartnerDocumentsNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToBusinessPartnerDocumentViewModelList() ?? new List<BusinessPartnerDocumentViewModel>());
                }
                else
                {
                    response.BusinessPartnerDocuments.AddRange(unitOfWork.GetBusinessPartnerDocumentRepository()
                        .GetBusinessPartnerDocuments(request.CompanyId)
                        ?.ConvertToBusinessPartnerDocumentViewModelList() ?? new List<BusinessPartnerDocumentViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartnerDocuments = new List<BusinessPartnerDocumentViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
