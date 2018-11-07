using DataMapper.Mappers.Banks;
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
    public static class BusinessPartnerBankService
    {
        public static List<BusinessPartnerBankViewModel> ConvertToBusinessPartnerBankViewModelList(this IEnumerable<BusinessPartnerBank> businessPartnerBanks)
        {
            List<BusinessPartnerBankViewModel> businessPartnerBankViewModels = new List<BusinessPartnerBankViewModel>();
            foreach (BusinessPartnerBank businessPartnerBank in businessPartnerBanks)
            {
                businessPartnerBankViewModels.Add(businessPartnerBank.ConvertToBusinessPartnerBankViewModel());
            }
            return businessPartnerBankViewModels;
        }

        public static BusinessPartnerBankViewModel ConvertToBusinessPartnerBankViewModel(this BusinessPartnerBank businessPartnerBank)
        {
            BusinessPartnerBankViewModel businessPartnerBankViewModel = new BusinessPartnerBankViewModel()
            {
                Id = businessPartnerBank.Id,
                Identifier = businessPartnerBank.Identifier,

                BusinessPartner = businessPartnerBank.BusinessPartner?.ConvertToBusinessPartnerViewModelLite(),

                Bank = businessPartnerBank.Bank?.ConvertToBankViewModelLite(),
                Country = businessPartnerBank.Country?.ConvertToCountryViewModelLite(),
                AccountNumber = businessPartnerBank.AccountNumber, 

                IsActive = businessPartnerBank.Active,

                CreatedBy = businessPartnerBank.CreatedBy?.ConvertToUserViewModelLite(),
                Company = businessPartnerBank.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = businessPartnerBank.UpdatedAt,
                CreatedAt = businessPartnerBank.CreatedAt,
            };
            return businessPartnerBankViewModel;
        }

        public static BusinessPartnerBankViewModel ConvertToBusinessPartnerBankViewModelLite(this BusinessPartnerBank businessPartnerBank)
        {
            BusinessPartnerBankViewModel businessPartnerBankViewModel = new BusinessPartnerBankViewModel()
            {
                Id = businessPartnerBank.Id,
                Identifier = businessPartnerBank.Identifier,

                AccountNumber = businessPartnerBank.AccountNumber,

                IsActive = businessPartnerBank.Active,

                UpdatedAt = businessPartnerBank.UpdatedAt,
                CreatedAt = businessPartnerBank.CreatedAt,
            };
            return businessPartnerBankViewModel;
        }

        public static BusinessPartnerBank ConvertToBusinessPartnerBank(this BusinessPartnerBankViewModel businessPartnerBankViewModel)
        {
            BusinessPartnerBank businessPartnerBank = new BusinessPartnerBank()
            {
                Id = businessPartnerBankViewModel.Id,
                Identifier = businessPartnerBankViewModel.Identifier,

                BusinessPartnerId = businessPartnerBankViewModel.BusinessPartner?.Id ?? null,

                BankId = businessPartnerBankViewModel.Bank?.Id ?? 0,
                CountryId = businessPartnerBankViewModel.Country?.Id ?? 0,
                AccountNumber = businessPartnerBankViewModel.AccountNumber,

                CreatedById = businessPartnerBankViewModel.CreatedBy?.Id ?? null,
                CompanyId = businessPartnerBankViewModel.Company?.Id ?? null,

                UpdatedAt = businessPartnerBankViewModel.UpdatedAt,
                CreatedAt = businessPartnerBankViewModel.CreatedAt,
            };
            return businessPartnerBank;
        }
    }
}
