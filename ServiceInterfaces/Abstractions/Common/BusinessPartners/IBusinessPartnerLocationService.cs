using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Common.BusinessPartners
{
    public interface IBusinessPartnerLocationService
    {
        BusinessPartnerLocationListResponse GetBusinessPartnerLocations(int companyId);
        BusinessPartnerLocationListResponse GetBusinessPartnerLocationsNewerThen(int companyId, DateTime? lastUpdateTime);

        BusinessPartnerLocationResponse Create(BusinessPartnerLocationViewModel businessPartnerLocation);
        BusinessPartnerLocationResponse Delete(Guid identifier);

        BusinessPartnerLocationListResponse Sync(SyncBusinessPartnerLocationRequest request);
    }
}
