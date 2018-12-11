using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Limitations;
using ServiceInterfaces.ViewModels.Limitations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Limitations
{
    public static class LimitationEmailMapper
    {
        public static List<LimitationEmailViewModel> ConvertToLimitationEmailViewModelList(this IEnumerable<LimitationEmail> LimitationEmails)
        {
            List<LimitationEmailViewModel> viewModels = new List<LimitationEmailViewModel>();
            foreach (LimitationEmail LimitationEmail in LimitationEmails)
            {
                viewModels.Add(LimitationEmail.ConvertToLimitationEmailViewModel());
            }
            return viewModels;
        }

        public static LimitationEmailViewModel ConvertToLimitationEmailViewModel(this LimitationEmail LimitationEmail)
        {
            LimitationEmailViewModel LimitationEmailViewModel = new LimitationEmailViewModel()
            {
                Id = LimitationEmail.Id,
                Identifier = LimitationEmail.Identifier,

                Name = LimitationEmail.Name,
                LastName = LimitationEmail.LastName,
                Email = LimitationEmail.Email,

                IsActive = LimitationEmail.Active,

                CreatedBy = LimitationEmail.CreatedBy?.ConvertToUserViewModelLite(),
                Company = LimitationEmail.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = LimitationEmail.UpdatedAt,
                CreatedAt = LimitationEmail.CreatedAt
            };

            return LimitationEmailViewModel;
        }

        public static LimitationEmailViewModel ConvertToLimitationEmailViewModelLite(this LimitationEmail LimitationEmail)
        {
            LimitationEmailViewModel LimitationEmailViewModel = new LimitationEmailViewModel()
            {
                Id = LimitationEmail.Id,
                Identifier = LimitationEmail.Identifier,

                Name = LimitationEmail.Name,
                LastName = LimitationEmail.LastName,
                Email = LimitationEmail.Email,

                IsActive = LimitationEmail.Active,

                UpdatedAt = LimitationEmail.UpdatedAt,
                CreatedAt = LimitationEmail.CreatedAt
            };

            return LimitationEmailViewModel;
        }

        public static LimitationEmail ConvertToLimitationEmail(this LimitationEmailViewModel LimitationEmailViewModel)
        {
            LimitationEmail LimitationEmail = new LimitationEmail()
            {
                Id = LimitationEmailViewModel.Id,
                Identifier = LimitationEmailViewModel.Identifier,

                Name = LimitationEmailViewModel.Name,
                LastName = LimitationEmailViewModel.LastName,
                Email = LimitationEmailViewModel.Email,

                Active = LimitationEmailViewModel.IsActive,

                CreatedById = LimitationEmailViewModel.CreatedBy?.Id ?? null,
                CompanyId = LimitationEmailViewModel.Company?.Id ?? null,

                CreatedAt = LimitationEmailViewModel.CreatedAt,
                UpdatedAt = LimitationEmailViewModel.UpdatedAt
            };

            return LimitationEmail;
        }
    }
}
