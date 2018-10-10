using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Common.BusinessPartners
{
    public interface IBusinessPartnerOrganizationUnitService
    {
        BusinessPartnerOrganizationUnitListResponse GetBusinessPartnerOrganizationUnits(int companyId);
        BusinessPartnerOrganizationUnitListResponse GetBusinessPartnerOrganizationUnitsNewerThen(int companyId, DateTime? lastUpdateTime);

        BusinessPartnerOrganizationUnitResponse Create(BusinessPartnerOrganizationUnitViewModel businessPartnerOrganizationUnit);
        BusinessPartnerOrganizationUnitResponse Delete(Guid identifier);

        BusinessPartnerOrganizationUnitListResponse Sync(SyncBusinessPartnerOrganizationUnitRequest request);
    }
}
