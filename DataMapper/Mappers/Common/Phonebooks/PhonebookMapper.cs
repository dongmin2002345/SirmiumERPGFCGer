using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DataMapper.Mappers.Common.Locations;
using DomainCore.Common.Phonebooks;
using ServiceInterfaces.ViewModels.Common.Phonebooks;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.Phonebooks
{
    public static class PhonebookMapper
    {
        public static List<PhonebookViewModel> ConvertToPhonebookViewModelList(this IEnumerable<Phonebook> Phonebooks)
        {
            List<PhonebookViewModel> PhonebookViewModels = new List<PhonebookViewModel>();
            foreach (Phonebook Phonebook in Phonebooks)
            {
                PhonebookViewModels.Add(Phonebook.ConvertToPhonebookViewModel());
            }
            return PhonebookViewModels;
        }

        public static PhonebookViewModel ConvertToPhonebookViewModel(this Phonebook Phonebook)
        {
            PhonebookViewModel PhonebookViewModel = new PhonebookViewModel()
            {
                Id = Phonebook.Id,
                Identifier = Phonebook.Identifier,

                Code = Phonebook.Code,

                Name = Phonebook.Name,

                Municipality = Phonebook.Municipality?.ConvertToMunicipalityViewModelLite(),
                City = Phonebook.City?.ConvertToCityViewModelLite(),

                Address = Phonebook.Address,

                IsActive = Phonebook.Active,

                CreatedBy = Phonebook.CreatedBy?.ConvertToUserViewModelLite(),
                Company = Phonebook.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = Phonebook.UpdatedAt,
                CreatedAt = Phonebook.CreatedAt
            };

            return PhonebookViewModel;
        }

        public static PhonebookViewModel ConvertToPhonebookViewModelLite(this Phonebook Phonebook)
        {
            PhonebookViewModel PhonebookViewModel = new PhonebookViewModel()
            {
                Id = Phonebook.Id,
                Identifier = Phonebook.Identifier,

                Code = Phonebook.Code,
                Name = Phonebook.Name,

                Address = Phonebook.Address,

                IsActive = Phonebook.Active,

                UpdatedAt = Phonebook.UpdatedAt,
                CreatedAt = Phonebook.CreatedAt
            };

            return PhonebookViewModel;
        }

        public static Phonebook ConvertToPhonebook(this PhonebookViewModel PhonebookViewModel)
        {
            Phonebook Phonebook = new Phonebook()
            {
                Id = PhonebookViewModel.Id,
                Identifier = PhonebookViewModel.Identifier,

                Code = PhonebookViewModel.Code,
                Name = PhonebookViewModel.Name,

                MunicipalityId = PhonebookViewModel?.Municipality?.Id ?? null,
                CityId = PhonebookViewModel?.City?.Id ?? null,
                Address = PhonebookViewModel.Address,

                Active = PhonebookViewModel.IsActive,
                CreatedById = PhonebookViewModel.CreatedBy?.Id ?? null,
                CompanyId = PhonebookViewModel.Company?.Id ?? null,

                CreatedAt = PhonebookViewModel.CreatedAt,
                UpdatedAt = PhonebookViewModel.UpdatedAt
            };

            return Phonebook;
        }
    }
}
