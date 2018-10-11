using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Employees
{
    public static class EmployeeItemMapper
    {
        public static List<EmployeeItemViewModel> ConvertToEmployeeItemViewModelList(this IEnumerable<EmployeeItem> EmployeeItems)
        {
            List<EmployeeItemViewModel> EmployeeItemViewModels = new List<EmployeeItemViewModel>();
            foreach (EmployeeItem EmployeeItem in EmployeeItems)
            {
                EmployeeItemViewModels.Add(EmployeeItem.ConvertToEmployeeItemViewModel());
            }
            return EmployeeItemViewModels;
        }

        public static EmployeeItemViewModel ConvertToEmployeeItemViewModel(this EmployeeItem EmployeeItem)
        {
            EmployeeItemViewModel EmployeeItemViewModel = new EmployeeItemViewModel()
            {
                Id = EmployeeItem.Id,
                Identifier = EmployeeItem.Identifier,

                Employee = EmployeeItem.Employee?.ConvertToEmployeeViewModelLite(),
                FamilyMember = EmployeeItem.FamilyMember?.ConvertToFamilyMemberViewModelLite(),

                Name = EmployeeItem.Name,
                
                DateOfBirth = EmployeeItem.DateOfBirth,
                
                CreatedBy = EmployeeItem.CreatedBy?.ConvertToUserViewModelLite(),
                Company = EmployeeItem.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = EmployeeItem.UpdatedAt,
                CreatedAt = EmployeeItem.CreatedAt
            };

            return EmployeeItemViewModel;
        }

        public static EmployeeItemViewModel ConvertToEmployeeItemViewModelLite(this EmployeeItem EmployeeItem)
        {
            EmployeeItemViewModel EmployeeItemViewModel = new EmployeeItemViewModel()
            {
                Id = EmployeeItem.Id,
                Identifier = EmployeeItem.Identifier,

                Name = EmployeeItem.Name,

                DateOfBirth = EmployeeItem.DateOfBirth,

                UpdatedAt = EmployeeItem.UpdatedAt,
                CreatedAt = EmployeeItem.CreatedAt
            };

            return EmployeeItemViewModel;
        }

        public static EmployeeItem ConvertToEmployeeItem(this EmployeeItemViewModel EmployeeItemViewModel)
        {
            EmployeeItem EmployeeItem = new EmployeeItem()
            {
                Id = EmployeeItemViewModel.Id,
                Identifier = EmployeeItemViewModel.Identifier,

                EmployeeId = EmployeeItemViewModel.Employee?.Id ?? null,
                FamilyMemberId = EmployeeItemViewModel.FamilyMember?.Id ?? null,

                Name = EmployeeItemViewModel.Name,

                DateOfBirth = (DateTime)EmployeeItemViewModel.DateOfBirth,

                CreatedById = EmployeeItemViewModel.CreatedBy?.Id ?? null,
                CompanyId = EmployeeItemViewModel.Company?.Id ?? null,

                CreatedAt = EmployeeItemViewModel.CreatedAt,
                UpdatedAt = EmployeeItemViewModel.UpdatedAt
            };

            return EmployeeItem;
        }
    }
}
