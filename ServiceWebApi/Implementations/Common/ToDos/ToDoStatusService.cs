using ApiExtension.Sender;
using ServiceInterfaces.Abstractions.Common.ToDos;
using ServiceInterfaces.Messages.Common.ToDos;
using ServiceInterfaces.ViewModels.Common.ToDos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWebApi.Implementations.Common.ToDos
{
    public class ToDoStatusService : IToDoStatusService
    {
        public ToDoStatusListResponse GetToDoStatuses(int companyId)
        {
            ToDoStatusListResponse response = new ToDoStatusListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<ToDoStatusViewModel>, ToDoStatusListResponse>("GetToDoStatuses", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.ToDoStatuses = new List<ToDoStatusViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ToDoStatusResponse Create(ToDoStatusViewModel ToDoStatus)
        {
            ToDoStatusResponse response = new ToDoStatusResponse();
            try
            {
                response = WpfApiHandler.SendToApi<ToDoStatusViewModel, ToDoStatusResponse>(ToDoStatus, "Create");
            }
            catch (Exception ex)
            {
                response.ToDoStatus = new ToDoStatusViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ToDoStatusResponse Delete(Guid identifier)
        {
            ToDoStatusResponse response = new ToDoStatusResponse();
            try
            {
                ToDoStatusViewModel ToDoStatus = new ToDoStatusViewModel();
                ToDoStatus.Identifier = identifier;
                response = WpfApiHandler.SendToApi<ToDoStatusViewModel, ToDoStatusResponse>(ToDoStatus, "Delete");
            }
            catch (Exception ex)
            {
                response.ToDoStatus = new ToDoStatusViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ToDoStatusListResponse Sync(SyncToDoStatusRequest request)
        {
            ToDoStatusListResponse response = new ToDoStatusListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncToDoStatusRequest, ToDoStatusViewModel, ToDoStatusListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.ToDoStatuses = new List<ToDoStatusViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
