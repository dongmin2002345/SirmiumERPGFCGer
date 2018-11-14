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
    public class BusinessPartnerInstitutionService : IBusinessPartnerInstitutionService
    {
        IUnitOfWork unitOfWork;

        public BusinessPartnerInstitutionService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public BusinessPartnerInstitutionListResponse Sync(SyncBusinessPartnerInstitutionRequest request)
        {
            BusinessPartnerInstitutionListResponse response = new BusinessPartnerInstitutionListResponse();
            try
            {
                response.BusinessPartnerInstitutions = new List<BusinessPartnerInstitutionViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.BusinessPartnerInstitutions.AddRange(unitOfWork.GetBusinessPartnerInstitutionRepository()
                        .GetBusinessPartnerInstitutionsNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToBusinessPartnerInstitutionViewModelList() ?? new List<BusinessPartnerInstitutionViewModel>());
                }
                else
                {
                    response.BusinessPartnerInstitutions.AddRange(unitOfWork.GetBusinessPartnerInstitutionRepository()
                        .GetBusinessPartnerInstitutions(request.CompanyId)
                        ?.ConvertToBusinessPartnerInstitutionViewModelList() ?? new List<BusinessPartnerInstitutionViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartnerInstitutions = new List<BusinessPartnerInstitutionViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
