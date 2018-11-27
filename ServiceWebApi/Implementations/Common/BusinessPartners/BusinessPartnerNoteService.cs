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
    public class BusinessPartnerNoteService : IBusinessPartnerNoteService
    {
        public BusinessPartnerNoteListResponse Sync(SyncBusinessPartnerNoteRequest request)
        {
            BusinessPartnerNoteListResponse response = new BusinessPartnerNoteListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncBusinessPartnerNoteRequest, BusinessPartnerNoteViewModel, BusinessPartnerNoteListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.BusinessPartnerNotes = new List<BusinessPartnerNoteViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
