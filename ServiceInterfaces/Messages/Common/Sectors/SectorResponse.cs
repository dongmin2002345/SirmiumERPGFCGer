using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.Sectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.Common.Sectors
{
	public class SectorResponse : BaseResponse
	{
		public SectorViewModel Sector { get; set; }
	}
}
