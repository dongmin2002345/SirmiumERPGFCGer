using DomainCore.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.BusinessPartners
{
    public interface IBusinessPartnerNoteRepository
    {
        List<BusinessPartnerNote> GetBusinessPartnerNotes(int companyId);
        List<BusinessPartnerNote> GetBusinessPartnerNotesByBusinessPartner(int BusinessPartnerId);
        List<BusinessPartnerNote> GetBusinessPartnerNotesNewerThen(int companyId, DateTime lastUpdateTime);

        BusinessPartnerNote Create(BusinessPartnerNote BusinessPartnerNote);
        BusinessPartnerNote Delete(Guid identifier);
    }
}
