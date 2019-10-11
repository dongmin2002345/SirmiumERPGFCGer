using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DataMapper.Mappers.Common.Locations;
using DomainCore.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.BusinessPartners
{
    public static class BusinessPartnerLocationMapper
    {
        public static List<BusinessPartnerLocationViewModel> ConvertToBusinessPartnerLocationViewModelList(this IEnumerable<BusinessPartnerLocation> businessPartnerLocations)
        {
            List<BusinessPartnerLocationViewModel> businessPartnerLocationViewModels = new List<BusinessPartnerLocationViewModel>();
            foreach (BusinessPartnerLocation businessPartnerLocation in businessPartnerLocations)
            {
                businessPartnerLocationViewModels.Add(businessPartnerLocation.ConvertToBusinessPartnerLocationViewModel());
            }
            return businessPartnerLocationViewModels;
        }

        public static BusinessPartnerLocationViewModel ConvertToBusinessPartnerLocationViewModel(this BusinessPartnerLocation businessPartnerLocation)
        {
            BusinessPartnerLocationViewModel businessPartnerLocationViewModel = new BusinessPartnerLocationViewModel()
            {
                Id = businessPartnerLocation.Id,
                Identifier = businessPartnerLocation.Identifier,

                BusinessPartner = businessPartnerLocation.BusinessPartner?.ConvertToBusinessPartnerViewModelLite(),

                Address = businessPartnerLocation.Address,

                Country = businessPartnerLocation.Country?.ConvertToCountryViewModelLite(),
                City = businessPartnerLocation.City?.ConvertToCityViewModelLite(),
                Municipality = businessPartnerLocation.Municipality?.ConvertToMunicipalityViewModelLite(),
                Region = businessPartnerLocation.Region?.ConvertToRegionViewModelLite(),
                ItemStatus = businessPartnerLocation.ItemStatus,
                IsActive = businessPartnerLocation.Active,

                CreatedBy = businessPartnerLocation.CreatedBy?.ConvertToUserViewModelLite(),
                Company = businessPartnerLocation.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = businessPartnerLocation.UpdatedAt,
                CreatedAt = businessPartnerLocation.CreatedAt,
            };
            return businessPartnerLocationViewModel;
        }

        public static BusinessPartnerLocationViewModel ConvertToBusinessPartnerLocationViewModelLite(this BusinessPartnerLocation businessPartnerLocation)
        {
            BusinessPartnerLocationViewModel businessPartnerLocationViewModel = new BusinessPartnerLocationViewModel()
            {
                Id = businessPartnerLocation.Id,
                Identifier = businessPartnerLocation.Identifier,

                Address = businessPartnerLocation.Address,
                ItemStatus = businessPartnerLocation.ItemStatus,
                IsActive = businessPartnerLocation.Active,

                UpdatedAt = businessPartnerLocation.UpdatedAt,
                CreatedAt = businessPartnerLocation.CreatedAt,
            };
            return businessPartnerLocationViewModel;
        }

        public static BusinessPartnerLocation ConvertToBusinessPartnerLocation(this BusinessPartnerLocationViewModel businessPartnerLocationViewModel)
        {
            BusinessPartnerLocation businessPartnerLocation = new BusinessPartnerLocation()
            {
                Id = businessPartnerLocationViewModel.Id,
                Identifier = businessPartnerLocationViewModel.Identifier,

                BusinessPartnerId = businessPartnerLocationViewModel.BusinessPartner?.Id ?? null,

                Address = businessPartnerLocationViewModel.Address,

                CountryId = businessPartnerLocationViewModel.Country?.Id ?? null,
                CityId = businessPartnerLocationViewModel.City?.Id ?? null,
                MunicipalityId = businessPartnerLocationViewModel.Municipality?.Id ?? null,
                RegionId = businessPartnerLocationViewModel.Region?.Id ?? null,
                ItemStatus = businessPartnerLocationViewModel.ItemStatus,
                CreatedById = businessPartnerLocationViewModel.CreatedBy?.Id ?? null,
                CompanyId = businessPartnerLocationViewModel.Company?.Id ?? null,

                UpdatedAt = businessPartnerLocationViewModel.UpdatedAt,
                CreatedAt = businessPartnerLocationViewModel.CreatedAt,
            };
            return businessPartnerLocation;
        }
    }
}