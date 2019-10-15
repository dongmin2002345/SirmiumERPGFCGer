using DataMapper.Mappers.Statuses;
using DomainCore.Statuses;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions;
using ServiceInterfaces.Messages.Statuses;
using ServiceInterfaces.ViewModels.Statuses;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Statuses
{
    public class StatusService : IStatusService
    {
        private IUnitOfWork unitOfWork;

        public StatusService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public StatusListResponse GetStatuses(int companyId)
        {
            StatusListResponse response = new StatusListResponse();
            try
            {
                response.Statuses = unitOfWork.GetStatusRepository().GetStatuses(companyId)
                    .ConvertToStatusViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Statuses = new List<StatusViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public StatusResponse Create(StatusViewModel re)
        {
            StatusResponse response = new StatusResponse();
            try
            {
                Status addedStatus = unitOfWork.GetStatusRepository().Create(re.ConvertToStatus());

                unitOfWork.Save();

                response.Status = addedStatus.ConvertToStatusViewModel();
                response.Success = true;
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
                Status deletedStatus = unitOfWork.GetStatusRepository().Delete(identifier);

                unitOfWork.Save();

                response.Status = deletedStatus.ConvertToStatusViewModel();
                response.Success = true;
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
                response.Statuses = new List<StatusViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.Statuses.AddRange(unitOfWork.GetStatusRepository()
                        .GetStatusesNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToStatusViewModelList() ?? new List<StatusViewModel>());
                }
                else
                {
                    response.Statuses.AddRange(unitOfWork.GetStatusRepository()
                        .GetStatuses(request.CompanyId)
                        ?.ConvertToStatusViewModelList() ?? new List<StatusViewModel>());
                }

                response.Success = true;
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
