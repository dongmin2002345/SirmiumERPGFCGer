using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.Sectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.Common.Sectors
{
    public class AgencyListResponse : BaseResponse
    {
        public List<AgencyViewModel> Agencies { get; set; }
        public int TotalItems { get; set; }
    }
}
