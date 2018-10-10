using DomainCore.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.BusinessPartners
{
    public interface IBusinessPartnerTypeRepository
    {
        List<BusinessPartnerType> GetBusinessPartnerTypes(int companyId);
        List<BusinessPartnerType> GetBusinessPartnerTypesNewerThen(int companyId, DateTime lastUpdateTime);

        BusinessPartnerType Create(BusinessPartnerType businessPartnerType);
        BusinessPartnerType Delete(Guid identifier);
    }
}
