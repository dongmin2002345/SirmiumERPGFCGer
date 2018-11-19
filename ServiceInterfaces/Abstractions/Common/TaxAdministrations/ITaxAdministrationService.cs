using ServiceInterfaces.Messages.Common.TaxAdministrations;
using ServiceInterfaces.ViewModels.Common.TaxAdministrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Common.TaxAdministrations
{
    public interface ITaxAdministrationService
    {
        TaxAdministrationListResponse GetTaxAdministrations(int companyId);
        TaxAdministrationListResponse GetTaxAdministrationsNewerThan(int companyId, DateTime? lastUpdateTime);

        TaxAdministrationResponse Create(TaxAdministrationViewModel taxAdministration);
        TaxAdministrationResponse Delete(Guid identifier);

        TaxAdministrationListResponse Sync(SyncTaxAdministrationRequest request);
    }
}
