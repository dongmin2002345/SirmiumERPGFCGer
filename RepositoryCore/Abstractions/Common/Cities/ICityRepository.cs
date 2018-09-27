using DomainCore.Common.Cities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.Cities
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
