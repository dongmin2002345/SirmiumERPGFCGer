using DomainCore.ConstructionSites;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.ConstructionSites
{
    public interface IConstructionSiteCalculationRepository
    {
        List<ConstructionSiteCalculation> GetConstructionSiteCalculations(int companyId);
        List<ConstructionSiteCalculation> GetConstructionSiteCalculationsByConstructionSite(int constructionSiteId);
        List<ConstructionSiteCalculation> GetConstructionSiteCalculationsNewerThen(int companyId, DateTime lastUpdateTime);
        ConstructionSiteCalculation GetLastConstructionSiteCalculation(int companyId, int constructionSiteId);

        ConstructionSiteCalculation Create(ConstructionSiteCalculation constructionSiteCalculation);
        ConstructionSiteCalculation Delete(Guid identifier);
    }
}
