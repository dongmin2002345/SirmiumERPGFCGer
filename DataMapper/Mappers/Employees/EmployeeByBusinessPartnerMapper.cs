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
    public static class EmployeeByBusinessPartnerMapper
    {
        public static List<EmployeeByBusinessPartnerViewModel> ConvertToEmployeeByBusinessPartnerViewModelList(this IEnumerable<EmployeeByBusinessPartner> employeeByBusinessPartners)
        {
            List<EmployeeByBusinessPartnerViewModel> employeeByBusinessPartnerViewModels = new List<EmployeeByBusinessPartnerViewModel>();
            foreach (EmployeeByBusinessPartner employeeByBusinessPartner in employeeByBusinessPartners)
            {
                employeeByBusinessPartnerViewModels.Add(employeeByBusinessPartner.ConvertToEmployeeByBusinessPartnerViewModel());
            }
            return employeeByBusinessPartnerViewModels;
        }

        public static EmployeeByBusinessPartnerViewModel ConvertToEmployeeByBusinessPartnerViewModel(this EmployeeByBusinessPartner employeeByBusinessPartner)
        {
            EmployeeByBusinessPartnerViewModel remedyViewModel = new EmployeeByBusinessPartnerViewModel()
            {
                Id = employeeByBusinessPartner.Id,
                Identifier = employeeByBusinessPartner.Identifier,

                Code = employeeByBusinessPartner.Code,

                StartDate = employeeByBusinessPartner.StartDate,
                EndDate = employeeByBusinessPartner.EndDate,
                RealEndDate = employeeByBusinessPartner.RealEndDate,

                Employee = employeeByBusinessPartner.Employee?.ConvertToEmployeeViewModelLite(),
                BusinessPartner = employeeByBusinessPartner.BusinessPartner?.ConvertToBusinessPartnerViewModelLite(),

                IsActive = employeeByBusinessPartner.Active,

                CreatedBy = employeeByBusinessPartner.CreatedBy?.ConvertToUserViewModelLite(),
                Company = employeeByBusinessPartner.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = employeeByBusinessPartner.UpdatedAt,
                CreatedAt = employeeByBusinessPartner.CreatedAt
            };



            return remedyViewModel;
        }

        public static EmployeeByBusinessPartnerViewModel ConvertToEmployeeByBusinessPartnerViewModelLite(this EmployeeByBusinessPartner employeeByBusinessPartner)
        {
            EmployeeByBusinessPartnerViewModel remedyViewModel = new EmployeeByBusinessPartnerViewModel()
            {
                Id = employeeByBusinessPartner.Id,
                Identifier = employeeByBusinessPartner.Identifier,

                Code = employeeByBusinessPartner.Code,

                StartDate = employeeByBusinessPartner.StartDate,
                EndDate = employeeByBusinessPartner.EndDate,
                RealEndDate = employeeByBusinessPartner.RealEndDate,

                IsActive = employeeByBusinessPartner.Active,

                UpdatedAt = employeeByBusinessPartner.UpdatedAt,
                CreatedAt = employeeByBusinessPartner.CreatedAt
            };


            return remedyViewModel;
        }

        public static EmployeeByBusinessPartner ConvertToEmployeeByBusinessPartner(this EmployeeByBusinessPartnerViewModel employeeByBusinessPartnerViewModel)
        {
            EmployeeByBusinessPartner employeeByBusinessPartner = new EmployeeByBusinessPartner()
            {
                Id = employeeByBusinessPartnerViewModel.Id,
                Identifier = employeeByBusinessPartnerViewModel.Identifier,

                Code = employeeByBusinessPartnerViewModel.Code,
                StartDate = employeeByBusinessPartnerViewModel.StartDate,
                EndDate = employeeByBusinessPartnerViewModel.EndDate,
                RealEndDate = employeeByBusinessPartnerViewModel.RealEndDate,

                EmployeeId = employeeByBusinessPartnerViewModel.Employee?.Id ?? null,
                BusinessPartnerId = employeeByBusinessPartnerViewModel.BusinessPartner?.Id ?? null,

                CreatedById = employeeByBusinessPartnerViewModel.CreatedBy?.Id ?? null,
                CompanyId = employeeByBusinessPartnerViewModel.Company?.Id ?? null,

                CreatedAt = employeeByBusinessPartnerViewModel.CreatedAt,
                UpdatedAt = employeeByBusinessPartnerViewModel.UpdatedAt
            };

            return employeeByBusinessPartner;
        }
    }
}
