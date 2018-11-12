using ServiceInterfaces.Messages.ConstructionSites;
using ServiceInterfaces.ViewModels.ConstructionSites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.ConstructionSites
{
    public interface IConstructionSiteCalculationService
    {
        ConstructionSiteCalculationListResponse GetConstructionSiteCalculations(int companyId);
        ConstructionSiteCalculationListResponse GetConstructionSiteCalculationsNewerThen(int companyId, DateTime? lastUpdateTime);

        ConstructionSiteCalculationResponse Create(ConstructionSiteCalculationViewModel constructionSiteCalculation);

        ConstructionSiteCalculationListResponse Sync(SyncConstructionSiteCalculationRequest request);
    }
}
