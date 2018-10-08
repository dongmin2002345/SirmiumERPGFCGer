using ServiceInterfaces.Messages.Common.Locations;
using ServiceInterfaces.ViewModels.Common.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Common.Locations
{
    public interface IMunicipalityService
    {
        MunicipalityListResponse GetMunicipalities(int companyId);
        MunicipalityListResponse GetMunicipalitiesNewerThen(int companyId, DateTime? lastUpdateTime);

        MunicipalityResponse Create(MunicipalityViewModel Municipality);
        MunicipalityResponse Delete(Guid identifier);

        MunicipalityListResponse Sync(SyncMunicipalityRequest request);
    }
}
