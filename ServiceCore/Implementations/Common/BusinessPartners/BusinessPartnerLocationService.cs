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

        public BusinessPartnerLocationListResponse GetBusinessPartnerLocations(int companyId)
        {
            BusinessPartnerLocationListResponse response = new BusinessPartnerLocationListResponse();
            try
            {
                response.BusinessPartnerLocations = unitOfWork.GetBusinessPartnerLocationRepository()
                    .GetBusinessPartnerLocations(companyId)
                    .ConvertToBusinessPartnerLocationViewModelList();
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

        public BusinessPartnerLocationListResponse GetBusinessPartnerLocationsNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            BusinessPartnerLocationListResponse response = new BusinessPartnerLocationListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.BusinessPartnerLocations = unitOfWork.GetBusinessPartnerLocationRepository()
                        .GetBusinessPartnerLocationsNewerThen(companyId, (DateTime)lastUpdateTime)
                        .ConvertToBusinessPartnerLocationViewModelList();
                }
                else
                {
                    response.BusinessPartnerLocations = unitOfWork.GetBusinessPartnerLocationRepository()
                        .GetBusinessPartnerLocations(companyId)
                        .ConvertToBusinessPartnerLocationViewModelList();
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

        public BusinessPartnerLocationResponse Create(BusinessPartnerLocationViewModel businessPartnerLocationViewModel)
        {
            BusinessPartnerLocationResponse response = new BusinessPartnerLocationResponse();
            try
            {
                BusinessPartnerLocation addedBusinessPartnerLocation = unitOfWork.GetBusinessPartnerLocationRepository().Create(businessPartnerLocationViewModel.ConvertToBusinessPartnerLocation());
                unitOfWork.Save();
                response.BusinessPartnerLocation = addedBusinessPartnerLocation.ConvertToBusinessPartnerLocationViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartnerLocation = new BusinessPartnerLocationViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public BusinessPartnerLocationResponse Delete(Guid identifier)
        {
            BusinessPartnerLocationResponse response = new BusinessPartnerLocationResponse();
            try
            {
                BusinessPartnerLocation deletedBusinessPartnerLocation = unitOfWork.GetBusinessPartnerLocationRepository().Delete(identifier);
                unitOfWork.Save();

                response.BusinessPartnerLocation = deletedBusinessPartnerLocation.ConvertToBusinessPartnerLocationViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartnerLocation = new BusinessPartnerLocationViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
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
