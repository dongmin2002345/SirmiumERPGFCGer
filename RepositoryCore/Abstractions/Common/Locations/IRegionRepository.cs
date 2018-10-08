using DomainCore.Common.Locations;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.Locations
{
    public interface IRegionRepository
    {
        List<Region> GetRegions(int companyId);
        List<Region> GetRegionsNewerThen(int companyId, DateTime lastUpdateTime);

        Region Create(Region region);
        Region Delete(Guid identifier);
    }
}
