using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Common.BusinessPartners
{
    public interface IBusinessPartnerService
    {
        BusinessPartnerListResponse GetBusinessPartners(int companyId);
        BusinessPartnerListResponse GetBusinessPartnersNewerThen(int companyId, DateTime? lastUpdateTime);

        BusinessPartnerResponse Create(BusinessPartnerViewModel businessPartner);
        BusinessPartnerResponse Delete(Guid identifier);

        BusinessPartnerListResponse Sync(SyncBusinessPartnerRequest request);
    }
}

