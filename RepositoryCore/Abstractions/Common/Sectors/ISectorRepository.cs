using DomainCore.Common.Sectors;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.Sectors
{
    public interface ISectorRepository
    {
		List<Sector> GetSectors(int companyId);
		List<Sector> GetSectorsNewerThen(int companyId, DateTime lastUpdateTime);

		Sector Create(Sector sector);
		Sector Delete(Guid identifier);
	}
}
