using DataMapper.Mappers.Common.BusinessPartners;
using DomainCore.Common.BusinessPartners;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Common.BusinessPartners
{
    public class BusinessPartnerBankService : IBusinessPartnerBankService
    {
        IUnitOfWork unitOfWork;

        public BusinessPartnerBankService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        
        public BusinessPartnerBankListResponse Sync(SyncBusinessPartnerBankRequest request)
        {
            BusinessPartnerBankListResponse response = new BusinessPartnerBankListResponse();
            try
            {
                response.BusinessPartnerBanks = new List<BusinessPartnerBankViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.BusinessPartnerBanks.AddRange(unitOfWork.GetBusinessPartnerBankRepository()
                        .GetBusinessPartnerBanksNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToBusinessPartnerBankViewModelList() ?? new List<BusinessPartnerBankViewModel>());
                }
                else
                {
                    response.BusinessPartnerBanks.AddRange(unitOfWork.GetBusinessPartnerBankRepository()
                        .GetBusinessPartnerBanks(request.CompanyId)
                        ?.ConvertToBusinessPartnerBankViewModelList() ?? new List<BusinessPartnerBankViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartnerBanks = new List<BusinessPartnerBankViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
