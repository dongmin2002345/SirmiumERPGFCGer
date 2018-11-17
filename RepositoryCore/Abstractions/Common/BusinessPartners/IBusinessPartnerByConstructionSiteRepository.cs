using DomainCore.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.BusinessPartners
{
    public interface IBusinessPartnerByConstructionSiteRepository
    {
        List<BusinessPartnerByConstructionSite> GetBusinessPartnerByConstructionSites(int companyId);
        List<BusinessPartnerByConstructionSite> GetBusinessPartnerByConstructionSitesNewerThen(int companyId, DateTime lastUpdateTime);

        BusinessPartnerByConstructionSite Create(BusinessPartnerByConstructionSite businessPartnerByConstructionSite);
        BusinessPartnerByConstructionSite Delete(BusinessPartnerByConstructionSite businessPartnerByConstructionSite);
    }
}
