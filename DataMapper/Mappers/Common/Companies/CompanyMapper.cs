using DataMapper.Mappers.Common.Identity;
using DomainCore.Common.Companies;
using DomainCore.Common.Identity;
using ServiceInterfaces.ViewModels.Common.Companies;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.Companies
{
    public static class CompanyMapper
    {
        public static List<CompanyViewModel> ConvertToCompanyViewModelList(this IEnumerable<Company> companies)
        {
            List<CompanyViewModel> CompanyViewModels = new List<CompanyViewModel>();
            foreach (Company company in companies)
            {
                CompanyViewModels.Add(company.ConvertToCompanyViewModel());
            }
            return CompanyViewModels;
        }

        public static List<CompanyViewModel> ConvertToCompanyViewModelListLite(this IEnumerable<Company> companies)
        {
            List<CompanyViewModel> CompanyViewModels = new List<CompanyViewModel>();
            foreach (Company company in companies)
            {
                if (company != null)
                {
                    CompanyViewModels.Add(company.ConvertToCompanyViewModelLite());
                }
            }
            return CompanyViewModels;
        }

        public static CompanyViewModel ConvertToCompanyViewModel(this Company company)
        {
            CompanyViewModel companyViewModel = new CompanyViewModel()
            {
                Id = company.Id,
                Identifier = company.Identifier,
                CompanyCode = company.Code,
                CompanyName = company.Name,
                Address = company.Address,
                BankAccountNo = company.BankAccountNo,
                BankAccountName = company.BankAccountName,
                IdentificationNumber = company.IdentificationNumber,
                PIBNumber = company.PIBNumber,
                PIONumber = company.PIONumber,
                PDVNumber = company.PDVNumber,
                IndustryCode = company.IndustryCode,
                IndustryName = company.IndustryName,
                Email = company.Email,
                WebSite = company.WebSite,
                UpdatedAt = company.UpdatedAt,

                IsActive = company.Active,

                CreatedBy = company.CreatedBy?.ConvertToUserViewModelLite(),
                CreatedAt = company.CreatedAt
            };
            return companyViewModel;
        }

        public static CompanyViewModel ConvertToCompanyViewModelLite(this Company company)
        {
            CompanyViewModel companyViewModel = new CompanyViewModel()
            {
                Id = company.Id,
                Identifier = company.Identifier,
                CompanyCode = company.Code,
                CompanyName = company.Name,
                Address = company.Address,
                BankAccountNo = company.BankAccountNo,
                BankAccountName = company.BankAccountName,
                IdentificationNumber = company.IdentificationNumber,
                PIBNumber = company.PIBNumber,
                PIONumber = company.PIONumber,
                PDVNumber = company.PDVNumber,
                IndustryCode = company.IndustryCode,
                IndustryName = company.IndustryName,
                Email = company.Email,
                WebSite = company.WebSite,

                IsActive = company.Active,

                UpdatedAt = company.UpdatedAt,
                CreatedAt = company.CreatedAt
            };
            return companyViewModel;
        }

        public static List<Company> ConvertToCompanyList(this IEnumerable<CompanyViewModel> companyViewModels)
        {
            List<Company> companies = new List<Company>();
            foreach (CompanyViewModel companyViewModel in companyViewModels)
            {
                companies.Add(companyViewModel.ConvertToCompany());
            }
            return companies;
        }

        public static Company ConvertToCompany(this CompanyViewModel companyViewModel)
        {
            Company Company = new Company()
            {
                Id = companyViewModel.Id,
                Identifier = companyViewModel.Identifier,
                Code = companyViewModel.CompanyCode,
                Name = companyViewModel.CompanyName,
                Address = companyViewModel.Address,
                BankAccountNo = companyViewModel.BankAccountNo,
                BankAccountName = companyViewModel.BankAccountName,
                IdentificationNumber = companyViewModel.IdentificationNumber,
                PIBNumber = companyViewModel.PIBNumber,
                PIONumber = companyViewModel.PIONumber,
                PDVNumber = companyViewModel.PDVNumber,
                IndustryCode = companyViewModel.IndustryCode,
                IndustryName = companyViewModel.IndustryName,
                Email = companyViewModel.Email,
                WebSite = companyViewModel.WebSite,

                CreatedBy = new User() { Id = companyViewModel.CreatedBy?.Id ?? 0 },
                CreatedAt = companyViewModel.CreatedAt
            };
            return Company;
        }
    }
}
