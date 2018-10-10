using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.BusinessPartners
{
    public interface IBusinessPartnerBusinessPartnerTypeRepository
    {
        void Create(int businessPartnerId, int businessPartnerTypeId);
        void Delete(int businessPartnerId);
    }
}
