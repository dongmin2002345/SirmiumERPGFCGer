using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Common.InputInvoices;
using ServiceInterfaces.ViewModels.Common.InputInvoices;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.InputInvoices
{
    public static class InputInvoiceNoteMapper
    {
        public static List<InputInvoiceNoteViewModel> ConvertToInputInvoiceNoteViewModelList(this IEnumerable<InputInvoiceNote> InputInvoiceNotes)
        {
            List<InputInvoiceNoteViewModel> InputInvoiceNoteViewModels = new List<InputInvoiceNoteViewModel>();
            foreach (InputInvoiceNote InputInvoiceNote in InputInvoiceNotes)
            {
                InputInvoiceNoteViewModels.Add(InputInvoiceNote.ConvertToInputInvoiceNoteViewModel());
            }
            return InputInvoiceNoteViewModels;
        }

        public static InputInvoiceNoteViewModel ConvertToInputInvoiceNoteViewModel(this InputInvoiceNote InputInvoiceNote)
        {
            InputInvoiceNoteViewModel InputInvoiceNoteViewModel = new InputInvoiceNoteViewModel()
            {
                Id = InputInvoiceNote.Id,
                Identifier = InputInvoiceNote.Identifier,

                InputInvoice = InputInvoiceNote.InputInvoice?.ConvertToInputInvoiceViewModelLite(),

                Note = InputInvoiceNote.Note,
                NoteDate = InputInvoiceNote.NoteDate,
                ItemStatus = InputInvoiceNote.ItemStatus,
                IsActive = InputInvoiceNote.Active,

                CreatedBy = InputInvoiceNote.CreatedBy?.ConvertToUserViewModelLite(),
                Company = InputInvoiceNote.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = InputInvoiceNote.UpdatedAt,
                CreatedAt = InputInvoiceNote.CreatedAt
            };

            return InputInvoiceNoteViewModel;
        }

        public static InputInvoiceNoteViewModel ConvertToInputInvoiceNoteViewModelLite(this InputInvoiceNote InputInvoiceNote)
        {
            InputInvoiceNoteViewModel InputInvoiceNoteViewModel = new InputInvoiceNoteViewModel()
            {
                Id = InputInvoiceNote.Id,
                Identifier = InputInvoiceNote.Identifier,

                Note = InputInvoiceNote.Note,
                NoteDate = InputInvoiceNote.NoteDate,
                ItemStatus = InputInvoiceNote.ItemStatus,
                IsActive = InputInvoiceNote.Active,

                UpdatedAt = InputInvoiceNote.UpdatedAt,
                CreatedAt = InputInvoiceNote.CreatedAt
            };

            return InputInvoiceNoteViewModel;
        }

        public static InputInvoiceNote ConvertToInputInvoiceNote(this InputInvoiceNoteViewModel InputInvoiceNoteViewModel)
        {
            InputInvoiceNote InputInvoiceNote = new InputInvoiceNote()
            {
                Id = InputInvoiceNoteViewModel.Id,
                Identifier = InputInvoiceNoteViewModel.Identifier,

                InputInvoiceId = InputInvoiceNoteViewModel.InputInvoice?.Id ?? null,

                Note = InputInvoiceNoteViewModel.Note,
                NoteDate = InputInvoiceNoteViewModel.NoteDate,
                ItemStatus = InputInvoiceNoteViewModel.ItemStatus,
                Active = InputInvoiceNoteViewModel.IsActive,

                CreatedById = InputInvoiceNoteViewModel.CreatedBy?.Id ?? null,
                CompanyId = InputInvoiceNoteViewModel.Company?.Id ?? null,

                CreatedAt = InputInvoiceNoteViewModel.CreatedAt,
                UpdatedAt = InputInvoiceNoteViewModel.UpdatedAt
            };

            return InputInvoiceNote;
        }
    }
}
