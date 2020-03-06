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
    public class ToDoStatusService : IToDoStatusService
    {
        private IUnitOfWork unitOfWork;

        public ToDoStatusService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public ToDoStatusListResponse GetToDoStatuses(int companyId)
        {
            ToDoStatusListResponse response = new ToDoStatusListResponse();
            try
            {
                response.ToDoStatuses = unitOfWork.GetToDoStatusRepository().GetToDoStatuses(companyId)
                    .ConvertToToDoStatusViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ToDoStatuses = new List<ToDoStatusViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ToDoStatusResponse Create(ToDoStatusViewModel re)
        {
            ToDoStatusResponse response = new ToDoStatusResponse();
            try
            {
                ToDoStatus addedToDoStatus = unitOfWork.GetToDoStatusRepository().Create(re.ConvertToToDoStatus());

                unitOfWork.Save();

                response.ToDoStatus = addedToDoStatus.ConvertToToDoStatusViewModel();
                response.Success = true;
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
                ToDoStatus deletedToDoStatus = unitOfWork.GetToDoStatusRepository().Delete(identifier);

                unitOfWork.Save();

                response.ToDoStatus = deletedToDoStatus.ConvertToToDoStatusViewModel();
                response.Success = true;
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
                response.ToDoStatuses = new List<ToDoStatusViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.ToDoStatuses.AddRange(unitOfWork.GetToDoStatusRepository()
                        .GetToDoStatusesNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToToDoStatusViewModelList() ?? new List<ToDoStatusViewModel>());
                }
                else
                {
                    response.ToDoStatuses.AddRange(unitOfWork.GetToDoStatusRepository()
                        .GetToDoStatuses(request.CompanyId)
                        ?.ConvertToToDoStatusViewModelList() ?? new List<ToDoStatusViewModel>());
                }

                response.Success = true;
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
