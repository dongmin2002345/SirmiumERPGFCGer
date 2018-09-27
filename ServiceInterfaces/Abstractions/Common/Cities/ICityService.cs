using ServiceInterfaces.Messages.Common.Cities;
using ServiceInterfaces.ViewModels.Common.Cities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Common.Cities
{
    public interface ICityService
    {
        CityListResponse GetCities();
        CityListResponse GetCitiesNewerThen(DateTime? lastUpdateTime);

        CityResponse Create(CityViewModel City);
        CityResponse Delete(Guid identifier);

        CityListResponse Sync(SyncCityRequest request);
    }
}
