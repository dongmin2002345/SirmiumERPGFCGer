using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Employees
{
    public static class PhysicalPersonItemMapper
    {
        public static List<PhysicalPersonItemViewModel> ConvertToPhysicalPersonItemViewModelList(this IEnumerable<PhysicalPersonItem> physicalPersonItems)
        {
            List<PhysicalPersonItemViewModel> physicalPersonItemViewModels = new List<PhysicalPersonItemViewModel>();
            foreach (PhysicalPersonItem physicalPersonItem in physicalPersonItems)
            {
                physicalPersonItemViewModels.Add(physicalPersonItem.ConvertToPhysicalPersonItemViewModel());
            }
            return physicalPersonItemViewModels;
        }

        public static PhysicalPersonItemViewModel ConvertToPhysicalPersonItemViewModel(this PhysicalPersonItem physicalPersonItem)
        {
            PhysicalPersonItemViewModel physicalPersonItemViewModel = new PhysicalPersonItemViewModel()
            {
                Id = physicalPersonItem.Id,
                Identifier = physicalPersonItem.Identifier,

                PhysicalPerson = physicalPersonItem.PhysicalPerson?.ConvertToPhysicalPersonViewModelLite(),
                FamilyMember = physicalPersonItem.FamilyMember?.ConvertToFamilyMemberViewModelLite(),

                Name = physicalPersonItem.Name,

                DateOfBirth = physicalPersonItem.DateOfBirth,
                EmbassyDate = physicalPersonItem.EmbassyDate,
                ItemStatus = physicalPersonItem.ItemStatus,

                IsActive = physicalPersonItem.Active,

                CreatedBy = physicalPersonItem.CreatedBy?.ConvertToUserViewModelLite(),
                Company = physicalPersonItem.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = physicalPersonItem.UpdatedAt,
                CreatedAt = physicalPersonItem.CreatedAt
            };

            return physicalPersonItemViewModel;
        }

        public static PhysicalPersonItemViewModel ConvertToPhysicalPersonItemViewModelLite(this PhysicalPersonItem physicalPersonItem)
        {
            PhysicalPersonItemViewModel physicalPersonItemViewModel = new PhysicalPersonItemViewModel()
            {
                Id = physicalPersonItem.Id,
                Identifier = physicalPersonItem.Identifier,

                Name = physicalPersonItem.Name,

                DateOfBirth = physicalPersonItem.DateOfBirth,
                EmbassyDate = physicalPersonItem.EmbassyDate,
                ItemStatus = physicalPersonItem.ItemStatus,

                IsActive = physicalPersonItem.Active,

                UpdatedAt = physicalPersonItem.UpdatedAt,
                CreatedAt = physicalPersonItem.CreatedAt
            };

            return physicalPersonItemViewModel;
        }

        public static PhysicalPersonItem ConvertToPhysicalPersonItem(this PhysicalPersonItemViewModel physicalPersonItemViewModel)
        {
            PhysicalPersonItem physicalPersonItem = new PhysicalPersonItem()
            {
                Id = physicalPersonItemViewModel.Id,
                Identifier = physicalPersonItemViewModel.Identifier,

                PhysicalPersonId = physicalPersonItemViewModel.PhysicalPerson?.Id ?? null,
                FamilyMemberId = physicalPersonItemViewModel.FamilyMember?.Id ?? null,

                Name = physicalPersonItemViewModel.Name,

                DateOfBirth = (DateTime)physicalPersonItemViewModel.DateOfBirth,
                EmbassyDate = physicalPersonItemViewModel.EmbassyDate,
                ItemStatus = physicalPersonItemViewModel.ItemStatus,


                CreatedById = physicalPersonItemViewModel.CreatedBy?.Id ?? null,
                CompanyId = physicalPersonItemViewModel.Company?.Id ?? null,

                CreatedAt = physicalPersonItemViewModel.CreatedAt,
                UpdatedAt = physicalPersonItemViewModel.UpdatedAt
            };

            return physicalPersonItem;
        }
    }
}
