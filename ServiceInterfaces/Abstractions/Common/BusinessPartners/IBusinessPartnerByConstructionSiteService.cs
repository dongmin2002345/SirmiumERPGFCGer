using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Common.BusinessPartners
{
    public interface IBusinessPartnerByConstructionSiteService
    {
        BusinessPartnerByConstructionSiteListResponse GetBusinessPartnerByConstructionSites(int companyId);
        BusinessPartnerByConstructionSiteListResponse GetBusinessPartnerByConstructionSitesNewerThen(int companyId, DateTime? lastUpdateTime);

        BusinessPartnerByConstructionSiteResponse Create(BusinessPartnerByConstructionSiteViewModel businessPartnerByConstructionSite);
        BusinessPartnerByConstructionSiteResponse Delete(Guid identifier);

        BusinessPartnerByConstructionSiteListResponse Sync(SyncBusinessPartnerByConstructionSiteRequest request);
    }
}
