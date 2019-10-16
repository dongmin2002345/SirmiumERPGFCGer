using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Common.Shipments;
using ServiceInterfaces.Messages.Common.Shipments;
using ServiceInterfaces.ViewModels.Common.Shipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Common.Shipments
{
    public class ShipmentDocumentService : IShipmentDocumentService
    {
        public ShipmentDocumentListResponse Sync(SyncShipmentDocumentRequest request)
        {
            ShipmentDocumentListResponse response = new ShipmentDocumentListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncShipmentDocumentRequest, ShipmentDocumentViewModel, ShipmentDocumentListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.ShipmentDocuments = new List<ShipmentDocumentViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
