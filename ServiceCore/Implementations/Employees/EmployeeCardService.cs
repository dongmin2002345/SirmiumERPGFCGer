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
    public class EmployeeCardService : IEmployeeCardService
    {
        IUnitOfWork unitOfWork;

        public EmployeeCardService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public EmployeeCardListResponse Sync(SyncEmployeeCardRequest request)
        {
            EmployeeCardListResponse response = new EmployeeCardListResponse();
            try
            {
                response.EmployeeCards = new List<EmployeeCardViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.EmployeeCards.AddRange(unitOfWork.GetEmployeeCardRepository()
                        .GetEmployeeCardsNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToEmployeeCardViewModelList() ?? new List<EmployeeCardViewModel>());
                }
                else
                {
                    response.EmployeeCards.AddRange(unitOfWork.GetEmployeeCardRepository()
                        .GetEmployeeCards(request.CompanyId)
                        ?.ConvertToEmployeeCardViewModelList() ?? new List<EmployeeCardViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.EmployeeCards = new List<EmployeeCardViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
