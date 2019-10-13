using DomainCore.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.BusinessPartners
{
    public interface IBusinessPartnerPhoneRepository
    {
        List<BusinessPartnerPhone> GetBusinessPartnerPhones(int companyId);
        List<BusinessPartnerPhone> GetBusinessPartnerPhonesByBusinessPartner(int businessPartnerId);
        List<BusinessPartnerPhone> GetBusinessPartnerPhonesNewerThen(int companyId, DateTime lastUpdateTime);

        BusinessPartnerPhone Create(BusinessPartnerPhone businessPartnerPhone);
        BusinessPartnerPhone Delete(Guid identifier);
    }
}
