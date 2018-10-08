using ServiceInterfaces.ViewModels.Common.Sectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.Common.Sectors
{
	public class SyncSectorRequest
	{
		public int CompanyId { get; set; }
		public DateTime? LastUpdatedAt { get; set; }
		public List<SectorViewModel> UnSyncedSectors { get; set; }
	}
}
