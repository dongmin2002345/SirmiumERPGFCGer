using ServiceInterfaces.ViewModels.Common.TaxAdministrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.Common.TaxAdministrations
{
    public class SyncTaxAdministrationRequest
    {
        public int CompanyId { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public List<TaxAdministrationViewModel> UnSyncedTaxAdministrations { get; set; }
    }
}
