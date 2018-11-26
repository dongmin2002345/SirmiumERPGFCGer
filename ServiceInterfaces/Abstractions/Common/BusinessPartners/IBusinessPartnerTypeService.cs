using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Common.BusinessPartners
{
    public interface IBusinessPartnerTypeService
    {
        BusinessPartnerTypeListResponse GetBusinessPartnerTypes(int companyId);
        BusinessPartnerTypeListResponse GetBusinessPartnerTypesNewerThen(int companyId, DateTime? lastUpdateTime);

        BusinessPartnerTypeResponse Create(BusinessPartnerTypeViewModel businessPartnerType);
        BusinessPartnerTypeResponse Delete(Guid identifier);

        BusinessPartnerTypeListResponse Sync(SyncBusinessPartnerTypeRequest request);
    }
}
