using ServiceInterfaces.Messages.Common.Locations;
using ServiceInterfaces.ViewModels.Common.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Common.Locations
{
    public interface ICountryService
    {
        CountryListResponse GetCountries(int companyId);
        CountryListResponse GetCountriesNewerThen(int companyId, DateTime? lastUpdateTime);

        CountryResponse Create(CountryViewModel country);
        CountryResponse Delete(Guid identifier);

        CountryListResponse Sync(SyncCountryRequest request);
    }
}
