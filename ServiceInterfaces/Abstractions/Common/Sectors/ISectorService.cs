using ServiceInterfaces.Messages.Common.Sectors;
using ServiceInterfaces.ViewModels.Common.Sectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Common.Sectors
{
	public interface ISectorService
	{
		SectorListResponse GetSectors(int companyId);
		SectorListResponse GetSectorsNewerThen(int companyId, DateTime? lastUpdateTime);

		SectorResponse Create(SectorViewModel Sector);
		SectorResponse Delete(Guid identifier);

		SectorListResponse Sync(SyncSectorRequest request);
	}
}
