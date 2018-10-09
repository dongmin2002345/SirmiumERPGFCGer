using DomainCore.Common.Sectors;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.Sectors
{
    public interface IAgencyRepository
    {
        List<Agency> GetAgencies(int companyId);
        List<Agency> GetAgenciesNewerThen(int companyId, DateTime lastUpdateTime);

        Agency Create(Agency agency);
        Agency Delete(Guid identifier);
    }
}
