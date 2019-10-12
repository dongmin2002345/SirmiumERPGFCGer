using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DataMapper.Mappers.Common.Locations;
using DataMapper.Mappers.Employees;
using DomainCore.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.PhysicalPersons
{
    public static class PhysicalPersonLicenceMapper
    {
        public static List<PhysicalPersonLicenceViewModel> ConvertToPhysicalPersonLicenceViewModelList(this IEnumerable<PhysicalPersonLicence> PhysicalPersonItems)
        {
            List<PhysicalPersonLicenceViewModel> PhysicalPersonItemViewModels = new List<PhysicalPersonLicenceViewModel>();
            foreach (PhysicalPersonLicence PhysicalPersonItem in PhysicalPersonItems)
            {
                PhysicalPersonItemViewModels.Add(PhysicalPersonItem.ConvertToPhysicalPersonLicenceViewModel());
            }
            return PhysicalPersonItemViewModels;
        }

        public static PhysicalPersonLicenceViewModel ConvertToPhysicalPersonLicenceViewModel(this PhysicalPersonLicence PhysicalPersonItem)
        {
            PhysicalPersonLicenceViewModel PhysicalPersonItemViewModel = new PhysicalPersonLicenceViewModel()
            {
                Id = PhysicalPersonItem.Id,
                Identifier = PhysicalPersonItem.Identifier,

                PhysicalPerson = PhysicalPersonItem.PhysicalPerson?.ConvertToPhysicalPersonViewModelLite(),
                Licence = PhysicalPersonItem.Licence?.ConvertToLicenceTypeViewModelLite(),
                Country = PhysicalPersonItem.Country?.ConvertToCountryViewModelLite(),

                ValidFrom = PhysicalPersonItem.ValidFrom,
                ValidTo = PhysicalPersonItem.ValidTo,
                ItemStatus = PhysicalPersonItem.ItemStatus,

                IsActive = PhysicalPersonItem.Active,

                CreatedBy = PhysicalPersonItem.CreatedBy?.ConvertToUserViewModelLite(),
                Company = PhysicalPersonItem.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = PhysicalPersonItem.UpdatedAt,
                CreatedAt = PhysicalPersonItem.CreatedAt
            };

            return PhysicalPersonItemViewModel;
        }

        public static PhysicalPersonLicenceViewModel ConvertToPhysicalPersonLicenceViewModelLite(this PhysicalPersonLicence PhysicalPersonItem)
        {
            PhysicalPersonLicenceViewModel PhysicalPersonItemViewModel = new PhysicalPersonLicenceViewModel()
            {
                Id = PhysicalPersonItem.Id,
                Identifier = PhysicalPersonItem.Identifier,

                ValidFrom = PhysicalPersonItem.ValidFrom,
                ValidTo = PhysicalPersonItem.ValidTo,
                ItemStatus = PhysicalPersonItem.ItemStatus,

                IsActive = PhysicalPersonItem.Active,

                UpdatedAt = PhysicalPersonItem.UpdatedAt,
                CreatedAt = PhysicalPersonItem.CreatedAt
            };

            return PhysicalPersonItemViewModel;
        }

        public static PhysicalPersonLicence ConvertToPhysicalPersonLicence(this PhysicalPersonLicenceViewModel PhysicalPersonItemViewModel)
        {
            PhysicalPersonLicence PhysicalPersonItem = new PhysicalPersonLicence()
            {
                Id = PhysicalPersonItemViewModel.Id,
                Identifier = PhysicalPersonItemViewModel.Identifier,

                PhysicalPersonId = PhysicalPersonItemViewModel.PhysicalPerson?.Id ?? null,
                LicenceId = PhysicalPersonItemViewModel.Licence?.Id ?? null,
                CountryId = PhysicalPersonItemViewModel.Country?.Id ?? null,

                ValidFrom = PhysicalPersonItemViewModel.ValidFrom,
                ValidTo = PhysicalPersonItemViewModel.ValidTo,
                ItemStatus = PhysicalPersonItemViewModel.ItemStatus,

                CreatedById = PhysicalPersonItemViewModel.CreatedBy?.Id ?? null,
                CompanyId = PhysicalPersonItemViewModel.Company?.Id ?? null,

                CreatedAt = PhysicalPersonItemViewModel.CreatedAt,
                UpdatedAt = PhysicalPersonItemViewModel.UpdatedAt
            };

            return PhysicalPersonItem;
        }
    }
}
