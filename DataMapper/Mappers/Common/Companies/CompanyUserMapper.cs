using DataMapper.Mappers.Common.Identity;
using DomainCore.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.Companies
{
    public static class CompanyUserMapper
    {
        public static List<CompanyUserViewModel> ConvertToCompanyUserViewModelList(this IEnumerable<CompanyUser> companyUsers)
        {
            List<CompanyUserViewModel> viewModels = new List<CompanyUserViewModel>();

            if (companyUsers != null)
            {
                foreach (var item in companyUsers)
                {
                    viewModels.Add(item?.ConvertToCompanyUserViewModel());
                }
            }
            return viewModels;
        }

        public static CompanyUserViewModel ConvertToCompanyUserViewModel(this CompanyUser companyUser)
        {
            CompanyUserViewModel viewModel = new CompanyUserViewModel()
            {
                Id = companyUser?.Id ?? 0,
                Identifier = companyUser?.Identifier ?? Guid.Empty,
                IsActive = companyUser?.Active ?? false,
                Company = companyUser?.Company?.ConvertToCompanyViewModelLite(),
                User = companyUser?.User?.ConvertToUserViewModelLite(),
                UpdatedAt = companyUser?.UpdatedAt ?? DateTime.MinValue,
                IsSynced = true,
                UserRoles = companyUser?.RolesCSV?.RolesFromCSV() ?? new List<UserRoleViewModel>()
            };
            return viewModel;
        }

        public static List<CompanyUser> ConvertToCompanyUserList(this List<CompanyUserViewModel> companyUsers)
        {
            List<CompanyUser> viewModels = new List<CompanyUser>();

            if (companyUsers != null)
            {
                foreach (var item in companyUsers)
                {
                    viewModels.Add(item?.ConvertToCompanyUser());
                }
            }
            return viewModels;
        }

        public static CompanyUser ConvertToCompanyUser(this CompanyUserViewModel companyUser)
        {
            CompanyUser viewModel = new CompanyUser()
            {
                Id = companyUser?.Id ?? 0,
                Identifier = companyUser?.Identifier ?? Guid.Empty,
                Active = companyUser?.IsActive ?? false,
                CompanyId = companyUser?.Company?.Id ?? null,
                UserId = companyUser?.User?.Id ?? null,
                UpdatedAt = companyUser?.UpdatedAt ?? DateTime.MinValue,
                RolesCSV = companyUser?.UserRoles?.RolesToCSV() ?? ""
            };
            return viewModel;
        }
    }
}
