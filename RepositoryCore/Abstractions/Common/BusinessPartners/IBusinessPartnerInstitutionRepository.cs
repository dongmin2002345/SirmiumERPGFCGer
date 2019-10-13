using DomainCore.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.BusinessPartners
{
    public interface IBusinessPartnerInstitutionRepository
    {
        List<BusinessPartnerInstitution> GetBusinessPartnerInstitutions(int companyId);
        List<BusinessPartnerInstitution> GetBusinessPartnerInstitutionsByBusinessPartner(int businessPartnerId);
        List<BusinessPartnerInstitution> GetBusinessPartnerInstitutionsNewerThen(int companyId, DateTime lastUpdateTime);

        BusinessPartnerInstitution Create(BusinessPartnerInstitution businessPartnerInstitution);
        BusinessPartnerInstitution Delete(Guid identifier);
    }
}
