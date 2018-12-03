using DataMapper.Mappers.Banks;
using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DataMapper.Mappers.Common.Locations;
using DomainCore.Common.TaxAdministrations;
using ServiceInterfaces.ViewModels.Common.TaxAdministrations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.TaxAdministrations
{
    public static class TaxAdministrationMapper
    {
        public static List<TaxAdministrationViewModel> ConvertToTaxAdministrationViewModelList(this IEnumerable<TaxAdministration> taxAdministrations)
        {
            List<TaxAdministrationViewModel> taxAdministrationViewModels = new List<TaxAdministrationViewModel>();
            foreach (TaxAdministration taxAdministration in taxAdministrations)
            {
                taxAdministrationViewModels.Add(taxAdministration.ConvertToTaxAdministrationViewModel());
            }
            return taxAdministrationViewModels;
        }

        public static TaxAdministrationViewModel ConvertToTaxAdministrationViewModel(this TaxAdministration taxAdministration)
        {
            TaxAdministrationViewModel taxAdministrationViewModel = new TaxAdministrationViewModel()
            {
                Id = taxAdministration.Id,
                Identifier = taxAdministration.Identifier,
                Code = taxAdministration.Code,
                SecondCode = taxAdministration.SecondCode,

                Name = taxAdministration.Name,

                Address1 = taxAdministration.Address1,
                Address2 = taxAdministration.Address2,
                Address3 = taxAdministration.Address3,
                IBAN1 = taxAdministration.IBAN1,
                SWIFT = taxAdministration.SWIFT,

                City = taxAdministration.City?.ConvertToCityViewModelLite(),
                Bank1 = taxAdministration.Bank1?.ConvertToBankViewModelLite(),
                Bank2 = taxAdministration.Bank2?.ConvertToBankViewModelLite(),
                Company = taxAdministration.Company?.ConvertToCompanyViewModelLite(),
                CreatedBy = taxAdministration.CreatedBy?.ConvertToUserViewModelLite(),

                IsActive = taxAdministration.Active, 

                UpdatedAt = taxAdministration.UpdatedAt,
                CreatedAt = taxAdministration.CreatedAt,
            };

            return taxAdministrationViewModel;
        }

        public static List<TaxAdministrationViewModel> ConvertToTaxAdministrationViewModelListLite(this IEnumerable<TaxAdministration> taxAdministrations)
        {
            List<TaxAdministrationViewModel> taxAdministrationViewModels = new List<TaxAdministrationViewModel>();
            foreach (TaxAdministration taxAdministration in taxAdministrations)
            {
                taxAdministrationViewModels.Add(taxAdministration.ConvertToTaxAdministrationViewModelLite());
            }
            return taxAdministrationViewModels;
        }

        public static TaxAdministrationViewModel ConvertToTaxAdministrationViewModelLite(this TaxAdministration taxAdministration)
        {
            TaxAdministrationViewModel taxAdministrationViewModel = new TaxAdministrationViewModel()
            {
                Id = taxAdministration.Id,
                Identifier = taxAdministration.Identifier,
                Code = taxAdministration.Code,
                SecondCode = taxAdministration.SecondCode,

                Name = taxAdministration.Name,

                Address1 = taxAdministration.Address1,
                Address2 = taxAdministration.Address2,
                Address3 = taxAdministration.Address3,
                IBAN1 = taxAdministration.IBAN1,
                SWIFT = taxAdministration.SWIFT,

                IsActive = taxAdministration.Active,

                UpdatedAt = taxAdministration.UpdatedAt,
                CreatedAt = taxAdministration.CreatedAt,
            };

            return taxAdministrationViewModel;
        }

        public static TaxAdministration ConvertToTaxAdministration(this TaxAdministrationViewModel taxAdministrationViewModel)
        {
            TaxAdministration taxAdministration = new TaxAdministration()
            {
                Id = taxAdministrationViewModel.Id,
                Identifier = taxAdministrationViewModel.Identifier,
                Code = taxAdministrationViewModel.Code,
                SecondCode = taxAdministrationViewModel.SecondCode,

                Name = taxAdministrationViewModel.Name,

                Address1 = taxAdministrationViewModel.Address1,
                Address2 = taxAdministrationViewModel.Address2,
                Address3 = taxAdministrationViewModel.Address3,
                IBAN1 = taxAdministrationViewModel.IBAN1,
                SWIFT = taxAdministrationViewModel.SWIFT,

                CityId = taxAdministrationViewModel.City?.Id ?? null,
                BankId1 = taxAdministrationViewModel.Bank1?.Id ?? null,
                BankId2 = taxAdministrationViewModel.Bank2?.Id ?? null,
                CreatedById = taxAdministrationViewModel.CreatedBy?.Id ?? null,
                CompanyId = taxAdministrationViewModel.Company?.Id ?? null,

                Active = taxAdministrationViewModel.IsActive,

                UpdatedAt = taxAdministrationViewModel.UpdatedAt,
                CreatedAt = taxAdministrationViewModel.CreatedAt,

            };

            return taxAdministration;
        }
    }
}
