using DomainCore.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.BusinessPartners
{
    public interface IBusinessPartnerDocumentRepository
    {
        List<BusinessPartnerDocument> GetBusinessPartnerDocuments(int companyId);
        List<BusinessPartnerDocument> GetBusinessPartnerDocumentsByBusinessPartner(int businessPartnerId);
        List<BusinessPartnerDocument> GetBusinessPartnerDocumentsNewerThen(int companyId, DateTime lastUpdateTime);

        BusinessPartnerDocument Create(BusinessPartnerDocument businessPartnerDocument);
        BusinessPartnerDocument Delete(Guid identifier);
    }
}
