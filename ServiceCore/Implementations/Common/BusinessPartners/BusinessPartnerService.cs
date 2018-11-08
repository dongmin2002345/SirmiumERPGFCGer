using DataMapper.Mappers.Common.BusinessPartners;
using DomainCore.Common.BusinessPartners;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public BusinessPartnerListResponse GetBusinessPartners(int companyId)
        {
            BusinessPartnerListResponse response = new BusinessPartnerListResponse();
            try
            {
                List<BusinessPartner> businessPartners = unitOfWork.GetBusinessPartnerRepository().GetBusinessPartners(companyId);
                response.BusinessPartners = businessPartners.ConvertToBusinessPartnerViewModelList();
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

        public BusinessPartnerListResponse GetBusinessPartnersNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            BusinessPartnerListResponse response = new BusinessPartnerListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.BusinessPartners = unitOfWork.GetBusinessPartnerRepository()
                        .GetBusinessPartnersNewerThen(companyId, (DateTime)lastUpdateTime)
                        .ConvertToBusinessPartnerViewModelList();
                }
                else
                {
                    response.BusinessPartners = unitOfWork.GetBusinessPartnerRepository()
                        .GetBusinessPartners(companyId)
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

        public BusinessPartnerResponse Create(BusinessPartnerViewModel businessPartnerViewModel)
        {
            BusinessPartnerResponse response = new BusinessPartnerResponse();
            try
            {
                // Backup items
                List<BusinessPartnerLocationViewModel> locations = businessPartnerViewModel.Locations?.ToList() ?? new List<BusinessPartnerLocationViewModel>();
                businessPartnerViewModel.Locations = null;
                //List<BusinessPartnerOrganizationUnitViewModel> organizationUnits = businessPartnerViewModel.OrganizationUnits?.ToList() ?? new List<BusinessPartnerOrganizationUnitViewModel>();
                //businessPartnerViewModel.OrganizationUnits = null;
                List<BusinessPartnerPhoneViewModel> phones = businessPartnerViewModel.Phones?.ToList() ?? new List<BusinessPartnerPhoneViewModel>();
                businessPartnerViewModel.Phones = null;
                List<BusinessPartnerTypeViewModel> businessPartnerTypes = businessPartnerViewModel.BusinessPartnerTypes?.ToList() ?? new List<BusinessPartnerTypeViewModel>();
                businessPartnerViewModel.BusinessPartnerTypes = null;

                // Create business partner
                BusinessPartner createdBusinessPartner = unitOfWork.GetBusinessPartnerRepository()
                    .Create(businessPartnerViewModel.ConvertToBusinessPartner());

                // Update items
                var locationsFromDB = unitOfWork.GetBusinessPartnerLocationRepository().GetBusinessPartnerLocationssByBusinessPartner(createdBusinessPartner.Id);
                foreach (var item in locationsFromDB)
                    if (!locations.Select(x => x.Identifier).Contains(item.Identifier))
                        unitOfWork.GetBusinessPartnerLocationRepository().Delete(item.Identifier);
                foreach (var item in locations)
                {
                    item.BusinessPartner = new BusinessPartnerViewModel() { Id = createdBusinessPartner.Id };
                    unitOfWork.GetBusinessPartnerLocationRepository().Create(item.ConvertToBusinessPartnerLocation());
                }

                //var organizationUnitsFromDB = unitOfWork.GetBusinessPartnerOrganizationUnitRepository().GetBusinessPartnerOrganizationUnitsByBusinessPartner(createdBusinessPartner.Id);
                //foreach (var item in organizationUnitsFromDB)
                //    if (!organizationUnits.Select(x => x.Identifier).Contains(item.Identifier))
                //        unitOfWork.GetBusinessPartnerOrganizationUnitRepository().Delete(item.Identifier);
                //foreach (var item in organizationUnits)
                //{
                //    item.BusinessPartner = new BusinessPartnerViewModel() { Id = createdBusinessPartner.Id };
                //    unitOfWork.GetBusinessPartnerOrganizationUnitRepository().Create(item.ConvertToBusinessPartnerOrganizationUnit());
                //}

                var phonesFromDB = unitOfWork.GetBusinessPartnerPhoneRepository().GetBusinessPartnerPhonesByBusinessPartner(createdBusinessPartner.Id);
                foreach (var item in phonesFromDB)
                    if (!phones.Select(x => x.Identifier).Contains(item.Identifier))
                        unitOfWork.GetBusinessPartnerPhoneRepository().Delete(item.Identifier);
                foreach (var item in phones)
                {
                    item.BusinessPartner = new BusinessPartnerViewModel() { Id = createdBusinessPartner.Id };
                    unitOfWork.GetBusinessPartnerPhoneRepository().Create(item.ConvertToBusinessPartnerPhone());
                }

                unitOfWork.GetBusinessPartnerBusinessPartnerTypeRepository().Delete(createdBusinessPartner.Id);
                foreach (var item in businessPartnerTypes)
                {
                    unitOfWork.GetBusinessPartnerBusinessPartnerTypeRepository().Create(createdBusinessPartner.Id, item.Id);
                }

                unitOfWork.Save();

                response.BusinessPartner = createdBusinessPartner.ConvertToBusinessPartnerViewModel();
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
                        .GetBusinessPartnersNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToBusinessPartnerViewModelList() ?? new List<BusinessPartnerViewModel>());
                }
                else
                {
                    response.BusinessPartners.AddRange(unitOfWork.GetBusinessPartnerRepository()
                        .GetBusinessPartners(request.CompanyId)
                        ?.ConvertToBusinessPartnerViewModelList() ?? new List<BusinessPartnerViewModel>());
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
