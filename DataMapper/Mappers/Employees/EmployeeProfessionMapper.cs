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
    public static class EmployeeProfessionMapper
    {
        public static List<EmployeeProfessionItemViewModel> ConvertToEmployeeProfessionViewModelList(this IEnumerable<EmployeeProfession> EmployeeItems)
        {
            List<EmployeeProfessionItemViewModel> EmployeeItemViewModels = new List<EmployeeProfessionItemViewModel>();
            foreach (EmployeeProfession EmployeeItem in EmployeeItems)
            {
                EmployeeItemViewModels.Add(EmployeeItem.ConvertToEmployeeProfessionViewModel());
            }
            return EmployeeItemViewModels;
        }

        public static EmployeeProfessionItemViewModel ConvertToEmployeeProfessionViewModel(this EmployeeProfession EmployeeItem)
        {
            EmployeeProfessionItemViewModel EmployeeItemViewModel = new EmployeeProfessionItemViewModel()
            {
                Id = EmployeeItem.Id,
                Identifier = EmployeeItem.Identifier,

                Employee = EmployeeItem.Employee?.ConvertToEmployeeViewModelLite(),
                Profession = EmployeeItem.Profession?.ConvertToProfessionViewModelLite(),
                Country = EmployeeItem.Country?.ConvertToCountryViewModelLite(),
                
                CreatedBy = EmployeeItem.CreatedBy?.ConvertToUserViewModelLite(),
                Company = EmployeeItem.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = EmployeeItem.UpdatedAt,
                CreatedAt = EmployeeItem.CreatedAt
            };

            return EmployeeItemViewModel;
        }

        public static EmployeeProfessionItemViewModel ConvertToEmployeeProfessionViewModelLite(this EmployeeProfession EmployeeItem)
        {
            EmployeeProfessionItemViewModel EmployeeItemViewModel = new EmployeeProfessionItemViewModel()
            {
                Id = EmployeeItem.Id,
                Identifier = EmployeeItem.Identifier,

                UpdatedAt = EmployeeItem.UpdatedAt,
                CreatedAt = EmployeeItem.CreatedAt
            };

            return EmployeeItemViewModel;
        }

        public static EmployeeProfession ConvertToEmployeeProfession(this EmployeeProfessionItemViewModel EmployeeItemViewModel)
        {
            EmployeeProfession EmployeeItem = new EmployeeProfession()
            {
                Id = EmployeeItemViewModel.Id,
                Identifier = EmployeeItemViewModel.Identifier,

                EmployeeId = EmployeeItemViewModel.Employee?.Id ?? null,
                ProfessionId = EmployeeItemViewModel.Profession?.Id ?? null,
                CountryId = EmployeeItemViewModel.Country?.Id ?? null,

                CreatedById = EmployeeItemViewModel.CreatedBy?.Id ?? null,
                CompanyId = EmployeeItemViewModel.Company?.Id ?? null,

                CreatedAt = EmployeeItemViewModel.CreatedAt,
                UpdatedAt = EmployeeItemViewModel.UpdatedAt
            };

            return EmployeeItem;
        }
    }
}
