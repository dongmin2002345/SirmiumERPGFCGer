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
    public static class EmployeeMapper
    {
        public static List<EmployeeViewModel> ConvertToEmployeeViewModelList(this IEnumerable<Employee> Employees)
        {
            List<EmployeeViewModel> EmployeeViewModels = new List<EmployeeViewModel>();
            foreach (Employee Employee in Employees)
            {
                EmployeeViewModels.Add(Employee.ConvertToEmployeeViewModel());
            }
            return EmployeeViewModels;
        }

        public static EmployeeViewModel ConvertToEmployeeViewModel(this Employee employee)
        {
            EmployeeViewModel EmployeeViewModel = new EmployeeViewModel()
            {
                Id = employee.Id,
                Identifier = employee.Identifier,

                Code = employee.Code,
                EmployeeCode = employee.EmployeeCode,
                Name = employee.Name,
                SurName = employee.SurName,

                ConstructionSiteCode = employee.ConstructionSiteCode,
                ConstructionSiteName = employee.ConstructionSiteName,

                DateOfBirth = employee.DateOfBirth,
                Gender = employee.Gender,

                Country = employee.Country?.ConvertToCountryViewModelLite(),
                Region = employee.Region?.ConvertToRegionViewModelLite(),
                Municipality = employee.Municipality?.ConvertToMunicipalityViewModelLite(),
                City = employee.City?.ConvertToCityViewModelLite(),

                Address = employee.Address,

                PassportCountry = employee.PassportCountry?.ConvertToCountryViewModelLite(),
                PassportCity = employee.PassportCity?.ConvertToCityViewModelLite(),
                Passport = employee.Passport,
                PassportMup = employee.PassportMup,
                VisaFrom = employee.VisaFrom,
                VisaTo = employee.VisaTo,

                ResidenceCountry = employee.ResidenceCountry?.ConvertToCountryViewModelLite(),
                ResidenceCity = employee.ResidenceCity?.ConvertToCityViewModelLite(),
                ResidenceAddress = employee.ResidenceAddress,
                
                EmbassyDate = employee.EmbassyDate,
                VisaDate = employee.VisaDate,
                VisaValidFrom = employee.VisaValidFrom,
                VisaValidTo = employee.VisaValidTo,
                WorkPermitFrom = employee.WorkPermitFrom,
                WorkPermitTo = employee.WorkPermitTo,

                IsActive = employee.Active,

                CreatedBy = employee.CreatedBy?.ConvertToUserViewModelLite(),
                Company = employee.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = employee.UpdatedAt,
                CreatedAt = employee.CreatedAt
            };

            return EmployeeViewModel;
        }

        public static EmployeeViewModel ConvertToEmployeeViewModelLite(this Employee employee)
        {
            EmployeeViewModel EmployeeViewModel = new EmployeeViewModel()
            {
                Id = employee.Id,
                Identifier = employee.Identifier,

                Code = employee.Code,
                EmployeeCode = employee.EmployeeCode,
                Name = employee.Name,
                SurName = employee.SurName,

                ConstructionSiteCode = employee.ConstructionSiteCode,
                ConstructionSiteName = employee.ConstructionSiteName,

                DateOfBirth = employee.DateOfBirth,

                Address = employee.Address,
                Passport = employee.Passport,

                EmbassyDate = employee.EmbassyDate,
                PassportMup = employee.PassportMup,
                VisaFrom = employee.VisaFrom,
                VisaTo = employee.VisaTo,
                WorkPermitFrom = employee.WorkPermitFrom,
                WorkPermitTo = employee.WorkPermitTo,

                IsActive = employee.Active,

                UpdatedAt = employee.UpdatedAt,
                CreatedAt = employee.CreatedAt
            };

            return EmployeeViewModel;
        }

        public static Employee ConvertToEmployee(this EmployeeViewModel employeeViewModel)
        {
            Employee Employee = new Employee()
            {
                Id = employeeViewModel.Id,
                Identifier = employeeViewModel.Identifier,

                Code = employeeViewModel.Code,
                EmployeeCode = employeeViewModel.EmployeeCode,
                Name = employeeViewModel.Name,
                SurName = employeeViewModel.SurName,

                ConstructionSiteCode = employeeViewModel.ConstructionSiteCode,
                ConstructionSiteName = employeeViewModel.ConstructionSiteName,

                DateOfBirth = (DateTime)employeeViewModel.DateOfBirth,
                Gender = employeeViewModel.Gender,
                CountryId = employeeViewModel?.Country?.Id ?? null,
                RegionId = employeeViewModel?.Region?.Id ?? null,
                MunicipalityId = employeeViewModel?.Municipality?.Id ?? null,
                CityId = employeeViewModel?.City?.Id ?? null,
                Address = employeeViewModel.Address,

                PassportCountryId = employeeViewModel?.PassportCountry?.Id ?? null,
                PassportCityId = employeeViewModel?.PassportCity?.Id ?? null,

                Passport = employeeViewModel.Passport,
                PassportMup = employeeViewModel.PassportMup,
                VisaFrom = employeeViewModel.VisaFrom,
                VisaTo = employeeViewModel.VisaTo,

                ResidenceCountryId = employeeViewModel?.ResidenceCountry?.Id ?? null,
                ResidenceCityId = employeeViewModel?.ResidenceCity?.Id ?? null,
                ResidenceAddress = employeeViewModel.ResidenceAddress,

                EmbassyDate = employeeViewModel.EmbassyDate,
                VisaDate = employeeViewModel.VisaDate,
                VisaValidFrom = employeeViewModel.VisaValidFrom,
                VisaValidTo = employeeViewModel.VisaValidTo,
                WorkPermitFrom = employeeViewModel.WorkPermitFrom,
                WorkPermitTo = employeeViewModel.WorkPermitTo,

                CreatedById = employeeViewModel.CreatedBy?.Id ?? null,
                CompanyId = employeeViewModel.Company?.Id ?? null,

                CreatedAt = employeeViewModel.CreatedAt,
                UpdatedAt = employeeViewModel.UpdatedAt
            };

            return Employee;
        }
    }
}
