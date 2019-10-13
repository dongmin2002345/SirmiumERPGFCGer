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
    public class BusinessPartnerLocationService : IBusinessPartnerLocationService
    {
        IUnitOfWork unitOfWork;

        public BusinessPartnerLocationService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

      
        public BusinessPartnerLocationListResponse Sync(SyncBusinessPartnerLocationRequest request)
        {
            BusinessPartnerLocationListResponse response = new BusinessPartnerLocationListResponse();
            try
            {
                response.BusinessPartnerLocations = new List<BusinessPartnerLocationViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.BusinessPartnerLocations.AddRange(unitOfWork.GetBusinessPartnerLocationRepository()
                        .GetBusinessPartnerLocationsNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToBusinessPartnerLocationViewModelList() ?? new List<BusinessPartnerLocationViewModel>());
                }
                else
                {
                    response.BusinessPartnerLocations.AddRange(unitOfWork.GetBusinessPartnerLocationRepository()
                        .GetBusinessPartnerLocations(request.CompanyId)
                        ?.ConvertToBusinessPartnerLocationViewModelList() ?? new List<BusinessPartnerLocationViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartnerLocations = new List<BusinessPartnerLocationViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
