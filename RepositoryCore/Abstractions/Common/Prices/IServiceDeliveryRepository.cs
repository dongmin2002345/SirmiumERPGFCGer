using DomainCore.Common.Prices;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.Prices
{
    public interface IServiceDeliveryRepository
    {
        List<ServiceDelivery> GetServiceDeliverys(int companyId);
        List<ServiceDelivery> GetServiceDeliverysNewerThen(int companyId, DateTime lastUpdateTime);
        ServiceDelivery GetServiceDelivery(int serviceDeliveryId);

        ServiceDelivery Create(ServiceDelivery serviceDelivery);
        ServiceDelivery Delete(Guid identifier);
    }
}
