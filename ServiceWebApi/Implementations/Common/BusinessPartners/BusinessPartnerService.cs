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
    public class BusinessPartnerService : IBusinessPartnerService
    {
        public BusinessPartnerListResponse GetBusinessPartners()
        {
            BusinessPartnerListResponse response = new BusinessPartnerListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<BusinessPartnerViewModel>, BusinessPartnerListResponse>("GetBusinessPartners",
                    new Dictionary<string, string>() {
                    });
            }
            catch (Exception ex)
            {
                response.BusinessPartners = new List<BusinessPartnerViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerListResponse GetBusinessPartnersNewerThen(DateTime? lastUpdateTime)
        {
            BusinessPartnerListResponse response = new BusinessPartnerListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<BusinessPartnerViewModel>, BusinessPartnerListResponse>("GetBusinessPartnersNewerThen", new Dictionary<string, string>()
                {
                    { "LastUpdateTime", lastUpdateTime.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.BusinessPartners = new List<BusinessPartnerViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerResponse Create(BusinessPartnerViewModel businessPartner)
        {
            BusinessPartnerResponse response = new BusinessPartnerResponse();
            try
            {
                response = WpfApiHandler.SendToApi<BusinessPartnerViewModel, BusinessPartnerResponse>(businessPartner, "Create");
            }
            catch (Exception ex)
            {
                response.BusinessPartner = new BusinessPartnerViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public BusinessPartnerResponse Delete(Guid identifier)
        {
            BusinessPartnerResponse response = new BusinessPartnerResponse();
            try
            {
                BusinessPartnerViewModel viewModel = new BusinessPartnerViewModel();
                viewModel.Identifier = identifier;
                response = WpfApiHandler.SendToApi<BusinessPartnerViewModel, BusinessPartnerResponse>(viewModel, "Delete");
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.BusinessPartner = new BusinessPartnerViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

                return response;
        }

        public BusinessPartnerListResponse Sync(SyncBusinessPartnerRequest request)
        {
            BusinessPartnerListResponse response = new BusinessPartnerListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncBusinessPartnerRequest, BusinessPartnerViewModel, BusinessPartnerListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.BusinessPartners = new List<BusinessPartnerViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
