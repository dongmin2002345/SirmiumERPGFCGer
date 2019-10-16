using ServiceInterfaces.Messages.Common.Shipments;
using ServiceInterfaces.ViewModels.Common.Shipments;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Abstractions.Common.Shipments
{
    public interface IShipmentService
    {
        ShipmentListResponse GetShipments(int companyId);
        ShipmentListResponse GetShipmentsNewerThan(int companyId, DateTime? lastUpdateTime);

        ShipmentResponse Create(ShipmentViewModel shipment);
        ShipmentResponse Delete(Guid identifier);

        ShipmentListResponse Sync(SyncShipmentRequest request);
    }
}
