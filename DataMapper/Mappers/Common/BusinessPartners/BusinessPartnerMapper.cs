using DataMapper.Mappers.Common.Companies;
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

        public static BusinessPartnerViewModel ConvertToBusinessPartnerViewModel(this BusinessPartner businessPartner)
        {
            BusinessPartnerViewModel businessPartnerViewModel = new BusinessPartnerViewModel()
            {
                Id = businessPartner.Id,
                Identifier = businessPartner.Identifier,

                Code = businessPartner.Code,
                Name = businessPartner.Name,

                PIB = businessPartner.PIB,
                PIO = businessPartner.PIO,
                PDV = businessPartner.PDV,
                IndustryCode = businessPartner.IndustryCode,
                IdentificationNumber = businessPartner.IdentificationNumber,

                Rebate = businessPartner.Rebate,
                DueDate = businessPartner.DueDate,

                WebSite = businessPartner.WebSite,
                ContactPerson = businessPartner.ContactPerson,

                IsInPDV = businessPartner.IsInPDV,

                JBKJS = businessPartner.JBKJS,

                CreatedBy = businessPartner.CreatedBy?.ConvertToUserViewModelLite(),
                Company = businessPartner.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = businessPartner.UpdatedAt,
                CreatedAt = businessPartner.CreatedAt,
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

                PIB = businessPartner.PIB,
                PIO = businessPartner.PIO,
                PDV = businessPartner.PDV,
                IndustryCode = businessPartner.IndustryCode,
                IdentificationNumber = businessPartner.IdentificationNumber,

                Rebate = businessPartner.Rebate,
                DueDate = businessPartner.DueDate,

                WebSite = businessPartner.WebSite,
                ContactPerson = businessPartner.ContactPerson,

                IsInPDV = businessPartner.IsInPDV,

                JBKJS = businessPartner.JBKJS,

                UpdatedAt = businessPartner.UpdatedAt,
                CreatedAt = businessPartner.CreatedAt,
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

                PIB = businessPartnerViewModel.PIB,
                PIO = businessPartnerViewModel.PIO,
                PDV = businessPartnerViewModel.PDV,
                IndustryCode = businessPartnerViewModel.IndustryCode,
                IdentificationNumber = businessPartnerViewModel.IdentificationNumber,

                Rebate = businessPartnerViewModel.Rebate,
                DueDate = businessPartnerViewModel.DueDate,

                WebSite = businessPartnerViewModel.WebSite,
                ContactPerson = businessPartnerViewModel.ContactPerson,

                IsInPDV = businessPartnerViewModel.IsInPDV,

                JBKJS = businessPartnerViewModel.JBKJS,

                CreatedById = businessPartnerViewModel.CreatedBy?.Id ?? null,
                CompanyId = businessPartnerViewModel.Company?.Id ?? null,

                UpdatedAt = businessPartnerViewModel.UpdatedAt,
                CreatedAt = businessPartnerViewModel.CreatedAt,
            };
            return businessPartner;
        }
    }
}
