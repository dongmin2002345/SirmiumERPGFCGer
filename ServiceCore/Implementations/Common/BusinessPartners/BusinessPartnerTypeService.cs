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
    public class BusinessPartnerTypeService : IBusinessPartnerTypeService
    {
        IUnitOfWork unitOfWork;

        public BusinessPartnerTypeService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public BusinessPartnerTypeListResponse GetBusinessPartnerTypes(int companyId)
        {
            BusinessPartnerTypeListResponse response = new BusinessPartnerTypeListResponse();
            try
            {
                response.BusinessPartnerTypes = unitOfWork.GetBusinessPartnerTypeRepository().GetBusinessPartnerTypes(companyId)
               .ConvertToBusinessPartnerTypeViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartnerTypes = new List<BusinessPartnerTypeViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerTypeListResponse GetBusinessPartnerTypesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            BusinessPartnerTypeListResponse response = new BusinessPartnerTypeListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.BusinessPartnerTypes = unitOfWork.GetBusinessPartnerTypeRepository()
                        .GetBusinessPartnerTypesNewerThen(companyId, (DateTime)lastUpdateTime)
                        .ConvertToBusinessPartnerTypeViewModelList();
                }
                else
                {
                    response.BusinessPartnerTypes = unitOfWork.GetBusinessPartnerTypeRepository()
                        .GetBusinessPartnerTypes(companyId)
                        .ConvertToBusinessPartnerTypeViewModelList();
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartnerTypes = new List<BusinessPartnerTypeViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerTypeResponse Create(BusinessPartnerTypeViewModel re)
        {
            BusinessPartnerTypeResponse response = new BusinessPartnerTypeResponse();
            try
            {
                BusinessPartnerType addedBusinessPartnerType = unitOfWork.GetBusinessPartnerTypeRepository().Create(re.ConvertToBusinessPartnerType());
                unitOfWork.Save();
                response.BusinessPartnerType = addedBusinessPartnerType.ConvertToBusinessPartnerTypeViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartnerType = new BusinessPartnerTypeViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerTypeResponse Delete(Guid identifier)
        {
            BusinessPartnerTypeResponse response = new BusinessPartnerTypeResponse();
            try
            {
                BusinessPartnerType deletedBusinessPartnerType = unitOfWork.GetBusinessPartnerTypeRepository().Delete(identifier);

                unitOfWork.Save();

                response.BusinessPartnerType = deletedBusinessPartnerType.ConvertToBusinessPartnerTypeViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartnerType = new BusinessPartnerTypeViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerTypeListResponse Sync(SyncBusinessPartnerTypeRequest request)
        {
            BusinessPartnerTypeListResponse response = new BusinessPartnerTypeListResponse();
            try
            {
                response.BusinessPartnerTypes = new List<BusinessPartnerTypeViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.BusinessPartnerTypes.AddRange(unitOfWork.GetBusinessPartnerTypeRepository()
                        .GetBusinessPartnerTypesNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToBusinessPartnerTypeViewModelList() ?? new List<BusinessPartnerTypeViewModel>());
                }
                else
                {
                    response.BusinessPartnerTypes.AddRange(unitOfWork.GetBusinessPartnerTypeRepository()
                        .GetBusinessPartnerTypes(request.CompanyId)
                        ?.ConvertToBusinessPartnerTypeViewModelList() ?? new List<BusinessPartnerTypeViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartnerTypes = new List<BusinessPartnerTypeViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
