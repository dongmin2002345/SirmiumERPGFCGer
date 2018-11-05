using DataMapper.Mappers.Common.ToDos;
using DomainCore.Common.ToDos;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.ToDos;
using ServiceInterfaces.Messages.Common.ToDos;
using ServiceInterfaces.ViewModels.Common.ToDos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Common.ToDos
{
    public class ToDoService : IToDoService
    {
        IUnitOfWork unitOfWork;

        public ToDoService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public ToDoListResponse GetToDos(int companyId)
        {
            ToDoListResponse response = new ToDoListResponse();
            try
            {
                response.ToDos = unitOfWork.GetToDoRepository()
                    .GetToDos(companyId)
                    .ConvertToToDoViewModelList();

                response.Success = true;
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
                if (lastUpdateTime != null)
                {
                    response.ToDos = unitOfWork.GetToDoRepository()
                        .GetToDosNewerThen(companyId, (DateTime)lastUpdateTime)
                        .ConvertToToDoViewModelList();
                }
                else
                {
                    response.ToDos = unitOfWork.GetToDoRepository()
                        .GetToDos(companyId)
                        .ConvertToToDoViewModelList();
                }
                response.Success = true;
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
                ToDo addedToDo = unitOfWork.GetToDoRepository().Create(li.ConvertToToDo());
                unitOfWork.Save();

                response.ToDo = addedToDo.ConvertToToDoViewModel();
                response.Success = true;
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
                ToDo deletedToDo = unitOfWork.GetToDoRepository().Delete(identifier);

                unitOfWork.Save();

                response.ToDo = deletedToDo.ConvertToToDoViewModel();
                response.Success = true;
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
                response.ToDos = new List<ToDoViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.ToDos.AddRange(unitOfWork.GetToDoRepository()
                        .GetToDosNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToToDoViewModelList() ?? new List<ToDoViewModel>());
                }
                else
                {
                    response.ToDos.AddRange(unitOfWork.GetToDoRepository()
                        .GetToDos(request.CompanyId)
                        ?.ConvertToToDoViewModelList() ?? new List<ToDoViewModel>());
                }

                response.Success = true;
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
