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
    public class BusinessPartnerOrganizationUnitService : IBusinessPartnerOrganizationUnitService
    {
        IUnitOfWork unitOfWork;

        public BusinessPartnerOrganizationUnitService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public BusinessPartnerOrganizationUnitListResponse GetBusinessPartnerOrganizationUnits(int companyId)
        {
            BusinessPartnerOrganizationUnitListResponse response = new BusinessPartnerOrganizationUnitListResponse();
            try
            {
                response.BusinessPartnerOrganizationUnits = unitOfWork.GetBusinessPartnerOrganizationUnitRepository()
                    .GetBusinessPartnerOrganizationUnits(companyId)
                    .ConvertToBusinessPartnerOrganizationUnitViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartnerOrganizationUnits = new List<BusinessPartnerOrganizationUnitViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public BusinessPartnerOrganizationUnitListResponse GetBusinessPartnerOrganizationUnitsNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            BusinessPartnerOrganizationUnitListResponse response = new BusinessPartnerOrganizationUnitListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.BusinessPartnerOrganizationUnits = unitOfWork.GetBusinessPartnerOrganizationUnitRepository()
                        .GetBusinessPartnerOrganizationUnitsNewerThen(companyId, (DateTime)lastUpdateTime)
                        .ConvertToBusinessPartnerOrganizationUnitViewModelList();
                }
                else
                {
                    response.BusinessPartnerOrganizationUnits = unitOfWork.GetBusinessPartnerOrganizationUnitRepository()
                        .GetBusinessPartnerOrganizationUnits(companyId)
                        .ConvertToBusinessPartnerOrganizationUnitViewModelList();
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartnerOrganizationUnits = new List<BusinessPartnerOrganizationUnitViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public BusinessPartnerOrganizationUnitResponse Create(BusinessPartnerOrganizationUnitViewModel businessPartnerOrganizationUnitViewModel)
        {
            BusinessPartnerOrganizationUnitResponse response = new BusinessPartnerOrganizationUnitResponse();
            try
            {
                BusinessPartnerOrganizationUnit addedBusinessPartnerOrganizationUnit = unitOfWork.GetBusinessPartnerOrganizationUnitRepository().Create(businessPartnerOrganizationUnitViewModel.ConvertToBusinessPartnerOrganizationUnit());
                unitOfWork.Save();
                response.BusinessPartnerOrganizationUnit = addedBusinessPartnerOrganizationUnit.ConvertToBusinessPartnerOrganizationUnitViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartnerOrganizationUnit = new BusinessPartnerOrganizationUnitViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public BusinessPartnerOrganizationUnitResponse Delete(Guid identifier)
        {
            BusinessPartnerOrganizationUnitResponse response = new BusinessPartnerOrganizationUnitResponse();
            try
            {
                BusinessPartnerOrganizationUnit deletedBusinessPartnerOrganizationUnit = unitOfWork.GetBusinessPartnerOrganizationUnitRepository().Delete(identifier);
                unitOfWork.Save();

                response.BusinessPartnerOrganizationUnit = deletedBusinessPartnerOrganizationUnit.ConvertToBusinessPartnerOrganizationUnitViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartnerOrganizationUnit = new BusinessPartnerOrganizationUnitViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public BusinessPartnerOrganizationUnitListResponse Sync(SyncBusinessPartnerOrganizationUnitRequest request)
        {
            BusinessPartnerOrganizationUnitListResponse response = new BusinessPartnerOrganizationUnitListResponse();
            try
            {
                response.BusinessPartnerOrganizationUnits = new List<BusinessPartnerOrganizationUnitViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.BusinessPartnerOrganizationUnits.AddRange(unitOfWork.GetBusinessPartnerOrganizationUnitRepository()
                        .GetBusinessPartnerOrganizationUnitsNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToBusinessPartnerOrganizationUnitViewModelList() ?? new List<BusinessPartnerOrganizationUnitViewModel>());
                }
                else
                {
                    response.BusinessPartnerOrganizationUnits.AddRange(unitOfWork.GetBusinessPartnerOrganizationUnitRepository()
                        .GetBusinessPartnerOrganizationUnits(request.CompanyId)
                        ?.ConvertToBusinessPartnerOrganizationUnitViewModelList() ?? new List<BusinessPartnerOrganizationUnitViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartnerOrganizationUnits = new List<BusinessPartnerOrganizationUnitViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
