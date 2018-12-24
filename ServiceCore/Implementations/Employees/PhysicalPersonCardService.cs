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
    public class PhysicalPersonCardService : IPhysicalPersonCardService
    {
        IUnitOfWork unitOfWork;

        public PhysicalPersonCardService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public PhysicalPersonCardListResponse Sync(SyncPhysicalPersonCardRequest request)
        {
            PhysicalPersonCardListResponse response = new PhysicalPersonCardListResponse();
            try
            {
                response.PhysicalPersonCards = new List<PhysicalPersonCardViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.PhysicalPersonCards.AddRange(unitOfWork.GetPhysicalPersonCardRepository()
                        .GetPhysicalPersonCardsNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToPhysicalPersonCardViewModelList() ?? new List<PhysicalPersonCardViewModel>());
                }
                else
                {
                    response.PhysicalPersonCards.AddRange(unitOfWork.GetPhysicalPersonCardRepository()
                        .GetPhysicalPersonCards(request.CompanyId)
                        ?.ConvertToPhysicalPersonCardViewModelList() ?? new List<PhysicalPersonCardViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.PhysicalPersonCards = new List<PhysicalPersonCardViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
