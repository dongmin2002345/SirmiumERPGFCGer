using DomainCore.Common.Locations;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.Locations
{
    public interface IMunicipalityRepository
    {
        List<Municipality> GetMunicipalities(int companyId);
        List<Municipality> GetMunicipalitiesNewerThen(int companyId, DateTime lastUpdateTime);

        Municipality Create(Municipality Municipality);
        Municipality Delete(Guid identifier);
    }
}
