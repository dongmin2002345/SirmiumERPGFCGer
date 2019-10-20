using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.BusinessPartners
{
    public static class BusinessPartnerNoteMapper
    {
        public static List<BusinessPartnerNoteViewModel> ConvertToBusinessPartnerNoteViewModelList(this IEnumerable<BusinessPartnerNote> BusinessPartnerNotes)
        {
            List<BusinessPartnerNoteViewModel> BusinessPartnerNoteViewModels = new List<BusinessPartnerNoteViewModel>();
            foreach (BusinessPartnerNote BusinessPartnerNote in BusinessPartnerNotes)
            {
                BusinessPartnerNoteViewModels.Add(BusinessPartnerNote.ConvertToBusinessPartnerNoteViewModel());
            }
            return BusinessPartnerNoteViewModels;
        }

        public static BusinessPartnerNoteViewModel ConvertToBusinessPartnerNoteViewModel(this BusinessPartnerNote BusinessPartnerNote)
        {
            BusinessPartnerNoteViewModel BusinessPartnerNoteViewModel = new BusinessPartnerNoteViewModel()
            {
                Id = BusinessPartnerNote.Id,
                Identifier = BusinessPartnerNote.Identifier,

                BusinessPartner = BusinessPartnerNote.BusinessPartner?.ConvertToBusinessPartnerViewModelLite(),

                Note = BusinessPartnerNote.Note,
                NoteDate = BusinessPartnerNote.NoteDate,
                ItemStatus = BusinessPartnerNote.ItemStatus,
                IsActive = BusinessPartnerNote.Active,

                CreatedBy = BusinessPartnerNote.CreatedBy?.ConvertToUserViewModelLite(),
                Company = BusinessPartnerNote.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = BusinessPartnerNote.UpdatedAt,
                CreatedAt = BusinessPartnerNote.CreatedAt
            };

            return BusinessPartnerNoteViewModel;
        }

        public static BusinessPartnerNoteViewModel ConvertToBusinessPartnerNoteViewModelLite(this BusinessPartnerNote BusinessPartnerNote)
        {
            BusinessPartnerNoteViewModel BusinessPartnerNoteViewModel = new BusinessPartnerNoteViewModel()
            {
                Id = BusinessPartnerNote.Id,
                Identifier = BusinessPartnerNote.Identifier,

                Note = BusinessPartnerNote.Note,
                NoteDate = BusinessPartnerNote.NoteDate,
                ItemStatus = BusinessPartnerNote.ItemStatus,
                IsActive = BusinessPartnerNote.Active,

                UpdatedAt = BusinessPartnerNote.UpdatedAt,
                CreatedAt = BusinessPartnerNote.CreatedAt
            };

            return BusinessPartnerNoteViewModel;
        }

        public static BusinessPartnerNote ConvertToBusinessPartnerNote(this BusinessPartnerNoteViewModel BusinessPartnerNoteViewModel)
        {
            BusinessPartnerNote BusinessPartnerNote = new BusinessPartnerNote()
            {
                Id = BusinessPartnerNoteViewModel.Id,
                Identifier = BusinessPartnerNoteViewModel.Identifier,

                BusinessPartnerId = BusinessPartnerNoteViewModel.BusinessPartner?.Id ?? null,

                Note = BusinessPartnerNoteViewModel.Note,
                NoteDate = BusinessPartnerNoteViewModel.NoteDate,
                ItemStatus = BusinessPartnerNoteViewModel.ItemStatus,
                Active = BusinessPartnerNoteViewModel.IsActive,
                CreatedById = BusinessPartnerNoteViewModel.CreatedBy?.Id ?? null,
                CompanyId = BusinessPartnerNoteViewModel.Company?.Id ?? null,

                CreatedAt = BusinessPartnerNoteViewModel.CreatedAt,
                UpdatedAt = BusinessPartnerNoteViewModel.UpdatedAt
            };

            return BusinessPartnerNote;
        }
    }
}
