using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Common.Prices;
using ServiceInterfaces.Messages.Common.Prices;
using ServiceInterfaces.ViewModels.Common.Prices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Common.Prices
{
    public class ServiceDeliveryService : IServiceDeliveryService
    {
        public ServiceDeliveryListResponse GetServiceDeliverys(int companyId)
        {
            ServiceDeliveryListResponse response = new ServiceDeliveryListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<ServiceDeliveryViewModel>, ServiceDeliveryListResponse>("GetServiceDeliverys", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.ServiceDeliverys = new List<ServiceDeliveryViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ServiceDeliveryResponse Create(ServiceDeliveryViewModel serviceDelivery)
        {
            ServiceDeliveryResponse response = new ServiceDeliveryResponse();
            try
            {
                response = WpfApiHandler.SendToApi<ServiceDeliveryViewModel, ServiceDeliveryResponse>(serviceDelivery, "Create");
            }
            catch (Exception ex)
            {
                response.ServiceDelivery = new ServiceDeliveryViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ServiceDeliveryResponse Delete(Guid identifier)
        {
            ServiceDeliveryResponse response = new ServiceDeliveryResponse();
            try
            {
                ServiceDeliveryViewModel serviceDelivery = new ServiceDeliveryViewModel();
                serviceDelivery.Identifier = identifier;
                response = WpfApiHandler.SendToApi<ServiceDeliveryViewModel, ServiceDeliveryResponse>(serviceDelivery, "Delete");
            }
            catch (Exception ex)
            {
                response.ServiceDelivery = new ServiceDeliveryViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ServiceDeliveryListResponse Sync(SyncServiceDeliveryRequest request)
        {
            ServiceDeliveryListResponse response = new ServiceDeliveryListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncServiceDeliveryRequest, ServiceDeliveryViewModel, ServiceDeliveryListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.ServiceDeliverys = new List<ServiceDeliveryViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}

