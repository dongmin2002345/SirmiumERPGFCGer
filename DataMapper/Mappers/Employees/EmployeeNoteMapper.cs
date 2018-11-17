using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Employees
{
    public static class EmployeeNoteMapper
    {
        public static List<EmployeeNoteViewModel> ConvertToEmployeeNoteViewModelList(this IEnumerable<EmployeeNote> EmployeeNotes)
        {
            List<EmployeeNoteViewModel> EmployeeNoteViewModels = new List<EmployeeNoteViewModel>();
            foreach (EmployeeNote EmployeeNote in EmployeeNotes)
            {
                EmployeeNoteViewModels.Add(EmployeeNote.ConvertToEmployeeNoteViewModel());
            }
            return EmployeeNoteViewModels;
        }

        public static EmployeeNoteViewModel ConvertToEmployeeNoteViewModel(this EmployeeNote EmployeeNote)
        {
            EmployeeNoteViewModel EmployeeNoteViewModel = new EmployeeNoteViewModel()
            {
                Id = EmployeeNote.Id,
                Identifier = EmployeeNote.Identifier,

                Employee = EmployeeNote.Employee?.ConvertToEmployeeViewModelLite(),

                Note = EmployeeNote.Note,
                NoteDate = EmployeeNote.NoteDate,

                IsActive = EmployeeNote.Active,

                CreatedBy = EmployeeNote.CreatedBy?.ConvertToUserViewModelLite(),
                Company = EmployeeNote.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = EmployeeNote.UpdatedAt,
                CreatedAt = EmployeeNote.CreatedAt
            };

            return EmployeeNoteViewModel;
        }

        public static EmployeeNoteViewModel ConvertToEmployeeNoteViewModelLite(this EmployeeNote EmployeeNote)
        {
            EmployeeNoteViewModel EmployeeNoteViewModel = new EmployeeNoteViewModel()
            {
                Id = EmployeeNote.Id,
                Identifier = EmployeeNote.Identifier,

                Note = EmployeeNote.Note,
                NoteDate = EmployeeNote.NoteDate,

                IsActive = EmployeeNote.Active,

                UpdatedAt = EmployeeNote.UpdatedAt,
                CreatedAt = EmployeeNote.CreatedAt
            };

            return EmployeeNoteViewModel;
        }

        public static EmployeeNote ConvertToEmployeeNote(this EmployeeNoteViewModel EmployeeNoteViewModel)
        {
            EmployeeNote EmployeeNote = new EmployeeNote()
            {
                Id = EmployeeNoteViewModel.Id,
                Identifier = EmployeeNoteViewModel.Identifier,

                EmployeeId = EmployeeNoteViewModel.Employee?.Id ?? null,

                Note = EmployeeNoteViewModel.Note,
                NoteDate = EmployeeNoteViewModel.NoteDate,

                CreatedById = EmployeeNoteViewModel.CreatedBy?.Id ?? null,
                CompanyId = EmployeeNoteViewModel.Company?.Id ?? null,

                CreatedAt = EmployeeNoteViewModel.CreatedAt,
                UpdatedAt = EmployeeNoteViewModel.UpdatedAt
            };

            return EmployeeNote;
        }
    }
}
