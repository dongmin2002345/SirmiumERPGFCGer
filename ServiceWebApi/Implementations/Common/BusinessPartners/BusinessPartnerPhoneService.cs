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
    public class BusinessPartnerPhoneService : IBusinessPartnerPhoneService
    {
        public BusinessPartnerPhoneListResponse GetBusinessPartnerPhones(int companyId)
        {
            BusinessPartnerPhoneListResponse response = new BusinessPartnerPhoneListResponse();
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("CompanyID", companyId.ToString());

                response = WpfApiHandler.GetFromApi<List<BusinessPartnerPhoneViewModel>, BusinessPartnerPhoneListResponse>("GetBusinessPartnerPhones", parameters);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public BusinessPartnerPhoneListResponse GetBusinessPartnerPhonesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            BusinessPartnerPhoneListResponse response = new BusinessPartnerPhoneListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<BusinessPartnerPhoneViewModel>, BusinessPartnerPhoneListResponse>("GetBusinessPartnerPhonesNewerThen", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() },
                    { "LastUpdateTime", lastUpdateTime.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public BusinessPartnerPhoneResponse Create(BusinessPartnerPhoneViewModel businessPartnerPhoneViewModel)
        {
            BusinessPartnerPhoneResponse response = new BusinessPartnerPhoneResponse();
            try
            {
                response = WpfApiHandler.SendToApi<BusinessPartnerPhoneViewModel, BusinessPartnerPhoneResponse>(businessPartnerPhoneViewModel, "Create");
            }
            catch (Exception ex)
            {
                response.BusinessPartnerPhone = new BusinessPartnerPhoneViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public BusinessPartnerPhoneResponse Delete(Guid identifier)
        {
            BusinessPartnerPhoneResponse response = new BusinessPartnerPhoneResponse();
            try
            {
                BusinessPartnerPhoneViewModel viewModel = new BusinessPartnerPhoneViewModel();
                viewModel.Identifier = identifier;
                response = WpfApiHandler.SendToApi<BusinessPartnerPhoneViewModel, BusinessPartnerPhoneResponse>(viewModel, "Delete");
            }
            catch (Exception ex)
            {
                response.BusinessPartnerPhone = new BusinessPartnerPhoneViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public BusinessPartnerPhoneListResponse Sync(SyncBusinessPartnerPhoneRequest request)
        {
            BusinessPartnerPhoneListResponse response = new BusinessPartnerPhoneListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncBusinessPartnerPhoneRequest, BusinessPartnerPhoneViewModel, BusinessPartnerPhoneListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.BusinessPartnerPhones = new List<BusinessPartnerPhoneViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
