using DomainCore.Common.Locations;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.Locations
{
    public interface ICityRepository
    {
        List<City> GetCities();
        List<City> GetCitiesNewerThen(DateTime lastUpdateTime);

        City GetCity(int id);

        City Create(City city);
        City Delete(Guid identifier);
    }
}
