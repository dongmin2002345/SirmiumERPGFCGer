using DataMapper.Mappers.Employees;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Employees
{
    public class PhysicalPersonItemService : IPhysicalPersonItemService
    {
        IUnitOfWork unitOfWork;

        public PhysicalPersonItemService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public PhysicalPersonItemListResponse Sync(SyncPhysicalPersonItemRequest request)
        {
            PhysicalPersonItemListResponse response = new PhysicalPersonItemListResponse();
            try
            {
                response.PhysicalPersonItems = new List<PhysicalPersonItemViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.PhysicalPersonItems.AddRange(unitOfWork.GetPhysicalPersonItemRepository()
                        .GetPhysicalPersonItemsNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToPhysicalPersonItemViewModelList() ?? new List<PhysicalPersonItemViewModel>());
                }
                else
                {
                    response.PhysicalPersonItems.AddRange(unitOfWork.GetPhysicalPersonItemRepository()
                        .GetPhysicalPersonItems(request.CompanyId)
                        ?.ConvertToPhysicalPersonItemViewModelList() ?? new List<PhysicalPersonItemViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.PhysicalPersonItems = new List<PhysicalPersonItemViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
