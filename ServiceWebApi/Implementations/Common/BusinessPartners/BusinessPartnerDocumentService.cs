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
    public class BusinessPartnerDocumentService : IBusinessPartnerDocumentService
    {
        public BusinessPartnerDocumentListResponse Sync(SyncBusinessPartnerDocumentRequest request)
        {
            BusinessPartnerDocumentListResponse response = new BusinessPartnerDocumentListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncBusinessPartnerDocumentRequest, BusinessPartnerDocumentViewModel, BusinessPartnerDocumentListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.BusinessPartnerDocuments = new List<BusinessPartnerDocumentViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
