using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.ConstructionSites;
using ServiceInterfaces.ViewModels.ConstructionSites;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.ConstructionSites
{
    public static class ConstructionSiteDocumentMapper
    {
        public static List<ConstructionSiteDocumentViewModel> ConvertToConstructionSiteDocumentViewModelList(this IEnumerable<ConstructionSiteDocument> constructionSiteDocuments)
        {
            List<ConstructionSiteDocumentViewModel> constructionSiteDocumentViewModels = new List<ConstructionSiteDocumentViewModel>();
            foreach (ConstructionSiteDocument constructionSiteDocument in constructionSiteDocuments)
            {
                constructionSiteDocumentViewModels.Add(constructionSiteDocument.ConvertToConstructionSiteDocumentViewModel());
            }
            return constructionSiteDocumentViewModels;
        }

        public static ConstructionSiteDocumentViewModel ConvertToConstructionSiteDocumentViewModel(this ConstructionSiteDocument constructionSiteDocument)
        {
            ConstructionSiteDocumentViewModel constructionSiteDocumentViewModel = new ConstructionSiteDocumentViewModel()
            {
                Id = constructionSiteDocument.Id,
                Identifier = constructionSiteDocument.Identifier,

                ConstructionSite = constructionSiteDocument.ConstructionSite?.ConvertToConstructionSiteViewModelLite(),

                Name = constructionSiteDocument.Name,
                CreateDate = constructionSiteDocument.CreateDate,
                Path = constructionSiteDocument.Path,
                ItemStatus = constructionSiteDocument.ItemStatus,

                IsActive = constructionSiteDocument.Active,

                CreatedBy = constructionSiteDocument.CreatedBy?.ConvertToUserViewModelLite(),
                Company = constructionSiteDocument.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = constructionSiteDocument.UpdatedAt,
                CreatedAt = constructionSiteDocument.CreatedAt
            };

            return constructionSiteDocumentViewModel;
        }

        public static ConstructionSiteDocumentViewModel ConvertToConstructionSiteDocumentViewModelLite(this ConstructionSiteDocument constructionSiteDocument)
        {
            ConstructionSiteDocumentViewModel constructionSiteDocumentViewModel = new ConstructionSiteDocumentViewModel()
            {
                Id = constructionSiteDocument.Id,
                Identifier = constructionSiteDocument.Identifier,

                Name = constructionSiteDocument.Name,
                CreateDate = constructionSiteDocument.CreateDate,
                Path = constructionSiteDocument.Path,
                ItemStatus = constructionSiteDocument.ItemStatus,

                IsActive = constructionSiteDocument.Active,

                UpdatedAt = constructionSiteDocument.UpdatedAt,
                CreatedAt = constructionSiteDocument.CreatedAt
            };

            return constructionSiteDocumentViewModel;
        }

        public static ConstructionSiteDocument ConvertToConstructionSiteDocument(this ConstructionSiteDocumentViewModel constructionSiteDocumentViewModel)
        {
            ConstructionSiteDocument constructionSiteDocument = new ConstructionSiteDocument()
            {
                Id = constructionSiteDocumentViewModel.Id,
                Identifier = constructionSiteDocumentViewModel.Identifier,

                ConstructionSiteId = constructionSiteDocumentViewModel.ConstructionSite?.Id ?? null,

                Name = constructionSiteDocumentViewModel.Name,
                CreateDate = constructionSiteDocumentViewModel.CreateDate,
                Path = constructionSiteDocumentViewModel.Path,
                ItemStatus = constructionSiteDocumentViewModel.ItemStatus,

                Active = constructionSiteDocumentViewModel.IsActive,

                CreatedById = constructionSiteDocumentViewModel.CreatedBy?.Id ?? null,
                CompanyId = constructionSiteDocumentViewModel.Company?.Id ?? null,

                CreatedAt = constructionSiteDocumentViewModel.CreatedAt,
                UpdatedAt = constructionSiteDocumentViewModel.UpdatedAt
            };

            return constructionSiteDocument;
        }
    }
}
