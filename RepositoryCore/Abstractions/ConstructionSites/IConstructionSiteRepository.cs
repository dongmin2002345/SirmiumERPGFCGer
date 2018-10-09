using DomainCore.ConstructionSites;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.ConstructionSites
{
    public interface IConstructionSiteRepository
    {
        List<ConstructionSite> GetConstructionSites(int companyId);
        List<ConstructionSite> GetConstructionSitesNewerThen(int companyId, DateTime lastUpdateTime);

        ConstructionSite Create(ConstructionSite constructionSite);
        ConstructionSite Delete(Guid identifier);
    }
}
