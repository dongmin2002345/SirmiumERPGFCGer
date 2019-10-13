using DomainCore.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.BusinessPartners
{
    public interface IBusinessPartnerLocationRepository
    {
        List<BusinessPartnerLocation> GetBusinessPartnerLocations(int companyId);
        List<BusinessPartnerLocation> GetBusinessPartnerLocationssByBusinessPartner(int businessPartnerId);
        List<BusinessPartnerLocation> GetBusinessPartnerLocationsNewerThen(int companyId, DateTime lastUpdateTime);

        BusinessPartnerLocation Create(BusinessPartnerLocation businessPartnerLocation);
        BusinessPartnerLocation Delete(Guid identifier);
    }
}
