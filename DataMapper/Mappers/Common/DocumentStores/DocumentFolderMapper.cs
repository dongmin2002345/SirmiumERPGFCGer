using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Common.DocumentStores;
using ServiceInterfaces.ViewModels.Common.DocumentStores;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.DocumentStores
{
    public static class DocumentFolderMapper
    {
        public static List<DocumentFolderViewModel> ConvertToDocumentFolderViewModelList(this IEnumerable<DocumentFolder> DocumentFolders)
        {
            List<DocumentFolderViewModel> DocumentFolderViewModels = new List<DocumentFolderViewModel>();
            foreach (DocumentFolder DocumentFolder in DocumentFolders)
            {
                DocumentFolderViewModels.Add(DocumentFolder.ConvertToDocumentFolderViewModel());
            }
            return DocumentFolderViewModels;
        }
        public static List<DocumentFolder> ConvertToDocumentFolderList(this IEnumerable<DocumentFolderViewModel> documentFolderViewModels)
        {
            List<DocumentFolder> documentFolders = new List<DocumentFolder>();
            foreach (DocumentFolderViewModel DocumentFolder in documentFolderViewModels)
            {
                documentFolders.Add(DocumentFolder.ConvertToDocumentFolder());
            }
            return documentFolders;
        }

        public static DocumentFolderViewModel ConvertToDocumentFolderViewModel(this DocumentFolder DocumentFolder)
        {
            DocumentFolderViewModel DocumentFolderViewModel = new DocumentFolderViewModel()
            {
                Id = DocumentFolder.Id,
                Identifier = DocumentFolder.Identifier,

                Name = DocumentFolder.Name,
                Path = DocumentFolder.Path,

                ParentFolder = DocumentFolder?.ParentFolder?.ConvertToDocumentFolderViewModelLite(),

                IsActive = DocumentFolder.Active,

                Company = DocumentFolder.Company?.ConvertToCompanyViewModelLite(),
                CreatedBy = DocumentFolder.CreatedBy?.ConvertToUserViewModelLite(),
                UpdatedAt = DocumentFolder.UpdatedAt,
                CreatedAt = DocumentFolder.CreatedAt
            };



            return DocumentFolderViewModel;
        }

        public static DocumentFolderViewModel ConvertToDocumentFolderViewModelLite(this DocumentFolder DocumentFolder)
        {
            DocumentFolderViewModel DocumentFolderViewModel = new DocumentFolderViewModel()
            {
                Id = DocumentFolder.Id,
                Identifier = DocumentFolder.Identifier,

                Name = DocumentFolder.Name,
                Path = DocumentFolder.Path,

                IsActive = DocumentFolder.Active,

                UpdatedAt = DocumentFolder.UpdatedAt,
                CreatedAt = DocumentFolder.CreatedAt
            };


            return DocumentFolderViewModel;
        }

        public static DocumentFolder ConvertToDocumentFolder(this DocumentFolderViewModel DocumentFolderViewModel)
        {
            DocumentFolder DocumentFolder = new DocumentFolder()
            {
                Id = DocumentFolderViewModel.Id,
                Identifier = DocumentFolderViewModel.Identifier,

                Name = DocumentFolderViewModel.Name,
                Path = DocumentFolderViewModel.Path,

                ParentFolderId = DocumentFolderViewModel?.ParentFolder?.Id ?? null,

                CompanyId = DocumentFolderViewModel.Company?.Id ?? null,
                CreatedById = DocumentFolderViewModel.CreatedBy?.Id ?? null,

                CreatedAt = DocumentFolderViewModel.CreatedAt,
                UpdatedAt = DocumentFolderViewModel.UpdatedAt
            };

            return DocumentFolder;
        }
    }
}
