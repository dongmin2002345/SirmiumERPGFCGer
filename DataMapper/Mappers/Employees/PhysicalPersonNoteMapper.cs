using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DataMapper.Mappers.Employees;
using DomainCore.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Employees
{
    public static class PhysicalPersonNoteMapper
    {
        public static List<PhysicalPersonNoteViewModel> ConvertToPhysicalPersonNoteViewModelList(this IEnumerable<PhysicalPersonNote> PhysicalPersonNotes)
        {
            List<PhysicalPersonNoteViewModel> PhysicalPersonNoteViewModels = new List<PhysicalPersonNoteViewModel>();
            foreach (PhysicalPersonNote PhysicalPersonNote in PhysicalPersonNotes)
            {
                PhysicalPersonNoteViewModels.Add(PhysicalPersonNote.ConvertToPhysicalPersonNoteViewModel());
            }
            return PhysicalPersonNoteViewModels;
        }

        public static PhysicalPersonNoteViewModel ConvertToPhysicalPersonNoteViewModel(this PhysicalPersonNote PhysicalPersonNote)
        {
            PhysicalPersonNoteViewModel PhysicalPersonNoteViewModel = new PhysicalPersonNoteViewModel()
            {
                Id = PhysicalPersonNote.Id,
                Identifier = PhysicalPersonNote.Identifier,

                PhysicalPerson = PhysicalPersonNote.PhysicalPerson?.ConvertToPhysicalPersonViewModelLite(),

                Note = PhysicalPersonNote.Note,
                NoteDate = PhysicalPersonNote.NoteDate,

                IsActive = PhysicalPersonNote.Active,

                CreatedBy = PhysicalPersonNote.CreatedBy?.ConvertToUserViewModelLite(),
                Company = PhysicalPersonNote.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = PhysicalPersonNote.UpdatedAt,
                CreatedAt = PhysicalPersonNote.CreatedAt
            };

            return PhysicalPersonNoteViewModel;
        }

        public static PhysicalPersonNoteViewModel ConvertToPhysicalPersonNoteViewModelLite(this PhysicalPersonNote PhysicalPersonNote)
        {
            PhysicalPersonNoteViewModel PhysicalPersonNoteViewModel = new PhysicalPersonNoteViewModel()
            {
                Id = PhysicalPersonNote.Id,
                Identifier = PhysicalPersonNote.Identifier,

                Note = PhysicalPersonNote.Note,
                NoteDate = PhysicalPersonNote.NoteDate,

                IsActive = PhysicalPersonNote.Active,

                UpdatedAt = PhysicalPersonNote.UpdatedAt,
                CreatedAt = PhysicalPersonNote.CreatedAt
            };

            return PhysicalPersonNoteViewModel;
        }

        public static PhysicalPersonNote ConvertToPhysicalPersonNote(this PhysicalPersonNoteViewModel PhysicalPersonNoteViewModel)
        {
            PhysicalPersonNote PhysicalPersonNote = new PhysicalPersonNote()
            {
                Id = PhysicalPersonNoteViewModel.Id,
                Identifier = PhysicalPersonNoteViewModel.Identifier,

                PhysicalPersonId = PhysicalPersonNoteViewModel.PhysicalPerson?.Id ?? null,

                Note = PhysicalPersonNoteViewModel.Note,
                NoteDate = PhysicalPersonNoteViewModel.NoteDate,

                CreatedById = PhysicalPersonNoteViewModel.CreatedBy?.Id ?? null,
                CompanyId = PhysicalPersonNoteViewModel.Company?.Id ?? null,

                CreatedAt = PhysicalPersonNoteViewModel.CreatedAt,
                UpdatedAt = PhysicalPersonNoteViewModel.UpdatedAt
            };

            return PhysicalPersonNote;
        }
    }
}
