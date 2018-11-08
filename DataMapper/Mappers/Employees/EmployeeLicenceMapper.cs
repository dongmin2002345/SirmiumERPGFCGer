using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DataMapper.Mappers.Common.Locations;
using DomainCore.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Employees
{
    public static class EmployeeLicenceMapper
    {
        public static List<EmployeeLicenceItemViewModel> ConvertToEmployeeLicenceViewModelList(this IEnumerable<EmployeeLicence> EmployeeItems)
        {
            List<EmployeeLicenceItemViewModel> EmployeeItemViewModels = new List<EmployeeLicenceItemViewModel>();
            foreach (EmployeeLicence EmployeeItem in EmployeeItems)
            {
                EmployeeItemViewModels.Add(EmployeeItem.ConvertToEmployeeLicenceViewModel());
            }
            return EmployeeItemViewModels;
        }

        public static EmployeeLicenceItemViewModel ConvertToEmployeeLicenceViewModel(this EmployeeLicence EmployeeItem)
        {
            EmployeeLicenceItemViewModel EmployeeItemViewModel = new EmployeeLicenceItemViewModel()
            {
                Id = EmployeeItem.Id,
                Identifier = EmployeeItem.Identifier,

                Employee = EmployeeItem.Employee?.ConvertToEmployeeViewModelLite(),
                Licence = EmployeeItem.Licence?.ConvertToLicenceTypeViewModelLite(),
                Country = EmployeeItem.Country?.ConvertToCountryViewModelLite(),

                ValidFrom = EmployeeItem.ValidFrom,
                ValidTo = EmployeeItem.ValidTo,

                IsActive = EmployeeItem.Active,

                CreatedBy = EmployeeItem.CreatedBy?.ConvertToUserViewModelLite(),
                Company = EmployeeItem.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = EmployeeItem.UpdatedAt,
                CreatedAt = EmployeeItem.CreatedAt
            };

            return EmployeeItemViewModel;
        }

        public static EmployeeLicenceItemViewModel ConvertToEmployeeLicenceViewModelLite(this EmployeeLicence EmployeeItem)
        {
            EmployeeLicenceItemViewModel EmployeeItemViewModel = new EmployeeLicenceItemViewModel()
            {
                Id = EmployeeItem.Id,
                Identifier = EmployeeItem.Identifier,

                ValidFrom = EmployeeItem.ValidFrom,
                ValidTo = EmployeeItem.ValidTo,

                IsActive = EmployeeItem.Active,

                UpdatedAt = EmployeeItem.UpdatedAt,
                CreatedAt = EmployeeItem.CreatedAt
            };

            return EmployeeItemViewModel;
        }

        public static EmployeeLicence ConvertToEmployeeLicence(this EmployeeLicenceItemViewModel EmployeeItemViewModel)
        {
            EmployeeLicence EmployeeItem = new EmployeeLicence()
            {
                Id = EmployeeItemViewModel.Id,
                Identifier = EmployeeItemViewModel.Identifier,

                EmployeeId = EmployeeItemViewModel.Employee?.Id ?? null,
                LicenceId = EmployeeItemViewModel.Licence?.Id ?? null,
                CountryId = EmployeeItemViewModel.Country?.Id ?? null,

                ValidFrom = EmployeeItemViewModel.ValidFrom,
                ValidTo = EmployeeItemViewModel.ValidTo,

                CreatedById = EmployeeItemViewModel.CreatedBy?.Id ?? null,
                CompanyId = EmployeeItemViewModel.Company?.Id ?? null,

                CreatedAt = EmployeeItemViewModel.CreatedAt,
                UpdatedAt = EmployeeItemViewModel.UpdatedAt
            };

            return EmployeeItem;
        }
    }
}
