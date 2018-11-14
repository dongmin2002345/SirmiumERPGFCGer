using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Common.BusinessPartners
{
    public class BusinessPartnerInstitutionService : IBusinessPartnerInstitutionService
    {
        public BusinessPartnerInstitutionListResponse Sync(SyncBusinessPartnerInstitutionRequest request)
        {
            BusinessPartnerInstitutionListResponse response = new BusinessPartnerInstitutionListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncBusinessPartnerInstitutionRequest, BusinessPartnerInstitutionViewModel, BusinessPartnerInstitutionListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.BusinessPartnerInstitutions = new List<BusinessPartnerInstitutionViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
