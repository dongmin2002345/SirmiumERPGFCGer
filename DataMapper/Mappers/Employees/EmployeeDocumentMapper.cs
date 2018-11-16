using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Employees
{
    public static class EmployeeDocumentMapper
    {
        public static List<EmployeeDocumentViewModel> ConvertToEmployeeDocumentViewModelList(this IEnumerable<EmployeeDocument> employeeDocuments)
        {
            List<EmployeeDocumentViewModel> EmployeeDocumentViewModels = new List<EmployeeDocumentViewModel>();
            foreach (EmployeeDocument EmployeeDocument in employeeDocuments)
            {
                EmployeeDocumentViewModels.Add(EmployeeDocument.ConvertToEmployeeDocumentViewModel());
            }
            return EmployeeDocumentViewModels;
        }

        public static EmployeeDocumentViewModel ConvertToEmployeeDocumentViewModel(this EmployeeDocument employeeDocument)
        {
            EmployeeDocumentViewModel EmployeeDocumentViewModel = new EmployeeDocumentViewModel()
            {
                Id = employeeDocument.Id,
                Identifier = employeeDocument.Identifier,

                Employee = employeeDocument.Employee?.ConvertToEmployeeViewModelLite(),

                Name = employeeDocument.Name,
                CreateDate = employeeDocument.CreateDate,
                Path = employeeDocument.Path,

                IsActive = employeeDocument.Active,

                CreatedBy = employeeDocument.CreatedBy?.ConvertToUserViewModelLite(),
                Company = employeeDocument.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = employeeDocument.UpdatedAt,
                CreatedAt = employeeDocument.CreatedAt
            };

            return EmployeeDocumentViewModel;
        }

        public static EmployeeDocumentViewModel ConvertToEmployeeDocumentViewModelLite(this EmployeeDocument employeeDocument)
        {
            EmployeeDocumentViewModel EmployeeDocumentViewModel = new EmployeeDocumentViewModel()
            {
                Id = employeeDocument.Id,
                Identifier = employeeDocument.Identifier,

                Name = employeeDocument.Name,
                CreateDate = employeeDocument.CreateDate,
                Path = employeeDocument.Path,

                IsActive = employeeDocument.Active,

                UpdatedAt = employeeDocument.UpdatedAt,
                CreatedAt = employeeDocument.CreatedAt
            };

            return EmployeeDocumentViewModel;
        }

        public static EmployeeDocument ConvertToEmployeeDocument(this EmployeeDocumentViewModel employeeDocumentViewModel)
        {
            EmployeeDocument EmployeeDocument = new EmployeeDocument()
            {
                Id = employeeDocumentViewModel.Id,
                Identifier = employeeDocumentViewModel.Identifier,

                EmployeeId = employeeDocumentViewModel.Employee?.Id ?? null,

                Name = employeeDocumentViewModel.Name,
                CreateDate = employeeDocumentViewModel.CreateDate,
                Path = employeeDocumentViewModel.Path,

                Active = employeeDocumentViewModel.IsActive,

                CreatedById = employeeDocumentViewModel.CreatedBy?.Id ?? null,
                CompanyId = employeeDocumentViewModel.Company?.Id ?? null,

                CreatedAt = employeeDocumentViewModel.CreatedAt,
                UpdatedAt = employeeDocumentViewModel.UpdatedAt
            };

            return EmployeeDocument;
        }
    }
}
