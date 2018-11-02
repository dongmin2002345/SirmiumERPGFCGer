using DomainCore.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.BusinessPartners
{
    public interface IBusinessPartnerByConstructionSiteHistoryRepository
    {
        List<BusinessPartnerByConstructionSiteHistory> GetBusinessPartnerByConstructionSiteHistories(int companyId);
        List<BusinessPartnerByConstructionSiteHistory> GetBusinessPartnerByConstructionSiteHistoriesNewerThen(int companyId, DateTime lastUpdateTime);

        BusinessPartnerByConstructionSiteHistory Create(BusinessPartnerByConstructionSiteHistory businessPartnerByConstructionSiteHistory);
        BusinessPartnerByConstructionSiteHistory Delete(Guid identifier);
    }
}
