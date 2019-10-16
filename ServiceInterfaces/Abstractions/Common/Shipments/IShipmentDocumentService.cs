using ServiceInterfaces.Messages.Common.Shipments;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Abstractions.Common.Shipments
{
    public interface IShipmentDocumentService
    {
        ShipmentDocumentListResponse Sync(SyncShipmentDocumentRequest request);
    }
}
