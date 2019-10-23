using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Common.Phonebooks;
using ServiceInterfaces.ViewModels.Common.Phonebooks;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.Phonebooks
{
    public static class PhonebookDocumentMapper
    {
        public static List<PhonebookDocumentViewModel> ConvertToPhonebookDocumentViewModelList(this IEnumerable<PhonebookDocument> PhonebookDocuments)
        {
            List<PhonebookDocumentViewModel> PhonebookDocumentViewModels = new List<PhonebookDocumentViewModel>();
            foreach (PhonebookDocument PhonebookDocument in PhonebookDocuments)
            {
                PhonebookDocumentViewModels.Add(PhonebookDocument.ConvertToPhonebookDocumentViewModel());
            }
            return PhonebookDocumentViewModels;
        }

        public static PhonebookDocumentViewModel ConvertToPhonebookDocumentViewModel(this PhonebookDocument PhonebookDocument)
        {
            PhonebookDocumentViewModel PhonebookDocumentViewModel = new PhonebookDocumentViewModel()
            {
                Id = PhonebookDocument.Id,
                Identifier = PhonebookDocument.Identifier,

                Phonebook = PhonebookDocument.Phonebook?.ConvertToPhonebookViewModelLite(),

                Name = PhonebookDocument.Name,
                CreateDate = PhonebookDocument.CreateDate,
                Path = PhonebookDocument.Path,

                ItemStatus = PhonebookDocument.ItemStatus,
                IsActive = PhonebookDocument.Active,

                CreatedBy = PhonebookDocument.CreatedBy?.ConvertToUserViewModelLite(),
                Company = PhonebookDocument.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = PhonebookDocument.UpdatedAt,
                CreatedAt = PhonebookDocument.CreatedAt,
            };
            return PhonebookDocumentViewModel;
        }

        public static PhonebookDocumentViewModel ConvertToPhonebookDocumentViewModelLite(this PhonebookDocument PhonebookDocument)
        {
            PhonebookDocumentViewModel PhonebookDocumentViewModel = new PhonebookDocumentViewModel()
            {
                Id = PhonebookDocument.Id,
                Identifier = PhonebookDocument.Identifier,

                Name = PhonebookDocument.Name,
                CreateDate = PhonebookDocument.CreateDate,
                Path = PhonebookDocument.Path,

                ItemStatus = PhonebookDocument.ItemStatus,
                IsActive = PhonebookDocument.Active,

                UpdatedAt = PhonebookDocument.UpdatedAt,
                CreatedAt = PhonebookDocument.CreatedAt,
            };
            return PhonebookDocumentViewModel;
        }

        public static PhonebookDocument ConvertToPhonebookDocument(this PhonebookDocumentViewModel PhonebookDocumentViewModel)
        {
            PhonebookDocument PhonebookDocument = new PhonebookDocument()
            {
                Id = PhonebookDocumentViewModel.Id,
                Identifier = PhonebookDocumentViewModel.Identifier,

                PhonebookId = PhonebookDocumentViewModel.Phonebook?.Id ?? null,

                Name = PhonebookDocumentViewModel.Name,
                CreateDate = PhonebookDocumentViewModel.CreateDate,
                Path = PhonebookDocumentViewModel.Path,

                ItemStatus = PhonebookDocumentViewModel.ItemStatus,
                Active = PhonebookDocumentViewModel.IsActive,
                CreatedById = PhonebookDocumentViewModel.CreatedBy?.Id ?? null,
                CompanyId = PhonebookDocumentViewModel.Company?.Id ?? null,

                UpdatedAt = PhonebookDocumentViewModel.UpdatedAt,
                CreatedAt = PhonebookDocumentViewModel.CreatedAt,
            };
            return PhonebookDocument;
        }
    }
}
