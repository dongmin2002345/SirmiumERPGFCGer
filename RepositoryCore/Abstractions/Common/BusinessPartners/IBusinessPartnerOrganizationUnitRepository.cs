using DomainCore.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.BusinessPartners
{
    public interface IBusinessPartnerOrganizationUnitRepository
    {
        List<BusinessPartnerOrganizationUnit> GetBusinessPartnerOrganizationUnits(int companyId);
        List<BusinessPartnerOrganizationUnit> GetBusinessPartnerOrganizationUnitsByBusinessPartner(int businessPartnerId);
        List<BusinessPartnerOrganizationUnit> GetBusinessPartnerOrganizationUnitsNewerThen(int companyId, DateTime lastUpdateTime);

        BusinessPartnerOrganizationUnit GetBusinessPartnerOrganizationUnit(int id);

        BusinessPartnerOrganizationUnit Create(BusinessPartnerOrganizationUnit businessPartnerOrganizationUnit);
        BusinessPartnerOrganizationUnit Delete(Guid identifier);
    }
}
