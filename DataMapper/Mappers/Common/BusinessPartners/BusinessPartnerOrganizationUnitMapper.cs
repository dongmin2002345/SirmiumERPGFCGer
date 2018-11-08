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
    public static class BusinessPartnerOrganizationUnitMapper
    {
        public static List<BusinessPartnerOrganizationUnitViewModel> ConvertToBusinessPartnerOrganizationUnitViewModelList(this IEnumerable<BusinessPartnerOrganizationUnit> businessPartnerOrganizationUnits)
        {
            List<BusinessPartnerOrganizationUnitViewModel> businessPartnerOrganizationUnitViewModels = new List<BusinessPartnerOrganizationUnitViewModel>();
            foreach (BusinessPartnerOrganizationUnit businessPartnerOrganizationUnit in businessPartnerOrganizationUnits)
            {
                businessPartnerOrganizationUnitViewModels.Add(businessPartnerOrganizationUnit.ConvertToBusinessPartnerOrganizationUnitViewModel());
            }
            return businessPartnerOrganizationUnitViewModels;
        }

        public static BusinessPartnerOrganizationUnitViewModel ConvertToBusinessPartnerOrganizationUnitViewModel(this BusinessPartnerOrganizationUnit businessPartnerOrganizationUnit)
        {
            BusinessPartnerOrganizationUnitViewModel businessPartnerOrganizationUnitViewModel = new BusinessPartnerOrganizationUnitViewModel()
            {
                Id = businessPartnerOrganizationUnit.Id,
                Identifier = businessPartnerOrganizationUnit.Identifier,

                BusinessPartner = businessPartnerOrganizationUnit.BusinessPartner?.ConvertToBusinessPartnerViewModelLite(),

                Code = businessPartnerOrganizationUnit.Code,
                Name = businessPartnerOrganizationUnit.Name,

                Address = businessPartnerOrganizationUnit.Address,

                Country = businessPartnerOrganizationUnit.Country?.ConvertToCountryViewModelLite(),
                City = businessPartnerOrganizationUnit.City?.ConvertToCityViewModelLite(),
                Municipality = businessPartnerOrganizationUnit.Municipality?.ConvertToMunicipalityViewModelLite(),

                ContactPerson = businessPartnerOrganizationUnit.ContactPerson,
                Phone = businessPartnerOrganizationUnit.Phone,
                Mobile = businessPartnerOrganizationUnit.Mobile,

                IsActive = businessPartnerOrganizationUnit.Active,

                CreatedBy = businessPartnerOrganizationUnit.CreatedBy?.ConvertToUserViewModelLite(),
                Company = businessPartnerOrganizationUnit.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = businessPartnerOrganizationUnit.UpdatedAt,
                CreatedAt = businessPartnerOrganizationUnit.CreatedAt,
            };
            return businessPartnerOrganizationUnitViewModel;
        }

        public static BusinessPartnerOrganizationUnitViewModel ConvertToBusinessPartnerOrganizationUnitViewModelLite(this BusinessPartnerOrganizationUnit businessPartnerOrganizationUnit)
        {
            BusinessPartnerOrganizationUnitViewModel businessPartnerOrganizationUnitViewModel = new BusinessPartnerOrganizationUnitViewModel()
            {
                Id = businessPartnerOrganizationUnit.Id,
                Identifier = businessPartnerOrganizationUnit.Identifier,

                Code = businessPartnerOrganizationUnit.Code,
                Name = businessPartnerOrganizationUnit.Name,

                Address = businessPartnerOrganizationUnit.Address,

                ContactPerson = businessPartnerOrganizationUnit.ContactPerson,
                Phone = businessPartnerOrganizationUnit.Phone,
                Mobile = businessPartnerOrganizationUnit.Mobile,

                IsActive = businessPartnerOrganizationUnit.Active,

                UpdatedAt = businessPartnerOrganizationUnit.UpdatedAt,
                CreatedAt = businessPartnerOrganizationUnit.CreatedAt,
            };
            return businessPartnerOrganizationUnitViewModel;
        }

        public static BusinessPartnerOrganizationUnit ConvertToBusinessPartnerOrganizationUnit(this BusinessPartnerOrganizationUnitViewModel businessPartnerOrganizationUnitViewModel)
        {
            BusinessPartnerOrganizationUnit businessPartnerOrganizationUnit = new BusinessPartnerOrganizationUnit()
            {
                Id = businessPartnerOrganizationUnitViewModel.Id,
                Identifier = businessPartnerOrganizationUnitViewModel.Identifier,

                BusinessPartnerId = businessPartnerOrganizationUnitViewModel.BusinessPartner?.Id ?? null,

                Code = businessPartnerOrganizationUnitViewModel.Code,
                Name = businessPartnerOrganizationUnitViewModel.Name,

                Address = businessPartnerOrganizationUnitViewModel.Address,

                CountryId = businessPartnerOrganizationUnitViewModel.Country?.Id ?? null,
                CityId = businessPartnerOrganizationUnitViewModel.City?.Id ?? null,
                MunicipalityId = businessPartnerOrganizationUnitViewModel.Municipality?.Id ?? null,

                ContactPerson = businessPartnerOrganizationUnitViewModel.ContactPerson,
                Phone = businessPartnerOrganizationUnitViewModel.Phone,
                Mobile = businessPartnerOrganizationUnitViewModel.Mobile,

                CreatedById = businessPartnerOrganizationUnitViewModel.CreatedBy?.Id ?? null,
                CompanyId = businessPartnerOrganizationUnitViewModel.Company?.Id ?? null,

                UpdatedAt = businessPartnerOrganizationUnitViewModel.UpdatedAt,
                CreatedAt = businessPartnerOrganizationUnitViewModel.CreatedAt,
            };
            return businessPartnerOrganizationUnit;
        }
    }
}
