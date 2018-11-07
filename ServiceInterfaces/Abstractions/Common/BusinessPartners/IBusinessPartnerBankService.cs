using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Common.BusinessPartners
{
    public interface IBusinessPartnerBankService
    {
        BusinessPartnerBankListResponse GetBusinessPartnerBanks(int companyId);
        BusinessPartnerBankListResponse GetBusinessPartnerBanksNewerThen(int companyId, DateTime? lastUpdateTime);

        BusinessPartnerBankResponse Create(BusinessPartnerBankViewModel businessPartnerBank);
        BusinessPartnerBankResponse Delete(Guid identifier);

        BusinessPartnerBankListResponse Sync(SyncBusinessPartnerBankRequest request);
    }
}
