using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.Common.BusinessPartners
{
    public class SyncBusinessPartnerRequest
    {
        public DateTime? LastUpdatedAt { get; set; }
        public List<BusinessPartnerViewModel> UnSyncedBusinessPartners { get; set; }
    }
}
