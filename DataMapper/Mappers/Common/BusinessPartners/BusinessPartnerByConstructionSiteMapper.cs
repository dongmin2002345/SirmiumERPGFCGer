﻿using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DataMapper.Mappers.ConstructionSites;
using DomainCore.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.BusinessPartners
{
    public static class BusinessPartnerByConstructionSiteMapper
    {
        public static List<BusinessPartnerByConstructionSiteViewModel> ConvertToBusinessPartnerByConstructionSiteViewModelList(this IEnumerable<BusinessPartnerByConstructionSite> businessPartnerByConstructionSites)
        {
            List<BusinessPartnerByConstructionSiteViewModel> businessPartnerByConstructionSiteViewModels = new List<BusinessPartnerByConstructionSiteViewModel>();
            foreach (BusinessPartnerByConstructionSite businessPartnerByConstructionSite in businessPartnerByConstructionSites)
            {
                businessPartnerByConstructionSiteViewModels.Add(businessPartnerByConstructionSite.ConvertToBusinessPartnerByConstructionSiteViewModel());
            }
            return businessPartnerByConstructionSiteViewModels;
        }

        public static BusinessPartnerByConstructionSiteViewModel ConvertToBusinessPartnerByConstructionSiteViewModel(this BusinessPartnerByConstructionSite businessPartnerByConstructionSite)
        {
            BusinessPartnerByConstructionSiteViewModel remedyViewModel = new BusinessPartnerByConstructionSiteViewModel()
            {
                Id = businessPartnerByConstructionSite.Id,
                Identifier = businessPartnerByConstructionSite.Identifier,

                Code = businessPartnerByConstructionSite.Code,

                StartDate = businessPartnerByConstructionSite.StartDate,
                EndDate = businessPartnerByConstructionSite.EndDate,
                RealEndDate = businessPartnerByConstructionSite.RealEndDate,

                MaxNumOfEmployees = businessPartnerByConstructionSite.MaxNumOfEmployees,

                BusinessPartner = businessPartnerByConstructionSite.BusinessPartner?.ConvertToBusinessPartnerViewModelLite(),
                BusinessPartnerCount = businessPartnerByConstructionSite.BusinessPartnerCount,
                ConstructionSite = businessPartnerByConstructionSite.ConstructionSite?.ConvertToConstructionSiteViewModelLite(),

                IsActive = businessPartnerByConstructionSite.Active,

                CreatedBy = businessPartnerByConstructionSite.CreatedBy?.ConvertToUserViewModelLite(),
                Company = businessPartnerByConstructionSite.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = businessPartnerByConstructionSite.UpdatedAt,
                CreatedAt = businessPartnerByConstructionSite.CreatedAt
            };



            return remedyViewModel;
        }

        public static BusinessPartnerByConstructionSiteViewModel ConvertToBusinessPartnerByConstructionSiteViewModelLite(this BusinessPartnerByConstructionSite businessPartnerByConstructionSite)
        {
            BusinessPartnerByConstructionSiteViewModel remedyViewModel = new BusinessPartnerByConstructionSiteViewModel()
            {
                Id = businessPartnerByConstructionSite.Id,
                Identifier = businessPartnerByConstructionSite.Identifier,

                Code = businessPartnerByConstructionSite.Code,

                StartDate = businessPartnerByConstructionSite.StartDate,
                EndDate = businessPartnerByConstructionSite.EndDate,
                RealEndDate = businessPartnerByConstructionSite.RealEndDate,

                BusinessPartnerCount = businessPartnerByConstructionSite.BusinessPartnerCount,

                MaxNumOfEmployees = businessPartnerByConstructionSite.MaxNumOfEmployees,

                IsActive = businessPartnerByConstructionSite.Active,

                UpdatedAt = businessPartnerByConstructionSite.UpdatedAt,
                CreatedAt = businessPartnerByConstructionSite.CreatedAt
            };


            return remedyViewModel;
        }

        public static BusinessPartnerByConstructionSite ConvertToBusinessPartnerByConstructionSite(this BusinessPartnerByConstructionSiteViewModel businessPartnerByConstructionSiteViewModel)
        {
            BusinessPartnerByConstructionSite businessPartnerByConstructionSite = new BusinessPartnerByConstructionSite()
            {
                Id = businessPartnerByConstructionSiteViewModel.Id,
                Identifier = businessPartnerByConstructionSiteViewModel.Identifier,

                Code = businessPartnerByConstructionSiteViewModel.Code,
                StartDate = businessPartnerByConstructionSiteViewModel.StartDate,
                EndDate = businessPartnerByConstructionSiteViewModel.EndDate,
                RealEndDate = businessPartnerByConstructionSiteViewModel.RealEndDate,

                MaxNumOfEmployees = businessPartnerByConstructionSiteViewModel.MaxNumOfEmployees,

                BusinessPartnerId = businessPartnerByConstructionSiteViewModel.BusinessPartner?.Id ?? null,
                BusinessPartnerCount = businessPartnerByConstructionSiteViewModel.BusinessPartnerCount,
                ConstructionSiteId = businessPartnerByConstructionSiteViewModel.ConstructionSite?.Id ?? null,

                CreatedById = businessPartnerByConstructionSiteViewModel.CreatedBy?.Id ?? null,
                CompanyId = businessPartnerByConstructionSiteViewModel.Company?.Id ?? null,

                CreatedAt = businessPartnerByConstructionSiteViewModel.CreatedAt,
                UpdatedAt = businessPartnerByConstructionSiteViewModel.UpdatedAt
            };

            return businessPartnerByConstructionSite;
        }
    }
}
