using DomainCore.Common.Shipments;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.Shipments
{
    public interface IShipmentDocumentRepository
    {
        List<ShipmentDocument> GetShipmentDocuments(int companyId);
        List<ShipmentDocument> GetShipmentDocumentsByShipment(int shipmentId);
        List<ShipmentDocument> GetShipmentDocumentsNewerThen(int companyId, DateTime lastUpdateTime);

        ShipmentDocument Create(ShipmentDocument ShipmentDocument);
        ShipmentDocument Delete(Guid identifier);
    }
}
