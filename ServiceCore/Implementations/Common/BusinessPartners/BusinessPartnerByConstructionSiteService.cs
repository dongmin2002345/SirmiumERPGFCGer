using DataMapper.Mappers.Common.BusinessPartners;
using DomainCore.Common.BusinessPartners;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Common
{
    public class BusinessPartnerByConstructionSiteService : IBusinessPartnerByConstructionSiteService
    {
        IUnitOfWork unitOfWork;

        public BusinessPartnerByConstructionSiteService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public BusinessPartnerByConstructionSiteListResponse GetBusinessPartnerByConstructionSites(int companyId)
        {
            BusinessPartnerByConstructionSiteListResponse response = new BusinessPartnerByConstructionSiteListResponse();
            try
            {
                response.BusinessPartnerByConstructionSites = unitOfWork.GetBusinessPartnerByConstructionSiteRepository().GetBusinessPartnerByConstructionSites(companyId)
               .ConvertToBusinessPartnerByConstructionSiteViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartnerByConstructionSites = new List<BusinessPartnerByConstructionSiteViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerByConstructionSiteListResponse GetBusinessPartnerByConstructionSitesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            BusinessPartnerByConstructionSiteListResponse response = new BusinessPartnerByConstructionSiteListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.BusinessPartnerByConstructionSites = unitOfWork.GetBusinessPartnerByConstructionSiteRepository()
                        .GetBusinessPartnerByConstructionSitesNewerThen(companyId, (DateTime)lastUpdateTime)
                        .ConvertToBusinessPartnerByConstructionSiteViewModelList();
                }
                else
                {
                    response.BusinessPartnerByConstructionSites = unitOfWork.GetBusinessPartnerByConstructionSiteRepository()
                        .GetBusinessPartnerByConstructionSites(companyId)
                        .ConvertToBusinessPartnerByConstructionSiteViewModelList();
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartnerByConstructionSites = new List<BusinessPartnerByConstructionSiteViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerByConstructionSiteResponse Create(BusinessPartnerByConstructionSiteViewModel re)
        {
            BusinessPartnerByConstructionSiteResponse response = new BusinessPartnerByConstructionSiteResponse();
            try
            {
                BusinessPartnerByConstructionSite addedBusinessPartnerByConstructionSite = unitOfWork.GetBusinessPartnerByConstructionSiteRepository().Create(re.ConvertToBusinessPartnerByConstructionSite());
                unitOfWork.Save();

                response.BusinessPartnerByConstructionSite = addedBusinessPartnerByConstructionSite.ConvertToBusinessPartnerByConstructionSiteViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartnerByConstructionSite = new BusinessPartnerByConstructionSiteViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerByConstructionSiteResponse Delete(BusinessPartnerByConstructionSiteViewModel re)
        {
            BusinessPartnerByConstructionSiteResponse response = new BusinessPartnerByConstructionSiteResponse();
            try
            {
                BusinessPartnerByConstructionSite deletedBusinessPartnerByConstructionSite = unitOfWork.GetBusinessPartnerByConstructionSiteRepository().Delete(re.ConvertToBusinessPartnerByConstructionSite());

                unitOfWork.Save();

                response.BusinessPartnerByConstructionSite = deletedBusinessPartnerByConstructionSite.ConvertToBusinessPartnerByConstructionSiteViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartnerByConstructionSite = new BusinessPartnerByConstructionSiteViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerByConstructionSiteListResponse Sync(SyncBusinessPartnerByConstructionSiteRequest request)
        {
            BusinessPartnerByConstructionSiteListResponse response = new BusinessPartnerByConstructionSiteListResponse();
            try
            {
                response.BusinessPartnerByConstructionSites = new List<BusinessPartnerByConstructionSiteViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.BusinessPartnerByConstructionSites.AddRange(unitOfWork.GetBusinessPartnerByConstructionSiteRepository()
                        .GetBusinessPartnerByConstructionSitesNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToBusinessPartnerByConstructionSiteViewModelList() ?? new List<BusinessPartnerByConstructionSiteViewModel>());
                }
                else
                {
                    response.BusinessPartnerByConstructionSites.AddRange(unitOfWork.GetBusinessPartnerByConstructionSiteRepository()
                        .GetBusinessPartnerByConstructionSites(request.CompanyId)
                        ?.ConvertToBusinessPartnerByConstructionSiteViewModelList() ?? new List<BusinessPartnerByConstructionSiteViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartnerByConstructionSites = new List<BusinessPartnerByConstructionSiteViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
