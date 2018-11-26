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
            List<ConstructionSiteDocumentViewModel> ConstructionSiteDocumentViewModels = new List<ConstructionSiteDocumentViewModel>();
            foreach (ConstructionSiteDocument ConstructionSiteDocument in constructionSiteDocuments)
            {
                ConstructionSiteDocumentViewModels.Add(ConstructionSiteDocument.ConvertToConstructionSiteDocumentViewModel());
            }
            return ConstructionSiteDocumentViewModels;
        }

        public static ConstructionSiteDocumentViewModel ConvertToConstructionSiteDocumentViewModel(this ConstructionSiteDocument constructionSiteDocument)
        {
            ConstructionSiteDocumentViewModel ConstructionSiteDocumentViewModel = new ConstructionSiteDocumentViewModel()
            {
                Id = constructionSiteDocument.Id,
                Identifier = constructionSiteDocument.Identifier,

                ConstructionSite = constructionSiteDocument.ConstructionSite?.ConvertToConstructionSiteViewModelLite(),

                Name = constructionSiteDocument.Name,
                CreateDate = constructionSiteDocument.CreateDate,
                Path = constructionSiteDocument.Path,

                IsActive = constructionSiteDocument.Active,

                CreatedBy = constructionSiteDocument.CreatedBy?.ConvertToUserViewModelLite(),
                Company = constructionSiteDocument.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = constructionSiteDocument.UpdatedAt,
                CreatedAt = constructionSiteDocument.CreatedAt
            };

            return ConstructionSiteDocumentViewModel;
        }

        public static ConstructionSiteDocumentViewModel ConvertToConstructionSiteDocumentViewModelLite(this ConstructionSiteDocument constructionSiteDocument)
        {
            ConstructionSiteDocumentViewModel ConstructionSiteDocumentViewModel = new ConstructionSiteDocumentViewModel()
            {
                Id = constructionSiteDocument.Id,
                Identifier = constructionSiteDocument.Identifier,

                Name = constructionSiteDocument.Name,
                CreateDate = constructionSiteDocument.CreateDate,
                Path = constructionSiteDocument.Path,

                IsActive = constructionSiteDocument.Active,

                UpdatedAt = constructionSiteDocument.UpdatedAt,
                CreatedAt = constructionSiteDocument.CreatedAt
            };

            return ConstructionSiteDocumentViewModel;
        }

        public static ConstructionSiteDocument ConvertToConstructionSiteDocument(this ConstructionSiteDocumentViewModel constructionSiteDocumentViewModel)
        {
            ConstructionSiteDocument ConstructionSiteDocument = new ConstructionSiteDocument()
            {
                Id = constructionSiteDocumentViewModel.Id,
                Identifier = constructionSiteDocumentViewModel.Identifier,

                ConstructionSiteId = constructionSiteDocumentViewModel.ConstructionSite?.Id ?? null,

                Name = constructionSiteDocumentViewModel.Name,
                CreateDate = constructionSiteDocumentViewModel.CreateDate,
                Path = constructionSiteDocumentViewModel.Path,

                Active = constructionSiteDocumentViewModel.IsActive,

                CreatedById = constructionSiteDocumentViewModel.CreatedBy?.Id ?? null,
                CompanyId = constructionSiteDocumentViewModel.Company?.Id ?? null,

                CreatedAt = constructionSiteDocumentViewModel.CreatedAt,
                UpdatedAt = constructionSiteDocumentViewModel.UpdatedAt
            };

            return ConstructionSiteDocument;
        }
    }
}
