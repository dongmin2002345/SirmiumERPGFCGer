using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Common.BusinessPartners
{
    public interface IBusinessPartnerPhoneService
    {
        BusinessPartnerPhoneListResponse GetBusinessPartnerPhones(int companyId);
        BusinessPartnerPhoneListResponse GetBusinessPartnerPhonesNewerThen(int companyId, DateTime? lastUpdateTime);

        BusinessPartnerPhoneResponse Create(BusinessPartnerPhoneViewModel businessPartnerPhone);
        BusinessPartnerPhoneResponse Delete(Guid identifier);

        BusinessPartnerPhoneListResponse Sync(SyncBusinessPartnerPhoneRequest request);
    }
}
