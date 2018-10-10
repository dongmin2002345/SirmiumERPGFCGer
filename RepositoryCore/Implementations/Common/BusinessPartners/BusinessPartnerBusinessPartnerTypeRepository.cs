using DomainCore.Common.BusinessPartners;
using RepositoryCore.Abstractions.Common.BusinessPartners;
using RepositoryCore.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryCore.Implementations.Common.BusinessPartners
{
    public class BusinessPartnerBusinessPartnerTypeRepository : IBusinessPartnerBusinessPartnerTypeRepository
    {
        private ApplicationDbContext context;

        public BusinessPartnerBusinessPartnerTypeRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void Create(int businessPartnerId, int businessPartnerTypeId)
        {
            BusinessPartnerBusinessPartnerType dbEntry = new BusinessPartnerBusinessPartnerType();
            dbEntry.BusinessPartnerId = businessPartnerId;
            dbEntry.BusinessPartnerTypeId = businessPartnerTypeId;

            context.BusinessPartnerBusinessPartnerTypes.Add(dbEntry);
        }

        public void Delete(int businessPartnerId)
        {
            context.BusinessPartnerBusinessPartnerTypes.RemoveRange(
                context.BusinessPartnerBusinessPartnerTypes.Where(x => x.BusinessPartnerId == businessPartnerId));
        }
    }
}
