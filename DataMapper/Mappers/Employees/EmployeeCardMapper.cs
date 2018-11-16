using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Employees
{
    public static class EmployeeCardMapper
    {
        public static List<EmployeeCardViewModel> ConvertToEmployeeCardViewModelList(this IEnumerable<EmployeeCard> employeeCards)
        {
            List<EmployeeCardViewModel> EmployeeCardViewModels = new List<EmployeeCardViewModel>();
            foreach (EmployeeCard EmployeeCard in employeeCards)
            {
                EmployeeCardViewModels.Add(EmployeeCard.ConvertToEmployeeCardViewModel());
            }
            return EmployeeCardViewModels;
        }

        public static EmployeeCardViewModel ConvertToEmployeeCardViewModel(this EmployeeCard employeeCard)
        {
            EmployeeCardViewModel EmployeeCardViewModel = new EmployeeCardViewModel()
            {
                Id = employeeCard.Id,
                Identifier = employeeCard.Identifier,

                Employee = employeeCard.Employee?.ConvertToEmployeeViewModelLite(),

                CardDate = employeeCard.CardDate,
                Description = employeeCard.Description,

                IsActive = employeeCard.Active,

                CreatedBy = employeeCard.CreatedBy?.ConvertToUserViewModelLite(),
                Company = employeeCard.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = employeeCard.UpdatedAt,
                CreatedAt = employeeCard.CreatedAt
            };

            return EmployeeCardViewModel;
        }

        public static EmployeeCardViewModel ConvertToEmployeeCardViewModelLite(this EmployeeCard employeeCard)
        {
            EmployeeCardViewModel EmployeeCardViewModel = new EmployeeCardViewModel()
            {
                Id = employeeCard.Id,
                Identifier = employeeCard.Identifier,

                CardDate = employeeCard.CardDate,
                Description = employeeCard.Description,

                IsActive = employeeCard.Active,

                UpdatedAt = employeeCard.UpdatedAt,
                CreatedAt = employeeCard.CreatedAt
            };

            return EmployeeCardViewModel;
        }

        public static EmployeeCard ConvertToEmployeeCard(this EmployeeCardViewModel employeeCardViewModel)
        {
            EmployeeCard EmployeeCard = new EmployeeCard()
            {
                Id = employeeCardViewModel.Id,
                Identifier = employeeCardViewModel.Identifier,

                EmployeeId = employeeCardViewModel.Employee?.Id ?? null,

                CardDate = (DateTime)employeeCardViewModel.CardDate,
                Description = employeeCardViewModel.Description,

                Active = employeeCardViewModel.IsActive,

                CreatedById = employeeCardViewModel.CreatedBy?.Id ?? null,
                CompanyId = employeeCardViewModel.Company?.Id ?? null,

                CreatedAt = employeeCardViewModel.CreatedAt,
                UpdatedAt = employeeCardViewModel.UpdatedAt
            };

            return EmployeeCard;
        }
    }
}
