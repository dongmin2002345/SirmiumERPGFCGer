using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Common.Phonebooks;
using ServiceInterfaces.ViewModels.Common.Phonebooks;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.Phonebooks
{
    public static class PhonebookPhoneMapper
    {
        public static List<PhonebookPhoneViewModel> ConvertToPhonebookPhoneViewModelList(this IEnumerable<PhonebookPhone> PhonebookPhones)
        {
            List<PhonebookPhoneViewModel> PhonebookPhoneViewModels = new List<PhonebookPhoneViewModel>();
            foreach (PhonebookPhone PhonebookPhone in PhonebookPhones)
            {
                PhonebookPhoneViewModels.Add(PhonebookPhone.ConvertToPhonebookPhoneViewModel());
            }
            return PhonebookPhoneViewModels;
        }

        public static PhonebookPhoneViewModel ConvertToPhonebookPhoneViewModel(this PhonebookPhone PhonebookPhone)
        {
            PhonebookPhoneViewModel PhonebookPhoneViewModel = new PhonebookPhoneViewModel()
            {
                Id = PhonebookPhone.Id,
                Identifier = PhonebookPhone.Identifier,

                Phonebook = PhonebookPhone.Phonebook?.ConvertToPhonebookViewModelLite(),

                Email = PhonebookPhone.Email,
                Name = PhonebookPhone.Name,
                SurName = PhonebookPhone.SurName,

                PhoneNumber = PhonebookPhone.PhoneNumber,

               
                ItemStatus = PhonebookPhone.ItemStatus,
                IsActive = PhonebookPhone.Active,

                CreatedBy = PhonebookPhone.CreatedBy?.ConvertToUserViewModelLite(),
                Company = PhonebookPhone.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = PhonebookPhone.UpdatedAt,
                CreatedAt = PhonebookPhone.CreatedAt,
            };
            return PhonebookPhoneViewModel;
        }

        public static PhonebookPhoneViewModel ConvertToPhonebookPhoneViewModelLite(this PhonebookPhone PhonebookPhone)
        {
            PhonebookPhoneViewModel PhonebookPhoneViewModel = new PhonebookPhoneViewModel()
            {
                Id = PhonebookPhone.Id,
                Identifier = PhonebookPhone.Identifier,

                Email = PhonebookPhone.Email,
                Name = PhonebookPhone.Name,
                SurName = PhonebookPhone.SurName,

                PhoneNumber = PhonebookPhone.PhoneNumber,


                ItemStatus = PhonebookPhone.ItemStatus,
                IsActive = PhonebookPhone.Active,

                UpdatedAt = PhonebookPhone.UpdatedAt,
                CreatedAt = PhonebookPhone.CreatedAt,
            };
            return PhonebookPhoneViewModel;
        }

        public static PhonebookPhone ConvertToPhonebookPhone(this PhonebookPhoneViewModel PhonebookPhoneViewModel)
        {
            PhonebookPhone PhonebookPhone = new PhonebookPhone()
            {
                Id = PhonebookPhoneViewModel.Id,
                Identifier = PhonebookPhoneViewModel.Identifier,

                PhonebookId = PhonebookPhoneViewModel.Phonebook?.Id ?? null,

                Email = PhonebookPhoneViewModel.Email,
                Name = PhonebookPhoneViewModel.Name,
                SurName = PhonebookPhoneViewModel.SurName,

                PhoneNumber = PhonebookPhoneViewModel.PhoneNumber,
                ItemStatus = PhonebookPhoneViewModel.ItemStatus,
                Active = PhonebookPhoneViewModel.IsActive,
                CreatedById = PhonebookPhoneViewModel.CreatedBy?.Id ?? null,
                CompanyId = PhonebookPhoneViewModel.Company?.Id ?? null,

                UpdatedAt = PhonebookPhoneViewModel.UpdatedAt,
                CreatedAt = PhonebookPhoneViewModel.CreatedAt,
            };
            return PhonebookPhone;
        }
    }
}
