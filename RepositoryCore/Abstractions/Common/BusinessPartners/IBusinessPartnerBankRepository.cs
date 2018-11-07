using DomainCore.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.BusinessPartners
{
    public interface IBusinessPartnerBankRepository
    {
        List<BusinessPartnerBank> GetBusinessPartnerBanks(int companyId);
        List<BusinessPartnerBank> GetBusinessPartnerBanksByBusinessPartner(int businessPartnerId);
        List<BusinessPartnerBank> GetBusinessPartnerBanksNewerThen(int companyId, DateTime lastUpdateTime);

        BusinessPartnerBank GetBusinessPartnerBank(int id);

        BusinessPartnerBank Create(BusinessPartnerBank businessPartnerBank);
        BusinessPartnerBank Delete(Guid identifier);
    }
}
