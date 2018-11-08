using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DataMapper.Mappers.Common.Locations;
using DataMapper.Mappers.Common.Sectors;
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
                InternalCode = businessPartner.InternalCode,
                Name = businessPartner.Name,

                PIB = businessPartner.PIB,
                PIO = businessPartner.PIO,
                PDV = businessPartner.PDV,
                IdentificationNumber = businessPartner.IdentificationNumber,

                Rebate = businessPartner.Rebate,
                DueDate = businessPartner.DueDate,

                WebSite = businessPartner.WebSite,
                ContactPerson = businessPartner.ContactPerson,

                IsInPDV = businessPartner.IsInPDV,

                JBKJS = businessPartner.JBKJS,

                NameGer = businessPartner.NameGer,
                Country = businessPartner.Country?.ConvertToCountryViewModelLite(),
                Sector = businessPartner.Sector?.ConvertToSectorViewModelLite(),
                Agency = businessPartner.Agency?.ConvertToAgencyViewModelLite(),
                TaxNr = businessPartner.TaxNr,
                CommercialNr = businessPartner.CommercialNr,
                ContactPersonGer = businessPartner.ContactPersonGer,

                IsActive = businessPartner.Active,

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
                InternalCode = businessPartner.InternalCode,
                Name = businessPartner.Name,

                PIB = businessPartner.PIB,
                PIO = businessPartner.PIO,
                PDV = businessPartner.PDV,
                IdentificationNumber = businessPartner.IdentificationNumber,

                Rebate = businessPartner.Rebate,
                DueDate = businessPartner.DueDate,

                WebSite = businessPartner.WebSite,
                ContactPerson = businessPartner.ContactPerson,

                IsInPDV = businessPartner.IsInPDV,

                JBKJS = businessPartner.JBKJS,

                NameGer = businessPartner.NameGer,
                TaxNr = businessPartner.TaxNr,
                CommercialNr = businessPartner.CommercialNr,
                ContactPersonGer = businessPartner.ContactPersonGer,

                IsActive = businessPartner.Active,

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
                InternalCode = businessPartnerViewModel.InternalCode,
                Name = businessPartnerViewModel.Name,

                PIB = businessPartnerViewModel.PIB,
                PIO = businessPartnerViewModel.PIO,
                PDV = businessPartnerViewModel.PDV,
                IdentificationNumber = businessPartnerViewModel.IdentificationNumber,

                Rebate = businessPartnerViewModel.Rebate,
                DueDate = businessPartnerViewModel.DueDate,

                WebSite = businessPartnerViewModel.WebSite,
                ContactPerson = businessPartnerViewModel.ContactPerson,

                IsInPDV = businessPartnerViewModel.IsInPDV,

                JBKJS = businessPartnerViewModel.JBKJS,

                NameGer = businessPartnerViewModel.NameGer,
                CountryId = businessPartnerViewModel.Country?.Id ?? null,
                SectorId = businessPartnerViewModel.Sector?.Id ?? null,
                AgencyId = businessPartnerViewModel.Agency?.Id ?? null,
                TaxNr = businessPartnerViewModel.TaxNr,
                CommercialNr = businessPartnerViewModel.CommercialNr,
                ContactPersonGer = businessPartnerViewModel.ContactPersonGer,

                CreatedById = businessPartnerViewModel.CreatedBy?.Id ?? null,
                CompanyId = businessPartnerViewModel.Company?.Id ?? null,

                UpdatedAt = businessPartnerViewModel.UpdatedAt,
                CreatedAt = businessPartnerViewModel.CreatedAt,
            };
            return businessPartner;
        }
    }
}
