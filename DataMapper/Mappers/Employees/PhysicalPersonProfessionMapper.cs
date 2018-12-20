using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DataMapper.Mappers.Common.Locations;
using DataMapper.Mappers.Common.Professions;
using DomainCore.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Employees
{
    public static class PhysicalPersonProfessionMapper
    {
        public static List<PhysicalPersonProfessionViewModel> ConvertToPhysicalPersonProfessionViewModelList(this IEnumerable<PhysicalPersonProfession> PhysicalPersonItems)
        {
            List<PhysicalPersonProfessionViewModel> PhysicalPersonItemViewModels = new List<PhysicalPersonProfessionViewModel>();
            foreach (PhysicalPersonProfession PhysicalPersonItem in PhysicalPersonItems)
            {
                PhysicalPersonItemViewModels.Add(PhysicalPersonItem.ConvertToPhysicalPersonProfessionViewModel());
            }
            return PhysicalPersonItemViewModels;
        }

        public static PhysicalPersonProfessionViewModel ConvertToPhysicalPersonProfessionViewModel(this PhysicalPersonProfession PhysicalPersonItem)
        {
            PhysicalPersonProfessionViewModel PhysicalPersonItemViewModel = new PhysicalPersonProfessionViewModel()
            {
                Id = PhysicalPersonItem.Id,
                Identifier = PhysicalPersonItem.Identifier,

                PhysicalPerson = PhysicalPersonItem.PhysicalPerson?.ConvertToPhysicalPersonViewModelLite(),
                Profession = PhysicalPersonItem.Profession?.ConvertToProfessionViewModelLite(),
                Country = PhysicalPersonItem.Country?.ConvertToCountryViewModelLite(),

                IsActive = PhysicalPersonItem.Active,

                CreatedBy = PhysicalPersonItem.CreatedBy?.ConvertToUserViewModelLite(),
                Company = PhysicalPersonItem.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = PhysicalPersonItem.UpdatedAt,
                CreatedAt = PhysicalPersonItem.CreatedAt
            };

            return PhysicalPersonItemViewModel;
        }

        public static PhysicalPersonProfessionViewModel ConvertToPhysicalPersonProfessionViewModelLite(this PhysicalPersonProfession PhysicalPersonItem)
        {
            PhysicalPersonProfessionViewModel PhysicalPersonItemViewModel = new PhysicalPersonProfessionViewModel()
            {
                Id = PhysicalPersonItem.Id,
                Identifier = PhysicalPersonItem.Identifier,

                IsActive = PhysicalPersonItem.Active,

                UpdatedAt = PhysicalPersonItem.UpdatedAt,
                CreatedAt = PhysicalPersonItem.CreatedAt
            };

            return PhysicalPersonItemViewModel;
        }

        public static PhysicalPersonProfession ConvertToPhysicalPersonProfession(this PhysicalPersonProfessionViewModel PhysicalPersonItemViewModel)
        {
            PhysicalPersonProfession PhysicalPersonItem = new PhysicalPersonProfession()
            {
                Id = PhysicalPersonItemViewModel.Id,
                Identifier = PhysicalPersonItemViewModel.Identifier,

                PhysicalPersonId = PhysicalPersonItemViewModel.PhysicalPerson?.Id ?? null,
                ProfessionId = PhysicalPersonItemViewModel.Profession?.Id ?? null,
                CountryId = PhysicalPersonItemViewModel.Country?.Id ?? null,

                CreatedById = PhysicalPersonItemViewModel.CreatedBy?.Id ?? null,
                CompanyId = PhysicalPersonItemViewModel.Company?.Id ?? null,

                CreatedAt = PhysicalPersonItemViewModel.CreatedAt,
                UpdatedAt = PhysicalPersonItemViewModel.UpdatedAt
            };

            return PhysicalPersonItem;
        }
    }
}
