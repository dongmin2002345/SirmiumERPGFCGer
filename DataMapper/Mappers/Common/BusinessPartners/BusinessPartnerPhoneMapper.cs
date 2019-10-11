using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.BusinessPartners
{
    public static class BusinessPartnerPhoneMapper
    {
        public static List<BusinessPartnerPhoneViewModel> ConvertToBusinessPartnerPhoneViewModelList(this IEnumerable<BusinessPartnerPhone> businessPartnerPhones)
        {
            List<BusinessPartnerPhoneViewModel> businessPartnerPhoneViewModels = new List<BusinessPartnerPhoneViewModel>();
            foreach (BusinessPartnerPhone businessPartnerPhone in businessPartnerPhones)
            {
                businessPartnerPhoneViewModels.Add(businessPartnerPhone.ConvertToBusinessPartnerPhoneViewModel());
            }
            return businessPartnerPhoneViewModels;
        }

        public static BusinessPartnerPhoneViewModel ConvertToBusinessPartnerPhoneViewModel(this BusinessPartnerPhone businessPartnerPhone)
        {
            BusinessPartnerPhoneViewModel businessPartnerPhoneViewModel = new BusinessPartnerPhoneViewModel()
            {
                Id = businessPartnerPhone.Id,
                Identifier = businessPartnerPhone.Identifier,

                BusinessPartner = businessPartnerPhone.BusinessPartner?.ConvertToBusinessPartnerViewModelLite(),

                Phone = businessPartnerPhone.Phone,
                Mobile = businessPartnerPhone.Mobile,
                Fax = businessPartnerPhone.Fax,
                Email = businessPartnerPhone.Email,
                ContactPersonFirstName = businessPartnerPhone.ContactPersonFirstName,
                ContactPersonLastName = businessPartnerPhone.ContactPersonLastName,

                Birthday = businessPartnerPhone.Birthday,

                Description = businessPartnerPhone.Description,
                ItemStatus = businessPartnerPhone.ItemStatus,
                IsActive = businessPartnerPhone.Active,

                CreatedBy = businessPartnerPhone.CreatedBy?.ConvertToUserViewModelLite(),
                Company = businessPartnerPhone.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = businessPartnerPhone.UpdatedAt,
                CreatedAt = businessPartnerPhone.CreatedAt,
            };
            return businessPartnerPhoneViewModel;
        }

        public static BusinessPartnerPhoneViewModel ConvertToBusinessPartnerPhoneViewModelLite(this BusinessPartnerPhone businessPartnerPhone)
        {
            BusinessPartnerPhoneViewModel businessPartnerPhoneViewModel = new BusinessPartnerPhoneViewModel()
            {
                Id = businessPartnerPhone.Id,
                Identifier = businessPartnerPhone.Identifier,

                Phone = businessPartnerPhone.Phone,
                Mobile = businessPartnerPhone.Mobile,
                Fax = businessPartnerPhone.Fax,
                Email = businessPartnerPhone.Email,
                ContactPersonFirstName = businessPartnerPhone.ContactPersonFirstName,
                ContactPersonLastName = businessPartnerPhone.ContactPersonLastName,

                Birthday = businessPartnerPhone.Birthday, 

                Description = businessPartnerPhone.Description,
                ItemStatus = businessPartnerPhone.ItemStatus,
                IsActive = businessPartnerPhone.Active,

                UpdatedAt = businessPartnerPhone.UpdatedAt,
                CreatedAt = businessPartnerPhone.CreatedAt,
            };
            return businessPartnerPhoneViewModel;
        }

        public static BusinessPartnerPhone ConvertToBusinessPartnerPhone(this BusinessPartnerPhoneViewModel businessPartnerPhoneViewModel)
        {
            BusinessPartnerPhone businessPartnerPhone = new BusinessPartnerPhone()
            {
                Id = businessPartnerPhoneViewModel.Id,
                Identifier = businessPartnerPhoneViewModel.Identifier,

                BusinessPartnerId = businessPartnerPhoneViewModel.BusinessPartner?.Id ?? null,

                Phone = businessPartnerPhoneViewModel.Phone,
                Mobile = businessPartnerPhoneViewModel.Mobile,
                Fax = businessPartnerPhoneViewModel.Fax,
                Email = businessPartnerPhoneViewModel.Email,
                ContactPersonFirstName = businessPartnerPhoneViewModel.ContactPersonFirstName,
                ContactPersonLastName = businessPartnerPhoneViewModel.ContactPersonLastName,

                Birthday = businessPartnerPhoneViewModel.Birthday, 

                Description = businessPartnerPhoneViewModel.Description,
                ItemStatus = businessPartnerPhoneViewModel.ItemStatus,
                CreatedById = businessPartnerPhoneViewModel.CreatedBy?.Id ?? null,
                CompanyId = businessPartnerPhoneViewModel.Company?.Id ?? null,

                UpdatedAt = businessPartnerPhoneViewModel.UpdatedAt,
                CreatedAt = businessPartnerPhoneViewModel.CreatedAt,
            };
            return businessPartnerPhone;
        }
    }
}
