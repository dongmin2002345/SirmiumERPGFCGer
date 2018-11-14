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
    public class BusinessPartnerPhoneService : IBusinessPartnerPhoneService
    {
        IUnitOfWork unitOfWork;

        public BusinessPartnerPhoneService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public BusinessPartnerPhoneListResponse Sync(SyncBusinessPartnerPhoneRequest request)
        {
            BusinessPartnerPhoneListResponse response = new BusinessPartnerPhoneListResponse();
            try
            {
                response.BusinessPartnerPhones = new List<BusinessPartnerPhoneViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.BusinessPartnerPhones.AddRange(unitOfWork.GetBusinessPartnerPhoneRepository()
                        .GetBusinessPartnerPhonesNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToBusinessPartnerPhoneViewModelList() ?? new List<BusinessPartnerPhoneViewModel>());
                }
                else
                {
                    response.BusinessPartnerPhones.AddRange(unitOfWork.GetBusinessPartnerPhoneRepository()
                        .GetBusinessPartnerPhones(request.CompanyId)
                        ?.ConvertToBusinessPartnerPhoneViewModelList() ?? new List<BusinessPartnerPhoneViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartnerPhones = new List<BusinessPartnerPhoneViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
