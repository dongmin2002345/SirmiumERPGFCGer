using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.ConstructionSites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.ConstructionSites
{
    public class ConstructionSiteListResponse : BaseResponse
    {
        public List<ConstructionSiteViewModel> ConstructionSites { get; set; }
        public int TotalItems { get; set; }
    }
}
