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
    public class ToDoService : IToDoService
    {
        public ToDoListResponse GetToDos(int companyId)
        {
            ToDoListResponse response = new ToDoListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<ToDoViewModel>, ToDoListResponse>("GetToDos", new Dictionary<string, string>()
                {
                   { "CompanyId", companyId.ToString() }
                });

            }
            catch (Exception ex)
            {
                response.ToDos = new List<ToDoViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ToDoListResponse GetToDosNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            ToDoListResponse response = new ToDoListResponse();
            try
            {
                response = WpfApiHandler.GetFromApi<List<ToDoViewModel>, ToDoListResponse>("GetToDosNewerThen", new Dictionary<string, string>()
                {
                    { "CompanyId", companyId.ToString() },
                    { "LastUpdateTime", lastUpdateTime.ToString() }
                });
            }
            catch (Exception ex)
            {
                response.ToDos = new List<ToDoViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ToDoResponse Create(ToDoViewModel li)
        {
            ToDoResponse response = new ToDoResponse();
            try
            {
                response = WpfApiHandler.SendToApi<ToDoViewModel, ToDoResponse>(li, "Create");
            }
            catch (Exception ex)
            {
                response.ToDo = new ToDoViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ToDoResponse Delete(Guid identifier)
        {
            ToDoResponse response = new ToDoResponse();
            try
            {
                ToDoViewModel li = new ToDoViewModel();
                li.Identifier = identifier;
                response = WpfApiHandler.SendToApi<ToDoViewModel, ToDoResponse>(li, "Delete");
            }
            catch (Exception ex)
            {
                response.ToDo = new ToDoViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ToDoListResponse Sync(SyncToDoRequest request)
        {
            ToDoListResponse response = new ToDoListResponse();
            try
            {
                response = WpfApiHandler.SendToApi<SyncToDoRequest, ToDoViewModel, ToDoListResponse>(request, "Sync");
            }
            catch (Exception ex)
            {
                response.ToDos = new List<ToDoViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
