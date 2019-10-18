using ApiExtension.Sender;
using ServiceInterfaces.Abstractions;
using ServiceInterfaces.Abstractions.Statuses;
using ServiceInterfaces.Messages.Statuses;
using ServiceInterfaces.ViewModels.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Statuses
{
    public class StatusService : IStatusService
    {
        public StatusListResponse GetStatuses(int companyId)
        {
            StatusListResponse response = new StatusListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<StatusViewModel>, StatusListResponse>("GetStatuses", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.Statuses = new List<StatusViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public StatusResponse Create(StatusViewModel Status)
        {
            StatusResponse response = new StatusResponse();
            try
            {
                response = WpfApiHandler.SendToApi<StatusViewModel, StatusResponse>(Status, "Create");
            }
            catch (Exception ex)
            {
                response.Status = new StatusViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public StatusResponse Delete(Guid identifier)
        {
            StatusResponse response = new StatusResponse();
            try
            {
                StatusViewModel Status = new StatusViewModel();
                Status.Identifier = identifier;
                response = WpfApiHandler.SendToApi<StatusViewModel, StatusResponse>(Status, "Delete");
            }
            catch (Exception ex)
            {
                response.Status = new StatusViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public StatusListResponse Sync(SyncStatusRequest request)
        {
            StatusListResponse response = new StatusListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncStatusRequest, StatusViewModel, StatusListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.Statuses = new List<StatusViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
