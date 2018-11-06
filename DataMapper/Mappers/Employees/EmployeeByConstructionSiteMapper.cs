using DataMapper.Mappers.Common.BusinessPartners;
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
    public static class EmployeeByConstructionSiteMapper
    {
        public static List<EmployeeByConstructionSiteViewModel> ConvertToEmployeeByConstructionSiteViewModelList(this IEnumerable<EmployeeByConstructionSite> employeeByConstructionSites)
        {
            List<EmployeeByConstructionSiteViewModel> employeeByConstructionSiteViewModels = new List<EmployeeByConstructionSiteViewModel>();
            foreach (EmployeeByConstructionSite employeeByConstructionSite in employeeByConstructionSites)
            {
                employeeByConstructionSiteViewModels.Add(employeeByConstructionSite.ConvertToEmployeeByConstructionSiteViewModel());
            }
            return employeeByConstructionSiteViewModels;
        }

        public static EmployeeByConstructionSiteViewModel ConvertToEmployeeByConstructionSiteViewModel(this EmployeeByConstructionSite employeeByConstructionSite)
        {
            EmployeeByConstructionSiteViewModel remedyViewModel = new EmployeeByConstructionSiteViewModel()
            {
                Id = employeeByConstructionSite.Id,
                Identifier = employeeByConstructionSite.Identifier,

                Code = employeeByConstructionSite.Code,

                StartDate = employeeByConstructionSite.StartDate,
                EndDate = employeeByConstructionSite.EndDate,

                Employee = employeeByConstructionSite.Employee?.ConvertToEmployeeViewModelLite(),
                BusinessPartner = employeeByConstructionSite.BusinessPartner?.ConvertToBusinessPartnerViewModel(), 
                ConstructionSite = employeeByConstructionSite.ConstructionSite?.ConvertToConstructionSiteViewModelLite(),

                CreatedBy = employeeByConstructionSite.CreatedBy?.ConvertToUserViewModelLite(),
                Company = employeeByConstructionSite.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = employeeByConstructionSite.UpdatedAt,
                CreatedAt = employeeByConstructionSite.CreatedAt
            };



            return remedyViewModel;
        }

        public static EmployeeByConstructionSiteViewModel ConvertToEmployeeByConstructionSiteViewModelLite(this EmployeeByConstructionSite employeeByConstructionSite)
        {
            EmployeeByConstructionSiteViewModel remedyViewModel = new EmployeeByConstructionSiteViewModel()
            {
                Id = employeeByConstructionSite.Id,
                Identifier = employeeByConstructionSite.Identifier,

                Code = employeeByConstructionSite.Code,

                StartDate = employeeByConstructionSite.StartDate,
                EndDate = employeeByConstructionSite.EndDate,

                UpdatedAt = employeeByConstructionSite.UpdatedAt,
                CreatedAt = employeeByConstructionSite.CreatedAt
            };


            return remedyViewModel;
        }

        public static EmployeeByConstructionSite ConvertToEmployeeByConstructionSite(this EmployeeByConstructionSiteViewModel employeeByConstructionSiteViewModel)
        {
            EmployeeByConstructionSite employeeByConstructionSite = new EmployeeByConstructionSite()
            {
                Id = employeeByConstructionSiteViewModel.Id,
                Identifier = employeeByConstructionSiteViewModel.Identifier,

                Code = employeeByConstructionSiteViewModel.Code,
                StartDate = employeeByConstructionSiteViewModel.StartDate,
                EndDate = employeeByConstructionSiteViewModel.EndDate,

                EmployeeId = employeeByConstructionSiteViewModel.Employee?.Id ?? null,
                BusinessPartnerId = employeeByConstructionSiteViewModel.BusinessPartner?.Id ?? null, 
                ConstructionSiteId = employeeByConstructionSiteViewModel.ConstructionSite?.Id ?? null,

                CreatedById = employeeByConstructionSiteViewModel.CreatedBy?.Id ?? null,
                CompanyId = employeeByConstructionSiteViewModel.Company?.Id ?? null,

                CreatedAt = employeeByConstructionSiteViewModel.CreatedAt,
                UpdatedAt = employeeByConstructionSiteViewModel.UpdatedAt
            };

            return employeeByConstructionSite;
        }
    }
}
