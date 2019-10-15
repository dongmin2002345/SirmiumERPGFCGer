using DataMapper.Mappers.Common.Prices;
using DomainCore.Common.Prices;
using RepositoryCore.UnitOfWork.Abstractions;
using ServiceInterfaces.Abstractions.Common.Prices;
using ServiceInterfaces.Messages.Common.Prices;
using ServiceInterfaces.ViewModels.Common.Prices;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCore.Implementations.Common.Prices
{
    public class DiscountService : IDiscountService
    {
        private IUnitOfWork unitOfWork;

        public DiscountService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public DiscountListResponse GetDiscounts(int companyId)
        {
            DiscountListResponse response = new DiscountListResponse();
            try
            {
                response.Discounts = unitOfWork.GetDiscountRepository().GetDiscounts(companyId)
                    .ConvertToDiscountViewModelList();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Discounts = new List<DiscountViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public DiscountResponse Create(DiscountViewModel re)
        {
            DiscountResponse response = new DiscountResponse();
            try
            {
                Discount addedDiscount = unitOfWork.GetDiscountRepository().Create(re.ConvertToDiscount());

                unitOfWork.Save();

                response.Discount = addedDiscount.ConvertToDiscountViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Discount = new DiscountViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public DiscountResponse Delete(Guid identifier)
        {
            DiscountResponse response = new DiscountResponse();
            try
            {
                Discount deletedDiscount = unitOfWork.GetDiscountRepository().Delete(identifier);

                unitOfWork.Save();

                response.Discount = deletedDiscount.ConvertToDiscountViewModel();
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Discount = new DiscountViewModel();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public DiscountListResponse Sync(SyncDiscountRequest request)
        {
            DiscountListResponse response = new DiscountListResponse();
            try
            {
                response.Discounts = new List<DiscountViewModel>();

                if (request.LastUpdatedAt != null)
                {
                    response.Discounts.AddRange(unitOfWork.GetDiscountRepository()
                        .GetDiscountsNewerThen(request.CompanyId, (DateTime)request.LastUpdatedAt)
                        ?.ConvertToDiscountViewModelList() ?? new List<DiscountViewModel>());
                }
                else
                {
                    response.Discounts.AddRange(unitOfWork.GetDiscountRepository()
                        .GetDiscounts(request.CompanyId)
                        ?.ConvertToDiscountViewModelList() ?? new List<DiscountViewModel>());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Discounts = new List<DiscountViewModel>();
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}

