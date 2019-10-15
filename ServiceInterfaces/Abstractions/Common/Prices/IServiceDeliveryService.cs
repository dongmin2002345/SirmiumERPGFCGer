using ServiceInterfaces.Messages.Common.Prices;
using ServiceInterfaces.ViewModels.Common.Prices;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Abstractions.Common.Prices
{
    public interface IServiceDeliveryService
    {
        ServiceDeliveryListResponse GetServiceDeliverys(int companyId);

        ServiceDeliveryResponse Create(ServiceDeliveryViewModel serviceDelivery);
        ServiceDeliveryResponse Delete(Guid identifier);

        ServiceDeliveryListResponse Sync(SyncServiceDeliveryRequest request);
    }
}
