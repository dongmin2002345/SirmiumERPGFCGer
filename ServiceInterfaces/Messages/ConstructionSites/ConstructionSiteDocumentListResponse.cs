using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.ConstructionSites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.ConstructionSites
{
    public class ConstructionSiteDocumentListResponse : BaseResponse
    {
        public List<ConstructionSiteDocumentViewModel> ConstructionSiteDocuments { get; set; }
        public int TotalItems { get; set; }
    }
}
