using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.BusinessPartners
{
    public static class BusinessPartnerTypeMapper
    {
        public static List<BusinessPartnerTypeViewModel> ConvertToBusinessPartnerTypeViewModelList(this IEnumerable<BusinessPartnerType> businessPartnerTypes)
        {
            List<BusinessPartnerTypeViewModel> businessPartnerTypeViewModels = new List<BusinessPartnerTypeViewModel>();
            foreach (BusinessPartnerType businessPartnerType in businessPartnerTypes)
            {
                businessPartnerTypeViewModels.Add(businessPartnerType.ConvertToBusinessPartnerTypeViewModel());
            }
            return businessPartnerTypeViewModels;
        }

        public static BusinessPartnerTypeViewModel ConvertToBusinessPartnerTypeViewModel(this BusinessPartnerType businessPartnerType)
        {
            BusinessPartnerTypeViewModel businessPartnerTypeViewModel = new BusinessPartnerTypeViewModel()
            {
                Id = businessPartnerType.Id,
                Identifier = businessPartnerType.Identifier,

                Code = businessPartnerType.Code,
                Name = businessPartnerType.Name,

                IsBuyer = businessPartnerType.IsBuyer,
                IsSupplier = businessPartnerType.IsSupplier,

                CreatedBy = businessPartnerType.CreatedBy?.ConvertToUserViewModelLite(),
                Company = businessPartnerType.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = businessPartnerType.UpdatedAt,
                CreatedAt = businessPartnerType.CreatedAt
            };
            return businessPartnerTypeViewModel;
        }

        public static BusinessPartnerTypeViewModel ConvertToBusinessPartnerTypeViewModelLite(this BusinessPartnerType businessPartnerType)
        {
            BusinessPartnerTypeViewModel businessPartnerTypeViewModel = new BusinessPartnerTypeViewModel()
            {
                Id = businessPartnerType.Id,
                Identifier = businessPartnerType.Identifier,

                Code = businessPartnerType.Code,
                Name = businessPartnerType.Name,

                IsBuyer = businessPartnerType.IsBuyer,
                IsSupplier = businessPartnerType.IsSupplier,

                UpdatedAt = businessPartnerType.UpdatedAt,
                CreatedAt = businessPartnerType.CreatedAt
            };
            return businessPartnerTypeViewModel;
        }

        public static BusinessPartnerType ConvertToBusinessPartnerType(this BusinessPartnerTypeViewModel businessPartnerTypeViewModel)
        {
            BusinessPartnerType businessPartnerType = new BusinessPartnerType()
            {
                Id = businessPartnerTypeViewModel.Id,
                Identifier = businessPartnerTypeViewModel.Identifier,

                Code = businessPartnerTypeViewModel.Code,
                Name = businessPartnerTypeViewModel.Name,

                IsBuyer = businessPartnerTypeViewModel.IsBuyer,
                IsSupplier = businessPartnerTypeViewModel.IsSupplier,

                CreatedById = businessPartnerTypeViewModel.CreatedBy?.Id ?? null,
                CompanyId = businessPartnerTypeViewModel.Company?.Id ?? null,

                CreatedAt = businessPartnerTypeViewModel.CreatedAt,
                UpdatedAt = businessPartnerTypeViewModel.UpdatedAt
            };
            return businessPartnerType;
        }
    }
}
