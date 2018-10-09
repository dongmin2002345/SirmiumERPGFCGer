using ServiceInterfaces.Messages.ConstructionSites;
using ServiceInterfaces.ViewModels.ConstructionSites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.ConstructionSites
{
    public interface IConstructionSiteService
    {
        ConstructionSiteListResponse GetConstructionSites(int companyId);
        ConstructionSiteListResponse GetConstructionSitesNewerThen(int companyId, DateTime? lastUpdateTime);

        ConstructionSiteResponse Create(ConstructionSiteViewModel ConstructionSite);
        ConstructionSiteResponse Delete(Guid identifier);

        ConstructionSiteListResponse Sync(SyncConstructionSiteRequest request);
    }
}
