using DomainCore.Common.Shipments;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.Shipments
{
    public interface IShipmentRepository
    {
        List<Shipment> GetShipments(int companyId);
        List<Shipment> GetShipmentsNewerThan(int companyId, DateTime lastUpdateTime);

        Shipment Create(Shipment shipment);
        Shipment Delete(Guid identifier);
    }
}
