using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DataMapper.Mappers.Common.Locations;
using DataMapper.Mappers.Common.Prices;
using DataMapper.Mappers.Common.Sectors;
using DataMapper.Mappers.Common.TaxAdministrations;
using DataMapper.Mappers.Vats;
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
                IdentificationNumber = businessPartner.IdentificationNumber,

                DueDate = businessPartner.DueDate,

                WebSite = businessPartner.WebSite,
                ContactPerson = businessPartner.ContactPerson,

                IsInPDV = businessPartner.IsInPDV,

                JBKJS = businessPartner.JBKJS,

                NameGer = businessPartner.NameGer,
                IsInPDVGer = businessPartner.IsInPDVGer,
                TaxAdministration = businessPartner.TaxAdministration?.ConvertToTaxAdministrationViewModelLite(), 
                IBAN = businessPartner.IBAN, 
                BetriebsNumber = businessPartner.BetriebsNumber,
                Customer = businessPartner.Customer,
                Country = businessPartner.Country?.ConvertToCountryViewModelLite(),
                Sector = businessPartner.Sector?.ConvertToSectorViewModelLite(),
                Agency = businessPartner.Agency?.ConvertToAgencyViewModelLite(),
                Vat = businessPartner.Vat?.ConvertToVatViewModelLite(),
                Discount = businessPartner.Discount?.ConvertToDiscountViewModelLite(),
                TaxNr = businessPartner.TaxNr,
                CommercialNr = businessPartner.CommercialNr,
                ContactPersonGer = businessPartner.ContactPersonGer,

                VatDeductionFrom = businessPartner.VatDeductionFrom, 
                VatDeductionTo = businessPartner.VatDeductionTo,

                PdvType = businessPartner.PdvType,

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
                IdentificationNumber = businessPartner.IdentificationNumber,

                DueDate = businessPartner.DueDate,

                WebSite = businessPartner.WebSite,
                ContactPerson = businessPartner.ContactPerson,

                IsInPDV = businessPartner.IsInPDV,

                JBKJS = businessPartner.JBKJS,

                NameGer = businessPartner.NameGer,
                IsInPDVGer = businessPartner.IsInPDVGer,
                IBAN = businessPartner.IBAN,
                BetriebsNumber = businessPartner.BetriebsNumber,
                Customer = businessPartner.Customer,
                TaxNr = businessPartner.TaxNr,
                CommercialNr = businessPartner.CommercialNr,
                ContactPersonGer = businessPartner.ContactPersonGer,

                VatDeductionFrom = businessPartner.VatDeductionFrom,
                VatDeductionTo = businessPartner.VatDeductionTo,

                PdvType = businessPartner.PdvType,

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
                IdentificationNumber = businessPartnerViewModel.IdentificationNumber,

                DueDate = businessPartnerViewModel.DueDate,

                WebSite = businessPartnerViewModel.WebSite,
                ContactPerson = businessPartnerViewModel.ContactPerson,

                IsInPDV = businessPartnerViewModel.IsInPDV,

                JBKJS = businessPartnerViewModel.JBKJS,

                NameGer = businessPartnerViewModel.NameGer,
                IsInPDVGer = businessPartnerViewModel.IsInPDVGer, 
                TaxAdministrationId = businessPartnerViewModel.TaxAdministration?.Id ?? null, 
                IBAN = businessPartnerViewModel.IBAN, 
                BetriebsNumber = businessPartnerViewModel.BetriebsNumber,
                Customer = businessPartnerViewModel.Customer,
                CountryId = businessPartnerViewModel.Country?.Id ?? null,
                SectorId = businessPartnerViewModel.Sector?.Id ?? null,
                AgencyId = businessPartnerViewModel.Agency?.Id ?? null,
                VatId = businessPartnerViewModel.Vat?.Id ?? null,
                DiscountId = businessPartnerViewModel.Discount?.Id ?? null,
                TaxNr = businessPartnerViewModel.TaxNr,
                CommercialNr = businessPartnerViewModel.CommercialNr,
                ContactPersonGer = businessPartnerViewModel.ContactPersonGer,

                VatDeductionFrom = businessPartnerViewModel.VatDeductionFrom,
                VatDeductionTo = businessPartnerViewModel.VatDeductionTo,

                PdvType = businessPartnerViewModel.PdvType,

                Active = businessPartnerViewModel.IsActive,
                CreatedById = businessPartnerViewModel.CreatedBy?.Id ?? null,
                CompanyId = businessPartnerViewModel.Company?.Id ?? null,

                UpdatedAt = businessPartnerViewModel.UpdatedAt,
                CreatedAt = businessPartnerViewModel.CreatedAt,
            };
            return businessPartner;
        }
    }
}
