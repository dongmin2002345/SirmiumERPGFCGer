using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.ConstructionSites;
using ServiceInterfaces.Messages.ConstructionSites;
using ServiceInterfaces.ViewModels.ConstructionSites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.ConstructionSites
{
    public class ConstructionSiteNoteService : IConstructionSiteNoteService
    {
        public ConstructionSiteNoteListResponse Sync(SyncConstructionSiteNoteRequest request)
        {
            ConstructionSiteNoteListResponse response = new ConstructionSiteNoteListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncConstructionSiteNoteRequest, ConstructionSiteNoteViewModel, ConstructionSiteNoteListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.ConstructionSiteNotes = new List<ConstructionSiteNoteViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
