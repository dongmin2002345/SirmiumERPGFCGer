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
    public class ConstructionSiteDocumentService : IConstructionSiteDocumentService
    {
        public ConstructionSiteDocumentListResponse Sync(SyncConstructionSiteDocumentRequest request)
        {
            ConstructionSiteDocumentListResponse response = new ConstructionSiteDocumentListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncConstructionSiteDocumentRequest, ConstructionSiteDocumentViewModel, ConstructionSiteDocumentListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.ConstructionSiteDocuments = new List<ConstructionSiteDocumentViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
