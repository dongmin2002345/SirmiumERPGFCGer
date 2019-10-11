using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.BusinessPartners
{
    public static class BusinessPartnerInstitutionMapper
    {
        public static List<BusinessPartnerInstitutionViewModel> ConvertToBusinessPartnerInstitutionViewModelList(this IEnumerable<BusinessPartnerInstitution> businessPartnerInstitutions)
        {
            List<BusinessPartnerInstitutionViewModel> businessPartnerInstitutionViewModels = new List<BusinessPartnerInstitutionViewModel>();
            foreach (BusinessPartnerInstitution businessPartnerInstitution in businessPartnerInstitutions)
            {
                businessPartnerInstitutionViewModels.Add(businessPartnerInstitution.ConvertToBusinessPartnerInstitutionViewModel());
            }
            return businessPartnerInstitutionViewModels;
        }

        public static BusinessPartnerInstitutionViewModel ConvertToBusinessPartnerInstitutionViewModel(this BusinessPartnerInstitution businessPartnerInstitution)
        {
            BusinessPartnerInstitutionViewModel businessPartnerInstitutionViewModel = new BusinessPartnerInstitutionViewModel()
            {
                Id = businessPartnerInstitution.Id,
                Identifier = businessPartnerInstitution.Identifier,

                BusinessPartner = businessPartnerInstitution.BusinessPartner?.ConvertToBusinessPartnerViewModelLite(),

                Institution = businessPartnerInstitution.Institution,
                Username = businessPartnerInstitution.Username,
                Password = businessPartnerInstitution.Password,
                ContactPerson = businessPartnerInstitution.ContactPerson,
                Phone = businessPartnerInstitution.Phone,
                Fax = businessPartnerInstitution.Fax,
                Email = businessPartnerInstitution.Email,
                ItemStatus = businessPartnerInstitution.ItemStatus,
                IsActive = businessPartnerInstitution.Active,

                CreatedBy = businessPartnerInstitution.CreatedBy?.ConvertToUserViewModelLite(),
                Company = businessPartnerInstitution.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = businessPartnerInstitution.UpdatedAt,
                CreatedAt = businessPartnerInstitution.CreatedAt,
            };
            return businessPartnerInstitutionViewModel;
        }

        public static BusinessPartnerInstitutionViewModel ConvertToBusinessPartnerInstitutionViewModelLite(this BusinessPartnerInstitution businessPartnerInstitution)
        {
            BusinessPartnerInstitutionViewModel businessPartnerInstitutionViewModel = new BusinessPartnerInstitutionViewModel()
            {
                Id = businessPartnerInstitution.Id,
                Identifier = businessPartnerInstitution.Identifier,

                Institution = businessPartnerInstitution.Institution,
                Username = businessPartnerInstitution.Username,
                Password = businessPartnerInstitution.Password,
                ContactPerson = businessPartnerInstitution.ContactPerson,
                Phone = businessPartnerInstitution.Phone,
                Fax = businessPartnerInstitution.Fax,
                Email = businessPartnerInstitution.Email,
                ItemStatus = businessPartnerInstitution.ItemStatus,
                IsActive = businessPartnerInstitution.Active,

                UpdatedAt = businessPartnerInstitution.UpdatedAt,
                CreatedAt = businessPartnerInstitution.CreatedAt,
            };
            return businessPartnerInstitutionViewModel;
        }

        public static BusinessPartnerInstitution ConvertToBusinessPartnerInstitution(this BusinessPartnerInstitutionViewModel businessPartnerInstitutionViewModel)
        {
            BusinessPartnerInstitution businessPartnerInstitution = new BusinessPartnerInstitution()
            {
                Id = businessPartnerInstitutionViewModel.Id,
                Identifier = businessPartnerInstitutionViewModel.Identifier,

                BusinessPartnerId = businessPartnerInstitutionViewModel.BusinessPartner?.Id ?? null,

                Institution = businessPartnerInstitutionViewModel.Institution,
                Username = businessPartnerInstitutionViewModel.Username,
                Password = businessPartnerInstitutionViewModel.Password,
                ContactPerson = businessPartnerInstitutionViewModel.ContactPerson,
                Phone = businessPartnerInstitutionViewModel.Phone,
                Fax = businessPartnerInstitutionViewModel.Fax,
                Email = businessPartnerInstitutionViewModel.Email,
                ItemStatus = businessPartnerInstitutionViewModel.ItemStatus,
                CreatedById = businessPartnerInstitutionViewModel.CreatedBy?.Id ?? null,
                CompanyId = businessPartnerInstitutionViewModel.Company?.Id ?? null,

                UpdatedAt = businessPartnerInstitutionViewModel.UpdatedAt,
                CreatedAt = businessPartnerInstitutionViewModel.CreatedAt,
            };
            return businessPartnerInstitution;
        }
    }
}
