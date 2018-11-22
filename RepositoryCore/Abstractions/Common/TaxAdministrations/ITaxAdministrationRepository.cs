using DomainCore.Common.TaxAdministrations;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.TaxAdministrations
{
    public interface ITaxAdministrationRepository
    {
        List<TaxAdministration> GetTaxAdministrations(int companyId);
        List<TaxAdministration> GetTaxAdministrationsNewerThan(int companyId, DateTime lastUpdateTime);

        TaxAdministration Create(TaxAdministration taxAdministration);
        TaxAdministration Delete(Guid identifier);
    }
}
