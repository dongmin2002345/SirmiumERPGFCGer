using DataMapper.Mappers.Common.Identity;
using DomainCore.Common.BusinessPartners;
using DomainCore.Common.Identity;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.BusinessPartners
{
    public static class BusinessPartnerMapper
    {
        public static List<BusinessPartnerViewModel> ConvertToBusinessPartnerViewModelList(this IEnumerable<BusinessPartner> businessPartners)
        {
            List<BusinessPartnerViewModel> businessPartnerViewModels = new List<BusinessPartnerViewModel>();
            foreach (BusinessPartner businessPartner in businessPartners)
            {
                businessPartnerViewModels.Add(businessPartner.ConvertToBusinessPartnerViewModel());
            }
            return businessPartnerViewModels;
        }

        public static List<BusinessPartnerViewModel> ConvertToBusinessPartnerViewModelListLite(this IEnumerable<BusinessPartner> businessPartners)
        {
            List<BusinessPartnerViewModel> businessPartnerViewModels = new List<BusinessPartnerViewModel>();
            foreach (BusinessPartner businessPartner in businessPartners)
            {
                businessPartnerViewModels.Add(businessPartner.ConvertToBusinessPartnerViewModelLite());
            }
            return businessPartnerViewModels;
        }

        public static BusinessPartnerViewModel ConvertToBusinessPartnerViewModel(this BusinessPartner businessPartner)
        {
            BusinessPartnerViewModel businessPartnerViewModel = new BusinessPartnerViewModel()
            {
                Id = businessPartner.Id,
                Identifier = businessPartner.Identifier,

                Code = businessPartner.Code,
                Name = businessPartner.Name,

                Director = businessPartner.Director,

                Address = businessPartner.Address,
                InoAddress = businessPartner.InoAddress,

                PIB = businessPartner.PIB,
                MatCode = businessPartner.MatCode,

                Mobile = businessPartner.Mobile,
                Phone = businessPartner.Phone,
                Email = businessPartner.Email,

                ActivityCode = businessPartner.ActivityCode,

                BankAccountNumber = businessPartner.BankAccountNumber,

                OpeningDate = businessPartner.OpeningDate,
                BranchOpeningDate = businessPartner.BranchOpeningDate,

                UpdatedAt = businessPartner.UpdatedAt,
                CreatedBy = businessPartner.CreatedBy?.ConvertToUserViewModelLite(),
                CreatedAt = businessPartner.CreatedAt
            };

            return businessPartnerViewModel;
        }

        public static BusinessPartnerViewModel ConvertToBusinessPartnerViewModelLite(this BusinessPartner businessPartner)
        {
            BusinessPartnerViewModel businessPartnerViewModel = new BusinessPartnerViewModel()
            {
                Id = businessPartner.Id,
                Identifier = businessPartner.Identifier,

                Code = businessPartner.Code,
                Name = businessPartner.Name,

                Director = businessPartner.Director,

                Address = businessPartner.Address,
                InoAddress = businessPartner.InoAddress,

                PIB = businessPartner.PIB,
                MatCode = businessPartner.MatCode,

                Mobile = businessPartner.Mobile,
                Phone = businessPartner.Phone,
                Email = businessPartner.Email,

                ActivityCode = businessPartner.ActivityCode,

                BankAccountNumber = businessPartner.BankAccountNumber,

                OpeningDate = businessPartner.OpeningDate,
                BranchOpeningDate = businessPartner.BranchOpeningDate,

                UpdatedAt = businessPartner.UpdatedAt,
                CreatedAt = businessPartner.CreatedAt
            };
            return businessPartnerViewModel;
        }

        public static BusinessPartner ConvertToBusinessPartner(this BusinessPartnerViewModel businessPartnerViewModel)
        {
            BusinessPartner businessPartner = new BusinessPartner()
            {
                Id = businessPartnerViewModel.Id,
                Identifier = businessPartnerViewModel.Identifier,

                Code = businessPartnerViewModel.Code,
                Name = businessPartnerViewModel.Name,

                Director = businessPartnerViewModel.Director,

                Address = businessPartnerViewModel.Address,
                InoAddress = businessPartnerViewModel.InoAddress,

                PIB = businessPartnerViewModel.PIB,
                MatCode = businessPartnerViewModel.MatCode,

                Mobile = businessPartnerViewModel.Mobile,
                Phone = businessPartnerViewModel.Phone,
                Email = businessPartnerViewModel.Email,

                ActivityCode = businessPartnerViewModel.ActivityCode,

                BankAccountNumber = businessPartnerViewModel.BankAccountNumber,
                
                OpeningDate = businessPartnerViewModel.OpeningDate,
                BranchOpeningDate = businessPartnerViewModel.BranchOpeningDate,

                CreatedById = businessPartnerViewModel.CreatedBy?.Id ?? null,
                CreatedAt = businessPartnerViewModel.CreatedAt
            };
            return businessPartner;

        }
    }
}
