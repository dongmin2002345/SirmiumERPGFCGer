using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DataMapper.Mappers.ConstructionSites;
using DomainCore.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.BusinessPartners
{
    public static class BusinessPartnerByConstructionSiteHistoryMapper
    {
        public static List<BusinessPartnerByConstructionSiteHistoryViewModel> ConvertToBusinessPartnerByConstructionSiteHistoryViewModelList(this IEnumerable<BusinessPartnerByConstructionSiteHistory> businessPartnerByConstructionSiteHistories)
        {
            List<BusinessPartnerByConstructionSiteHistoryViewModel> businessPartnerByConstructionSiteHistoryViewModels = new List<BusinessPartnerByConstructionSiteHistoryViewModel>();
            foreach (BusinessPartnerByConstructionSiteHistory businessPartnerByConstructionSiteHistory in businessPartnerByConstructionSiteHistories)
            {
                businessPartnerByConstructionSiteHistoryViewModels.Add(businessPartnerByConstructionSiteHistory.ConvertToBusinessPartnerByConstructionSiteHistoryViewModel());
            }
            return businessPartnerByConstructionSiteHistoryViewModels;
        }

        public static BusinessPartnerByConstructionSiteHistoryViewModel ConvertToBusinessPartnerByConstructionSiteHistoryViewModel(this BusinessPartnerByConstructionSiteHistory businessPartnerByConstructionSiteHistory)
        {
            BusinessPartnerByConstructionSiteHistoryViewModel businessPartnerByConstructionSiteHistoryViewModel = new BusinessPartnerByConstructionSiteHistoryViewModel()
            {
                Id = businessPartnerByConstructionSiteHistory.Id,
                Identifier = businessPartnerByConstructionSiteHistory.Identifier,

                Code = businessPartnerByConstructionSiteHistory.Code,

                StartDate = businessPartnerByConstructionSiteHistory.StartDate,
                EndDate = businessPartnerByConstructionSiteHistory.EndDate,

                BusinessPartner = businessPartnerByConstructionSiteHistory.BusinessPartner?.ConvertToBusinessPartnerViewModelLite(),
                ConstructionSite = businessPartnerByConstructionSiteHistory.ConstructionSite?.ConvertToConstructionSiteViewModelLite(),

                IsActive = businessPartnerByConstructionSiteHistory.Active,

                CreatedBy = businessPartnerByConstructionSiteHistory.CreatedBy?.ConvertToUserViewModelLite(),
                Company = businessPartnerByConstructionSiteHistory.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = businessPartnerByConstructionSiteHistory.UpdatedAt,
                CreatedAt = businessPartnerByConstructionSiteHistory.CreatedAt
            };



            return businessPartnerByConstructionSiteHistoryViewModel;
        }

        public static BusinessPartnerByConstructionSiteHistoryViewModel ConvertToBusinessPartnerByConstructionSiteHistoryViewModelLite(this BusinessPartnerByConstructionSiteHistory businessPartnerByConstructionSiteHistory)
        {
            BusinessPartnerByConstructionSiteHistoryViewModel businessPartnerByConstructionSiteHistoryViewModel = new BusinessPartnerByConstructionSiteHistoryViewModel()
            {
                Id = businessPartnerByConstructionSiteHistory.Id,
                Identifier = businessPartnerByConstructionSiteHistory.Identifier,

                Code = businessPartnerByConstructionSiteHistory.Code,

                StartDate = businessPartnerByConstructionSiteHistory.StartDate,
                EndDate = businessPartnerByConstructionSiteHistory.EndDate,

                IsActive = businessPartnerByConstructionSiteHistory.Active,

                UpdatedAt = businessPartnerByConstructionSiteHistory.UpdatedAt,
                CreatedAt = businessPartnerByConstructionSiteHistory.CreatedAt
            };


            return businessPartnerByConstructionSiteHistoryViewModel;
        }

        public static BusinessPartnerByConstructionSiteHistory ConvertToBusinessPartnerByConstructionSiteHistory(this BusinessPartnerByConstructionSiteHistoryViewModel businessPartnerByConstructionSiteHistoryViewModel)
        {
            BusinessPartnerByConstructionSiteHistory businessPartnerByConstructionSiteHistory = new BusinessPartnerByConstructionSiteHistory()
            {
                Id = businessPartnerByConstructionSiteHistoryViewModel.Id,
                Identifier = businessPartnerByConstructionSiteHistoryViewModel.Identifier,

                Code = businessPartnerByConstructionSiteHistoryViewModel.Code,
                StartDate = businessPartnerByConstructionSiteHistoryViewModel.StartDate,
                EndDate = businessPartnerByConstructionSiteHistoryViewModel.EndDate,

                BusinessPartnerId = businessPartnerByConstructionSiteHistoryViewModel.BusinessPartner?.Id ?? null,
                ConstructionSiteId = businessPartnerByConstructionSiteHistoryViewModel.ConstructionSite?.Id ?? null,

                CreatedById = businessPartnerByConstructionSiteHistoryViewModel.CreatedBy?.Id ?? null,
                CompanyId = businessPartnerByConstructionSiteHistoryViewModel.Company?.Id ?? null,

                CreatedAt = businessPartnerByConstructionSiteHistoryViewModel.CreatedAt,
                UpdatedAt = businessPartnerByConstructionSiteHistoryViewModel.UpdatedAt
            };

            return businessPartnerByConstructionSiteHistory;
        }
    }
}
