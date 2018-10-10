using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
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

                Address = Employee.Address,
                Passport = Employee.Passport,
                Interest = Employee.Interest,
                License = Employee.License,

                EmbassyDate = Employee.EmbassyDate,
                VisaFrom = Employee.VisaFrom,
                VisaTo = Employee.VisaTo,
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
                Interest = Employee.Interest,
                License = Employee.License,

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

                Address = EmployeeViewModel.Address,
                Passport = EmployeeViewModel.Passport,
                Interest = EmployeeViewModel.Interest,
                License = EmployeeViewModel.License,

                EmbassyDate = (DateTime)EmployeeViewModel.EmbassyDate,
                VisaFrom = (DateTime)EmployeeViewModel.VisaFrom,
                VisaTo = (DateTime)EmployeeViewModel.VisaTo,
                WorkPermitFrom = (DateTime)EmployeeViewModel.WorkPermitFrom,
                WorkPermitTo = (DateTime)EmployeeViewModel.WorkPermitTo,

                CreatedById = EmployeeViewModel.CreatedBy?.Id ?? null,
                CompanyId = EmployeeViewModel.Company?.Id ?? null,

                CreatedAt = EmployeeViewModel.CreatedAt,
                UpdatedAt = EmployeeViewModel.UpdatedAt
            };

            return Employee;
        }
    }
}
