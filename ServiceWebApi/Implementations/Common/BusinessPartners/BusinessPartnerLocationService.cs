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
    public class BusinessPartnerLocationService : IBusinessPartnerLocationService
    {
        public BusinessPartnerLocationListResponse GetBusinessPartnerLocations(int companyId)
        {
            BusinessPartnerLocationListResponse response = new BusinessPartnerLocationListResponse();
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("CompanyID", companyId.ToString());

                response = WpfApiHandler.GetFromApi<List<BusinessPartnerLocationViewModel>, BusinessPartnerLocationListResponse>("GetBusinessPartnerLocations", parameters);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public BusinessPartnerLocationListResponse GetBusinessPartnerLocationsNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            BusinessPartnerLocationListResponse response = new BusinessPartnerLocationListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<BusinessPartnerLocationViewModel>, BusinessPartnerLocationListResponse>("GetBusinessPartnerLocationsNewerThen", new Dictionary<string, string>()
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

        public BusinessPartnerLocationResponse Create(BusinessPartnerLocationViewModel businessPartnerLocationViewModel)
        {
            BusinessPartnerLocationResponse response = new BusinessPartnerLocationResponse();
            try
            {
                response = WpfApiHandler.SendToApi<BusinessPartnerLocationViewModel, BusinessPartnerLocationResponse>(businessPartnerLocationViewModel, "Create");
            }
            catch (Exception ex)
            {
                response.BusinessPartnerLocation = new BusinessPartnerLocationViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public BusinessPartnerLocationResponse Delete(Guid identifier)
        {
            BusinessPartnerLocationResponse response = new BusinessPartnerLocationResponse();
            try
            {
                BusinessPartnerLocationViewModel viewModel = new BusinessPartnerLocationViewModel();
                viewModel.Identifier = identifier;
                response = WpfApiHandler.SendToApi<BusinessPartnerLocationViewModel, BusinessPartnerLocationResponse>(viewModel, "Delete");
            }
            catch (Exception ex)
            {
                response.BusinessPartnerLocation = new BusinessPartnerLocationViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public BusinessPartnerLocationListResponse Sync(SyncBusinessPartnerLocationRequest request)
        {
            BusinessPartnerLocationListResponse response = new BusinessPartnerLocationListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncBusinessPartnerLocationRequest, BusinessPartnerLocationViewModel, BusinessPartnerLocationListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.BusinessPartnerLocations = new List<BusinessPartnerLocationViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
