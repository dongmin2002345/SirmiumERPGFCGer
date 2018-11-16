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
