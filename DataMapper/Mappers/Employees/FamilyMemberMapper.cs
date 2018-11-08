using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Employees
{
    public static class FamilyMemberMapper
    {
        public static List<FamilyMemberViewModel> ConvertToFamilyMemberViewModelList(this IEnumerable<FamilyMember> familyMembers)
        {
            List<FamilyMemberViewModel> citiesViewModels = new List<FamilyMemberViewModel>();
            foreach (FamilyMember familyMember in familyMembers)
            {
                citiesViewModels.Add(familyMember.ConvertToFamilyMemberViewModel());
            }
            return citiesViewModels;
        }

        public static FamilyMemberViewModel ConvertToFamilyMemberViewModel(this FamilyMember familyMember)
        {
            FamilyMemberViewModel FamilyMemberViewModel = new FamilyMemberViewModel()
            {
                Id = familyMember.Id,
                Identifier = familyMember.Identifier,

                Code = familyMember.Code,
                Name = familyMember.Name,

                IsActive = familyMember.Active,

                CreatedBy = familyMember.CreatedBy?.ConvertToUserViewModelLite(),
                Company = familyMember.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = familyMember.UpdatedAt,
                CreatedAt = familyMember.CreatedAt

            };
            return FamilyMemberViewModel;
        }

        public static List<FamilyMemberViewModel> ConvertToFamilyMemberViewModelListLite(this IEnumerable<FamilyMember> cities)
        {
            List<FamilyMemberViewModel> FamilyMemberViewModels = new List<FamilyMemberViewModel>();
            foreach (FamilyMember remedy in cities)
            {
                FamilyMemberViewModels.Add(remedy.ConvertToFamilyMemberViewModelLite());
            }
            return FamilyMemberViewModels;
        }


        public static FamilyMemberViewModel ConvertToFamilyMemberViewModelLite(this FamilyMember familyMember)
        {
            FamilyMemberViewModel familyMemberViewModel = new FamilyMemberViewModel()
            {
                Id = familyMember.Id,
                Identifier = familyMember.Identifier,

                Code = familyMember.Code,
                Name = familyMember.Name,

                IsActive = familyMember.Active,

                CreatedAt = familyMember.CreatedAt,
                UpdatedAt = familyMember.UpdatedAt
            };
            return familyMemberViewModel;
        }

        public static FamilyMember ConvertToFamilyMember(this FamilyMemberViewModel familyMemberViewModel)
        {
            FamilyMember familyMember = new FamilyMember()
            {
                Id = familyMemberViewModel.Id,
                Identifier = familyMemberViewModel.Identifier,

                Code = familyMemberViewModel.Code,
                Name = familyMemberViewModel.Name,

                CreatedById = familyMemberViewModel.CreatedBy?.Id ?? null,
                CompanyId = familyMemberViewModel.Company?.Id ?? null,

                CreatedAt = familyMemberViewModel.CreatedAt,
                UpdatedAt = familyMemberViewModel.UpdatedAt

            };
            return familyMember;
        }
    }
}


