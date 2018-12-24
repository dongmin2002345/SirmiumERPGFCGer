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
        public static List<PhysicalPersonItemViewModel> ConvertToPhysicalPersonItemViewModelList(this IEnumerable<PhysicalPersonItem> PhysicalPersonItems)
        {
            List<PhysicalPersonItemViewModel> PhysicalPersonItemViewModels = new List<PhysicalPersonItemViewModel>();
            foreach (PhysicalPersonItem PhysicalPersonItem in PhysicalPersonItems)
            {
                PhysicalPersonItemViewModels.Add(PhysicalPersonItem.ConvertToPhysicalPersonItemViewModel());
            }
            return PhysicalPersonItemViewModels;
        }

        public static PhysicalPersonItemViewModel ConvertToPhysicalPersonItemViewModel(this PhysicalPersonItem PhysicalPersonItem)
        {
            PhysicalPersonItemViewModel PhysicalPersonItemViewModel = new PhysicalPersonItemViewModel()
            {
                Id = PhysicalPersonItem.Id,
                Identifier = PhysicalPersonItem.Identifier,

                PhysicalPerson = PhysicalPersonItem.PhysicalPerson?.ConvertToPhysicalPersonViewModelLite(),
                FamilyMember = PhysicalPersonItem.FamilyMember?.ConvertToFamilyMemberViewModelLite(),

                Name = PhysicalPersonItem.Name,

                DateOfBirth = PhysicalPersonItem.DateOfBirth,
                EmbassyDate = PhysicalPersonItem.EmbassyDate,

                IsActive = PhysicalPersonItem.Active,

                CreatedBy = PhysicalPersonItem.CreatedBy?.ConvertToUserViewModelLite(),
                Company = PhysicalPersonItem.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = PhysicalPersonItem.UpdatedAt,
                CreatedAt = PhysicalPersonItem.CreatedAt
            };

            return PhysicalPersonItemViewModel;
        }

        public static PhysicalPersonItemViewModel ConvertToPhysicalPersonItemViewModelLite(this PhysicalPersonItem PhysicalPersonItem)
        {
            PhysicalPersonItemViewModel PhysicalPersonItemViewModel = new PhysicalPersonItemViewModel()
            {
                Id = PhysicalPersonItem.Id,
                Identifier = PhysicalPersonItem.Identifier,

                Name = PhysicalPersonItem.Name,

                DateOfBirth = PhysicalPersonItem.DateOfBirth,
                EmbassyDate = PhysicalPersonItem.EmbassyDate,

                IsActive = PhysicalPersonItem.Active,

                UpdatedAt = PhysicalPersonItem.UpdatedAt,
                CreatedAt = PhysicalPersonItem.CreatedAt
            };

            return PhysicalPersonItemViewModel;
        }

        public static PhysicalPersonItem ConvertToPhysicalPersonItem(this PhysicalPersonItemViewModel PhysicalPersonItemViewModel)
        {
            PhysicalPersonItem PhysicalPersonItem = new PhysicalPersonItem()
            {
                Id = PhysicalPersonItemViewModel.Id,
                Identifier = PhysicalPersonItemViewModel.Identifier,

                PhysicalPersonId = PhysicalPersonItemViewModel.PhysicalPerson?.Id ?? null,
                FamilyMemberId = PhysicalPersonItemViewModel.FamilyMember?.Id ?? null,

                Name = PhysicalPersonItemViewModel.Name,

                DateOfBirth = (DateTime)PhysicalPersonItemViewModel.DateOfBirth,
                EmbassyDate = PhysicalPersonItemViewModel.EmbassyDate,

                CreatedById = PhysicalPersonItemViewModel.CreatedBy?.Id ?? null,
                CompanyId = PhysicalPersonItemViewModel.Company?.Id ?? null,

                CreatedAt = PhysicalPersonItemViewModel.CreatedAt,
                UpdatedAt = PhysicalPersonItemViewModel.UpdatedAt
            };

            return PhysicalPersonItem;
        }
    }
}
