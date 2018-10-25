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
    public class EmployeeLicenceService : IEmployeeLicenceService
    {
        IUnitOfWork unitOfWork;

        public EmployeeLicenceService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public EmployeeLicenceItemListResponse GetEmployeeItems(int companyId)
        {
            EmployeeLicenceItemListResponse response = new EmployeeLicenceItemListResponse();
            try
            {
                response.EmployeeLicenceItems = unitOfWork.GetEmployeeLicenceRepository()
                    .GetEmployeeItems(companyId)
                    .ConvertToEmployeeLicenceViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.EmployeeLicenceItems = new List<EmployeeLicenceItemViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public EmployeeLicenceItemListResponse GetEmployeeItemsNewerThen(int companyId, DateTime? lastUpdateTime)
        {
            EmployeeLicenceItemListResponse response = new EmployeeLicenceItemListResponse();
            try
            {
                if (lastUpdateTime != null)
                {
                    response.EmployeeLicenceItems = unitOfWork.GetEmployeeLicenceRepository()
                        .GetEmployeeItemsNewerThan(companyId, (DateTime)lastUpdateTime)
                        .ConvertToEmployeeLicenceViewModelList();
                }
                else
                {
                    response.EmployeeLicenceItems = unitOfWork.GetEmployeeLicenceRepository()
                        .GetEmployeeItems(companyId)
                        .ConvertToEmployeeLicenceViewModelList();
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.EmployeeLicenceItems = new List<EmployeeLicenceItemViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public EmployeeLicenceItemResponse Create(EmployeeLicenceItemViewModel EmployeeItemViewModel)
        {
            EmployeeLicenceItemResponse response = new EmployeeLicenceItemResponse();
            try
            {
                var addedEmployeeItem = unitOfWork.GetEmployeeLicenceRepository().Create(EmployeeItemViewModel.ConvertToEmployeeLicence());
                unitOfWork.Save();
                response.EmployeeLicenceItem = addedEmployeeItem.ConvertToEmployeeLicenceViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.EmployeeLicenceItem = new EmployeeLicenceItemViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public EmployeeLicenceItemListResponse Sync(SyncEmployeeLicenceItemRequest request)
        {
            EmployeeLicenceItemListResponse response = new EmployeeLicenceItemListResponse();
            try
            {
                response.EmployeeLicenceItems = new List<EmployeeLicenceItemViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.EmployeeLicenceItems.AddRange(unitOfWork.GetEmployeeLicenceRepository()
                        .GetEmployeeItemsNewerThan(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToEmployeeLicenceViewModelList() ?? new List<EmployeeLicenceItemViewModel>());
                }
                else
                {
                    response.EmployeeLicenceItems.AddRange(unitOfWork.GetEmployeeLicenceRepository()
                        .GetEmployeeItems(request.CompanyId)
                        ?.ConvertToEmployeeLicenceViewModelList() ?? new List<EmployeeLicenceItemViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.EmployeeLicenceItems = new List<EmployeeLicenceItemViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
