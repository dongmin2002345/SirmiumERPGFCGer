using DomainCore.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.BusinessPartners
{
    public interface IBusinessPartnerRepository
    {
        List<BusinessPartner> GetBusinessPartners();

        List<BusinessPartner> GetBusinessPartnersNewerThen(DateTime lastUpdateTime);

        BusinessPartner GetBusinessPartner(int id);

        BusinessPartner Create(BusinessPartner businessPartner);
        BusinessPartner Delete(Guid identifier);
    }
}
