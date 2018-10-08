using ServiceInterfaces.Messages.Common.Locations;
using ServiceInterfaces.ViewModels.Common.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Common.Locations
{
    public interface IRegionService
    {
        RegionListResponse GetRegions(int companyId);
        RegionListResponse GetRegionsNewerThen(int companyId, DateTime? lastUpdateTime);

        RegionResponse Create(RegionViewModel region);
        RegionResponse Delete(Guid identifier);

        RegionListResponse Sync(SyncRegionRequest request);
    }
}
