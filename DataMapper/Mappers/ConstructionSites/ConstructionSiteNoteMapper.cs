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
        public static List<ConstructionSiteNoteViewModel> ConvertToConstructionSiteNoteViewModelList(this IEnumerable<ConstructionSiteNote> constructionSiteNotes)
        {
            List<ConstructionSiteNoteViewModel> constructionSiteNoteViewModels = new List<ConstructionSiteNoteViewModel>();
            foreach (ConstructionSiteNote constructionSiteNote in constructionSiteNotes)
            {
                constructionSiteNoteViewModels.Add(constructionSiteNote.ConvertToConstructionSiteNoteViewModel());
            }
            return constructionSiteNoteViewModels;
        }

        public static ConstructionSiteNoteViewModel ConvertToConstructionSiteNoteViewModel(this ConstructionSiteNote constructionSiteNote)
        {
            ConstructionSiteNoteViewModel constructionSiteNoteViewModel = new ConstructionSiteNoteViewModel()
            {
                Id = constructionSiteNote.Id,
                Identifier = constructionSiteNote.Identifier,

                ConstructionSite = constructionSiteNote.ConstructionSite?.ConvertToConstructionSiteViewModelLite(),

                Note = constructionSiteNote.Note,
                NoteDate = constructionSiteNote.NoteDate,
                ItemStatus = constructionSiteNote.ItemStatus,

                IsActive = constructionSiteNote.Active,

                CreatedBy = constructionSiteNote.CreatedBy?.ConvertToUserViewModelLite(),
                Company = constructionSiteNote.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = constructionSiteNote.UpdatedAt,
                CreatedAt = constructionSiteNote.CreatedAt
            };

            return constructionSiteNoteViewModel;
        }

        public static ConstructionSiteNoteViewModel ConvertToConstructionSiteNoteViewModelLite(this ConstructionSiteNote constructionSiteNote)
        {
            ConstructionSiteNoteViewModel constructionSiteNoteViewModel = new ConstructionSiteNoteViewModel()
            {
                Id = constructionSiteNote.Id,
                Identifier = constructionSiteNote.Identifier,

                Note = constructionSiteNote.Note,
                NoteDate = constructionSiteNote.NoteDate,
                ItemStatus = constructionSiteNote.ItemStatus,

                IsActive = constructionSiteNote.Active,

                UpdatedAt = constructionSiteNote.UpdatedAt,
                CreatedAt = constructionSiteNote.CreatedAt
            };

            return constructionSiteNoteViewModel;
        }

        public static ConstructionSiteNote ConvertToConstructionSiteNote(this ConstructionSiteNoteViewModel constructionSiteNoteViewModel)
        {
            ConstructionSiteNote ConstructionSiteNote = new ConstructionSiteNote()
            {
                Id = constructionSiteNoteViewModel.Id,
                Identifier = constructionSiteNoteViewModel.Identifier,

                ConstructionSiteId = constructionSiteNoteViewModel.ConstructionSite?.Id ?? null,

                Note = constructionSiteNoteViewModel.Note,
                NoteDate = constructionSiteNoteViewModel.NoteDate,
                ItemStatus = constructionSiteNoteViewModel.ItemStatus,

                CreatedById = constructionSiteNoteViewModel.CreatedBy?.Id ?? null,
                CompanyId = constructionSiteNoteViewModel.Company?.Id ?? null,

                CreatedAt = constructionSiteNoteViewModel.CreatedAt,
                UpdatedAt = constructionSiteNoteViewModel.UpdatedAt
            };

            return ConstructionSiteNote;
        }
    }
}
