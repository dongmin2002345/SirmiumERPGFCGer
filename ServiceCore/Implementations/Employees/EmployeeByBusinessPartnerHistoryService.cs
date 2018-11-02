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
    public class EmployeeByBusinessPartnerHistoryService : IEmployeeByBusinessPartnerHistoryService
    {
        IUnitOfWork unitOfWork;

        public EmployeeByBusinessPartnerHistoryService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public EmployeeByBusinessPartnerHistoryListResponse GetEmployeeByBusinessPartnerHistories(int companyId)
        {
            EmployeeByBusinessPartnerHistoryListResponse response = new EmployeeByBusinessPartnerHistoryListResponse();
            try
            {
                response.EmployeeByBusinessPartnerHistories = unitOfWork.GetEmployeeByBusinessPartnerHistoryRepository().GetEmployeeByBusinessPartnerHistories(companyId)
               .ConvertToEmployeeByBusinessPartnerHistoryViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.EmployeeByBusinessPartnerHistories = new List<EmployeeByBusinessPartnerHistoryViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public EmployeeByBusinessPartnerHistoryListResponse GetEmployeeByBusinessPartnerHistoriesNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            EmployeeByBusinessPartnerHistoryListResponse response = new EmployeeByBusinessPartnerHistoryListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.EmployeeByBusinessPartnerHistories = unitOfWork.GetEmployeeByBusinessPartnerHistoryRepository()
                        .GetEmployeeByBusinessPartnerHistoriesNewerThen(companyId, (DateTime)lastUpdateTime)
                        .ConvertToEmployeeByBusinessPartnerHistoryViewModelList();
                }
                else
                {
                    response.EmployeeByBusinessPartnerHistories = unitOfWork.GetEmployeeByBusinessPartnerHistoryRepository()
                        .GetEmployeeByBusinessPartnerHistories(companyId)
                        .ConvertToEmployeeByBusinessPartnerHistoryViewModelList();
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.EmployeeByBusinessPartnerHistories = new List<EmployeeByBusinessPartnerHistoryViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public EmployeeByBusinessPartnerHistoryResponse Create(EmployeeByBusinessPartnerHistoryViewModel re)
        {
            EmployeeByBusinessPartnerHistoryResponse response = new EmployeeByBusinessPartnerHistoryResponse();
            try
            {
                EmployeeByBusinessPartnerHistory addedEmployeeByBusinessPartnerHistory = unitOfWork.GetEmployeeByBusinessPartnerHistoryRepository().Create(re.ConvertToEmployeeByBusinessPartnerHistory());
                unitOfWork.Save();
                response.EmployeeByBusinessPartnerHistory = addedEmployeeByBusinessPartnerHistory.ConvertToEmployeeByBusinessPartnerHistoryViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.EmployeeByBusinessPartnerHistory = new EmployeeByBusinessPartnerHistoryViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public EmployeeByBusinessPartnerHistoryResponse Delete(Guid identifier)
        {
            EmployeeByBusinessPartnerHistoryResponse response = new EmployeeByBusinessPartnerHistoryResponse();
            try
            {
                EmployeeByBusinessPartnerHistory deletedEmployeeByBusinessPartnerHistory = unitOfWork.GetEmployeeByBusinessPartnerHistoryRepository().Delete(identifier);

                unitOfWork.Save();

                response.EmployeeByBusinessPartnerHistory = deletedEmployeeByBusinessPartnerHistory.ConvertToEmployeeByBusinessPartnerHistoryViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.EmployeeByBusinessPartnerHistory = new EmployeeByBusinessPartnerHistoryViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public EmployeeByBusinessPartnerHistoryListResponse Sync(SyncEmployeeByBusinessPartnerHistoryRequest request)
        {
            EmployeeByBusinessPartnerHistoryListResponse response = new EmployeeByBusinessPartnerHistoryListResponse();
            try
            {
                response.EmployeeByBusinessPartnerHistories = new List<EmployeeByBusinessPartnerHistoryViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.EmployeeByBusinessPartnerHistories.AddRange(unitOfWork.GetEmployeeByBusinessPartnerHistoryRepository()
                        .GetEmployeeByBusinessPartnerHistoriesNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToEmployeeByBusinessPartnerHistoryViewModelList() ?? new List<EmployeeByBusinessPartnerHistoryViewModel>());
                }
                else
                {
                    response.EmployeeByBusinessPartnerHistories.AddRange(unitOfWork.GetEmployeeByBusinessPartnerHistoryRepository()
                        .GetEmployeeByBusinessPartnerHistories(request.CompanyId)
                        ?.ConvertToEmployeeByBusinessPartnerHistoryViewModelList() ?? new List<EmployeeByBusinessPartnerHistoryViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.EmployeeByBusinessPartnerHistories = new List<EmployeeByBusinessPartnerHistoryViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
