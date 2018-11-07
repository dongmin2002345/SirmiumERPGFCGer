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

        public BusinessPartnerPhoneListResponse GetBusinessPartnerPhones(int companyId)
        {
            BusinessPartnerPhoneListResponse response = new BusinessPartnerPhoneListResponse();
            try
            {
                response.BusinessPartnerPhones = unitOfWork.GetBusinessPartnerPhoneRepository()
                    .GetBusinessPartnerPhones(companyId)
                    .ConvertToBusinessPartnerPhoneViewModelList();
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

        public BusinessPartnerPhoneListResponse GetBusinessPartnerPhonesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            BusinessPartnerPhoneListResponse response = new BusinessPartnerPhoneListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.BusinessPartnerPhones = unitOfWork.GetBusinessPartnerPhoneRepository()
                        .GetBusinessPartnerPhonesNewerThen(companyId, (DateTime)lastUpdateTime)
                        .ConvertToBusinessPartnerPhoneViewModelList();
                }
                else
                {
                    response.BusinessPartnerPhones = unitOfWork.GetBusinessPartnerPhoneRepository()
                        .GetBusinessPartnerPhones(companyId)
                        .ConvertToBusinessPartnerPhoneViewModelList();
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

        public BusinessPartnerPhoneResponse Create(BusinessPartnerPhoneViewModel boxViewModel)
        {
            BusinessPartnerPhoneResponse response = new BusinessPartnerPhoneResponse();
            try
            {
                BusinessPartnerPhone addedBusinessPartnerPhone = unitOfWork.GetBusinessPartnerPhoneRepository().Create(boxViewModel.ConvertToBusinessPartnerPhone());
                unitOfWork.Save();
                response.BusinessPartnerPhone = addedBusinessPartnerPhone.ConvertToBusinessPartnerPhoneViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartnerPhone = new BusinessPartnerPhoneViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public BusinessPartnerPhoneResponse Delete(Guid identifier)
        {
            BusinessPartnerPhoneResponse response = new BusinessPartnerPhoneResponse();
            try
            {
                BusinessPartnerPhone deletedBusinessPartnerPhone = unitOfWork.GetBusinessPartnerPhoneRepository().Delete(identifier);
                unitOfWork.Save();

                response.BusinessPartnerPhone = deletedBusinessPartnerPhone.ConvertToBusinessPartnerPhoneViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartnerPhone = new BusinessPartnerPhoneViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
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
