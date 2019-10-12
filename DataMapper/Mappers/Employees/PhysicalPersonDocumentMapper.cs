using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Employees
{
    public static class PhysicalPersonDocumentMapper
    {
        public static List<PhysicalPersonDocumentViewModel> ConvertToPhysicalPersonDocumentViewModelList(this IEnumerable<PhysicalPersonDocument> employeeDocuments)
        {
            List<PhysicalPersonDocumentViewModel> PhysicalPersonDocumentViewModels = new List<PhysicalPersonDocumentViewModel>();
            foreach (PhysicalPersonDocument PhysicalPersonDocument in employeeDocuments)
            {
                PhysicalPersonDocumentViewModels.Add(PhysicalPersonDocument.ConvertToPhysicalPersonDocumentViewModel());
            }
            return PhysicalPersonDocumentViewModels;
        }

        public static PhysicalPersonDocumentViewModel ConvertToPhysicalPersonDocumentViewModel(this PhysicalPersonDocument employeeDocument)
        {
            PhysicalPersonDocumentViewModel PhysicalPersonDocumentViewModel = new PhysicalPersonDocumentViewModel()
            {
                Id = employeeDocument.Id,
                Identifier = employeeDocument.Identifier,

                PhysicalPerson = employeeDocument.PhysicalPerson?.ConvertToPhysicalPersonViewModelLite(),

                Name = employeeDocument.Name,
                CreateDate = employeeDocument.CreateDate,
                Path = employeeDocument.Path,
                ItemStatus = employeeDocument.ItemStatus,

                IsActive = employeeDocument.Active,

                CreatedBy = employeeDocument.CreatedBy?.ConvertToUserViewModelLite(),
                Company = employeeDocument.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = employeeDocument.UpdatedAt,
                CreatedAt = employeeDocument.CreatedAt
            };

            return PhysicalPersonDocumentViewModel;
        }

        public static PhysicalPersonDocumentViewModel ConvertToPhysicalPersonDocumentViewModelLite(this PhysicalPersonDocument employeeDocument)
        {
            PhysicalPersonDocumentViewModel PhysicalPersonDocumentViewModel = new PhysicalPersonDocumentViewModel()
            {
                Id = employeeDocument.Id,
                Identifier = employeeDocument.Identifier,

                Name = employeeDocument.Name,
                CreateDate = employeeDocument.CreateDate,
                Path = employeeDocument.Path,
                ItemStatus = employeeDocument.ItemStatus,

                IsActive = employeeDocument.Active,

                UpdatedAt = employeeDocument.UpdatedAt,
                CreatedAt = employeeDocument.CreatedAt
            };

            return PhysicalPersonDocumentViewModel;
        }

        public static PhysicalPersonDocument ConvertToPhysicalPersonDocument(this PhysicalPersonDocumentViewModel employeeDocumentViewModel)
        {
            PhysicalPersonDocument PhysicalPersonDocument = new PhysicalPersonDocument()
            {
                Id = employeeDocumentViewModel.Id,
                Identifier = employeeDocumentViewModel.Identifier,

                PhysicalPersonId = employeeDocumentViewModel.PhysicalPerson?.Id ?? null,

                Name = employeeDocumentViewModel.Name,
                CreateDate = employeeDocumentViewModel.CreateDate,
                Path = employeeDocumentViewModel.Path,
                ItemStatus = employeeDocumentViewModel.ItemStatus,

                Active = employeeDocumentViewModel.IsActive,

                CreatedById = employeeDocumentViewModel.CreatedBy?.Id ?? null,
                CompanyId = employeeDocumentViewModel.Company?.Id ?? null,

                CreatedAt = employeeDocumentViewModel.CreatedAt,
                UpdatedAt = employeeDocumentViewModel.UpdatedAt
            };

            return PhysicalPersonDocument;
        }
    }
}
