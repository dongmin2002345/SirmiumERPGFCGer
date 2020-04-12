using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Common.DocumentStores;
using ServiceInterfaces.ViewModels.Common.DocumentStores;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.DocumentStores
{
    public static class DocumentFileMapper
    {
        public static List<DocumentFileViewModel> ConvertToDocumentFileViewModelList(this IEnumerable<DocumentFile> DocumentFiles)
        {
            List<DocumentFileViewModel> DocumentFileViewModels = new List<DocumentFileViewModel>();
            foreach (DocumentFile DocumentFile in DocumentFiles)
            {
                DocumentFileViewModels.Add(DocumentFile.ConvertToDocumentFileViewModel());
            }
            return DocumentFileViewModels;
        }
        public static List<DocumentFile> ConvertToDocumentFileList(this IEnumerable<DocumentFileViewModel> documentFileViewModels)
        {
            List<DocumentFile> documentFiles = new List<DocumentFile>();
            foreach (DocumentFileViewModel DocumentFile in documentFileViewModels)
            {
                documentFiles.Add(DocumentFile.ConvertToDocumentFile());
            }
            return documentFiles;
        }

        public static DocumentFileViewModel ConvertToDocumentFileViewModel(this DocumentFile DocumentFile)
        {
            DocumentFileViewModel DocumentFileViewModel = new DocumentFileViewModel()
            {
                Id = DocumentFile.Id,
                Identifier = DocumentFile.Identifier,

                Name = DocumentFile.Name,
                Path = DocumentFile.Path,
                Size = DocumentFile.Size,

                DocumentFolder = DocumentFile?.DocumentFolder?.ConvertToDocumentFolderViewModelLite(),

                IsActive = DocumentFile.Active,

                Company = DocumentFile.Company?.ConvertToCompanyViewModelLite(),
                CreatedBy = DocumentFile.CreatedBy?.ConvertToUserViewModelLite(),
                UpdatedAt = DocumentFile.UpdatedAt,
                CreatedAt = DocumentFile.CreatedAt
            };



            return DocumentFileViewModel;
        }

        public static DocumentFileViewModel ConvertToDocumentFileViewModelLite(this DocumentFile DocumentFile)
        {
            DocumentFileViewModel DocumentFileViewModel = new DocumentFileViewModel()
            {
                Id = DocumentFile.Id,
                Identifier = DocumentFile.Identifier,

                Name = DocumentFile.Name,
                Path = DocumentFile.Path,
                Size = DocumentFile.Size,

                IsActive = DocumentFile.Active,

                UpdatedAt = DocumentFile.UpdatedAt,
                CreatedAt = DocumentFile.CreatedAt
            };


            return DocumentFileViewModel;
        }

        public static DocumentFile ConvertToDocumentFile(this DocumentFileViewModel DocumentFileViewModel)
        {
            DocumentFile DocumentFile = new DocumentFile()
            {
                Id = DocumentFileViewModel.Id,
                Identifier = DocumentFileViewModel.Identifier,

                Name = DocumentFileViewModel.Name,
                Path = DocumentFileViewModel.Path,
                Size = DocumentFileViewModel.Size,

                DocumentFolderId = DocumentFileViewModel?.DocumentFolder?.Id ?? null,

                CompanyId = DocumentFileViewModel.Company?.Id ?? null,
                CreatedById = DocumentFileViewModel.CreatedBy?.Id ?? null,

                CreatedAt = DocumentFileViewModel.CreatedAt,
                UpdatedAt = DocumentFileViewModel.UpdatedAt
            };

            return DocumentFile;
        }
    }
}
