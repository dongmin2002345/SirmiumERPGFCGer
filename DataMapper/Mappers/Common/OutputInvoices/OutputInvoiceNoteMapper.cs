using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Common.OutputInvoices;
using ServiceInterfaces.ViewModels.Common.OutputInvoices;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.OutputInvoices
{
    public static class OutputInvoiceNoteMapper
    {
        public static List<OutputInvoiceNoteViewModel> ConvertToOutputInvoiceNoteViewModelList(this IEnumerable<OutputInvoiceNote> OutputInvoiceNotes)
        {
            List<OutputInvoiceNoteViewModel> OutputInvoiceNoteViewModels = new List<OutputInvoiceNoteViewModel>();
            foreach (OutputInvoiceNote OutputInvoiceNote in OutputInvoiceNotes)
            {
                OutputInvoiceNoteViewModels.Add(OutputInvoiceNote.ConvertToOutputInvoiceNoteViewModel());
            }
            return OutputInvoiceNoteViewModels;
        }

        public static OutputInvoiceNoteViewModel ConvertToOutputInvoiceNoteViewModel(this OutputInvoiceNote OutputInvoiceNote)
        {
            OutputInvoiceNoteViewModel OutputInvoiceNoteViewModel = new OutputInvoiceNoteViewModel()
            {
                Id = OutputInvoiceNote.Id,
                Identifier = OutputInvoiceNote.Identifier,

                OutputInvoice = OutputInvoiceNote.OutputInvoice?.ConvertToOutputInvoiceViewModelLite(),

                Note = OutputInvoiceNote.Note,
                NoteDate = OutputInvoiceNote.NoteDate,
                ItemStatus = OutputInvoiceNote.ItemStatus,

                IsActive = OutputInvoiceNote.Active,

                CreatedBy = OutputInvoiceNote.CreatedBy?.ConvertToUserViewModelLite(),
                Company = OutputInvoiceNote.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = OutputInvoiceNote.UpdatedAt,
                CreatedAt = OutputInvoiceNote.CreatedAt
            };

            return OutputInvoiceNoteViewModel;
        }

        public static OutputInvoiceNoteViewModel ConvertToOutputInvoiceNoteViewModelLite(this OutputInvoiceNote OutputInvoiceNote)
        {
            OutputInvoiceNoteViewModel OutputInvoiceNoteViewModel = new OutputInvoiceNoteViewModel()
            {
                Id = OutputInvoiceNote.Id,
                Identifier = OutputInvoiceNote.Identifier,

                Note = OutputInvoiceNote.Note,
                NoteDate = OutputInvoiceNote.NoteDate,
                ItemStatus = OutputInvoiceNote.ItemStatus,

                IsActive = OutputInvoiceNote.Active,

                UpdatedAt = OutputInvoiceNote.UpdatedAt,
                CreatedAt = OutputInvoiceNote.CreatedAt
            };

            return OutputInvoiceNoteViewModel;
        }

        public static OutputInvoiceNote ConvertToOutputInvoiceNote(this OutputInvoiceNoteViewModel OutputInvoiceNoteViewModel)
        {
            OutputInvoiceNote OutputInvoiceNote = new OutputInvoiceNote()
            {
                Id = OutputInvoiceNoteViewModel.Id,
                Identifier = OutputInvoiceNoteViewModel.Identifier,

                OutputInvoiceId = OutputInvoiceNoteViewModel.OutputInvoice?.Id ?? null,

                Note = OutputInvoiceNoteViewModel.Note,
                NoteDate = OutputInvoiceNoteViewModel.NoteDate,
                ItemStatus = OutputInvoiceNoteViewModel.ItemStatus,

                CreatedById = OutputInvoiceNoteViewModel.CreatedBy?.Id ?? null,
                CompanyId = OutputInvoiceNoteViewModel.Company?.Id ?? null,

                CreatedAt = OutputInvoiceNoteViewModel.CreatedAt,
                UpdatedAt = OutputInvoiceNoteViewModel.UpdatedAt
            };

            return OutputInvoiceNote;
        }
    }
}
