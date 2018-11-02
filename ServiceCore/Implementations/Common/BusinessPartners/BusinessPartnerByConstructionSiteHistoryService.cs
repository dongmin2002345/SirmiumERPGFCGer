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
    public class BusinessPartnerByConstructionSiteHistoryService : IBusinessPartnerByConstructionSiteHistoryService
    {
        IUnitOfWork unitOfWork;

        public BusinessPartnerByConstructionSiteHistoryService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public BusinessPartnerByConstructionSiteHistoryListResponse GetBusinessPartnerByConstructionSiteHistories(int companyId)
        {
            BusinessPartnerByConstructionSiteHistoryListResponse response = new BusinessPartnerByConstructionSiteHistoryListResponse();
            try
            {
                response.BusinessPartnerByConstructionSiteHistories = unitOfWork.GetBusinessPartnerByConstructionSiteHistoryRepository().GetBusinessPartnerByConstructionSiteHistories(companyId)
               .ConvertToBusinessPartnerByConstructionSiteHistoryViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartnerByConstructionSiteHistories = new List<BusinessPartnerByConstructionSiteHistoryViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerByConstructionSiteHistoryListResponse GetBusinessPartnerByConstructionSiteHistoriesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            BusinessPartnerByConstructionSiteHistoryListResponse response = new BusinessPartnerByConstructionSiteHistoryListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.BusinessPartnerByConstructionSiteHistories = unitOfWork.GetBusinessPartnerByConstructionSiteHistoryRepository()
                        .GetBusinessPartnerByConstructionSiteHistoriesNewerThen(companyId, (DateTime)lastUpdateTime)
                        .ConvertToBusinessPartnerByConstructionSiteHistoryViewModelList();
                }
                else
                {
                    response.BusinessPartnerByConstructionSiteHistories = unitOfWork.GetBusinessPartnerByConstructionSiteHistoryRepository()
                        .GetBusinessPartnerByConstructionSiteHistories(companyId)
                        .ConvertToBusinessPartnerByConstructionSiteHistoryViewModelList();
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartnerByConstructionSiteHistories = new List<BusinessPartnerByConstructionSiteHistoryViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerByConstructionSiteHistoryResponse Create(BusinessPartnerByConstructionSiteHistoryViewModel re)
        {
            BusinessPartnerByConstructionSiteHistoryResponse response = new BusinessPartnerByConstructionSiteHistoryResponse();
            try
            {
                BusinessPartnerByConstructionSiteHistory addedBusinessPartnerByConstructionSiteHistory = unitOfWork.GetBusinessPartnerByConstructionSiteHistoryRepository().Create(re.ConvertToBusinessPartnerByConstructionSiteHistory());
                unitOfWork.Save();
                response.BusinessPartnerByConstructionSiteHistory = addedBusinessPartnerByConstructionSiteHistory.ConvertToBusinessPartnerByConstructionSiteHistoryViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartnerByConstructionSiteHistory = new BusinessPartnerByConstructionSiteHistoryViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerByConstructionSiteHistoryResponse Delete(Guid identifier)
        {
            BusinessPartnerByConstructionSiteHistoryResponse response = new BusinessPartnerByConstructionSiteHistoryResponse();
            try
            {
                BusinessPartnerByConstructionSiteHistory deletedBusinessPartnerByConstructionSiteHistory = unitOfWork.GetBusinessPartnerByConstructionSiteHistoryRepository().Delete(identifier);

                unitOfWork.Save();

                response.BusinessPartnerByConstructionSiteHistory = deletedBusinessPartnerByConstructionSiteHistory.ConvertToBusinessPartnerByConstructionSiteHistoryViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartnerByConstructionSiteHistory = new BusinessPartnerByConstructionSiteHistoryViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerByConstructionSiteHistoryListResponse Sync(SyncBusinessPartnerByConstructionSiteHistoryRequest request)
        {
            BusinessPartnerByConstructionSiteHistoryListResponse response = new BusinessPartnerByConstructionSiteHistoryListResponse();
            try
            {
                response.BusinessPartnerByConstructionSiteHistories = new List<BusinessPartnerByConstructionSiteHistoryViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.BusinessPartnerByConstructionSiteHistories.AddRange(unitOfWork.GetBusinessPartnerByConstructionSiteHistoryRepository()
                        .GetBusinessPartnerByConstructionSiteHistoriesNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToBusinessPartnerByConstructionSiteHistoryViewModelList() ?? new List<BusinessPartnerByConstructionSiteHistoryViewModel>());
                }
                else
                {
                    response.BusinessPartnerByConstructionSiteHistories.AddRange(unitOfWork.GetBusinessPartnerByConstructionSiteHistoryRepository()
                        .GetBusinessPartnerByConstructionSiteHistories(request.CompanyId)
                        ?.ConvertToBusinessPartnerByConstructionSiteHistoryViewModelList() ?? new List<BusinessPartnerByConstructionSiteHistoryViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartnerByConstructionSiteHistories = new List<BusinessPartnerByConstructionSiteHistoryViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
