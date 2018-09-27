using DataMapper.Mappers.Common.BusinessPartners;
using DomainCore.Common.BusinessPartners;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ServiceCore.Implementations.Common.BusinessPartners
{
    public class BusinessPartnerService : IBusinessPartnerService
    {
        IUnitOfWork unitOfWork;

        public BusinessPartnerService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public BusinessPartnerListResponse GetBusinessPartners()
        {
            BusinessPartnerListResponse response = new BusinessPartnerListResponse();
            try
            {
                response.BusinessPartners = unitOfWork.GetBusinessPartnerRepository()
                    .GetBusinessPartners()
                    .ConvertToBusinessPartnerViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartners = new List<BusinessPartnerViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }


        public BusinessPartnerListResponse GetBusinessPartnersNewerThen(DateTime? lastUpdateTime)
        {
            BusinessPartnerListResponse response = new BusinessPartnerListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.BusinessPartners = unitOfWork.GetBusinessPartnerRepository()
                        .GetBusinessPartnersNewerThen((DateTime)lastUpdateTime)
                        .ConvertToBusinessPartnerViewModelList();
                }
                else
                {
                    response.BusinessPartners = unitOfWork.GetBusinessPartnerRepository()
                        .GetBusinessPartners()
                        .ConvertToBusinessPartnerViewModelList();
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartners = new List<BusinessPartnerViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerResponse Create(BusinessPartnerViewModel businessPartner)
        {
            BusinessPartnerResponse response = new BusinessPartnerResponse();
            try
            {

                BusinessPartner addedBusinessPartner = unitOfWork.GetBusinessPartnerRepository().Create(businessPartner.ConvertToBusinessPartner());
                unitOfWork.Save();
                response.BusinessPartner = addedBusinessPartner.ConvertToBusinessPartnerViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartner = new BusinessPartnerViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerResponse Delete(Guid identifier)
        {
            BusinessPartnerResponse response = new BusinessPartnerResponse();
            try
            {
                BusinessPartner deletedBusinessPartner = unitOfWork.GetBusinessPartnerRepository().Delete(identifier);

                unitOfWork.Save();

                response.BusinessPartner = deletedBusinessPartner.ConvertToBusinessPartnerViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartner = new BusinessPartnerViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerListResponse Sync(SyncBusinessPartnerRequest request)
        {
            BusinessPartnerListResponse response = new BusinessPartnerListResponse();
            try
            {
                response.BusinessPartners = new List<BusinessPartnerViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.BusinessPartners.AddRange(unitOfWork.GetBusinessPartnerRepository()
                        .GetBusinessPartnersNewerThen((DateTime)request.LastUpdatedAt)
                        ?.ConvertToBusinessPartnerViewModelList() ?? new List<BusinessPartnerViewModel>());
                }
                else
                {
                    response.BusinessPartners.AddRange(unitOfWork.GetBusinessPartnerRepository()
                        .GetBusinessPartners()
                        ?.ConvertToBusinessPartnerViewModelList() ?? new List<BusinessPartnerViewModel>());
                }

                List<BusinessPartner> addedBusinessPartners = new List<BusinessPartner>();
                foreach (var remedy in request.UnSyncedBusinessPartners)
                {
                    addedBusinessPartners.Add(unitOfWork.GetBusinessPartnerRepository().Create(remedy.ConvertToBusinessPartner()));
                }

                unitOfWork.Save();

                foreach (var item in addedBusinessPartners)
                {
                    response.BusinessPartners.Add(unitOfWork.GetBusinessPartnerRepository().GetBusinessPartner(item.Id).ConvertToBusinessPartnerViewModel());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartners = new List<BusinessPartnerViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}

