using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DataMapper.Mappers.Common.Locations;
using DomainCore.Common.Professions;
using ServiceInterfaces.ViewModels.Common.Professions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.Professions
{
    public static class ProfessionMapper
    {
        public static List<ProfessionViewModel> ConvertToProfessionViewModelList(this IEnumerable<Profession> professions)
        {
            List<ProfessionViewModel> professionsViewModels = new List<ProfessionViewModel>();
            foreach (Profession profession in professions)
            {
                professionsViewModels.Add(profession.ConvertToProfessionViewModel());
            }
            return professionsViewModels;
        }

        public static ProfessionViewModel ConvertToProfessionViewModel(this Profession profession)
        {
            ProfessionViewModel professionViewModel = new ProfessionViewModel()
            {
                Id = profession.Id,
                Identifier = profession.Identifier,

                Code = profession.Code,
                SecondCode = profession.SecondCode,
                Name = profession.Name,

                Country = profession.Country?.ConvertToCountryViewModelLite(),

                IsActive = profession.Active,

                CreatedBy = profession.CreatedBy?.ConvertToUserViewModelLite(),
                Company = profession.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = profession.UpdatedAt,
                CreatedAt = profession.CreatedAt

            };
            return professionViewModel;
        }

        public static List<ProfessionViewModel> ConvertToProfessionViewModelListLite(this IEnumerable<Profession> professions)
        {
            List<ProfessionViewModel> professionViewModels = new List<ProfessionViewModel>();
            foreach (Profession profession in professions)
            {
                professionViewModels.Add(profession.ConvertToProfessionViewModelLite());
            }
            return professionViewModels;
        }


        public static ProfessionViewModel ConvertToProfessionViewModelLite(this Profession profession)
        {
            ProfessionViewModel professionViewModel = new ProfessionViewModel()
            {
                Id = profession.Id,
                Identifier = profession.Identifier,

                Code = profession.Code,
                SecondCode = profession.SecondCode,
                Name = profession.Name,

                IsActive = profession.Active,

                CreatedAt = profession.CreatedAt,
                UpdatedAt = profession.UpdatedAt
            };
            return professionViewModel;
        }

        public static Profession ConvertToProfession(this ProfessionViewModel professionViewModel)
        {
            Profession profession = new Profession()
            {
                Id = professionViewModel.Id,
                Identifier = professionViewModel.Identifier,

                Code = professionViewModel.Code,
                SecondCode = professionViewModel.SecondCode ?? "",
                Name = professionViewModel.Name,

                CountryId = professionViewModel.Country?.Id ?? null,

                CreatedById = professionViewModel.CreatedBy?.Id ?? null,
                CompanyId = professionViewModel.Company?.Id ?? null,

                CreatedAt = professionViewModel.CreatedAt,
                UpdatedAt = professionViewModel.UpdatedAt

            };
            return profession;
        }
    }
}
