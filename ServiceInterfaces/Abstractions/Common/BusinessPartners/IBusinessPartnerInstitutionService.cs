using ServiceInterfaces.Messages.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Common.BusinessPartners
{
    public interface IBusinessPartnerInstitutionService
    {
        BusinessPartnerInstitutionListResponse Sync(SyncBusinessPartnerInstitutionRequest request);
    }
}
