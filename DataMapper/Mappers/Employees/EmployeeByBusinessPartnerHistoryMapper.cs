using DataMapper.Mappers.Common.BusinessPartners;
using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Employees
{
    public static class EmployeeByBusinessPartnerHistoryMapper
    {
        public static List<EmployeeByBusinessPartnerHistoryViewModel> ConvertToEmployeeByBusinessPartnerHistoryViewModelList(this IEnumerable<EmployeeByBusinessPartnerHistory> employeeByBusinessPartnerHistories)
        {
            List<EmployeeByBusinessPartnerHistoryViewModel> employeeByBusinessPartnerHistoryViewModels = new List<EmployeeByBusinessPartnerHistoryViewModel>();
            foreach (EmployeeByBusinessPartnerHistory employeeByBusinessPartnerHistory in employeeByBusinessPartnerHistories)
            {
                employeeByBusinessPartnerHistoryViewModels.Add(employeeByBusinessPartnerHistory.ConvertToEmployeeByBusinessPartnerHistoryViewModel());
            }
            return employeeByBusinessPartnerHistoryViewModels;
        }

        public static EmployeeByBusinessPartnerHistoryViewModel ConvertToEmployeeByBusinessPartnerHistoryViewModel(this EmployeeByBusinessPartnerHistory employeeByBusinessPartnerHistory)
        {
            EmployeeByBusinessPartnerHistoryViewModel employeeByBusinessPartnerHistoryViewModel = new EmployeeByBusinessPartnerHistoryViewModel()
            {
                Id = employeeByBusinessPartnerHistory.Id,
                Identifier = employeeByBusinessPartnerHistory.Identifier,

                Code = employeeByBusinessPartnerHistory.Code,

                StartDate = employeeByBusinessPartnerHistory.StartDate,
                EndDate = employeeByBusinessPartnerHistory.EndDate,

                Employee = employeeByBusinessPartnerHistory.Employee?.ConvertToEmployeeViewModelLite(),
                BusinessPartner = employeeByBusinessPartnerHistory.BusinessPartner?.ConvertToBusinessPartnerViewModelLite(),

                CreatedBy = employeeByBusinessPartnerHistory.CreatedBy?.ConvertToUserViewModelLite(),
                Company = employeeByBusinessPartnerHistory.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = employeeByBusinessPartnerHistory.UpdatedAt,
                CreatedAt = employeeByBusinessPartnerHistory.CreatedAt
            };



            return employeeByBusinessPartnerHistoryViewModel;
        }

        public static EmployeeByBusinessPartnerHistoryViewModel ConvertToEmployeeByBusinessPartnerHistoryViewModelLite(this EmployeeByBusinessPartnerHistory employeeByBusinessPartnerHistory)
        {
            EmployeeByBusinessPartnerHistoryViewModel employeeByBusinessPartnerHistoryViewModel = new EmployeeByBusinessPartnerHistoryViewModel()
            {
                Id = employeeByBusinessPartnerHistory.Id,
                Identifier = employeeByBusinessPartnerHistory.Identifier,

                Code = employeeByBusinessPartnerHistory.Code,

                StartDate = employeeByBusinessPartnerHistory.StartDate,
                EndDate = employeeByBusinessPartnerHistory.EndDate,

                UpdatedAt = employeeByBusinessPartnerHistory.UpdatedAt,
                CreatedAt = employeeByBusinessPartnerHistory.CreatedAt
            };


            return employeeByBusinessPartnerHistoryViewModel;
        }

        public static EmployeeByBusinessPartnerHistory ConvertToEmployeeByBusinessPartnerHistory(this EmployeeByBusinessPartnerHistoryViewModel employeeByBusinessPartnerHistoryViewModel)
        {
            EmployeeByBusinessPartnerHistory employeeByBusinessPartnerHistory = new EmployeeByBusinessPartnerHistory()
            {
                Id = employeeByBusinessPartnerHistoryViewModel.Id,
                Identifier = employeeByBusinessPartnerHistoryViewModel.Identifier,

                Code = employeeByBusinessPartnerHistoryViewModel.Code,
                StartDate = employeeByBusinessPartnerHistoryViewModel.StartDate,
                EndDate = employeeByBusinessPartnerHistoryViewModel.EndDate,

                EmployeeId = employeeByBusinessPartnerHistoryViewModel.Employee?.Id ?? null,
                BusinessPartnerId = employeeByBusinessPartnerHistoryViewModel.BusinessPartner?.Id ?? null,

                CreatedById = employeeByBusinessPartnerHistoryViewModel.CreatedBy?.Id ?? null,
                CompanyId = employeeByBusinessPartnerHistoryViewModel.Company?.Id ?? null,

                CreatedAt = employeeByBusinessPartnerHistoryViewModel.CreatedAt,
                UpdatedAt = employeeByBusinessPartnerHistoryViewModel.UpdatedAt
            };

            return employeeByBusinessPartnerHistory;
        }
    }
}
