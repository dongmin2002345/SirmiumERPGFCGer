using DataMapper.Mappers.Employees;
using DomainCore.Employees;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Employees
{
    public class EmployeeByConstructionSiteHistoryService : IEmployeeByConstructionSiteHistoryService
    {
        IUnitOfWork unitOfWork;

        public EmployeeByConstructionSiteHistoryService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public EmployeeByConstructionSiteHistoryListResponse GetEmployeeByConstructionSiteHistories(int companyId)
        {
            EmployeeByConstructionSiteHistoryListResponse response = new EmployeeByConstructionSiteHistoryListResponse();
            try
            {
                response.EmployeeByConstructionSiteHistories = unitOfWork.GetEmployeeByConstructionSiteHistoryRepository().GetEmployeeByConstructionSiteHistories(companyId)
               .ConvertToEmployeeByConstructionSiteHistoryViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.EmployeeByConstructionSiteHistories = new List<EmployeeByConstructionSiteHistoryViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public EmployeeByConstructionSiteHistoryListResponse GetEmployeeByConstructionSiteHistoriesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            EmployeeByConstructionSiteHistoryListResponse response = new EmployeeByConstructionSiteHistoryListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.EmployeeByConstructionSiteHistories = unitOfWork.GetEmployeeByConstructionSiteHistoryRepository()
                        .GetEmployeeByConstructionSiteHistoriesNewerThen(companyId, (DateTime)lastUpdateTime)
                        .ConvertToEmployeeByConstructionSiteHistoryViewModelList();
                }
                else
                {
                    response.EmployeeByConstructionSiteHistories = unitOfWork.GetEmployeeByConstructionSiteHistoryRepository()
                        .GetEmployeeByConstructionSiteHistories(companyId)
                        .ConvertToEmployeeByConstructionSiteHistoryViewModelList();
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.EmployeeByConstructionSiteHistories = new List<EmployeeByConstructionSiteHistoryViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public EmployeeByConstructionSiteHistoryResponse Create(EmployeeByConstructionSiteHistoryViewModel re)
        {
            EmployeeByConstructionSiteHistoryResponse response = new EmployeeByConstructionSiteHistoryResponse();
            try
            {
                EmployeeByConstructionSiteHistory addedEmployeeByConstructionSiteHistory = unitOfWork.GetEmployeeByConstructionSiteHistoryRepository().Create(re.ConvertToEmployeeByConstructionSiteHistory());
                unitOfWork.Save();
                response.EmployeeByConstructionSiteHistory = addedEmployeeByConstructionSiteHistory.ConvertToEmployeeByConstructionSiteHistoryViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.EmployeeByConstructionSiteHistory = new EmployeeByConstructionSiteHistoryViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public EmployeeByConstructionSiteHistoryResponse Delete(Guid identifier)
        {
            EmployeeByConstructionSiteHistoryResponse response = new EmployeeByConstructionSiteHistoryResponse();
            try
            {
                EmployeeByConstructionSiteHistory deletedEmployeeByConstructionSiteHistory = unitOfWork.GetEmployeeByConstructionSiteHistoryRepository().Delete(identifier);

                unitOfWork.Save();

                response.EmployeeByConstructionSiteHistory = deletedEmployeeByConstructionSiteHistory.ConvertToEmployeeByConstructionSiteHistoryViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.EmployeeByConstructionSiteHistory = new EmployeeByConstructionSiteHistoryViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public EmployeeByConstructionSiteHistoryListResponse Sync(SyncEmployeeByConstructionSiteHistoryRequest request)
        {
            EmployeeByConstructionSiteHistoryListResponse response = new EmployeeByConstructionSiteHistoryListResponse();
            try
            {
                response.EmployeeByConstructionSiteHistories = new List<EmployeeByConstructionSiteHistoryViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.EmployeeByConstructionSiteHistories.AddRange(unitOfWork.GetEmployeeByConstructionSiteHistoryRepository()
                        .GetEmployeeByConstructionSiteHistoriesNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToEmployeeByConstructionSiteHistoryViewModelList() ?? new List<EmployeeByConstructionSiteHistoryViewModel>());
                }
                else
                {
                    response.EmployeeByConstructionSiteHistories.AddRange(unitOfWork.GetEmployeeByConstructionSiteHistoryRepository()
                        .GetEmployeeByConstructionSiteHistories(request.CompanyId)
                        ?.ConvertToEmployeeByConstructionSiteHistoryViewModelList() ?? new List<EmployeeByConstructionSiteHistoryViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.EmployeeByConstructionSiteHistories = new List<EmployeeByConstructionSiteHistoryViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
