using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Employees
{
    public static class EmployeeAttachmentMapper
    {
        public static List<EmployeeAttachmentViewModel> ConvertToEmployeeAttachmentViewModelList(this IEnumerable<EmployeeAttachment> employeeAttachments)
        {
            List<EmployeeAttachmentViewModel> EmployeeAttachmentViewModels = new List<EmployeeAttachmentViewModel>();
            foreach (EmployeeAttachment EmployeeAttachment in employeeAttachments)
            {
                EmployeeAttachmentViewModels.Add(EmployeeAttachment.ConvertToEmployeeAttachmentViewModel());
            }
            return EmployeeAttachmentViewModels;
        }

        public static EmployeeAttachmentViewModel ConvertToEmployeeAttachmentViewModel(this EmployeeAttachment employeeAttachment)
        {
            EmployeeAttachmentViewModel EmployeeAttachmentViewModel = new EmployeeAttachmentViewModel()
            {
                Id = employeeAttachment.Id,
                Identifier = employeeAttachment.Identifier,

                Code = employeeAttachment.Code,

                Employee = employeeAttachment.Employee?.ConvertToEmployeeViewModelLite(),

                OK = employeeAttachment.OK,
                IsActive = employeeAttachment.Active,

                CreatedBy = employeeAttachment.CreatedBy?.ConvertToUserViewModelLite(),
                Company = employeeAttachment.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = employeeAttachment.UpdatedAt,
                CreatedAt = employeeAttachment.CreatedAt
            };

            return EmployeeAttachmentViewModel;
        }

        public static EmployeeAttachmentViewModel ConvertToEmployeeAttachmentViewModelLite(this EmployeeAttachment employeeAttachment)
        {
            EmployeeAttachmentViewModel EmployeeAttachmentViewModel = new EmployeeAttachmentViewModel()
            {
                Id = employeeAttachment.Id,
                Identifier = employeeAttachment.Identifier,

                Code = employeeAttachment.Code,

                OK = employeeAttachment.OK,
                IsActive = employeeAttachment.Active,

                UpdatedAt = employeeAttachment.UpdatedAt,
                CreatedAt = employeeAttachment.CreatedAt
            };

            return EmployeeAttachmentViewModel;
        }

        public static EmployeeAttachment ConvertToEmployeeAttachment(this EmployeeAttachmentViewModel employeeAttachmentViewModel)
        {
            EmployeeAttachment EmployeeAttachment = new EmployeeAttachment()
            {
                Id = employeeAttachmentViewModel.Id,
                Identifier = employeeAttachmentViewModel.Identifier,

                Code = employeeAttachmentViewModel.Code,

                EmployeeId = employeeAttachmentViewModel.Employee?.Id ?? null,

                OK = employeeAttachmentViewModel.OK,
                Active = employeeAttachmentViewModel.IsActive,

                CreatedById = employeeAttachmentViewModel.CreatedBy?.Id ?? null,
                CompanyId = employeeAttachmentViewModel.Company?.Id ?? null,

                CreatedAt = employeeAttachmentViewModel.CreatedAt,
                UpdatedAt = employeeAttachmentViewModel.UpdatedAt
            };

            return EmployeeAttachment;
        }
    }
}
