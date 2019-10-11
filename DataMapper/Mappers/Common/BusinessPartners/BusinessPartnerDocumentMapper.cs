using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using DomainCore.Common.BusinessPartners;
using System.Text;
using DataMapper.Mappers.Common.Identity;
using DataMapper.Mappers.Common.Companies;

namespace DataMapper.Mappers.Common.BusinessPartners
{
    public static class BusinessPartnerDocumentMapper
    {
        public static List<BusinessPartnerDocumentViewModel> ConvertToBusinessPartnerDocumentViewModelList(this IEnumerable<BusinessPartnerDocument> businessPartnerDocuments)
        {
            List<BusinessPartnerDocumentViewModel> BusinessPartnerDocumentViewModels = new List<BusinessPartnerDocumentViewModel>();
            foreach (BusinessPartnerDocument BusinessPartnerDocument in businessPartnerDocuments)
            {
                BusinessPartnerDocumentViewModels.Add(BusinessPartnerDocument.ConvertToBusinessPartnerDocumentViewModel());
            }
            return BusinessPartnerDocumentViewModels;
        }

        public static BusinessPartnerDocumentViewModel ConvertToBusinessPartnerDocumentViewModel(this BusinessPartnerDocument businessPartnerDocument)
        {
            BusinessPartnerDocumentViewModel BusinessPartnerDocumentViewModel = new BusinessPartnerDocumentViewModel()
            {
                Id = businessPartnerDocument.Id,
                Identifier = businessPartnerDocument.Identifier,

                BusinessPartner = businessPartnerDocument.BusinessPartner?.ConvertToBusinessPartnerViewModelLite(),

                Name = businessPartnerDocument.Name,
                CreateDate = businessPartnerDocument.CreateDate,
                Path = businessPartnerDocument.Path,
                ItemStatus = businessPartnerDocument.ItemStatus,
                IsActive = businessPartnerDocument.Active,

                CreatedBy = businessPartnerDocument.CreatedBy?.ConvertToUserViewModelLite(),
                Company = businessPartnerDocument.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = businessPartnerDocument.UpdatedAt,
                CreatedAt = businessPartnerDocument.CreatedAt
            };

            return BusinessPartnerDocumentViewModel;
        }

        public static BusinessPartnerDocumentViewModel ConvertToBusinessPartnerDocumentViewModelLite(this BusinessPartnerDocument businessPartnerDocument)
        {
            BusinessPartnerDocumentViewModel BusinessPartnerDocumentViewModel = new BusinessPartnerDocumentViewModel()
            {
                Id = businessPartnerDocument.Id,
                Identifier = businessPartnerDocument.Identifier,

                Name = businessPartnerDocument.Name,
                CreateDate = businessPartnerDocument.CreateDate,
                Path = businessPartnerDocument.Path,
                ItemStatus = businessPartnerDocument.ItemStatus,
                IsActive = businessPartnerDocument.Active,

                UpdatedAt = businessPartnerDocument.UpdatedAt,
                CreatedAt = businessPartnerDocument.CreatedAt
            };

            return BusinessPartnerDocumentViewModel;
        }

        public static BusinessPartnerDocument ConvertToBusinessPartnerDocument(this BusinessPartnerDocumentViewModel businessPartnerDocumentViewModel)
        {
            BusinessPartnerDocument BusinessPartnerDocument = new BusinessPartnerDocument()
            {
                Id = businessPartnerDocumentViewModel.Id,
                Identifier = businessPartnerDocumentViewModel.Identifier,

                BusinessPartnerId = businessPartnerDocumentViewModel.BusinessPartner?.Id ?? null,

                Name = businessPartnerDocumentViewModel.Name,
                CreateDate = businessPartnerDocumentViewModel.CreateDate,
                Path = businessPartnerDocumentViewModel.Path,
                ItemStatus = businessPartnerDocumentViewModel.ItemStatus,
                Active = businessPartnerDocumentViewModel.IsActive,

                CreatedById = businessPartnerDocumentViewModel.CreatedBy?.Id ?? null,
                CompanyId = businessPartnerDocumentViewModel.Company?.Id ?? null,

                CreatedAt = businessPartnerDocumentViewModel.CreatedAt,
                UpdatedAt = businessPartnerDocumentViewModel.UpdatedAt
            };

            return BusinessPartnerDocument;
        }
    }
}
