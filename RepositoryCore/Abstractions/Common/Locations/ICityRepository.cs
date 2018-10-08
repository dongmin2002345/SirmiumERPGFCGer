using DomainCore.Common.Locations;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.Locations
{
    public interface ICityRepository
    {
        List<City> GetCities(int companyId);
        List<City> GetCitiesNewerThen(int companyId, DateTime lastUpdateTime);

        City Create(City city);
        City Delete(Guid identifier);
    }
}
