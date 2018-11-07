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

        public BusinessPartnerBankListResponse GetBusinessPartnerBanks(int companyId)
        {
            BusinessPartnerBankListResponse response = new BusinessPartnerBankListResponse();
            try
            {
                response.BusinessPartnerBanks = unitOfWork.GetBusinessPartnerBankRepository()
                    .GetBusinessPartnerBanks(companyId)
                    .ConvertToBusinessPartnerBankViewModelList();
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

        public BusinessPartnerBankListResponse GetBusinessPartnerBanksNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            BusinessPartnerBankListResponse response = new BusinessPartnerBankListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.BusinessPartnerBanks = unitOfWork.GetBusinessPartnerBankRepository()
                        .GetBusinessPartnerBanksNewerThen(companyId, (DateTime)lastUpdateTime)
                        .ConvertToBusinessPartnerBankViewModelList();
                }
                else
                {
                    response.BusinessPartnerBanks = unitOfWork.GetBusinessPartnerBankRepository()
                        .GetBusinessPartnerBanks(companyId)
                        .ConvertToBusinessPartnerBankViewModelList();
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

        public BusinessPartnerBankResponse Create(BusinessPartnerBankViewModel boxViewModel)
        {
            BusinessPartnerBankResponse response = new BusinessPartnerBankResponse();
            try
            {
                BusinessPartnerBank addedBusinessPartnerBank = unitOfWork.GetBusinessPartnerBankRepository().Create(boxViewModel.ConvertToBusinessPartnerBank());
                unitOfWork.Save();
                response.BusinessPartnerBank = addedBusinessPartnerBank.ConvertToBusinessPartnerBankViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartnerBank = new BusinessPartnerBankViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public BusinessPartnerBankResponse Delete(Guid identifier)
        {
            BusinessPartnerBankResponse response = new BusinessPartnerBankResponse();
            try
            {
                BusinessPartnerBank deletedBusinessPartnerBank = unitOfWork.GetBusinessPartnerBankRepository().Delete(identifier);
                unitOfWork.Save();

                response.BusinessPartnerBank = deletedBusinessPartnerBank.ConvertToBusinessPartnerBankViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartnerBank = new BusinessPartnerBankViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
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
