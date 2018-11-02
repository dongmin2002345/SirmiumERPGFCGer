using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Common.BusinessPartners
{
    public interface IBusinessPartnerByConstructionSiteHistoryService
    {
        BusinessPartnerByConstructionSiteHistoryListResponse GetBusinessPartnerByConstructionSiteHistories(int companyId);
        BusinessPartnerByConstructionSiteHistoryListResponse GetBusinessPartnerByConstructionSiteHistoriesNewerThen(int companyId, DateTime? lastUpdateTime);

        BusinessPartnerByConstructionSiteHistoryResponse Create(BusinessPartnerByConstructionSiteHistoryViewModel businessPartnerByConstructionSiteHistory);
        BusinessPartnerByConstructionSiteHistoryResponse Delete(Guid identifier);

        BusinessPartnerByConstructionSiteHistoryListResponse Sync(SyncBusinessPartnerByConstructionSiteHistoryRequest request);
    }
}
