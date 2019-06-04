using ServiceInterfaces.Messages.Common.Locations;
using ServiceInterfaces.ViewModels.Common.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Common.Locations
{
    public interface ICityService
    {
        CityListResponse GetCities(int companyId);

        CityResponse Create(CityViewModel City);
        CityResponse Delete(Guid identifier);

        CityListResponse Sync(SyncCityRequest request);
    }
}
