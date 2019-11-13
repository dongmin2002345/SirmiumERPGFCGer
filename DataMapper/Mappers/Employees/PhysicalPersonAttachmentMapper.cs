using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Employees
{
    public static class PhysicalPersonAttachmentMapper
    {
        public static List<PhysicalPersonAttachmentViewModel> ConvertToPhysicalPersonAttachmentViewModelList(this IEnumerable<PhysicalPersonAttachment> PhysicalPersonAttachments)
        {
            List<PhysicalPersonAttachmentViewModel> PhysicalPersonAttachmentViewModels = new List<PhysicalPersonAttachmentViewModel>();
            foreach (PhysicalPersonAttachment PhysicalPersonAttachment in PhysicalPersonAttachments)
            {
                PhysicalPersonAttachmentViewModels.Add(PhysicalPersonAttachment.ConvertToPhysicalPersonAttachmentViewModel());
            }
            return PhysicalPersonAttachmentViewModels;
        }

        public static PhysicalPersonAttachmentViewModel ConvertToPhysicalPersonAttachmentViewModel(this PhysicalPersonAttachment PhysicalPersonAttachment)
        {
            PhysicalPersonAttachmentViewModel PhysicalPersonAttachmentViewModel = new PhysicalPersonAttachmentViewModel()
            {
                Id = PhysicalPersonAttachment.Id,
                Identifier = PhysicalPersonAttachment.Identifier,

                Code = PhysicalPersonAttachment.Code,

                PhysicalPerson = PhysicalPersonAttachment.PhysicalPerson?.ConvertToPhysicalPersonViewModelLite(),

                OK = PhysicalPersonAttachment.OK,
                IsActive = PhysicalPersonAttachment.Active,

                CreatedBy = PhysicalPersonAttachment.CreatedBy?.ConvertToUserViewModelLite(),
                Company = PhysicalPersonAttachment.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = PhysicalPersonAttachment.UpdatedAt,
                CreatedAt = PhysicalPersonAttachment.CreatedAt
            };

            return PhysicalPersonAttachmentViewModel;
        }

        public static PhysicalPersonAttachmentViewModel ConvertToPhysicalPersonAttachmentViewModelLite(this PhysicalPersonAttachment PhysicalPersonAttachment)
        {
            PhysicalPersonAttachmentViewModel PhysicalPersonAttachmentViewModel = new PhysicalPersonAttachmentViewModel()
            {
                Id = PhysicalPersonAttachment.Id,
                Identifier = PhysicalPersonAttachment.Identifier,

                Code = PhysicalPersonAttachment.Code,

                OK = PhysicalPersonAttachment.OK,
                IsActive = PhysicalPersonAttachment.Active,

                UpdatedAt = PhysicalPersonAttachment.UpdatedAt,
                CreatedAt = PhysicalPersonAttachment.CreatedAt
            };

            return PhysicalPersonAttachmentViewModel;
        }

        public static PhysicalPersonAttachment ConvertToPhysicalPersonAttachment(this PhysicalPersonAttachmentViewModel PhysicalPersonAttachmentViewModel)
        {
            PhysicalPersonAttachment PhysicalPersonAttachment = new PhysicalPersonAttachment()
            {
                Id = PhysicalPersonAttachmentViewModel.Id,
                Identifier = PhysicalPersonAttachmentViewModel.Identifier,

                Code = PhysicalPersonAttachmentViewModel.Code,

                PhysicalPersonId = PhysicalPersonAttachmentViewModel.PhysicalPerson?.Id ?? null,

                OK = PhysicalPersonAttachmentViewModel.OK,
                Active = PhysicalPersonAttachmentViewModel.IsActive,

                CreatedById = PhysicalPersonAttachmentViewModel.CreatedBy?.Id ?? null,
                CompanyId = PhysicalPersonAttachmentViewModel.Company?.Id ?? null,

                CreatedAt = PhysicalPersonAttachmentViewModel.CreatedAt,
                UpdatedAt = PhysicalPersonAttachmentViewModel.UpdatedAt
            };

            return PhysicalPersonAttachment;
        }
    }
}
