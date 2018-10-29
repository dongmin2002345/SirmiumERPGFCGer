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
    public class EmployeeItemService : IEmployeeItemService
    {
        IUnitOfWork unitOfWork;

        public EmployeeItemService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public EmployeeItemListResponse GetEmployeeItems(int companyId)
        {
            EmployeeItemListResponse response = new EmployeeItemListResponse();
            try
            {
                response.EmployeeItems = unitOfWork.GetEmployeeItemRepository()
                    .GetEmployeeItems(companyId)
                    .ConvertToEmployeeItemViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.EmployeeItems = new List<EmployeeItemViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public EmployeeItemListResponse GetEmployeeItemsNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            EmployeeItemListResponse response = new EmployeeItemListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.EmployeeItems = unitOfWork.GetEmployeeItemRepository()
                        .GetEmployeeItemsNewerThen(companyId, (DateTime)lastUpdateTime)
                        .ConvertToEmployeeItemViewModelList();
                }
                else
                {
                    response.EmployeeItems = unitOfWork.GetEmployeeItemRepository()
                        .GetEmployeeItems(companyId)
                        .ConvertToEmployeeItemViewModelList();
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.EmployeeItems = new List<EmployeeItemViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public EmployeeItemResponse Create(EmployeeItemViewModel EmployeeItemViewModel)
        {
            EmployeeItemResponse response = new EmployeeItemResponse();
            try
            {
                EmployeeItem addedEmployeeItem = unitOfWork.GetEmployeeItemRepository().Create(EmployeeItemViewModel.ConvertToEmployeeItem());
                unitOfWork.Save();
                response.EmployeeItem = addedEmployeeItem.ConvertToEmployeeItemViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.EmployeeItem = new EmployeeItemViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public EmployeeItemListResponse Sync(SyncEmployeeItemRequest request)
        {
            EmployeeItemListResponse response = new EmployeeItemListResponse();
            try
            {
                response.EmployeeItems = new List<EmployeeItemViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.EmployeeItems.AddRange(unitOfWork.GetEmployeeItemRepository()
                        .GetEmployeeItemsNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToEmployeeItemViewModelList() ?? new List<EmployeeItemViewModel>());
                }
                else
                {
                    response.EmployeeItems.AddRange(unitOfWork.GetEmployeeItemRepository()
                        .GetEmployeeItems(request.CompanyId)
                        ?.ConvertToEmployeeItemViewModelList() ?? new List<EmployeeItemViewModel>());
                }

                List<EmployeeItem> added = new List<EmployeeItem>();
                foreach (var item in request.UnSyncedEmployeeItems)
                {
                    if (item.Id == 0)
                        added.Add(unitOfWork.GetEmployeeItemRepository().Create(item.ConvertToEmployeeItem()));
                    ////else
                    ////    added.Add(unitOfWork.GetEmployeeRepository().Update(item.ConvertToFoodInputHay()));
                }

                unitOfWork.Save();

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.EmployeeItems = new List<EmployeeItemViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
