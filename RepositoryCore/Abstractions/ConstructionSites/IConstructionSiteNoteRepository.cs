using DomainCore.ConstructionSites;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.ConstructionSites
{
    public interface IConstructionSiteNoteRepository
    {
        List<ConstructionSiteNote> GetConstructionSiteNotes(int companyId);
        List<ConstructionSiteNote> GetConstructionSiteNotesByConstructionSite(int ConstructionSiteId);
        List<ConstructionSiteNote> GetConstructionSiteNotesNewerThen(int companyId, DateTime lastUpdateTime);

        ConstructionSiteNote Create(ConstructionSiteNote ConstructionSiteNote);
        ConstructionSiteNote Delete(Guid identifier);
    }
}
