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
    public class EmployeeProfessionService : IEmployeeProfessionService
    {
        IUnitOfWork unitOfWork;

        public EmployeeProfessionService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public EmployeeProfessionItemListResponse GetEmployeeItems(int companyId)
        {
            EmployeeProfessionItemListResponse response = new EmployeeProfessionItemListResponse();
            try
            {
                response.EmployeeProfessionItems = unitOfWork.GetEmployeeProfessionRepository()
                    .GetEmployeeItems(companyId)
                    .ConvertToEmployeeProfessionViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.EmployeeProfessionItems = new List<EmployeeProfessionItemViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public EmployeeProfessionItemListResponse GetEmployeeItemsNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            EmployeeProfessionItemListResponse response = new EmployeeProfessionItemListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.EmployeeProfessionItems = unitOfWork.GetEmployeeProfessionRepository()
                        .GetEmployeeItemsNewerThan(companyId, (DateTime)lastUpdateTime)
                        .ConvertToEmployeeProfessionViewModelList();
                }
                else
                {
                    response.EmployeeProfessionItems = unitOfWork.GetEmployeeProfessionRepository()
                        .GetEmployeeItems(companyId)
                        .ConvertToEmployeeProfessionViewModelList();
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.EmployeeProfessionItems = new List<EmployeeProfessionItemViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public EmployeeProfessionItemResponse Create(EmployeeProfessionItemViewModel EmployeeItemViewModel)
        {
            EmployeeProfessionItemResponse response = new EmployeeProfessionItemResponse();
            try
            {
                var addedEmployeeItem = unitOfWork.GetEmployeeProfessionRepository().Create(EmployeeItemViewModel.ConvertToEmployeeProfession());
                unitOfWork.Save();
                response.EmployeeProfessionItem = addedEmployeeItem.ConvertToEmployeeProfessionViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.EmployeeProfessionItem = new EmployeeProfessionItemViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public EmployeeProfessionItemListResponse 
            Sync(SyncEmployeeProfessionItemRequest request)
        {
            EmployeeProfessionItemListResponse response = new EmployeeProfessionItemListResponse();
            try
            {
                response.EmployeeProfessionItems = new List<EmployeeProfessionItemViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.EmployeeProfessionItems.AddRange(unitOfWork.GetEmployeeProfessionRepository()
                        .GetEmployeeItemsNewerThan(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToEmployeeProfessionViewModelList() ?? new List<EmployeeProfessionItemViewModel>());
                }
                else
                {
                    response.EmployeeProfessionItems.AddRange(unitOfWork.GetEmployeeProfessionRepository()
                        .GetEmployeeItems(request.CompanyId)
                        ?.ConvertToEmployeeProfessionViewModelList() ?? new List<EmployeeProfessionItemViewModel>());
                }

                List<EmployeeProfession> added = new List<EmployeeProfession>();
                foreach (var item in request.UnSyncedEmployeeProfessionItems)
                {
                    if (item.Id == 0)
                        added.Add(unitOfWork.GetEmployeeProfessionRepository().Create(item.ConvertToEmployeeProfession()));
                    ////else
                    ////    added.Add(unitOfWork.GetEmployeeRepository().Update(item.ConvertToFoodInputHay()));
                }

                unitOfWork.Save();

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.EmployeeProfessionItems = new List<EmployeeProfessionItemViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
