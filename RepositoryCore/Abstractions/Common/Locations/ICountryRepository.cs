using DomainCore.Common.Locations;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.Locations
{
    public interface ICountryRepository
    {
        List<Country> GetCountries(int companyId);
        List<Country> GetCountriesNewerThen(int companyId, DateTime lastUpdateTime);

        Country Create(Country country);
        Country Delete(Guid identifier);
    }
}
