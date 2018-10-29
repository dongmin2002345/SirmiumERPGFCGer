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

        public static EmployeeViewModel ConvertToEmployeeViewModel(this Employee Employee)
        {
            EmployeeViewModel EmployeeViewModel = new EmployeeViewModel()
            {
                Id = Employee.Id,
                Identifier = Employee.Identifier,

                Code = Employee.Code,
                EmployeeCode = Employee.EmployeeCode,
                Name = Employee.Name,
                SurName = Employee.SurName,

                DateOfBirth = Employee.DateOfBirth,
                Gender = Employee.Gender,

                Country = Employee.Country?.ConvertToCountryViewModelLite(),
                Region = Employee.Region?.ConvertToRegionViewModelLite(),
                Municipality = Employee.Municipality?.ConvertToMunicipalityViewModelLite(),
                City = Employee.City?.ConvertToCityViewModelLite(),

                Address = Employee.Address,

                PassportCountry = Employee.PassportCountry?.ConvertToCountryViewModelLite(),
                PassportCity = Employee.PassportCity?.ConvertToCityViewModelLite(),
                Passport = Employee.Passport,
                VisaFrom = Employee.VisaFrom,
                VisaTo = Employee.VisaTo,

                ResidenceCity = Employee.ResidenceCity?.ConvertToCityViewModelLite(),
                ResidenceAddress = Employee.ResidenceAddress,
                
                EmbassyDate = Employee.EmbassyDate,
                VisaDate = Employee.VisaDate,
                VisaValidFrom = Employee.VisaValidFrom,
                VisaValidTo = Employee.VisaValidTo,
                WorkPermitFrom = Employee.WorkPermitFrom,
                WorkPermitTo = Employee.WorkPermitTo,

                CreatedBy = Employee.CreatedBy?.ConvertToUserViewModelLite(),
                Company = Employee.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = Employee.UpdatedAt,
                CreatedAt = Employee.CreatedAt
            };

            return EmployeeViewModel;
        }

        public static EmployeeViewModel ConvertToEmployeeViewModelLite(this Employee Employee)
        {
            EmployeeViewModel EmployeeViewModel = new EmployeeViewModel()
            {
                Id = Employee.Id,
                Identifier = Employee.Identifier,

                Code = Employee.Code,
                EmployeeCode = Employee.EmployeeCode,
                Name = Employee.Name,
                SurName = Employee.SurName,

                DateOfBirth = Employee.DateOfBirth,

                Address = Employee.Address,
                Passport = Employee.Passport,

                EmbassyDate = Employee.EmbassyDate,
                VisaFrom = Employee.VisaFrom,
                VisaTo = Employee.VisaTo,
                WorkPermitFrom = Employee.WorkPermitFrom,
                WorkPermitTo = Employee.WorkPermitTo,

                UpdatedAt = Employee.UpdatedAt,
                CreatedAt = Employee.CreatedAt
            };

            return EmployeeViewModel;
        }

        public static Employee ConvertToEmployee(this EmployeeViewModel EmployeeViewModel)
        {
            Employee Employee = new Employee()
            {
                Id = EmployeeViewModel.Id,
                Identifier = EmployeeViewModel.Identifier,

                Code = EmployeeViewModel.Code,
                EmployeeCode = EmployeeViewModel.EmployeeCode,
                Name = EmployeeViewModel.Name,
                SurName = EmployeeViewModel.SurName,

                DateOfBirth = (DateTime)EmployeeViewModel.DateOfBirth,
                Gender = EmployeeViewModel.Gender,
                CountryId = EmployeeViewModel?.Country?.Id ?? null,
                RegionId = EmployeeViewModel?.Region?.Id ?? null,
                MunicipalityId = EmployeeViewModel?.Municipality?.Id ?? null,
                CityId = EmployeeViewModel?.City?.Id ?? null,
                Address = EmployeeViewModel.Address,

                PassportCountryId = EmployeeViewModel?.PassportCountry?.Id ?? null,
                PassportCityId = EmployeeViewModel?.PassportCity?.Id ?? null,

                Passport = EmployeeViewModel.Passport,
                VisaFrom = EmployeeViewModel.VisaFrom,
                VisaTo = EmployeeViewModel.VisaTo,

                ResidenceCityId = EmployeeViewModel?.ResidenceCity?.Id ?? null,
                ResidenceAddress = EmployeeViewModel.ResidenceAddress,

                EmbassyDate = EmployeeViewModel.EmbassyDate,
                VisaDate = EmployeeViewModel.VisaDate,
                VisaValidFrom = EmployeeViewModel.VisaValidFrom,
                VisaValidTo = EmployeeViewModel.VisaValidTo,
                WorkPermitFrom = EmployeeViewModel.WorkPermitFrom,
                WorkPermitTo = EmployeeViewModel.WorkPermitTo,

                CreatedById = EmployeeViewModel.CreatedBy?.Id ?? null,
                CompanyId = EmployeeViewModel.Company?.Id ?? null,

                CreatedAt = EmployeeViewModel.CreatedAt,
                UpdatedAt = EmployeeViewModel.UpdatedAt
            };

            return Employee;
        }
    }
}
