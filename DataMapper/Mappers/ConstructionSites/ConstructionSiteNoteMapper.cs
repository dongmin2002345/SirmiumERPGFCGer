using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.ConstructionSites;
using ServiceInterfaces.ViewModels.ConstructionSites;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.ConstructionSites
{
    public static class ConstructionSiteNoteMapper
    {
        public static List<ConstructionSiteNoteViewModel> ConvertToConstructionSiteNoteViewModelList(this IEnumerable<ConstructionSiteNote> ConstructionSiteNotes)
        {
            List<ConstructionSiteNoteViewModel> ConstructionSiteNoteViewModels = new List<ConstructionSiteNoteViewModel>();
            foreach (ConstructionSiteNote ConstructionSiteNote in ConstructionSiteNotes)
            {
                ConstructionSiteNoteViewModels.Add(ConstructionSiteNote.ConvertToConstructionSiteNoteViewModel());
            }
            return ConstructionSiteNoteViewModels;
        }

        public static ConstructionSiteNoteViewModel ConvertToConstructionSiteNoteViewModel(this ConstructionSiteNote ConstructionSiteNote)
        {
            ConstructionSiteNoteViewModel ConstructionSiteNoteViewModel = new ConstructionSiteNoteViewModel()
            {
                Id = ConstructionSiteNote.Id,
                Identifier = ConstructionSiteNote.Identifier,

                ConstructionSite = ConstructionSiteNote.ConstructionSite?.ConvertToConstructionSiteViewModelLite(),

                Note = ConstructionSiteNote.Note,
                NoteDate = ConstructionSiteNote.NoteDate,

                IsActive = ConstructionSiteNote.Active,

                CreatedBy = ConstructionSiteNote.CreatedBy?.ConvertToUserViewModelLite(),
                Company = ConstructionSiteNote.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = ConstructionSiteNote.UpdatedAt,
                CreatedAt = ConstructionSiteNote.CreatedAt
            };

            return ConstructionSiteNoteViewModel;
        }

        public static ConstructionSiteNoteViewModel ConvertToConstructionSiteNoteViewModelLite(this ConstructionSiteNote ConstructionSiteNote)
        {
            ConstructionSiteNoteViewModel ConstructionSiteNoteViewModel = new ConstructionSiteNoteViewModel()
            {
                Id = ConstructionSiteNote.Id,
                Identifier = ConstructionSiteNote.Identifier,

                Note = ConstructionSiteNote.Note,
                NoteDate = ConstructionSiteNote.NoteDate,

                IsActive = ConstructionSiteNote.Active,

                UpdatedAt = ConstructionSiteNote.UpdatedAt,
                CreatedAt = ConstructionSiteNote.CreatedAt
            };

            return ConstructionSiteNoteViewModel;
        }

        public static ConstructionSiteNote ConvertToConstructionSiteNote(this ConstructionSiteNoteViewModel ConstructionSiteNoteViewModel)
        {
            ConstructionSiteNote ConstructionSiteNote = new ConstructionSiteNote()
            {
                Id = ConstructionSiteNoteViewModel.Id,
                Identifier = ConstructionSiteNoteViewModel.Identifier,

                ConstructionSiteId = ConstructionSiteNoteViewModel.ConstructionSite?.Id ?? null,

                Note = ConstructionSiteNoteViewModel.Note,
                NoteDate = ConstructionSiteNoteViewModel.NoteDate,

                CreatedById = ConstructionSiteNoteViewModel.CreatedBy?.Id ?? null,
                CompanyId = ConstructionSiteNoteViewModel.Company?.Id ?? null,

                CreatedAt = ConstructionSiteNoteViewModel.CreatedAt,
                UpdatedAt = ConstructionSiteNoteViewModel.UpdatedAt
            };

            return ConstructionSiteNote;
        }
    }
}
