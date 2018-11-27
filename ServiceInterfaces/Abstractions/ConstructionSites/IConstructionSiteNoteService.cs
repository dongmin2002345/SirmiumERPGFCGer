using ServiceInterfaces.Messages.ConstructionSites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.ConstructionSites
{
    public interface IConstructionSiteNoteService
    {
        ConstructionSiteNoteListResponse Sync(SyncConstructionSiteNoteRequest request);
    }
}
