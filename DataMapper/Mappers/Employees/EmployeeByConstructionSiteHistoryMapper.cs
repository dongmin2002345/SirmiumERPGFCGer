using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DataMapper.Mappers.ConstructionSites;
using DomainCore.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Employees
{
    public static class EmployeeByConstructionSiteHistoryMapper
    {
        public static List<EmployeeByConstructionSiteHistoryViewModel> ConvertToEmployeeByConstructionSiteHistoryViewModelList(this IEnumerable<EmployeeByConstructionSiteHistory> employeeByConstructionSiteHistories)
        {
            List<EmployeeByConstructionSiteHistoryViewModel> employeeByConstructionSiteHistoryViewModels = new List<EmployeeByConstructionSiteHistoryViewModel>();
            foreach (EmployeeByConstructionSiteHistory employeeByConstructionSiteHistory in employeeByConstructionSiteHistories)
            {
                employeeByConstructionSiteHistoryViewModels.Add(employeeByConstructionSiteHistory.ConvertToEmployeeByConstructionSiteHistoryViewModel());
            }
            return employeeByConstructionSiteHistoryViewModels;
        }

        public static EmployeeByConstructionSiteHistoryViewModel ConvertToEmployeeByConstructionSiteHistoryViewModel(this EmployeeByConstructionSiteHistory employeeByConstructionSiteHistory)
        {
            EmployeeByConstructionSiteHistoryViewModel employeeByConstructionSiteHistoryViewModel = new EmployeeByConstructionSiteHistoryViewModel()
            {
                Id = employeeByConstructionSiteHistory.Id,
                Identifier = employeeByConstructionSiteHistory.Identifier,

                Code = employeeByConstructionSiteHistory.Code,

                StartDate = employeeByConstructionSiteHistory.StartDate,
                EndDate = employeeByConstructionSiteHistory.EndDate,

                Employee = employeeByConstructionSiteHistory.Employee?.ConvertToEmployeeViewModelLite(),
                ConstructionSite = employeeByConstructionSiteHistory.ConstructionSite?.ConvertToConstructionSiteViewModelLite(),

                IsActive = employeeByConstructionSiteHistory.Active,

                CreatedBy = employeeByConstructionSiteHistory.CreatedBy?.ConvertToUserViewModelLite(),
                Company = employeeByConstructionSiteHistory.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = employeeByConstructionSiteHistory.UpdatedAt,
                CreatedAt = employeeByConstructionSiteHistory.CreatedAt
            };



            return employeeByConstructionSiteHistoryViewModel;
        }

        public static EmployeeByConstructionSiteHistoryViewModel ConvertToEmployeeByConstructionSiteHistoryViewModelLite(this EmployeeByConstructionSiteHistory employeeByConstructionSiteHistory)
        {
            EmployeeByConstructionSiteHistoryViewModel employeeByConstructionSiteHistoryViewModel = new EmployeeByConstructionSiteHistoryViewModel()
            {
                Id = employeeByConstructionSiteHistory.Id,
                Identifier = employeeByConstructionSiteHistory.Identifier,

                Code = employeeByConstructionSiteHistory.Code,

                StartDate = employeeByConstructionSiteHistory.StartDate,
                EndDate = employeeByConstructionSiteHistory.EndDate,

                IsActive = employeeByConstructionSiteHistory.Active,

                UpdatedAt = employeeByConstructionSiteHistory.UpdatedAt,
                CreatedAt = employeeByConstructionSiteHistory.CreatedAt
            };


            return employeeByConstructionSiteHistoryViewModel;
        }

        public static EmployeeByConstructionSiteHistory ConvertToEmployeeByConstructionSiteHistory(this EmployeeByConstructionSiteHistoryViewModel employeeByConstructionSiteHistoryViewModel)
        {
            EmployeeByConstructionSiteHistory employeeByConstructionSiteHistory = new EmployeeByConstructionSiteHistory()
            {
                Id = employeeByConstructionSiteHistoryViewModel.Id,
                Identifier = employeeByConstructionSiteHistoryViewModel.Identifier,

                Code = employeeByConstructionSiteHistoryViewModel.Code,
                StartDate = employeeByConstructionSiteHistoryViewModel.StartDate,
                EndDate = employeeByConstructionSiteHistoryViewModel.EndDate,

                EmployeeId = employeeByConstructionSiteHistoryViewModel.Employee?.Id ?? null,
                ConstructionSiteId = employeeByConstructionSiteHistoryViewModel.ConstructionSite?.Id ?? null,

                CreatedById = employeeByConstructionSiteHistoryViewModel.CreatedBy?.Id ?? null,
                CompanyId = employeeByConstructionSiteHistoryViewModel.Company?.Id ?? null,

                CreatedAt = employeeByConstructionSiteHistoryViewModel.CreatedAt,
                UpdatedAt = employeeByConstructionSiteHistoryViewModel.UpdatedAt
            };

            return employeeByConstructionSiteHistory;
        }
    }
}
