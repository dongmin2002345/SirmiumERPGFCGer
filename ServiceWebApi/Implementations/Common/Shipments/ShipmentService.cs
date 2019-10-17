using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Common.Shipments;
using ServiceInterfaces.Messages.Common.Shipments;
using ServiceInterfaces.ViewModels.Common.Shipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Common.Shipments
{
    public class ShipmentService : IShipmentService
    {
        public ShipmentListResponse GetShipments(int companyId)
        {
            ShipmentListResponse response = new ShipmentListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<ShipmentViewModel>, ShipmentListResponse>("GetShipments", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.Shipments = new List<ShipmentViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ShipmentListResponse GetShipmentsNewerThan(int companyId, DateTime? lastUpdateTime)
        {
            ShipmentListResponse response = new ShipmentListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<ShipmentViewModel>, ShipmentListResponse>("GetShipmentsNewerThan", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() },
                    { "LastUpdateTime", lastUpdateTime.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.Shipments = new List<ShipmentViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ShipmentResponse Create(ShipmentViewModel re)
        {
            ShipmentResponse response = new ShipmentResponse();
            try
            {
                response = WpfApiHandler.SendToApi<ShipmentViewModel, ShipmentResponse>(re, "Create");
            }
            catch (Exception ex)
            {
                response.Shipment = new ShipmentViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ShipmentResponse Delete(Guid identifier)
        {
            ShipmentResponse response = new ShipmentResponse();
            try
            {
                ShipmentViewModel re = new ShipmentViewModel();
                re.Identifier = identifier;
                response = WpfApiHandler.SendToApi<ShipmentViewModel, ShipmentResponse>(re, "Delete");
            }
            catch (Exception ex)
            {
                response.Shipment = new ShipmentViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ShipmentListResponse Sync(SyncShipmentRequest request)
        {
            ShipmentListResponse response = new ShipmentListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncShipmentRequest, ShipmentViewModel, ShipmentListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.Shipments = new List<ShipmentViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
