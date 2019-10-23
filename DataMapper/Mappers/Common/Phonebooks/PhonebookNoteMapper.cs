using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Common.Phonebooks;
using ServiceInterfaces.ViewModels.Common.Phonebooks;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.Phonebooks
{
    public static class PhonebookNoteMapper
    {
        public static List<PhonebookNoteViewModel> ConvertToPhonebookNoteViewModelList(this IEnumerable<PhonebookNote> PhonebookNotes)
        {
            List<PhonebookNoteViewModel> PhonebookNoteViewModels = new List<PhonebookNoteViewModel>();
            foreach (PhonebookNote PhonebookNote in PhonebookNotes)
            {
                PhonebookNoteViewModels.Add(PhonebookNote.ConvertToPhonebookNoteViewModel());
            }
            return PhonebookNoteViewModels;
        }

        public static PhonebookNoteViewModel ConvertToPhonebookNoteViewModel(this PhonebookNote PhonebookNote)
        {
            PhonebookNoteViewModel PhonebookNoteViewModel = new PhonebookNoteViewModel()
            {
                Id = PhonebookNote.Id,
                Identifier = PhonebookNote.Identifier,

                Phonebook = PhonebookNote.Phonebook?.ConvertToPhonebookViewModelLite(),

                Note = PhonebookNote.Note,
                NoteDate = PhonebookNote.NoteDate,
               
                ItemStatus = PhonebookNote.ItemStatus,
                IsActive = PhonebookNote.Active,

                CreatedBy = PhonebookNote.CreatedBy?.ConvertToUserViewModelLite(),
                Company = PhonebookNote.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = PhonebookNote.UpdatedAt,
                CreatedAt = PhonebookNote.CreatedAt,
            };
            return PhonebookNoteViewModel;
        }

        public static PhonebookNoteViewModel ConvertToPhonebookNoteViewModelLite(this PhonebookNote PhonebookNote)
        {
            PhonebookNoteViewModel PhonebookNoteViewModel = new PhonebookNoteViewModel()
            {
                Id = PhonebookNote.Id,
                Identifier = PhonebookNote.Identifier,

                Note = PhonebookNote.Note,
                NoteDate = PhonebookNote.NoteDate,
               
                ItemStatus = PhonebookNote.ItemStatus,
                IsActive = PhonebookNote.Active,

                UpdatedAt = PhonebookNote.UpdatedAt,
                CreatedAt = PhonebookNote.CreatedAt,
            };
            return PhonebookNoteViewModel;
        }

        public static PhonebookNote ConvertToPhonebookNote(this PhonebookNoteViewModel PhonebookNoteViewModel)
        {
            PhonebookNote PhonebookNote = new PhonebookNote()
            {
                Id = PhonebookNoteViewModel.Id,
                Identifier = PhonebookNoteViewModel.Identifier,

                PhonebookId = PhonebookNoteViewModel.Phonebook?.Id ?? null,

                Note = PhonebookNoteViewModel.Note,
                NoteDate = PhonebookNoteViewModel.NoteDate,
                
                ItemStatus = PhonebookNoteViewModel.ItemStatus,
                Active = PhonebookNoteViewModel.IsActive,
                CreatedById = PhonebookNoteViewModel.CreatedBy?.Id ?? null,
                CompanyId = PhonebookNoteViewModel.Company?.Id ?? null,

                UpdatedAt = PhonebookNoteViewModel.UpdatedAt,
                CreatedAt = PhonebookNoteViewModel.CreatedAt,
            };
            return PhonebookNote;
        }
    }
}
