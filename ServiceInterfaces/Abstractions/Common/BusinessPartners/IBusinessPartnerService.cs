using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Common.BusinessPartners
{
    public interface IBusinessPartnerService
    {
        BusinessPartnerListResponse GetBusinessPartners(string filterString);

        BusinessPartnerListResponse GetBusinessPartnersByPage(int currentPage = 1, int itemsPerPage = 20, string searchParameter = "");

        BusinessPartnerListResponse GetBusinessPartnersForPopup(string filterString);

        BusinessPartnerListResponse GetBusinessPartnersCount(string searchParameter = "");

        BusinessPartnerResponse GetBusinessPartner(int id);

        BusinessPartnerCodeResponse GetNewCodeValue();

        BusinessPartnerResponse Create(BusinessPartnerViewModel businessPartner);

        BusinessPartnerResponse Update(BusinessPartnerViewModel businessPartner);

        BusinessPartnerResponse Delete(int id);
    }
}
